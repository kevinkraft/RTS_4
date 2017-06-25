using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : Menu
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
        //populate the info for the Entity
        string text = "";
        //EntityAction ent_act = (EntityAction)ent;
        EntityAction ent_act = ent as EntityAction;
        if (!ent_act)
            text = "No Actions";
        else
            text = ent_act.printActions();
        mText.text = text;
    }

    public void clear()
    {
        mText.text = "No Selection";
    }
}
