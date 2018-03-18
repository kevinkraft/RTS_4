using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for Units entering and staying in Buildings


public class Garrison : Action
{

    //public members
    public Unit mActer;
    public Building mTarget;

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
        //is it dead?
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
            doGarrison();
            return;
        }
        else
        {
            //is it in range of the bounds?
			if (mActer.calculateExtentsDistance(mTarget) < mActer.getIntrRange())
            {
                doGarrison();
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
        return string.Format("Garrison: {0}\n", mTarget.mName);
    }
    public void setTarget(Building target)
    {
        mTarget = target;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void doGarrison()
    {
        //does the target have enough space in unit inventory?
        if (mTarget.getUnitInventorySize() == mTarget.getUnitInventory().mCapacity)
        {
            Debug.Log("Not enough space to Garrison this Unit.");
            mComplete = true;
        }
        else
        {
            mActer.garrison(mTarget);
            mComplete = true;
        }
    }

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
    }

}
