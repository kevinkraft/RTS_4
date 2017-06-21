using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//Class for the Exhange action

//Notes:
// * The float amount in the ExchangeList can also be negative for moving items from the target to the acter 

public class Exchange : Action
{

    //public members
    public Unit mActer;
    public EntityAction mTarget;
    public Dictionary<GameTypes.ItemType, float> mExchangeList = new Dictionary<GameTypes.ItemType, float>();

    //private members
    private GameTypes.ItemType mDoneType;
    private bool mEmptyItem;
    private bool mWaitForMenu = false;
    private Player mPlayer;
    private ExchangeMenu mExchangeMenu;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
        //get the player and the ExchangeMenu
        mPlayer = transform.root.GetComponentInChildren<Player>();
        if ( !mPlayer && !GetComponentInParent<GameObjectList>() )
            Debug.LogError("Didn't find player.");
    }
    public override void Start()
    {
        getActer();
        if (!mActer || !mTarget )
        {
            Debug.Log("No acter and/or target set, or there is nothing in the Exchange list.");
            mComplete = true;
            return;
        }
        //get the exchange menu
        mExchangeMenu = mPlayer.mHud.mExchangeMenu;
        if (!mExchangeMenu)
            Debug.Log("Didn't find the ExchangeMenu");
        //do we need to get a menu selection?
        if (mExchangeList.Count == 0)
        {
            mWaitForMenu = true;
            mExchangeMenu.populate(mActer, mTarget);
        }
    }
    public override void Update()
    {
        //has the menu been cancelled
        if ( mWaitForMenu && mExchangeMenu.isCancelled() )
        {
            mComplete = true;
            return;
        }
        //has the exchange list been populated?
        if ( mWaitForMenu )
        {
            Debug.Log("waiting for menu.");
            if (mExchangeMenu.hasMadeSelection() == true)
            {
                Debug.Log("menu selection made.");
                mExchangeList = mExchangeMenu.getExchangeList();
                mWaitForMenu = false;
                mExchangeMenu.clear();
                Debug.Log(string.Format("size of exhacnge list {0}.",mExchangeList.Count));
            }
            return;
        }
        //is it finished
        if (mExchangeList.Count == 0 && mWaitForMenu == false)
        {
            Debug.Log("exchange action is finished");
            mComplete = true;
            return;
        }
        //is target in range?
        float dist = Vector3.Distance(mActer.transform.position, mTarget.transform.position);
        if (dist < mActer.mIntrRange)
        {
            doExchange();
            return;
        }
        else
        {
            //is it in range of the bounds?
            if (mActer.calculateExtentsDistance(mTarget) < mActer.mIntrRange)
            {
                doExchange();
                return;
            }
        }
        //move to the target taking the bounds into account
        Debug.Log("moving to target for exchange action");
        Vector3 direction = mActer.pointOfTouchingBounds(mTarget);
        mActer.moveTo(direction, false);
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public override string print()
    {
        return "Exchange\n";
    }
    public void setTarget(EntityAction target)
    {
        mTarget = target;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private bool checkAndExchangeItems(EntityAction giver, EntityAction getter, GameTypes.ItemType type, float cycle_amount)
    //checks everything and moves some of one item from the giver to the getter
    {
        //does the acter have the item?
        Item act_item = giver.getItemOfType(type);
        if (!act_item)
        {
            //acter doesnt have the item, so cant do exchange
            setRemoveItem(type);
            Debug.Log("acter doesnt have the item");
            return false;
        }
        //does the acter have enough of the item
        if (act_item.mAmount < cycle_amount)
        {
            cycle_amount = act_item.mAmount;
            Debug.Log("acter doesnt have enough of the item, adjusting amount");
        }
        //does the target have the item to receive?
        Item tar_item = getter.getItemOfType(type);
        if (!tar_item)
        {
            //target doesnt have the item, make one
            tar_item = ObjectManager.initItem(type, getter.getInventory().transform);
            getter.addItem(tar_item);
            Debug.Log("target doesnt have th item");
        }
        //does the target have enough space for this item
        if (getter.getInventorySize() + cycle_amount > getter.getInventory().mCapacity)
        {
            //target doesnt have space
            setRemoveItem(type);
            return false;
        }
        else
        {
            //target has space, add and remove the amount
            tar_item.mAmount = tar_item.mAmount + cycle_amount;
            act_item.mAmount = act_item.mAmount - cycle_amount;
            if (mExchangeList[type] >= 0)
            {
                mExchangeList[type] = mExchangeList[type] - cycle_amount;
            }
            else if (mExchangeList[type] < 0)
            {
                mExchangeList[type] = mExchangeList[type] + cycle_amount;
            }
        }
        return true;
    }

    private void doExchange()
    {
        //Debug.Log("doing the exchange");
        //Debug.Log("in do exchange size of the exchange list is "+mExchangeList.Count.ToString());
        mDoneType = GameTypes.ItemType.Unknown;
        mEmptyItem = false;
        //loop over the type,amount pairs in the ExchangeList
        List<GameTypes.ItemType> tlist = new List<GameTypes.ItemType>(mExchangeList.Keys);
        foreach (GameTypes.ItemType itype in tlist)
        {
            float ival = mExchangeList[itype];
            //check if the amount is almost zero
            if (Mathf.Abs(ival) < 0.01)
            {
                //if its zero remove from the ExchangeList
                setRemoveItem(itype);
                continue;
            }
            //split it into separate functions for positive and negative amounts
            //positive amount, move items from acter to target
            float cycle_amount = mActer.mExchangeSpeed / 10f;
            if (ival > 0)
            {
                //Debug.Log("positive value");
                if (checkAndExchangeItems(mActer, mTarget, itype, cycle_amount) == false)
                    continue;
                else
                    break;
            }
            //negative amount, move items from target to acter
            else if (ival < 0)
            {
                Debug.Log("negative value");
                if (checkAndExchangeItems(mTarget, mActer, itype, cycle_amount) == false)
                    continue;
                else
                    break;
            }

        }
        //remove the empty entries in ExchangeList
        if (mEmptyItem && mDoneType != GameTypes.ItemType.Unknown)
            mExchangeList.Remove(mDoneType);

    }

    private void getActer()
    {
        mActer = GetComponentInParent<Unit>();
    }

    private void setRemoveItem(GameTypes.ItemType type)
    {
        //sets the members to remove an element from the list
        mDoneType = type;
        mEmptyItem = true;

    }
}
