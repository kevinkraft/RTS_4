using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Attack : Action
{

    //public members
    public Unit mActer;
    public EntityHP mTarget;

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
        if (!mActer || !mTarget )
            Debug.LogError("No acter and/or target set.");
    }
    public override void Update()
    {
        //is it dead yet?
        if ( !mTarget )
        {
            Debug.Log("target is null, it must be dead.");
            mComplete = true;
            return;
        }   
        //is target in range?
        float dist = Vector3.Distance(mActer.transform.position, mTarget.transform.position);
        if (dist < mActer.mIntrRange)
        {
            doDamage();
            return;
        }
        else
        {
            //is it in range of the bounds?
            if (mActer.calculateExtentsDistance(mTarget) < mActer.mIntrRange)
            {
                doDamage();
                return;
            }
        }
        //move to the target taking the bounds into account
        Vector3 direction = mActer.pointOfTouchingBounds(mTarget);
        mActer.moveTo( direction, false);
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public override string print()
    {
        return string.Format("Attack: {0}\n", mTarget.mName);
    }
    public void setTarget(EntityHP target)
    {
        mTarget = target;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void doDamage()
    {
        mTarget.setHP( mTarget.getHP() - mActer.mAttackSpeed/10f );
    }

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
    }

}
