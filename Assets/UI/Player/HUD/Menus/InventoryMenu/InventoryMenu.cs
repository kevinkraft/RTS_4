using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : Menu
{

    //private members
    private Text mText;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
        mText = GetComponentInChildren<Text>();
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void populate(Entity ent)
    {
        //populate the inventory for the Entity
        //EntityAction ent_act = (EntityAction)ent;
        EntityAction ent_act = ent as EntityAction;
        if (ent_act)
            mText.text = ent_act.printInventory();
        else
            mText.text = "No Inventory";
    }

    public void clear()
    {
        mText.text = "No Selection";
    }

}
