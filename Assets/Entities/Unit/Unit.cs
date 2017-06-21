using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Unit : EntityAction {

    //public members
    public float mIntrRange;
    public float mAttackSpeed;
    public float mExchangeSpeed;

    //private members
    //private float mSpeed = UNIT_SPEED;
    //private float mRotateSpeed = UNIT_ROTATE_SPEED;
    public float mMoveSpeed;
    public float mRotateSpeed;

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public float getMoveSpeed()
    {
        return mMoveSpeed;
    }
    public float getRotateSpeed()
    {
        return mRotateSpeed;
    }

    /*public override void mouseClick(GameObject hitObject, Vector3 hitPoint)
    {
        //process a left mouse click while this entity is selected by the player
        Debug.Log("in unit, this function does nothing yet");
    }*/

    public void moveTo( Vector3 destination, bool clear=true )
    {
        //make a new move instance as a child of the action group object
        //Movement move = Instantiate(ObjectManager.getAction("Movement"), mActions.transform).GetComponent<Movement>();
        Movement move = ObjectManager.initMove(mActions.transform);
        //set destination
        move.setDestination(destination);
        //add the action to the action group component
        if ( clear )
            mActions.clearAddAction(move);
        else
            mActions.prependAction(move);
    }

    public void moveTo(Entity ent, bool clear = true)
    {
        //make a new move instance as a child of the action group object
        //Movement move = Instantiate(ObjectManager.getAction("Movement"), mActions.transform).GetComponent<Movement>();
        Movement move = ObjectManager.initMove(mActions.transform);
        //set destination
        move.setDestination(ent);
        //add the action to the action group component
        if (clear)
            mActions.clearAddAction(move);
        else
            mActions.prependAction(move);
    }

    public void attack(EntityHP target)
    {
        //make a new attack instance as a child of the action group object
        Attack att = Instantiate(ObjectManager.getAction("Attack"), mActions.transform).GetComponent<Attack>();
        //set target
        att.setTarget(target);
        //add the action to the action group component
        mActions.clearAddAction(att);

    }
   

}
