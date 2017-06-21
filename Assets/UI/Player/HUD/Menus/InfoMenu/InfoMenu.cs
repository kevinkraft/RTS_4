using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//class where info about an entity is displayed

//Notes:
// * must be placed inside a parent panel(is this true?)

public class InfoMenu : Menu
{

    //private members
    private Text mText;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Start()
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
        string text = string.Format("Name: {0} \n",ent.mName);
        EntityHP ent_hp = (EntityHP)ent;
        if ( ent_hp )
            text += string.Format("HP: {0:0} \n", ent_hp.mHP);
        mText.text = text;
    }

    public void clear()
    {
        mText.text = "No Selection";
    }

}
