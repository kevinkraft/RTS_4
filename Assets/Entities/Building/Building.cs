using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Building : EntityAction
{

    //public members
    public GameTypes.BuildingType mType = GameTypes.BuildingType.Unknown;

    //protected members
    protected UnitInventory mUnitInventory;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
		base.Awake();
		//get the UnitInventory
		List<UnitInventory> uinvl = new List<UnitInventory>(GetComponentsInChildren<UnitInventory>());
		foreach( UnitInventory uinv in uinvl )
		{
			if ( uinv.mName == "UnitInventory" )
				mUnitInventory = uinv;
		}
		if ( uinvl.Count == 0 )
			Debug.LogError("UnitInventory is null.");
    }

	public override void Start()
    {
		//Debug.Log("in building start: "+mType.ToString());
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
		
	public GameTypes.BuildingType getType()
	{
		return mType;
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
			case GameTypes.BuildingType.TownHall:
                getInventory().mCapacity = 200;
                mUnitInventory.mCapacity = 30;
                break;
			case GameTypes.BuildingType.House:
				getInventory().mCapacity = 15;
				mUnitInventory.mCapacity = 7;
				break;
            case GameTypes.BuildingType.Stockpile:
                getInventory().mCapacity = 1000;
                mUnitInventory.mCapacity = 2;
                break;
            default:
                WorkedBuilding wb = (WorkedBuilding)this;
                if (!wb)
                    Debug.LogError("Building type not recognised");
                break;

        }
    }

}
