using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for containing units garrisoned in buildings

//Notes:
// * Has a maximum unit capacity
// * The Units do not become children of the UnitInventory when they are added

public class UnitInventory : EntityContainer
{

    //public 
    public int mCapacity;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
        newGroup("units");
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void addUnit(Unit unit)
    {
        //Debug.Log("capacity " + mCapacity.ToString());
        //Debug.Log("size group " + mGroupMap["units"].Count.ToString());
        if (mGroupMap["units"].Count == mCapacity)
        {
            Debug.Log("Can't add Unit to UnitInventory as UnitInventory is full.");
        }
        else
        {
            addEntity("units", unit);
        }
    }
    public void clear()
    {
        //clears the unit group, but doesnt get rid of it
        clearGroup("units");
    }
    public int getSize()
    {
        return mGroupMap["units"].Count;
    }
    public List<Unit> getUnits()
    {
        List<Unit> units = new List<Unit>();
        foreach(Entity ent in mGroupMap["units"])
        {
            Unit unit = ent as Unit;
            if (unit)
                units.Add(unit);
        }
        return units;
    }
    public void removeUnit(Unit unit)
    {
        removeEntity("units", unit);
    }



}
