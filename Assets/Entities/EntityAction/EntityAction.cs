using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class EntityAction : EntityHP
{

    //public memebers

    //protected members
    protected ActionGroup mActions;

    //private
    private ItemGroup mInventory;
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
    public ItemGroup getInventory()
    {
        return mInventory;
    }
    public Dictionary<GameTypes.ItemType, float> getInventoryDictionary()
    {
        return mInventory.getInventoryDictionary();
    }
    public float getInventorySize()
    {
        return mInventory.getSize();
    }
    public Item getItemOfType(GameTypes.ItemType type)
    {
        return mInventory.getItemOfType(type);
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
