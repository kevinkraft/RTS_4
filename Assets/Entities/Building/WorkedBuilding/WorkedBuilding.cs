using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for a Building that can be worked to produce something

//Notes:
// * Has a progress bar which the working Unit fills as they work to produce some item
// * WorkedBuilding takes in no items, need to make a child class that takes in items
// * Has a max number of workers
//   * But it doesnt need to know who the workers are

public class WorkedBuilding : Building
{

    //private member
    private float mProgress = 0f;
    private float mMaxProgress = 100f;
    private GameTypes.ItemType mCreateItemType = GameTypes.ItemType.Unknown;
    private int mCreateItemAmount = 5;
	private UnitInventory mWorkers;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
	public override void Awake()
	{
		//dont call building.Awake as it only knows about a single UnitInventory, there are two here
		base.Awake();
		//get the UnitInventory
		List<UnitInventory> uinvl = new List<UnitInventory>(GetComponentsInChildren<UnitInventory>());
		foreach( UnitInventory uinv in uinvl )
		{
			if ( uinv.mName == "UnitInventory" )
				mUnitInventory = uinv;
			else if ( uinv.mName == "WorkerContainer" )
				mWorkers = uinv;
			else
				Debug.LogError("UnitInventory "+uinv.mName+" is not recognised.");
		}
		if ( uinvl.Count == 0 )
			Debug.LogError("UnitInventory is null.");
	}
    private void Start()
    {
        base.Start();
        //setup the type
        setupType();

    }
    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
	public void addWorker(Unit worker)
	{
		if ( getMaxWorkers() > getNWorkers() )
			mWorkers.addUnit(worker);
	}

	public float displayProgress()
    {
        return 100f * mProgress / mMaxProgress;
    }
    
    public void doCycle(float prog = 0.1f)
    {
        //is the progress bar full?
        if (mProgress >= mMaxProgress)
        {
            //yes its full, does it have space for item?
            int invsize = getInventorySize();
            int invcap = getInventory().mCapacity;
            int iamount = mCreateItemAmount;
            if (isInventoryFull())
            {
                return;
            }
            else if (invsize + iamount > invcap)
            {
                //not enough space, reduce amount
                iamount = invcap - invsize;
            }
            //so make the item
            Item item = getItemOfType(mCreateItemType);
            if (!item)
            {
                //item not in inventory, make it
                item = ObjectManager.initItem(mCreateItemType, getInventory().transform);
                item.mAmount = iamount;
                this.addItem(item);
            }
            else
            {
                //it has the item so just increase the amount
                item.mAmount += iamount;
            }
            mProgress = 0;
            return;
        }
        //to be called from the Work Action 
        mProgress += prog;
    }

    public GameTypes.ItemType getCreateItemType()
    {
        return mCreateItemType;
    }

	public int getMaxWorkers()
	{
		return mWorkers.mCapacity;
	}

	public int getNWorkers()
	{
		return mWorkers.getSize();
	}

    public float getProgress()
    {
        return mProgress;
    }

	public bool needsWorkers()
	{
		if ( getNWorkers() < getMaxWorkers() )
			return true;
		else
			return false;
	}

	public void removeWorker(Unit worker)
	{
		mWorkers.removeUnit(worker);
	}

    public void setProgress(float p)
    {
        mProgress = p;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void setupType()
    {
        //sets up the type info in case it isnt initalised correct and so I can remember what it should be
        switch (mType)
        {
            case GameTypes.BuildingType.Farm:
                getInventory().mCapacity = 50; //default 50
                mUnitInventory.mCapacity = 2;
                mMaxProgress = 100; //default 100
                mCreateItemType = GameTypes.ItemType.Food;
				mWorkers.mCapacity = 2;
                break;
            default:
                Debug.LogError(string.Format("WorkedBuilding type {0} not recognised",mType.ToString()));
                break;

        }
    }
}


