using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for collecting resource action

//Notes:
// * This action continues until it is cancelled
// * Unit collects items then returns to its Town stockpile

//To Do:
// * Maybe you should add something that make the unit dump their inventory if
//   its already very full, because its a waste of time going back and forward to the
//   stockpile if they cant carry much

public class Collect : Action
{
    
    //public members
    public Unit mActer;
    public Resource mTarget;
    public GameTypes.ItemType mType;

    ////private members
    //privtae bool mReturnToStockpile = false;
    //private bool mCollectedOnPreviousCycle = false;
    private int mCyclesToSkip = 3;
    private int mCycleCounter = 0;
    private bool mOneRound = false; //if true the unit will stop when its inventory is full
	private float mProgress = 0f;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Start()
    {
        getActer();
        if (!mActer || !mTarget)
            Debug.LogError("No acter and/or target set.");
    }

    public override void Update()
    {
        /*//the collect speed is too fast so only collect every second cycle
        if ( mCollectedOnPreviousCycle )
        {
            mCollectedOnPreviousCycle = true;
            return;
        }*/
         //the collect speed is too fast so only collect every nth cycle
        if ( mCycleCounter % mCyclesToSkip == 0 )
        {
            if (mCycleCounter == mCyclesToSkip)
                mCycleCounter = 1;
            else
                mCycleCounter++;
            return;
        }
        //is the resource empty (i.e. dead)?
        if (!mTarget)
        {
            //try to find another instance of this resource
            Resource res = mActer.mTown.getRegion().getClosestResourceOfType(mType, mActer);
            if (!res)
            {
                //no other resources
                mComplete = true;
                return;
            }
            else
            {
                mTarget = res;
            }
        }
        //is target in range?
        float dist = Vector3.Distance(mActer.transform.position, mTarget.transform.position);
		if (dist < mActer.getIntrRange())
        {
            doCollect();
            return;
        }
        else
        {
            //is it in range of the bounds?
			if (mActer.calculateExtentsDistance(mTarget) < mActer.getIntrRange())
            {
                doCollect();
                return;
            }
        }
        //move to the target taking the bounds into account
        Vector3 direction = mActer.pointOfTouchingBounds(mTarget);
        mActer.moveTo(direction, false);

    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public override string print()
    {
        return string.Format( "Collect: {0}\n", mTarget.mType.ToString() );
    }

    public void setOneRound(bool b)
    {
        mOneRound = b;
    }

    public void setTarget(Resource target)
    {
        mTarget = target;
        mType = mTarget.mType;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void doCollect()
    {
        //if the units inventory is full, send it back to the stockpile
        if (mActer.isInventoryFull())
        {
            //acters inventory is full, return to stockpile or stop
            //check if the Unit contains some of the target type, if not, its inventory was probably full before
            //starting the action, and if its only doing one round this will cause an infinite loop
            //Debug.Log("acters Inventory is full, returning to stockpile");
            Item item = mActer.getItemOfType(mTarget.mType);
            if (mOneRound && item)
            {
                mComplete = true;
                return;
            }
            else if (mOneRound && !item)
            {
                mActer.dropOrDumpInventory();
            }
            else
            {
                mActer.returnToStockpile(mTarget.mType);
                return;
            }
        }
		//update the progress and continue if its a full cycle
		int amount = 0;
		if (mProgress >= 100)
		{
			amount = 1;
			mProgress = 0f;
		}
		else
		{
			mProgress += mActer.getExchangeSpeed() / 0.5f; //50 cycles to add one
			return;
		}
        //int amount = mActer.mExchangeSpeed / 49f; //default exchange speed is too fast
        ////cant collect 0.01 or less
        //if (amount <= 0)
        //{
        //    amount = 0.011f;
        //}
        //does the resource have enough to give?
        if ( amount > mTarget.mAmount )
        {
            amount = mTarget.mAmount;
        }
        //does the unit have the item to recieve
        Item act_item = mActer.getItemOfType(mTarget.mType);
        if (!act_item)
        {
            //unit doesnt have the item, so make it
            act_item = ObjectManager.initItem(mTarget.mType, mActer.getInventory().transform);
            mActer.addItem(act_item);
            //Debug.Log("acter doesnt have the item, making an instance");
            act_item = mActer.getItemOfType(mTarget.mType);
        }
        //does the acter have enough space to recieve item?
        int invsize = mActer.getInventorySize();
        int invcap = mActer.getInventory().mCapacity;
        if (  invsize + amount > invcap )
        {
            //acter doesnt have emough space
            //Debug.Log("lowering amount becuase unit doesnt have enough space");
            amount = invcap - invsize;
        }
        //set the amount and remove from the resource
        //Debug.Log("moving the amount from resource to unit");
		act_item.setAmount( act_item.getAmount() + amount );
        mTarget.mAmount -= amount;
    }

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
    }

    /*private void returnToStockpile()
    {
        Item item = mActer.getItemOfType(mTarget.mType); 
        if (!item)
        {
            //this means the unit doesnt contain any of this item, which means the inventory is already full
            //so dump the inventory
            //Debug.Log("No item of type, the inventory must already be full. Dumping inventory");
            mActer.dumpInventory();
            return;
        }
        Debug.Log(mActer.mName);
        mActer.exchangeWith( mActer.getStockpile(), mTarget.mType, item.mAmount);
    }*/

}
