using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class that is an inventory that handles adding and removing EquipItems from the inventory
//and the effect they have on the associated Unit

//Notes:
// * The unit is a parent of the object containing this script.

public class EquipmentInventory : ItemGroup 
{

	//private
	private Unit mActer; 
	private List<GameTypes.EquipmentSlots> mSlots = new List<GameTypes.EquipmentSlots>();
	private Dictionary<GameTypes.EquipmentSlots, EquipItem> mSlotMap = new Dictionary<GameTypes.EquipmentSlots, EquipItem>();


	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------

	public override void Awake()
	{
		base.Awake();
		//get the parent unit
		mActer = GetComponentInParent<Unit>();
		if ( !mActer )
			Debug.LogError("mActer is null.");
		//set the default slots
		mSlots = Globals.DEFAULT_UNIT_EQUIPMENT_SLOTS;
	}
		
	public override void Update()
	{
		//remove an item if it has 0 or less
		foreach ( Item item in mItems)
		{
			if (item.getAmount() <= 0)
			{
				EquipItem ei = item as EquipItem;
				if (ei)
				{
					if ( ei.isEquipped() ) unequipItem(ei);
				}
			}
		}
		base.Update();
	}

	//unequip the item if it is an equip item and it is equipped


	//-------------------------------------------------------------------------------------------------
	// public methods
	//-------------------------------------------------------------------------------------------------

	public void clearSlot(GameTypes.EquipmentSlots slot)
	{
		//is this slot filled?
		if ( !hasSlot(slot) || !slotFilled(slot) )
		{
			Debug.Log("The inventory doesnt have this slot, or it is not already filled");
		}
		//find the item in the slot
		EquipItem ei = mSlotMap[slot];
		//make sure it's valid
		if (!ei) 
		{
			Debug.Log("The item in this slot is invalid");
		}
		//unequip the item
		unequipItem(ei);
	}

	public void equipItem(EquipItem eitem)
	{
		//Debug.Log("Equipment inventory is equipping the item.");
		//make sure the item is actually in the inventory
		Item it = getItemOfType( eitem.getType() );
		//does it have the item
		if ( !it )
		{
			Debug.LogWarning("The inventory doesnt have this item");
			return;
		}
		//is there enough of it?
		if ( it.getAmount() < 1)
		{
			Debug.LogWarning("The inventory doesnt have enough of this item");
			return;
		}
		//Is the slot for this item valid for this inventory
		GameTypes.EquipmentSlots slot = eitem.getSlot();
		if ( !hasSlot(slot) )
		{
			Debug.LogError("The inventory doesnt have a slot for this item.");
			return;
		}
		//check if the slot is already filled
		if ( slotFilled(slot) == true )
		{
			//clear the slot of the item it contains
			clearSlot(slot);
		}
		//equip the item
		eitem.equip();
		//add the attributes to the UnitStats
		_applyEffectsToUnit(eitem);
		//add it to the map
		mSlotMap[eitem.getSlot()] = eitem;
	}


	public List<EquipItem> getAvailableEquipItems()
	{
		//Items that can be equipped but are not equipped.
		List<EquipItem> alist = new List<EquipItem>();
		foreach (Item it in mItems)
		{
			EquipItem ei = it as EquipItem;
			if (!ei) continue;
			if ( ei.isEquipped() == false )
			{
				alist.Add(ei);
			}
		}
		return alist;
	}

	public List<EquipItem> getEquippedItems()
	{
		List<EquipItem> elist = new List<EquipItem>();
		foreach (Item it in mItems)
		{
			EquipItem ei = it as EquipItem;
			if (!ei) continue;
			if ( ei.isEquipped() == true )
			{
				elist.Add(ei);
			}
		}
		return elist;
	}

	public override bool hasEquipItem()
	{
		foreach ( Item item in mItems)
		{
			EquipItem ei = item as EquipItem;
			if (ei) return true;
		}
		return false;
	}

	public bool hasSlot(GameTypes.EquipmentSlots slot)
	{
		if ( mSlots.Contains(slot) ) return true;
		else return false;
	}

	public void unequipItem(EquipItem ei)
	{
		//make sure the item is actually in the inventory
		Item it = getItemOfType(ei.getType());
		//does it have the item
		if ( !it )
		{
			Debug.LogWarning("The inventory doesnt have this item.");
			return;
		}
		//make sure its in there
		if ( !mSlots.Contains(ei.getSlot()) )
		{
			Debug.LogWarning("The inventory doesnt have this item equipped.");
			return;
		}
		if ( mSlotMap[ei.getSlot()] != ei )
		{
			Debug.LogWarning("The inventory doesnt have this item equipped.");
			return;
		}
		ei.unequip();
		_removeEffectsFomrUnit(ei);
		mSlotMap.Remove(ei.getSlot());
	}

	public override void wipe()
	{
		//deletes the Items, then resets the Item list to be empty
		//doesnt remove the equipped items
		foreach(Item item in mItems)
		{
			EquipItem ei = item as EquipItem;
			if (ei)
			{
				if (ei.isEquipped()) continue;
			}
			Destroy(item.gameObject);
		}
		mItems = new List<Item>();
	}

	//-------------------------------------------------------------------------------------------------
	// private methods
	//-------------------------------------------------------------------------------------------------

	private void _applyEffectsToUnit(EquipItem eitem)
	{
		Dictionary<GameTypes.UnitStatType,float> effs = eitem.getEffectDict();
		foreach (KeyValuePair<GameTypes.UnitStatType,float> eff in effs)
		{
			mActer.addToStat(eff.Key, eff.Value);
		}
		return;
	}

	private void _removeEffectsFomrUnit(EquipItem eitem)
	{
		Dictionary<GameTypes.UnitStatType,float> effs = eitem.getEffectDict();
		foreach (KeyValuePair<GameTypes.UnitStatType,float> eff in effs)
		{
			mActer.addToStat(eff.Key, -1*eff.Value);
		}
		return;
	}

	private bool slotFilled(GameTypes.EquipmentSlots slot)
	{
		if ( mSlotMap.ContainsKey(slot) )
		{
			//the slot is filled
			return true;
		}
		else return false;
	}
	




}
