using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for towns which for now just hold entities and tells them where the stockpile is

//Notes:
// * Has two groups, one for Building and another for Units


public class Town : EntityContainer
{

    //public members
    public Building mStockpile;
    public Building mMainBuilding;

    //private members
    private Region mRegion;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    private void Awake()
    {
        base.Awake();
        //get the main building
        List<Building> bs = new List<Building>(GetComponentsInChildren<Building>());
        foreach (Building b in bs)
        {
            if (b.mType == GameTypes.BuildingType.MainHut)
            {
                mMainBuilding = b;
                break;
            }
        }
        if (!mMainBuilding)
            Debug.LogError("No Main building found for Town");

    }
    void Start()
    {
        //search for entities in children and put them in the right group
        foreach (Transform child in transform)
        {
            //if its a unit add it to unit group
            Unit unit = child.GetComponent<Unit>();
            if ( unit )
            {
                addEntity("units",unit);
            }
            //if its a Building add it to building group
            Building b = child.GetComponent<Building>();
            if (b)
            {
                addEntity("buildings", b);
            }
        }

        //get the Region, which should be the parent of the Towns object
        mRegion = transform.parent.parent.GetComponent<Region>();
        if (!mRegion && !GetComponentInParent<GameObjectList>())
            Debug.LogError("No Region found as parent of Town");
  
    }

    private void Update()
    {
        base.Update();
        //check if there are no Buildings or Units
        if (checkEmpty())
            return;
        //check the stockpile isnt dead
        if (!mStockpile)
        {
            //stockpile is dead, set as another building
            Building sp = getBuildingOfType(GameTypes.BuildingType.Stockpile);
            if (!sp)
            {
                //there is no stockpile, set it to any building
                mStockpile = mGroupMap["buildings"][0] as Building;
            }
        }

    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void addEntity(string group_name, Building b)
    {
        base.addEntity(group_name, b);
        b.transform.SetParent(this.transform);
        if (!b)
            return;
        //if its a stockpile building and there isn't one already then add it as the Town stockpile
        if (mStockpile)
        {
            if (mStockpile.mType != GameTypes.BuildingType.Stockpile && b.mType == GameTypes.BuildingType.Stockpile)
                mStockpile = b;
        }
        else if (!mStockpile)
        {
            mStockpile = b;
        }

    }
    public void addEntity(string group_name, Unit u)
    {
        base.addEntity(group_name, u);
        u.transform.SetParent(this.transform);
    }
    public Building getBuildingOfType(GameTypes.BuildingType type)
    {
        Building res = null;
        foreach (Building b in mGroupMap["buildings"])
        {
            if (b.mType == type)
            {
                return b;
            }
        }
        return null;
    }
	public List<Building> getBuildings()
	{
		List<Building> blist = new List<Building>();
		foreach ( Entity ent in mGroupMap["buildings"] )
		{
			Building b = ent as Building;
			if ( b )
				blist.Add(b);
		}
		return blist;
	}
    public Region getRegion()
    {
        return mRegion;
    }
	public List<Unit> getUnits()
	{
		List<Unit> ulist = new List<Unit>();
		foreach ( Entity ent in mGroupMap["units"] )
		{
			Unit u = ent as Unit;
			if ( u )
				ulist.Add(u);
		}
		return ulist;
	}

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    /*private void removeDeadEntities()
    {
        //clear buildings that are dead
        List<int> to_remove = new List<int>();
        int i = 0;
        foreach (Building b in mGroupMap["buildings"])
        {
            if (!b)
                to_remove.Add(i);
            i++;
        }
        foreach (int j in to_remove)
        {
            mGroupMap["buildings"].RemoveAt(j);
        }
    }*/


    private bool checkEmpty()
    {
        if (mGroupMap["buildings"].Count == 0)
        {
            //no buildings
            if (mGroupMap["units"].Count == 0)
            {
                //no units
                deleteSelf();
                return true;
            }
            else
            {
                //no buildings but there are units, need to find another town for units
                List<EntityContainer> towns = mRegion.getTowns();
                if ( towns.Count <= 1 )
                {
                    //this is the only town, for now just kill the units
                    foreach (Entity ent in mGroupMap["units"])
                    {
                        ent.setDead(true);
                    }
                }
                else
                {
                    //there are other towns, assign units to that town
                    foreach(EntityContainer ec in towns)
                    {
                        if (ec != this)
                        {
                            foreach (Entity ent in mGroupMap["units"])
                            {
                                Unit unit = ent as Unit;
                                Town tw = ec as Town;
                                if (!unit || !tw)
                                    Debug.LogError("Moving Units from dead Town to new Town, this control flow shouldn't be possible.");
                                else
                                {
                                    unit.mTown = tw;
                                    tw.addEntity("units", unit);
                                }
                            }
                        }
                    }
                }
                deleteSelf();
                return true;
            }
        }
        return false;
    }
    private void deleteSelf()
    {
        mRegion.removeContainer("towns", this);
        Destroy(this.gameObject);
    }
    private void setStockpile(Building sp)
    {
        mStockpile = sp;
    }


}
