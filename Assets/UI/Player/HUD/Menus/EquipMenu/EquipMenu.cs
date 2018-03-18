
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS;

//class for a menu for user selection and display of equippped items

//Notes:
// * Will have a dropdown menu for selecting the item to equip
// * Has a text display showing what is equipped. 
// * The text display and the dropdown menu need to show the slot the item uses
//   and info about its effects.
// * Will have a remove button that removes the top item equip in the text display


public class EquipMenu : Menu
{
    //public methods
    public Dropdown mItemDropdown;
	public Text mDisplayText;
    public bool mInUse = false;

    //private members
    private Unit mActer;
	private Dictionary<EquipItem,string> mEquippedItemDict = new Dictionary<EquipItem,string>();
	private Dictionary<EquipItem,string> mAvailableItems = new Dictionary<EquipItem,string>();
	private List<string> mOptStrings = new List<string>();

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    public void clear()
    {
		mEquippedItemDict.Clear();
		mAvailableItems.Clear();
		mInUse = false;
		mActer = null;
		mItemDropdown.ClearOptions();
		mDisplayText.text = "";
		transform.SetAsFirstSibling();
		gameObject.SetActive(false);
    }

	public void closeMenu()
	{
		clear();
	}

	public void equipChosenItem()
	{
		//equip the item in the current dropdown
		string choice = mOptStrings[mItemDropdown.value];
		//Debug.Log(choice);
		//find the corresponding item
		EquipItem citem = null;
		foreach (KeyValuePair<EquipItem,string> es in mAvailableItems)
		{
			//Debug.Log(es.Value);
			if (choice == es.Value)
			{
				
				citem = es.Key;
				break;
			}
		}
		if (!citem) Debug.LogError("Chosen item not found.");
		//equip the item
		mActer.equipItem(citem);
		//update the display
		_updateEquippedItems();
	}

    public void populate(Unit acter)
    {
		//Debug.Log("Populating EquipMenu");
        gameObject.SetActive(true);
        mInUse = true;
		mActer = acter;
		//update the display with currently equipped items
		_updateEquippedItems();

        //bring to front of the sidebar
        transform.SetAsLastSibling();
    }
		
	public void removeItem()
	{
		//Removes the first item from the equipped items
		_updateEquippedItems();
		foreach (KeyValuePair<EquipItem,string> es in mEquippedItemDict)
		{
			mActer.unequipItem(es.Key);
			break;
		}
		_updateEquippedItems();
	}

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

	private List<string> _getStringsFromDict(Dictionary<EquipItem,string> edict)
	{
		List<string> ls = new List<string>();
		foreach (KeyValuePair<EquipItem,string> es in edict )
		{
			ls.Add(es.Value);
		}
		return ls;
	}

	private void _populateDisplayText()
	{
		string display = "";
		foreach (KeyValuePair<EquipItem,string> es in mEquippedItemDict)
		{
			display += es.Value+"\n";
		}
		if (display == "") display = "Nothing equipped";
		mDisplayText.text = display;
	}

	private Dictionary<EquipItem,string> _setDisplayStringsOfItems(List<EquipItem> elist)
	{
		//adds a string to display that shows the slot of an item and its attributes.
		Dictionary<EquipItem,string> edict = new Dictionary<EquipItem, string>();
		foreach (EquipItem ei in elist)
		{
			string es = ei.getDisplayString();
			//Debug.Log(es);
			edict[ei] = es;
		}
		return edict;
	}


	private void _updateEquippedItems()
    {
		//make list of currently equipped items and get their strings
		List<EquipItem> equipped_items = mActer.getEquippedItems();
		mEquippedItemDict = _setDisplayStringsOfItems(equipped_items); 
		//populate the display text of equipped items
		_populateDisplayText();

		//get avail items and their strings
		List<EquipItem> avail_items = mActer.getAvailableEquipItems();
		mAvailableItems = _setDisplayStringsOfItems(avail_items); 

		//now make a string for each entry
		mOptStrings = _getStringsFromDict(mAvailableItems);
		//Debug.Log(mAvailableItems.Count.ToString());
		mItemDropdown.ClearOptions();
		mItemDropdown.AddOptions(mOptStrings);

	}



}
