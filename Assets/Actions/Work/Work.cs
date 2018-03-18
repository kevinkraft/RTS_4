using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Work : Action
{

    //public members
    public Unit mActer;
    public WorkedBuilding mTarget;

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
        if (!mActer || !mTarget)
            Debug.LogError("No acter and/or target set.");
		//can the work building hve any more workers?
		if ( mTarget.getMaxWorkers() <= mTarget.getNWorkers() )
		{
			//no space for more workers
			mComplete = true;
			return;
		}
		else
		{
			mTarget.addWorker( mActer );
		}
    }

    public override void Update()
    {
        //is it dead yet?
        if (!mTarget)
        {
            Debug.Log("target is null, it must be dead.");
            mComplete = true;
            return;
        }
        //is target in range?
        float dist = Vector3.Distance(mActer.transform.position, mTarget.transform.position);
		if (dist < mActer.getIntrRange())
        {
            doWork();
            return;
        }
        else
        {
            //is it in range of the bounds?
			if (mActer.calculateExtentsDistance(mTarget) < mActer.getIntrRange())
            {
                doWork();
                return;
            }
        }
        //move to the target taking the bounds into account
        Vector3 direction = mActer.pointOfTouchingBounds(mTarget);
        mActer.moveTo(direction, false);
    }

	public virtual void OnDestroy()
	{
		//remove this worker from the WorkedBuilding worker container
		if ( mTarget )
			mTarget.removeWorker(mActer);
	}


    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public override string print()
    {
        return string.Format("Work: {0}\n", mTarget.mName);
    }

    public void setTarget(WorkedBuilding target)
    {
        mTarget = target;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    
	private void doWork()
    {
		/*if (mActer.isInventoryFull())
		{
			//make sure there is nothing else filling up the inventory
			if ( mActer.makeInventorySpaceFor(mTarget.getCreateItemType()) )
				return;
			mActer.returnToStockpile(mTarget.getCreateItemType());
		}

        //is the worked buildings inventory more than half full?
        if ( mTarget.getInventorySize() >= mTarget.getInventory().mCapacity/2f )
        {
            //worked buildings inventory is more than half full, fill inventory
            //if the inventory is already full, return to stockpile

			//if (mActer.isInventoryFull())
            //{
            //    //this will dump the inventory if it full of some other item
            //    
            //}
            //else
            //{
				//clear the unit inventory of everything but the required item

            int invspace = mActer.getInventoryFreeSpace();
			//dump the inventory after ...
			//mActer.dumpInventory();
			//after making an exchange with the building
			mActer.exchangeWith(mTarget, mTarget.getCreateItemType(), -1*invspace);

            //}
            return;
        }*/

		//does the unit not have space for the created items?
		if ( mActer.getInventoryFreeSpace() < mTarget.getCreateItemAmount() )
		{
			//yes it doesn't have space
			mActer.dumpInventory();
			return;
		}

		//does the unit have the create item amount in their inventory already?
		Item it = mActer.getItemOfType(mTarget.getCreateItemType());
		if (it)
		{
			if ( it.getAmount() >= mTarget.getCreateItemAmount())
			{
				//return to stockpile
				mActer.returnToStockpile(mTarget.getCreateItemType());
				return;
			}
		}
				
		//is the inventory of the target half full, then take items from the target
		if ( mTarget.getInventorySize() >= mTarget.getInventory().mCapacity/2f )
		{
			int invspace = mActer.getInventoryFreeSpace();
			mActer.exchangeWith(mTarget, mTarget.getCreateItemType(), -1*invspace);
			return;
		}

		//is it a WorkedProdBuilding and does it need materials?
		WorkedProdBuilding wpb = mTarget as WorkedProdBuilding;
		if ( wpb )
		{
			//it is a wpb, does it need materials?
			Debug.Log("Its a WorkedProdBuilding");
			KeyValuePair<GameTypes.ItemType,int> needed = wpb.neededItem();
			if ( !needed.Equals(new KeyValuePair<GameTypes.ItemType,int>()) )
			{
				//need to get items
				Debug.Log("It needed resources");
				getItemsForProduction(needed);
				return;
			}
			else
			{
				Debug.Log("It doesnt need resources");
				wpb.doCycle( mActer.getWorkSpeed()/10f );
				return;
			}
		}
		else
		{
			mTarget.doCycle( mActer.getWorkSpeed()/10f );
			return;
		}
    }

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
    }


	private void getItemsForProduction(KeyValuePair<GameTypes.ItemType,int> needed)
	{
		mActer.retrieveItemsAndGiveToTarget(needed, mTarget);
	}


}
