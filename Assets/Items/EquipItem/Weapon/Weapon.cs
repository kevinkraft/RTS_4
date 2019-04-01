using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for weapon equip items

public class Weapon : EquipItem 
{

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

	private void setupType()
	{
		//sets up the type info in case it isnt initalised correct and so I can remember what it should be
		switch (getType())
		{
		case GameTypes.ItemType.StoneSpear:
			mEquipSlots = Globals.EQUIP_ITEM_STONESPEAR_SLOTS;
			mEquipEffects = Globals.EQUIP_ITEM_STONESPEAR_EQUIP_EFFECTS;
			break;
		default:
			Weapon wep = this as Weapon;
			if (!wep)
				Debug.LogError("EquipItem type not recognised");
			break;

		}
	}

}
