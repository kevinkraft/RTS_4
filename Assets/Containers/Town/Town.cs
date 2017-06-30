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


    //private Region* mRegion;

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
            //if its a Building add it to unit group
            Building b = child.GetComponent<Building>();
            if (b)
            {
                addEntity("buildings", b);
            }
        }

        //Debug.Log("number of groups is " + mGroupMap.Count);
        //Debug.Log("number of units is " + mGroupMap["units"].Count);
        //Debug.Log("number of buildings is " + mGroupMap["buildings"].Count);

    }

    private void Update()
    {
        base.Update();
        //Debug.Log(string.Format("{0}", mGroupMap["units"].Count));

    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void addEntity(string group_name, Building b)
    {
        base.addEntity(group_name, b);
        //if its a stockpile building and there isn't one already then add it as the Town stockpile
        if (b && mStockpile.mType != GameTypes.BuildingType.Stockpile)
        {
            mStockpile = b;
        }
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void setStockpile(Building sp)
    {
        mStockpile = sp;
    }


}
