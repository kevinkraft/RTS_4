using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Movement : Action
{

    //public members
    public Vector3 mDestination = Globals.InvalidPosition;
    public Unit mActer;

    //private members
    private bool mMoving, mRotating;
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
        mRotating = true;
        mMoving = false;
    }
    public override void Update()
    {
        if (mRotating) TurnToTarget();
        else if (mMoving) MakeMove();
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    /*public override Action init()
    {
        Movement move = Instantiate(ObjectManager.getAction("Movement"), mActions.transform).GetComponent<Movement>();
    }*/
    public override string print()
    {
        return string.Format("Move to: ({0:0},{1:0},{2:0})\n", mDestination.x, mDestination.y, mDestination.z);
    }
    public void setDestination(Vector3 dest)
    {
        mDestination = dest;
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
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void getActer()
    {
       mActer = GetComponentInParent<Unit>();
    }

    private void TurnToTarget()
    {
        mActer.transform.rotation = Quaternion.RotateTowards(mActer.transform.rotation, mDestinationRotation, mActer.getRotateSpeed() );
        //sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
        Quaternion inverseTargetRotation = new Quaternion(-mDestinationRotation.x, -mDestinationRotation.y, -mDestinationRotation.z, -mDestinationRotation.w);
        if ( mActer.transform.rotation == mDestinationRotation || mActer.transform.rotation == inverseTargetRotation )
        {
            mRotating = false;
            mMoving = true;
        }

    }

    private void MakeMove()
    {
        mActer.transform.position = Vector3.MoveTowards( mActer.transform.position, mDestination, Time.deltaTime * mActer.getMoveSpeed() );
        if (mActer.transform.position == mDestination)
        {
            mMoving = false;
            mComplete = true;
        }
    }




}
