using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS;

//class where info about an entity is displayed

//Notes:
// * must be placed inside a parent panel(is this true?)

public class InfoMenu : Menu
{

    //public members
    public Button mButton1;

    //private members
    private Text mText;


    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
        mText = GetComponentInChildren<Text>();
        /*mButton1 = GetComponentInChildren<Button>();
        if (!mButton1)
            Debug.LogError("Can't find Button 1");*/
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void populate(Entity ent)
    {
        //clear menu 
        clear();
        //populate the info for the Entity
        string text = string.Format("Name: {0} \n", ent.mName);
        EntityHP ent_hp = ent as EntityHP;
        if (ent_hp)
            text += string.Format("HP: {0:0} \n", ent_hp.mHP);
        EntityAction ent_act = ent as EntityAction;
        if (ent_act)
            text += string.Format("InvCap.: {0:0} \n", ent_act.getInventory().mCapacity);
        Resource res = ent as Resource;
        if (res)
            text += string.Format("Amount: {0:0} \n", res.mAmount);
        Unit unit = ent as Unit;
        if (unit)
        {
            //hunger
            text += string.Format("Hunger: {0:0}\n", unit.getHunger());
            //garrison status
            if (unit.isGarrisoned())
            {
                text += string.Format("Garrison: {0}\n", unit.getGarrison().mName);
                setupUnitUngarrisonButton(unit, mButton1);
            }
            //gender
            text += string.Format("Gender: {0}\n", unit.mGender.ToString());
            //pregnancy
            if (unit.mGender == GameTypes.GenderTypes.Female )
            {
                text += string.Format("Pregnant: {0}\n", unit.isPregnant().ToString());
                if (unit.isPregnant())
                    text += string.Format("Preg.Prog.: {0:0}\n", unit.getPregnancyProgress());
            }
        }
        Building build = ent as Building;
        if (build)
        {
            text += string.Format("GarrisonCap.: {0}\n", build.getUnitInventory().mCapacity);
            text += string.Format("Garrisoned: {0}\n", build.getUnitInventorySize());
            if (build.getUnitInventorySize() > 0)
            {
                //make the ungarrisonall button
                setupBuildingUngarrisonButton(build, mButton1);
            }
        }
        mText.text = text;
    }
    public void clear()
    {
        mText.text = "No Selection";
        mButton1.onClick.RemoveAllListeners();
        mButton1.gameObject.SetActive(false);
    }
    public void turnOffButton1()
    {
        mButton1.gameObject.SetActive(false);
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void setupBuildingUngarrisonButton(Building b, Button but)
    {
        Debug.Log("making building ungarrison button");
        Text text = but.GetComponentInChildren<Text>();
        if (!text)
            Debug.LogError("can't find ungarrison building button text");
        text.text = "Ungarrison";
        but.gameObject.SetActive(true);
        but.onClick.AddListener(() => { b.ungarrison(); });
        but.onClick.AddListener(() => { turnOffButton1(); });
    }
    private void setupUnitUngarrisonButton( Unit unit, Button but )
    {
        Debug.Log("making unit ungarrison button");
        Text text = but.GetComponentInChildren<Text>();
        if (!text)
            Debug.LogError("can't find button text");
        text.text = "Ungarrison";
        but.gameObject.SetActive(true);
        but.onClick.AddListener(() => { unit.ungarrisonSelf(); });
        but.onClick.AddListener(() => { turnOffButton1(); });
    }




}
