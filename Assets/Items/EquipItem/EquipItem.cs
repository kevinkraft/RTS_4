using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for items that can be equipped by Units into their EquipmentInventory

public class EquipItem : Item 
{

	//protected members
	protected List<GameTypes.EquipmentSlots> mEquipSlots = new List<GameTypes.EquipmentSlots>();
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
		rs += getEquipmentSlotsString();
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

	public string getEquipmentSlotsString()
	{
		string rs = "";
		foreach (GameTypes.EquipmentSlots et in mEquipSlots)
		{
			rs += "("+et.ToString()+")";
		}
		return rs;
	}

	public List<GameTypes.EquipmentSlots> getSlots()
	{
		return mEquipSlots;
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
				mEquipSlots = Globals.EQUIP_ITEM_MAGICHAT_SLOTS;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICHAT_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.MagicBoots:
				mEquipSlots = Globals.EQUIP_ITEM_MAGICBOOTS_SLOTS;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICBOOTS_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.MagicClub:
				mEquipSlots = Globals.EQUIP_ITEM_MAGICCLUB_SLOTS;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICCLUB_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.MagicSword:
				mEquipSlots = Globals.EQUIP_ITEM_MAGICSWORD_SLOTS;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICSWORD_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.MagicHammer:
				mEquipSlots = Globals.EQUIP_ITEM_MAGICHAMMER_SLOTS;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICHAMMER_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.MagicBag:
				mEquipSlots = Globals.EQUIP_ITEM_MAGICBAG_SLOTS;
				mEquipEffects = Globals.EQUIP_ITEM_MAGICBAG_EQUIP_EFFECTS;
				break;
			case GameTypes.ItemType.HandCart:
				mEquipSlots = Globals.EQUIP_ITEM_HANDCART_SLOTS;
				mEquipEffects = Globals.EQUIP_ITEM_HANDCART_EQUIP_EFFECTS;
				break;
			default:
				Weapon wep = this as Weapon;
				if (!wep)
					Debug.LogError("EquipItem type not recognised");
				break;

		}

	}



}
