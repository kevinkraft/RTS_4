using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for units

//Notes:
// * When the unit is Garrisoned, just put it below the map (no this is bad, turn off its object)
// * Does setting the game object when garrisoning make it null in other scripts?
//   * this doesnt work because then the units actions arent cycled, i.e. the garrison action isnt deleted
//   * turn off the model object instead?

public class Unit : EntityAction
{

    //public members
    public float mIntrRange;
    public float mAttackSpeed;
    public float mExchangeSpeed;
    public float mMoveSpeed;
    public float mRotateSpeed;
    public float mProcreateChance;
    public GameTypes.GenderTypes mGender;

    //private members
    //private float mSpeed = UNIT_SPEED;
    //private float mRotateSpeed = UNIT_ROTATE_SPEED;
    private bool mGarrisoned = false;
    public Building mGarrison;
    private bool mPregnant = false;
    private float mPregnancyProgress = 0f;
    private System.Random mRandomGen = new System.Random();
    private float mHunger = 0f;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Update()
    {
        if (!isInstantiated)
            return;
        base.Update();
        //update pregnancy
        updatePregnancy();
        updateHunger();
    }


    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void attack(EntityHP target)
    {
        //make a new attack instance as a child of the action group object
        Attack att = Instantiate(ObjectManager.getAction("Attack"), mActions.transform).GetComponent<Attack>();
        //set target
        att.setTarget(target);
        //add the action to the action group component
        mActions.clearAddAction(att);
    }
    public void exchangeWith(EntityAction target, GameTypes.ItemType type, float amount)
    {
        Exchange ex = ObjectManager.initExchange(transform);
        ex.setTarget(target);
        ex.setExchangeItem(type, amount);
        mActions.prependAction(ex);
    }
    public void garrison(Building b)
    {
        b.addUnit(this);
        mGarrisoned = true;
        if (!b)
            Debug.LogError("Building to garrison in is null");
        mGarrison = b;
        //transform.position = new Vector3(transform.position.x, -10f, transform.position.z);
        //gameObject.SetActive(false);
        setModelActive(false);
    }
    public void garrisonIn(Building b)
    {
        //make a new garrison instance as a child of the action group object
        Garrison gar = ObjectManager.initGarrison(transform);
        //set building to garrison in
        gar.setTarget(b);
        //add the action to the action group component
        mActions.prependAction(gar);
    }
    public Building getGarrison()
    {
        return mGarrison;
    }
    public float getHunger()
    {
        return mHunger;
    }
    public float getMoveSpeed()
    {
        return mMoveSpeed;
    }
    public float getPregnancyProgress()
    {
        return mPregnancyProgress;
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
    public bool isGarrisoned()
    {
        return mGarrisoned;
    }
    public bool isPregnant()
    {
        return mPregnant;
    }
    public void makePregnant()
    {
        if ( mGender == GameTypes.GenderTypes.Female && mPregnant == false)
        {
            mPregnant = true;
            mPregnancyProgress = 0f;
        }
    }
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
    public void setHunger(float h)
    {
        mHunger = h;
    }
    public void ungarrisonSelf()
    {
        //DO NOT CALL THIS FROM BUILDING
        if (!mGarrison)
            Debug.LogError("Garrison Building not set. unit name "+mName);
        mGarrison.removeUnitByUnit(this);
        /*mGarrisoned = false;
        mGarrison = null;
        //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        //gameObject.SetActive(true);
        setModelActive(true);*/
        ungarrison();
    }
    public void ungarrisonByBuilding()
    {
        //DO NOT CALL THIS FROM UNIT
        /*mGarrisoned = false;
        mGarrison = null;
        //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        //gameObject.SetActive(true);
        setModelActive(true);
        */
        ungarrison();
    }
    public void waitAt(Vector3 destination, bool clear = true)
    {
        //make a new wait instance as a child of the action group object
        Wait wt = ObjectManager.initWait(mActions.transform);
        //set destination
        wt.setDestination(destination);
        //add the action to the action group component
        if (clear)
            mActions.clearAddAction(wt);
        else
            mActions.prependAction(wt);
    }

    public void waitAt(Entity ent, bool clear = true)
    {
        //make a new wait instance as a child of the action group object
        Wait wt = ObjectManager.initWait(mActions.transform);
        //set destination
        wt.setDestination(ent);
        //add the action to the action group component
        if (clear)
            mActions.clearAddAction(wt);
        else
            mActions.prependAction(wt);
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void ungarrison()
    {
        mGarrisoned = false;
        mGarrison = null;
        setModelActive(true);
    }
    private void updateHunger()
    {
        mHunger += Globals.UNIT_HUNGER_CYCLE_INCREASE;
        if (mHunger >= 100f)
        {
            mHunger = 100f;
            mHP -= Globals.UNIT_HUNGRY_DAMAGE_TAKEN;
        }
        if ( mHunger > 80f && !mActions.hasActionOfType("Eat") )
        {
            //go and eat
            Debug.Log("Adding Eat Action");
            Eat eat = ObjectManager.initEat(mActions.transform);
            mActions.prependAction(eat);
        }
    }

    private void updatePregnancy()
    {
        if (mPregnant == true && mGender == GameTypes.GenderTypes.Female)
        {
            Debug.Log("processing pregnancy");
            if (mPregnancyProgress >= 100)
            {
                //make new unit instance with random gender
                GameTypes.GenderTypes[] genders = new[] { GameTypes.GenderTypes.Female, GameTypes.GenderTypes.Male };
                GameTypes.GenderTypes gender = genders[mRandomGen.Next(genders.Length)];
                //gender is always the same, so change the generator
                int temp = mRandomGen.Next(0,10);
                Unit unit = ObjectManager.initUnit(transform.position, gender, mTown);
                //send new units to the main hut, so the user knows they are new
                unit.waitAt(mTown.mMainBuilding);
                //reset pregnancy
                mPregnant = false;
                mPregnancyProgress = 0f;
            }
            else
            {
                mPregnancyProgress += Globals.UNIT_PREGNANCY_CYCLE_PROGRESS;
            }
        }
    }


}
