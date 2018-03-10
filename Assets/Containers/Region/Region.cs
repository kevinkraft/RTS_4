using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class to contain the Towns, Resources in a Region and its Map

//Notes:
// * The seed needs to be set when this class is instantiated
// * mGridPos needs to be public so I can set it for preexisting Regions
//   but dont assign to it without using setGridPos()
//   * But when initiating a new region you cant use setGridPos,
//     you need to yous mGridPos directly, as the towns and resources are made at 0,0
//     then when you set the grid pos with setGridPos it moves everything to the correct 
//     location (this isnt true)

public class Region : EntityContainer
{
    //public members
    public int mSeed;
	public GameTypes.MapType mType;

    //private members
	public Vector2 mGridPos; //this is temporarily public so I can start the game with one extra region
    private int mWidth = Globals.REGION_MAP_WIDTH;
    private Map mMap;
    private GameObject mResourcesObject;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Start()
    {
		//dont start the Region versions in GameObjectsList
		GameObjectList gol = GetComponentInParent<GameObjectList>();
		if (gol)
		{
			return;
		}
		
        //set the position from the grid pos
        setPosition();
        //get the resources object
        mResourcesObject = GetComponentInChildren<Resources>().gameObject;
        if (!mResourcesObject)
        {
            Debug.LogError("Can't find resources object as child of Region");
        }

        //get or make the Map, the map makes the Resources
        Map map = GetComponentInChildren<Map>();
        if (!map)
        {
            //no map exists
			mMap = ObjectManager.initMap(transform.position, mType, this, mSeed);
        }

        //search for entities in children and put them in the right group
        addChildernToGroups(this.transform, 0, 2);

    }
    private void Update()
    {
        base.Update();
        //remove empty resources
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void addMap(Map m)
    {
        mMap = m;
    }
    public Resource getClosestResourceOfType(GameTypes.ItemType type, Entity ent)
    {
        Resource res = null;
        float mag = -1f;
        float close_mag = -1f;
        foreach ( Resource lres in mGroupMap["resources"] )
        {
            if (lres.mType == type)
            {
                mag = (ent.pointOfTouchingBounds(lres) - ent.transform.position).magnitude;
                if ( close_mag < 0 || mag < close_mag )
                {
                    close_mag = mag;
                    res = lres;
                }
            }
        }
        return res;
    }
	public List<Vector2> getFourCorners()
	{
		//returns the four 2d vectors that define the corners of the Region in game coords
		//due to a mess up, the first corner is actually negx, posz, but labelled wrongly
		Vector2 pos = new Vector2(transform.position.x, transform.position.z);
		float w2 = (mWidth+0.0f)/2.0f;
		Vector2 posx_posz = new Vector2(pos.x - w2, pos.y + w2);
		Vector2 negx_posz = new Vector2(pos.x + w2, pos.y + w2);
		Vector2 negx_negz = new Vector2(pos.x + w2, pos.y - w2);
		Vector2 posx_negz = new Vector2(pos.x - w2, pos.y - w2);
		List<Vector2> ret_list = new List<Vector2> {posx_posz, negx_posz, negx_negz, posx_negz};
		return ret_list;
	}
	public Vector2 getGridPos()
	{
		return mGridPos;
	}
    public GameObject getResourceObject()
    {
        return mResourcesObject;
    }
	public GameTypes.MapType getType()
	{
		return mType;
	}
    public float getWidth()
    {
        return mWidth;
    }
	public List<Building> getAllBuildings()
	{
		List<Building> blist = new List<Building>();
		foreach ( Town t in mContainerMap["towns"])
		{
			blist.AddRange(t.getBuildings());
		}
		return blist;
	}
	public List<Unit> getAllUnits()
	{
		List<Unit> ulist = new List<Unit>();
		foreach ( Town t in mContainerMap["towns"])
		{
			ulist.AddRange(t.getUnits());
		}
		return ulist;
	}
    public List<EntityContainer> getTowns()
    {
        return mContainerMap["towns"];
    }
	public void setGridPos(Vector2 v)
	{
		mGridPos = v;
		setPosition();
	}
    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

    private void addChildernToGroups(Transform tf, int index, int max_index)
    {
        //looks in the transforms children and adds them to the correct group
        //index tells the function how far to go into the childresn hierarchy
		//make blank groups first
		newGroup("resources");
		newContainerGroup("towns");
        if (index >= max_index)
            return;
        foreach (Transform child in tf)
        {
            //Debug.Log("child in transform");
            //if its a Resource add it to resource group
            Resource res = child.GetComponent<Resource>();
            Town tw = child.GetComponent<Town>();
            if (res)
            {
                addEntity("resources", res);
            }
            //if its a town add it to the town container group
            else if (tw)
            {
                addContainer("towns", tw);
            }
            else
            {
                addChildernToGroups(child, index++, max_index);
            }

        }
    }

    private void setPosition()
    {
        //sets the Regions position from the gridpos
        Vector3 pos = new Vector3(mGridPos[0] * mWidth, 0f, mGridPos[1] * mWidth);
		this.gameObject.transform.position = pos;
    }


}
