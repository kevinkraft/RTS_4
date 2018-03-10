using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for the eat action which reduces unit hunger and makes then get food

//Notes:
// * This is an automatic action and can't be given by the user
// * The Units do the Eat Action when their hunger is high
// * Eating is instant, there is no Eat speed.
// * Also the Units dont need to go to the Sockpile to check if its empty,
//   they know whats in the stockpile without being in range of it.
// * If the units inventory is full it drops everything into the stockpile
// * If the units inventory and the stockpile are full the unit drops its inventory,
//   which means the items are deleted.

public class Eat : Action
{

    //public members
    public Unit mActer;

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
        if (!mActer)
            Debug.LogError("No acter set.");
    }
    public override void Update()
    {
        //is the units hunger almost zero? Then stop
        isFinished();
        /*//does the unit have food in its inventory
        Item item = mActer.getItemOfType(GameTypes.ItemType.Food);
        if ( item )
        {
            //unit has some food
            Debug.Log("unit has some food");
            doEat();
            return;
        }
        //unit doesnt have food, does the stockpile have food?
        Building sp = mActer.mTown.mStockpile;
        if ( sp.getItemOfType(GameTypes.ItemType.Food) )
        {
            //if the units inventory is full it wont have space for food, so needs to drop its inventory
            if ( mActer.isInventoryFull() )
            {
                //if the stockpile is full?
                if ( sp.isInventoryFull() )
                {
                    Debug.Log("Dropping inventory");
                    mActer.dropInventory();
                }
                else
                {
                    Debug.Log("Dumping inventory");
                    mActer.dumpInventory();
                }
            }
            //stockpile has food, get 5 food
            mActer.exchangeWith(sp, GameTypes.ItemType.Food, -5f);
        }
        else
        {
            Debug.Log("Stockpile has no food, Need to implement Region so that units can search for food sources.");
        }*/

        if ( mActer.getResource(GameTypes.ItemType.Food,5f) )
        {
            //unit has some food
            //Debug.Log("unit has some food");
            doEat();
            return;
        }


    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public override string print()
    {
        return "Eat\n";
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void doEat()
    {
        //Debug.Log("Eating the food in the inventory");
        //how much food is needed?
        float amount = mActer.getHunger() / Globals.UNIT_FOOD_HUNGER_VALUE;
        //does the unit have enough food?
        Item item = mActer.getItemOfType(GameTypes.ItemType.Food);
        if (!item)
        {
            Debug.LogError("Can't find Item of type Food. This shouldn't be possible");
        }
        if ( amount > item.mAmount)
        {
            amount = item.mAmount;
        }
        //remove from the item and remove from the hunger
        item.mAmount -= amount;
        mActer.setHunger(mActer.getHunger() - amount * Globals.UNIT_FOOD_HUNGER_VALUE );
        isFinished();
        return;
    }

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
    }

    private bool isFinished()
    {
        if (mActer.getHunger() < 0.01f)
        {
            mComplete = true;
            return true;
        }
        return false;
    }
}
