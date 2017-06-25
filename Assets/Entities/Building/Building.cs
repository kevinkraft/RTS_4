using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Building : EntityAction
{

    //public members
    public GameTypes.BuildingType mType = GameTypes.BuildingType.Unknown;

    //private
    private UnitInventory mUnitInventory;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
        //get the UnitInventory
        mUnitInventory = GetComponentInChildren<UnitInventory>();
        if (!mUnitInventory)
            Debug.LogError("UnitInventory is null.");
    }

    private void Start()
    {
        base.Start();
        //setup the type
        setupType();
    }

    public override void Update()
    {
        if (!isInstantiated)
            return;
        base.Update();
        //if its dead, ungarrison everyone
        if (mDead)
        {
            ungarrison();
        }

    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void addUnit(Unit unit)
    {
        //this function adds a unit to the UnitInventory but doesn't change anything for the unit
        //see Unit.garrison(Building b)
        mUnitInventory.addUnit(unit);
    }
    public int getUnitInventorySize()
    {
        return mUnitInventory.getSize();
    }
    public UnitInventory getUnitInventory()
    {
        return mUnitInventory;
    }
    public List<Unit> getGarrisonedUnits()
    {
        return mUnitInventory.getUnits();
    }
    public void removeUnit(Unit unit)
    {
        //DO NOT CALL THIS FROM UNIT
        //remove unit from the garrison and deals with settng the units garrison values
        mUnitInventory.removeUnit(unit);
        unit.ungarrisonByBuilding();
    }
    public void removeUnitByUnit(Unit unit)
    {
        //remove unit from the garrison and doesnt deal with the units garrsion info
        mUnitInventory.removeUnit(unit);
    }
    public void ungarrison()
    {
        //ungarrisons all units
        foreach ( Unit unit in mUnitInventory.getGroup("units") )
        {
            unit.ungarrisonByBuilding();
        }
        mUnitInventory.clear();
    }


    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

    private void setupType()
    {
        //sets up the type info in case it isnt initalised correct and so I can remember what it should be
        switch (mType)
        {
            case GameTypes.BuildingType.Unknown:
                Debug.LogError("Unknown Building type");
                break;
            case GameTypes.BuildingType.MainHut:
                getInventory().mCapacity = 100;
                mUnitInventory.mCapacity = 10;
                break;
            case GameTypes.BuildingType.Stockpile:
                getInventory().mCapacity = 1000;
                mUnitInventory.mCapacity = 2;
                break;
            default:
                Debug.LogError("Building type not recognised");
                break;

        }
    }

}
