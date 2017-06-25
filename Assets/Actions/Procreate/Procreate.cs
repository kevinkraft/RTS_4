using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RTS;

//class for making new units

//Notes:
// * Two units, both with procreate actions enter a building
// * One has to Male and the other is Female
// * Each one has a procreate chance, we roll dice until its sucessful
// * The females Unit.makePregnant function is called
// * The female action is marked as finished
// * The male stays and waits for other females until its queue is cleared, or 
//   if mContinuousMales is false
// * As the females does other actions her pregnancy progress fills, when
//   its full a unit with a random gender is made.
//
// * The garrison building is given by the building you click on, for now
//   * Maybe later you can tell the unit who to procreate with or to procreate in their home
// * If the Unit is ungarrisoned the action will be cancelled
// * Take the minimum precreate chance of the two units
// * mContinuousMale is true if its a user defined Procreate, if it's automatic,
//   (which isnt implemented yet) then mContinousMale will be false

public class Procreate : Action
{

    //public members
    public Unit mActer;
    public Building mGarrisonBuilding;
    public Unit mPartner = null;

    //private members
    private bool mActive = true;
    private bool mWasGarrisoned = false;
    private GameTypes.GenderTypes mActerGender = GameTypes.GenderTypes.Unknown;
    private bool mFoundPartner = false;
    private System.Random mRandomGen;
    private bool mContinuousMale = true;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
        mRandomGen = new System.Random();
    }
    public override void Start()
    {
        getActer();
        if (!mActer || !mGarrisonBuilding)
            Debug.LogError("No acter and/or garrison building set.");
    }
    public override void Update()
    {
        //Debug.Log("running procreate action");
        //is the unit already pregnant?
        if ( mActer.isPregnant() )
        {
            mComplete = true;
            return;
        }
        //is the garrison dead
        if (!mGarrisonBuilding)
        {
            //Debug.Log("garrison building is null, it must be dead.");
            mComplete = true;
            return;
        }
        //was a partner found but they left?
        if ( mFoundPartner == true )
        {
            if (mPartner == null)
            {
                //Debug.LogError("A partner was found but it's now null");
                resetPartner();
                return;
            }
            if ( mPartner.mGarrison != mGarrisonBuilding )
            {
                //partner is no longer garrisoned
                //Debug.Log("A partner was found but its not garrisoned now");
                resetPartner();
            }
        }
        //has the unit been ungarrisoned from the building?
        if (mWasGarrisoned == true && mActer.isGarrisoned() == false )
        {
            //the acter was ungarrisoned by some external function, cancel action
            //Debug.Log("the unit was ungarrisoned from the building");
            mComplete = true;
            return;
        }
        //is the unit garrisoned?
        if ( mActer.mGarrison != mGarrisonBuilding )
        {
            //go to garrison in the building
            mActer.garrisonIn(mGarrisonBuilding);
            //Debug.Log("going to garrison");
        }
        else
        {
            mWasGarrisoned = true;
        }
        //search the building for another unit with a procreate action, if this is female
        //the male will just wait until the females Procreate action marks it as done
        if ( mActerGender == GameTypes.GenderTypes.Female && mPartner == null )
        {
            //check if there are any other units first becuase it's probably faster
            if (mGarrisonBuilding.getUnitInventorySize() > 1)
            {
                foreach (Unit unit in mGarrisonBuilding.getGarrisonedUnits())
                {
                    if (unit.activeActionType() == "Procreate")
                    {
                        //are they opposite genders
                        if (unit.mGender == GameTypes.GenderTypes.Male)
                        {
                            //its male, start the dice rolling
                            //Debug.Log("found partner");
                            setPartner(unit);
                            break;
                        }
                    }
                    //Debug.Log("No partner found");
                }
            }
        }
        //does the acter have a partner already?
        if ( mPartner )
        {
            doProcreate();
        }
        
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public override string print()
    {
        return string.Format("Procreate\n");
    }
    public void setContinuousMale(bool b)
    {
        mContinuousMale = b;
    }
    public void setGarrisonBuilding(Building b)
    {
        mGarrisonBuilding = b;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void doProcreate()
    {
        //Debug.Log("Rolling dice for procreate chance.");
        int rnd = mRandomGen.Next(0, 100);

        //Debug.Log("random is " + rnd.ToString());
        //Debug.Log("precreate change is " + Mathf.Min(mActer.mProcreateChance, mPartner.mProcreateChance).ToString());

        if ( rnd < Mathf.Min(mActer.mProcreateChance, mPartner.mProcreateChance) )
        {
            //Debug.Log("procreated!");
            //sucessfully procreated
            if (mContinuousMale == false)
            {
                //set the males action as done
                Action part_act = mPartner.getActiveAction();
                Procreate part_proc = part_act as Procreate;
                if (!part_proc)
                    Debug.LogError("The partners active action is not Procreate. This shouldn't be possible.");
                part_proc.setComplete(true);
            }
            //set the females pregnant values
            mActer.makePregnant();
            //finish
            mComplete = true;
        }
    }

    

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
        mActerGender = mActer.mGender;
    }
    private void setPartner(Unit partner)
    {
        //Debug.Log("Setting partner.");
        mPartner = partner;
        mFoundPartner = true;
    }
    private void resetPartner()
    {
        //Debug.Log("resetting partner");
        mPartner = null;
        mFoundPartner = false;
    }

}
