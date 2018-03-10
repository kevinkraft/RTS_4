using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (dist < mActer.mIntrRange)
        {
            doWork();
            return;
        }
        else
        {
            //is it in range of the bounds?
            if (mActer.calculateExtentsDistance(mTarget) < mActer.mIntrRange)
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
		if (mActer.isInventoryFull())
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
            mActer.exchangeWith(mTarget, mTarget.getCreateItemType(), -1*invspace);
            //}
            return;
        }
        mTarget.doCycle();
    }

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
    }

}
