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
        string text = string.Format("Name: {0}\n", ent.mName);
        Vector3 epos = ent.transform.position;
        text += string.Format("Pos: ({0:0},{1:0})\n", epos.x, epos.z);
        EntityHP ent_hp = ent as EntityHP;
        if (ent)
        {
            //TEMP, see if the entity is in the visible  list
            foreach (Entity e in ObjectManager.getVisibleEntities())
            {
                if (ent == e)
                {
                    text += string.Format("Is Visible: True\n");
                    break;
                }
            }
        }
        if (ent_hp)
            text += string.Format("HP: {0:0} \n", ent_hp.mHP);
        EntityAction ent_act = ent as EntityAction;
		Unit unit = ent as Unit;
        if (ent_act)
        { 
			if (!unit) text += string.Format("InvCap.: {0:0} \n", ent_act.getInventory().mCapacity);
            if (ent_act.mTown)
            {
                text += string.Format("Town: {0}\n", ent_act.mTown.mName);
                //text += string.Format("StockPile: {0}\n", ent_act.mTown.mStockpile.mName);
            }
        }
        Resource res = ent as Resource;
        if (res)
            text += string.Format("Amount: {0:0} \n", res.mAmount);
		//unit
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
			text += string.Format("Gender: {0}\n", unit.getGender().ToString());
            //pregnancy
			if (unit.getGender() == GameTypes.GenderType.Female )
            {
                text += string.Format("Pregnant: {0}\n", unit.isPregnant().ToString());
                if (unit.isPregnant())
                    text += string.Format("Preg.Prog.: {0:0}\n", unit.getPregnancyProgress());
            }
			text += unit.printStats();
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
        Construction constro = ent as Construction;
        if (constro)
        {
            text += constro.printMaterialsMap();
            text += string.Format("Progress: {0:0}", constro.getProgress());
        }
        WorkedBuilding wb = ent as WorkedBuilding;
        if (wb)
        {
            text += string.Format("Progress: {0:0}\n", wb.displayProgress());
			text += string.Format("NumWorkers: {0}/{1}\n", wb.getNWorkers(), wb.getMaxWorkers() );
			WorkedProdBuilding wpb = wb as WorkedProdBuilding;
			if ( wpb )
			{
				text += wpb.printNeededItems();
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
