using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for the movement and rotation action

//Notes:
// * The Wait action inherits from this and overrides the setComplete function

public class Movement : Action
{

    //public members
    public Vector3 mDestination = Globals.InvalidPosition;
    public Unit mActer;

    //private members
    //private bool mMoving, mRotating;
    private Quaternion mDestinationRotation;

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
        if (!mActer || mDestination == Globals.InvalidPosition)
            Debug.Log("Movement::Start: No acter or destination is defined.");
        mDestinationRotation = Quaternion.LookRotation(mDestination - mActer.transform.position);
        mComplete = false;
        //mRotating = true;
        //mMoving = false;
    }
    public override void Update()
    {
        //make sure the unit isnt garrisoned
        if (mActer.isGarrisoned())
        {
            mActer.ungarrisonSelf();
            /*//need to reset the rotation of the unit as its wrong
            Debug.Log("resetting rotation");
            mActer.gameObject.transform.localEulerAngles = new Vector3(0f, mActer.gameObject.transform.localEulerAngles.y, 0f);*/
            return;
        }
        //do the move, need to check if rotation is done as if the move action is interrupted(Another Action is prepended to the Units queue),
        //the rotation will be wrong when the Unit returns to this action
        if ( TurnToTarget() )
            MakeMove();
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    public override string print()
    {
        return string.Format("Move to: ({0:0},{1:0},{2:0})\n", mDestination.x, mDestination.y, mDestination.z);
    }
    public void setDestination(Vector3 dest)
    {
        mDestination = dest;
    }
    public override void setComplete(bool b)
    {
        //Debug.Log("in move set complete");
        mComplete = b;
    }
    public void setDestination(Entity ent)
    {
        //set the destination given the physical size of the target object and acter
        if (!ent)
            Debug.LogError("Object target is null.");
        getActer();
        mDestination = mActer.pointOfTouchingBounds(ent);
    }

    //-------------------------------------------------------------------------------------------------
    // protected methods
    //-------------------------------------------------------------------------------------------------


    protected virtual void MakeMove()
    {
        mActer.transform.position = Vector3.MoveTowards(mActer.transform.position, mDestination, Time.deltaTime * mActer.getMoveSpeed());
        if (mActer.transform.position == mDestination)
        {
            //mMoving = false;
            setComplete(true);
        }
    }

    protected virtual bool TurnToTarget()
    {
        //reset the destination rotation incase the Movement was interrupted and the Unit is now coming from a different angle
        mDestinationRotation = Quaternion.LookRotation(mDestination - mActer.transform.position);
        //do the rotation
        mActer.transform.rotation = Quaternion.RotateTowards(mActer.transform.rotation, mDestinationRotation, mActer.getRotateSpeed());
        //sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
        Quaternion inverseTargetRotation = new Quaternion(-mDestinationRotation.x, -mDestinationRotation.y, -mDestinationRotation.z, -mDestinationRotation.w);
        if (mActer.transform.rotation == mDestinationRotation || mActer.transform.rotation == inverseTargetRotation)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void getActer()
    {
       mActer = GetComponentInParent<Unit>();
    }




}
