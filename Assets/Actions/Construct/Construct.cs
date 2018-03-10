using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Construct : Action
{

    //public members
    public Unit mActer;
    public Construction mTarget;

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
    }

    public override void Update()
    {
        //is it dead yet?
        if (!mTarget)
        {
            Debug.Log("Target is null, it must be dead, or the Construction is finished.");
            mComplete = true;
            return;
        }
        //does the target need resources
       KeyValuePair<GameTypes.ItemType,int> res_needed = mTarget.neededResource();
        if ( !res_needed.Equals(new KeyValuePair<GameTypes.ItemType,int>()) )
        {
            //resources are needed, send Unit to get them
            if (mActer.getResource(res_needed.Key, res_needed.Value))
            {
                //unit has the resource, give the right amount to the Construction
                Item item = mActer.getItemOfType(res_needed.Key);
                int amount = 0;
                if (!item)
                    Debug.LogError("The acter doesn't have the Item. This shouldn't be possible.");
                //if the unit has more than is needed then set to the needed amount
                if ( item.mAmount > res_needed.Value )
                {
                    amount = res_needed.Value;
                }
                else
                {
                    amount = item.mAmount;
                }
                //give items to the Construction
                mActer.exchangeWith(mTarget, res_needed.Key, amount);
                return;
            }
            else
            {
                //unit has been given actions to go and get the resource
                return;
            }
        }
        //no resources are needed, go to the Construction and start building progress
        //is target in range?
        float dist = Vector3.Distance(mActer.transform.position, mTarget.transform.position);
        if (dist < mActer.mIntrRange)
        {
            doConstruct();
            return;
        }
        else
        {
            //is it in range of the bounds?
            if (mActer.calculateExtentsDistance(mTarget) < mActer.mIntrRange)
            {
                doConstruct();
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
        return string.Format("Construct: {0}\n", mTarget.mName);
    }

    public void setTarget(Construction target)
    {
        mTarget = target;
    }

    public void setTarget(Vector3 loc)
    {
        Debug.Log("Not implemented yet.");
        mComplete = true;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void doConstruct()
    {
        Debug.Log("Doing the construct.");
        mTarget.setProgress(mTarget.getProgress() + mActer.mConstructSpeed / 50f);
        if (mTarget.getProgress() >= 100f)
            mComplete = true;
    }

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
    }
}
