using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for the action that moves a new unit to another town

public class Travel : Action
{

    //private members
	[SerializeField]
	private Building mDestTownHall;
	private Unit mActer;
	private WorldManager mWM;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        getActer();
		if (!mActer)
            Debug.LogError("No acter is defined.");
		if ( mDestTownHall==null )
			Debug.LogWarning("No destination is defined.");
		mComplete = false;
		mWM = transform.root.GetComponentInChildren<WorldManager>();
    }

    public override void Update()
    {
		//Debug.Log("Running the travel action.");
		//if this is the units current town, then don't do anything
		if ( mActer.getTown() == mDestTownHall.getTown() )
		{
			mComplete = true;
			return;
		}
		//How far is the unit from the town?
		//If they're in the same region then no need to get food
		//ask the world manager
		Region creg = mWM.getRegionFromPosition(new Vector3(mActer.transform.position.x, 0, mActer.transform.position.z));
		Region nreg = mDestTownHall.getTown().getRegion();
		if ( creg == nreg )
		{
			//they're in the same region, so do the change over
			_changeActersTown();
			return;
		}
		//not in the same region, unit will need food. Fills the inventory
		if ( mActer.getResource(GameTypes.ItemType.Food, -1) )
		{
			//unit has filled the inventory with food
			_changeActersTown();
		}
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    public override string print()
    {
		if ( mDestTownHall == null ) return "Travel: NULL\n";
		return string.Format("Travel to: "+mDestTownHall.getTown().mName+"\n");
    }

	public void setDestination(Building dest)
    {
		//Debug.Log("setting the travel destination");
		//Debug.Log(dest.mName  );
		if ( dest.getType() != GameTypes.BuildingType.TownHall )
		{
			Debug.Log("Can only travel to a TownHall building.");
		}
		mDestTownHall = dest;
		//Debug.Log(mDestTownHall.mName);
    }

    public override void setComplete(bool b)
    {
        mComplete = b;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

	private void _changeActersTown()
	{
		//is the unit in range of the dest building?
		if ( mActer.moveToInRange(mDestTownHall) == true )
		{
			mActer.changeTown( mDestTownHall.getTown() );
		}
	}

    private void getActer()
    {
       mActer = GetComponentInParent<Unit>();
    }




}
