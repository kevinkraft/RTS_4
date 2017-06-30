using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for a Building that can be worked to produce something

//Notes:
// * Has a progress bar which the working Unit fills as they work to produce some item
// * Generally the WorkedBuildings will add an item to the buildings inventory, so this
//   can be handled in this class, and ignored in any child classes that dont need it

public class WorkedBuilding : Building
{

    //private member
    private float mProgress = 0f;
    private float mMaxProgress = 100f;
    private GameTypes.ItemType mCreateItemType = GameTypes.ItemType.Unknown;
    private float mCreateItemAmount = 5;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    private void Start()
    {
        base.Start();
        //setup the type
        setupType();
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public float displayProgress()
    {
        return 100f * mProgress / mMaxProgress;
    }
    public void doCycle(float amount=0.01f)
    {
        //is the progress bar full?
        if (mProgress >= mMaxProgress)
        {
            //yes its full, does it have space for item?
            float invsize = getInventorySize();
            float invcap = getInventory().mCapacity;
            float iamount = mCreateItemAmount;
            if (isInventoryFull())
            {
                return;
            }
            else if (invsize + iamount > invcap)
            {
                //not enough space, reduce amount
                iamount = invcap - invsize;
            }
            //so make the item
            Item item = getItemOfType(mCreateItemType);
            if (!item)
            {
                //item not in inventory, make it
                item = ObjectManager.initItem(mCreateItemType, getInventory().transform);
                item.mAmount = iamount;
                this.addItem(item);
            }
            else
            {
                //it has the item so just increase the amount
                item.mAmount += iamount;
            }
            mProgress = 0;
            return;
        }
        //to be called from the Work Action 
        mProgress += amount;

    }
    public float getProgress()
    {
        return mProgress;
    }
    public void setProgress(float p)
    {
        mProgress = p;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void setupType()
    {
        //sets up the type info in case it isnt initalised correct and so I can remember what it should be
        switch (mType)
        {
            case GameTypes.BuildingType.Farm:
                getInventory().mCapacity = 50;
                mUnitInventory.mCapacity = 2;
                mMaxProgress = 40; //default 100
                mCreateItemType = GameTypes.ItemType.Food;
                break;
            default:
                Debug.LogError(string.Format("WorkedBuilding type {0} not recognised",mType.ToString()));
                break;

        }
    }
}


