using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class GameObjectList : MonoBehaviour
{

    //public members
    //public GameObject[] buildings;
    //public GameObject[] units;
    public List<GameObject> mActions;
    public List<GameObject> mItems;
    public List<GameObject> mMenus;
    public List<GameObject> mResources;
    public List<GameObject> mUnits;
    public List<GameObject> mBuildings;
    public List<GameObject> mConstructions;
    public List<GameObject> mMaps;
	public List<GameObject> mRegions;
	public List<GameObject> mTowns;
	//public GameObject player;

    //private members
    private static bool created = false;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
       if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            mActions = getActionComponentObjects();
            mItems = getItemComponentObjects();
            mMenus = getMenuComponentObjects();
            mResources = getResourceComponentObjects();
            mUnits = getUnitComponentObjects();
            mBuildings = getBuildingComponentObjects();
            mConstructions = getConstructionComponentObjects();
            mMaps = getMapComponentObjects();
			mRegions = getRegionComponentObjects();
			mTowns = getTownComponentObjects();
			ObjectManager.setGameObjectList(this);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public GameObject getAction(string act_name)
    {
        for (int i = 0; i < mActions.Count; i++)
        {
            Action act = mActions[i].GetComponent<Action>();
            if (act && act.mName == act_name) return mActions[i];
        }
        return null;
    }

    public GameObject getItem(string item_name)
    {
        for (int i = 0; i < mItems.Count; i++)
        {
            Item item = mItems[i].GetComponent<Item>();
            if (item && item.mName == item_name) return mItems[i];
        }
        return null;
    }

    public GameObject getMenu(string menu_name)
    {
        for (int i = 0; i < mMenus.Count; i++)
        {
            Menu menu = mMenus[i].GetComponent<Menu>();
            if (menu && menu.mName == menu_name) return mMenus[i];
        }
        return null;
    }
    public GameObject getResource(string res_name)
    {
        for (int i = 0; i < mResources.Count; i++)
        {
            Resource res = mResources[i].GetComponent<Resource>();
            if (res && res.mName == res_name) return mResources[i];
        }
        return null;
    }
    public GameObject getUnit(string unit_name)
    {
        for (int i = 0; i < mUnits.Count; i++)
        {
            Unit unit = mUnits[i].GetComponent<Unit>();
            if (unit && unit.mName == unit_name) return mUnits[i];
        }
        return null;
    }
    public GameObject getBuilding(string b_name)
    {
        for (int i = 0; i < mBuildings.Count; i++)
        {
            Building b = mBuildings[i].GetComponent<Building>();
            if (b && b.mName == b_name) return mBuildings[i];
        }
        return null;
    }
    public GameObject getConstruction(string c_name)
    {
        for (int i = 0; i < mConstructions.Count; i++)
        {
            Construction c = mConstructions[i].GetComponent<Construction>();
            if (c && c.mName == c_name) return mConstructions[i];
        }
        return null;
    }
    public GameObject getMap(string m_name)
    {
        for (int i = 0; i < mMaps.Count; i++)
        {
            Map m = mMaps[i].GetComponent<Map>();
            if (m && m.mName == m_name) return mMaps[i];
        }
        return null;
    }
	public GameObject getRegion(string r_name)
	{
		for (int i = 0; i < mRegions.Count; i++)
		{
			Region r = mRegions[i].GetComponent<Region>();
			if (r && r.mName == r_name) return mRegions[i];
		}
		return null;
	}
	public GameObject getTown(string t_name)
	{
		for (int i = 0; i < mTowns.Count; i++)
		{
			Town tw = mTowns[i].GetComponent<Town>();
			if (tw && tw.mName == t_name) return mTowns[i];
		}
		return null;
	}

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

    private List<GameObject> getActionComponentObjects()
    {
        //returns a list of child game objects which contain an action script component 
        //this allows to just drop the action prefabs as children in GameObjectList/Actions
        List<GameObject> act_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        //Debug.Log(string.Format("count children: {0}",child_objects.Length) );
        foreach ( Component cobj in child_objects )
        {
            //Debug.Log(cobj.name);
            if ( cobj.gameObject.GetComponent<Action>() != null )
                act_objects.Add( cobj.gameObject );
        }
        //Debug.Log(string.Format("count num with actions: {0}", act_objects.Count));
        return act_objects;
    }

    private List<GameObject> getItemComponentObjects()
    {
        //returns a list of child game objects which contain an item script component 
        //this allows to just drop the item prefabs as children in GameObjectList/Items
        List<GameObject> item_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Item>() != null)
                item_objects.Add(cobj.gameObject);
        }
        return item_objects;
    }

    private List<GameObject> getMenuComponentObjects()
    {
        //returns a list of child game objects which contain a menu script component 
        //this allows to just drop the menu prefabs as children in GameObjectList/Menus
        List<GameObject> menu_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Menu>() != null)
                menu_objects.Add(cobj.gameObject);
        }
        return menu_objects;
    }
    private List<GameObject> getResourceComponentObjects()
    {
        //returns a list of child game objects which contain a Resource script component 
        //this allows to just drop the Resource prefabs as children in GameObjectList/Resources
        List<GameObject> res_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Resource>() != null)
                res_objects.Add(cobj.gameObject);
        }
        return res_objects;
    }
    private List<GameObject> getUnitComponentObjects()
    {
        //returns a list of child game objects which contain a Unit script component 
        //this allows to just drop the Unit prefabs as children in GameObjectList/Units
        List<GameObject> unit_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Unit>() != null)
                unit_objects.Add(cobj.gameObject);
        }
        return unit_objects;
    }
    private List<GameObject> getBuildingComponentObjects()
    {
        //returns a list of child game objects which contain a Building script component 
        //this allows to just drop the Building prefabs as children in GameObjectList/Buildings
        List<GameObject> b_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Building>() != null)
                b_objects.Add(cobj.gameObject);
        }
        return b_objects;
    }
    private List<GameObject> getConstructionComponentObjects()
    {
        //returns a list of child game objects which contain a Construction script component 
        //this allows to just drop the Construction prefabs as children in GameObjectList/Constructions
        List<GameObject> c_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Construction>() != null)
                c_objects.Add(cobj.gameObject);
        }
        return c_objects;
    }
    private List<GameObject> getMapComponentObjects()
    {
        //returns a list of child game objects which contain a Map script component 
        //this allows to just drop the Map prefabs as children in GameObjectList/Maps
        List<GameObject> c_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Map>() != null)
                c_objects.Add(cobj.gameObject);
        }
        return c_objects;
    }
	private List<GameObject> getRegionComponentObjects()
	{
		//returns a list of child game objects which contain a Region script component 
		//this allows to just drop the Region prefabs as children in GameObjectList/Regions
		List<GameObject> c_objects = new List<GameObject>();
		Component[] child_objects = this.GetComponentsInChildren<Transform>();
		foreach (Component cobj in child_objects)
		{
			if (cobj.gameObject.GetComponent<Region>() != null)
				c_objects.Add(cobj.gameObject);
		}
		return c_objects;
	}
	private List<GameObject> getTownComponentObjects()
	{
		//returns a list of child game objects which contain a Town script component 
		//this allows to just drop the Town prefabs as children in GameObjectList/Towns
		List<GameObject> c_objects = new List<GameObject>();
		Component[] child_objects = this.GetComponentsInChildren<Transform>();
		foreach (Component cobj in child_objects)
		{
			if (cobj.gameObject.GetComponent<Town>() != null)
				c_objects.Add(cobj.gameObject);
		}
		return c_objects;
	}




}
