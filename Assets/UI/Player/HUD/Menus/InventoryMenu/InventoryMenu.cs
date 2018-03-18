using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : Menu
{

    //private members
    private Text mText;
	private Button mEquipMenuButton;
	private EquipMenu mEquipMenu;
	private EntityAction mActer;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
        mText = GetComponentInChildren<Text>();
		mEquipMenuButton = GetComponentInChildren<Button>();
		if (!mEquipMenuButton)
		{
			Debug.LogError("Equipment button not found.");
		}
		//get the equip menu
		if (transform.parent.GetComponentInParent<GameObjectList>()) return;
		Player player = transform.root.GetComponentInChildren<Player>(); 
		if (!player)
		{
			Debug.LogError("Player not found, so can't find the equip menu.");
			return;
		}
		mEquipMenu = player.mHud.mEquipMenu;
		if (!mEquipMenu)
		{
			Debug.LogError("EquipMenu not found.");
		}
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    
	public void populate(Entity ent)
    {
        //populate the inventory for the Entity
        //EntityAction ent_act = (EntityAction)ent;
        EntityAction ent_act = ent as EntityAction;
        if (!ent_act)
		{
		    mText.text = "No Inventory";
		    return;
		}
		mActer = ent_act;
		mText.text = ent_act.printInventory();
		//should the equip button be shown?
		if ( ent_act.hasEquipItem() )
		{
			//show the button
			mEquipMenuButton.gameObject.SetActive(true);
		}
		else
		{
			mEquipMenuButton.gameObject.SetActive(false);
		}
    }

	public void showEquipMenu()
	{
		//Debug.Log("showing equip menu");
		//make sure we have an active entity, make sure its a Unit
		Unit unit = mActer as Unit;
		if (!unit)
		{
			Debug.Log("No active unit.");
			return;
		}
		//populate the equip menu
		mEquipMenu.populate(unit);
	}

    public void clear()
    {
        mText.text = "No Selection";
		mEquipMenuButton.gameObject.SetActive(false);
		mActer = null;
    }

}
