using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for items that can be equipped by Units into their EquipmentInventory

public class EquipItem : Item 
{

	//protected members
	protected GameTypes.EquipmentSlots mEquipSlot = GameTypes.EquipmentSlots.Unknown;
	protected Dictionary<GameTypes.UnitStatType,float> mEquipEffects = new Dictionary<GameTypes.UnitStatType,float>();

	//private
	[SerializeField]
	private bool mEquipped = false;


	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------

	private void Start () 
	{
		setupType();
	}
		
	//-------------------------------------------------------------------------------------------------
	// public methods
	//-------------------------------------------------------------------------------------------------

	public void equip()
	{
		if (mEquipped == true) Debug.LogWarning("This item is already equipped.");
		mEquipped = true;
	}

	public string getDisplayString()
	{
		//a string with the name, slot and attributes of the item
		string rs = mName;
		rs += "("+mEquipSlot.ToString()+")";
		foreach (KeyValuePair<GameTypes.UnitStatType,float> ap in mEquipEffects)
		{
			rs += "("+Globals.UNIT_STATS_DISPLAY_TEXT[ap.Key] + ap.Value.ToString()+")"; 
		}
		return rs;
	}

	public Dictionary<GameTypes.UnitStatType,float> getEffectDict()
	{
		return mEquipEffects;
	}

	public GameTypes.EquipmentSlots getSlot()
	{
		return mEquipSlot;
	}

	public bool isEquipped()
	{
		return mEquipped;
	}

	public void unequip()
	{
		if (mEquipped == false) Debug.LogWarning("This item is not equipped.");
		mEquipped = false;
	}

	//-------------------------------------------------------------------------------------------------
	// Private methods
	//-------------------------------------------------------------------------------------------------

	private void setupType()
	{
		//sets the attributes of the various equip items
		switch (getType())
		{
			case GameTypes.ItemType.Unknown:
				Debug.LogError("Unknown Item type.");
				break;
			case GameTypes.ItemType.MagicHat:
				mEquipSlot = Globals.EQUIP_ITEM_MAGICHAT_SLOT;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICHAT_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.MagicBoots:
				mEquipSlot = Globals.EQUIP_ITEM_MAGICBOOTS_SLOT;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICBOOTS_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.MagicClub:
				mEquipSlot = Globals.EQUIP_ITEM_MAGICCLUB_SLOT;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICCLUB_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.MagicSword:
				mEquipSlot = Globals.EQUIP_ITEM_MAGICSWORD_SLOT;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICSWORD_EQUIP_EFFECTS;
				break;
			default:
				Weapon wep = this as Weapon;
				if (!wep)
					Debug.LogError("EquipItem type not recognised");
				break;

		}

	}



}
