using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class action for telling a Unit to stay in a certain place

//Notes
// * The Unit will always return to this location unless the action
//   queue is cleared
// * Inherits from Movement

public class Wait : Movement
{

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Update()
    {
        if (mActer.transform.position != mDestination)
        {
            //make sure the unit isnt garrisoned
            if (mActer.isGarrisoned())
            {
                mActer.ungarrisonSelf();
                return;
            }
            //do the move, need to check if rotation is done as if the move action is interrupted(Another Action is prepended to the Units queue),
            //the rotation will be wrong when the Unit returns to this action
            if (TurnToTarget())
                this.MakeMove();
        }
        else
        {
            return;
        }
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public override string print()
    {
        return string.Format("Wait: ({0:0},{1:0},{2:0})\n", mDestination.x, mDestination.y, mDestination.z);
    }
    //-------------------------------------------------------------------------------------------------
    // protected methods
    //-------------------------------------------------------------------------------------------------
    protected override void MakeMove()
    {
        //Debug.Log("in wait make move");
        mActer.transform.position = Vector3.MoveTowards(mActer.transform.position, mDestination, Time.deltaTime * mActer.getMoveSpeed());
        if (mActer.transform.position == mDestination)
        {
            //mMoving = false;
            this.setComplete();
        }
    }
    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

    private void setComplete()
    {
        //Debug.Log("in wait set complete");
        return;
    }


}
