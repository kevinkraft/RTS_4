using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class EntityAction : EntityHP
{

    //public memebers

    //protected members
    protected ActionGroup mActions;
	protected ItemGroup mInventory;

    //private
    //private float mIntrRange = UNIT_INTERACTION_RANGE;
    //private float mAttackDamage = UNIT_ATTACK_DAMAGE;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
        mActions = GetComponentInChildren<ActionGroup>();
        if ( !mActions )
            Debug.LogError("ActionGroup is null.");
        mInventory = GetComponentInChildren<ItemGroup>();
        if (!mActions)
            Debug.LogError("ItemGroup is null.");
    }

    public override void Start()
    {
     base.Start();
    }

    public override void Update()
    {
        if (!isInstantiated)
            return;
        base.Update();
        /*foreach (var act in mActions.mActions)
        {
            Debug.Log(string.Format("EntityAction::Update: Length of mActions = {0}", mActions.size()));
            Debug.Log("EntityAction::Update: " + act.mName);
        }*/
        if (mActions.size() >= 1)
        {
            if (mActions.mActions[0].isComplete() == true)
            {
                Action act = mActions.mActions[0];
                mActions.remove(0);
                Destroy(act.gameObject);
            }
            else
            {
                if (mActions.mActions[0].enabled == false)
                    mActions.mActions[0].enabled = true;
            }
        }
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void addAction(Action act, string option ="")
    {
        switch (option)
        {
            case "":
                mActions.clearAddAction(act);
                break;
            case "append":
                mActions.appendAction(act);
                break;
            case "prepend":
                mActions.prependAction(act);
                break;
            default:
                Debug.LogError(string.Format("Option {0} not recognised",option));
                break;
        }
    }

    public bool addItem(Item item)
    {
        return mInventory.addItem(item);
    }

    public virtual void dropInventory()
    {
        //drop the units inventory on the ground
        //To Do: Implement Items that are visible on the map
        //Currently I'm just deleting them
        mInventory.wipe();
    }

    public string activeActionType()
    {
        if (mActions.size() > 0)
        {
            return mActions.mActions[0].mName;
        }
        return "";
    }

    public Action getActiveAction()
    {
        if (mActions.size() > 0)
        {
            return mActions.mActions[0];
        }
        return null;
    }

    public ItemGroup getInventory()
    {
        return mInventory;
    }

	public int getInventoryCapacity()
	{
		return mInventory.mCapacity;
	}

    public Dictionary<GameTypes.ItemType, int> getInventoryDictionary()
    {
        return mInventory.getInventoryDictionary();
    }

    public int getInventorySize()
    {
        return mInventory.getSize();
    }

    public int getInventoryFreeSpace()
    {
        return mInventory.getFreeSpace();
    }

    public Item getItemOfType(GameTypes.ItemType type)
    {
        return mInventory.getItemOfType(type);
    }

	public virtual bool hasEquipItem()
	{
		return mInventory.hasEquipItem();
	}


	public bool isIdle()
	{
		if ( mActions.size() == 0 )
			return true;
		else
			return false;
	}

    public bool isInventoryFull()
    {
        return mInventory.isFull();
    }

    public string printActions()
    {
        return mActions.print();
    }

    public string printInventory()
    {
        return mInventory.print();
    }

    public override void mouseClick(GameObject hitObject, Vector3 hitPoint)
    {
        //process a left mouse click while this entity is selected by the player
        Debug.Log("This function does nothing.");
    }


    

}
