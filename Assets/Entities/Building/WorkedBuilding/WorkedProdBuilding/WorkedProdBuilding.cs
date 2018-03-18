using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for a Building that can be worked to produce an Item, that requires input items

public class WorkedProdBuilding : WorkedBuilding 
{

	//private members
	private Dictionary<GameTypes.ItemType,int> mNeededItemsDict = new Dictionary<GameTypes.ItemType,int>();
	private Dictionary<GameTypes.ItemType,int> mMaterialsNeededToMakeItem = new Dictionary<GameTypes.ItemType,int>();

	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------
	public void awake()
	{
		base.Awake();
	}

	void Start () 
	{
		base.Start();
		//setup the type
		setupType();
	}

	private void Update()
	{
		if (!isInstantiated)
			return;
		base.Update();
		//move items from inventory to materials map if the materials map is not empty
		if (mNeededItemsDict.Count != 0)
			moveItems();
	}

	//-------------------------------------------------------------------------------------------------
	// public methods
	//-------------------------------------------------------------------------------------------------

	public override void doCycle(float prog)
	{
		//do we have all the necessary items?
		if( mNeededItemsDict.Count != 0 )
		{
			//items are needed, check the inventory for the items
			moveItems();
			return;
			//the working unit is responsible for getting the necessary items
		}
		else
		{
			if ( checkProgressAndMakeItem(prog)==true )
			{
				//item made, reset needed items, progress already reset in above function
				mNeededItemsDict = new Dictionary<GameTypes.ItemType,int>(mMaterialsNeededToMakeItem);
			}
		}

	}

	public KeyValuePair<GameTypes.ItemType, int> neededItem()
	{
		//for now the items are obtained one by one, even if they are only small numbers
		if (mNeededItemsDict.Count == 0)
			return new KeyValuePair<GameTypes.ItemType, int>();
		else
		{
			foreach (KeyValuePair<GameTypes.ItemType,int> pair in mNeededItemsDict)
			{
				return pair;
			}
		}
		return new KeyValuePair<GameTypes.ItemType, int>();
	}

	public string printNeededItems()
	{
		//add entries for the items needed
		string rtext = "";
		foreach (KeyValuePair<GameTypes.ItemType, int> pair in mNeededItemsDict)
		{
			rtext += string.Format("{0} Needed : {1}\n",pair.Key.ToString(),pair.Value);
		}
		return rtext;
	}

	//-------------------------------------------------------------------------------------------------
	// private methods
	//-------------------------------------------------------------------------------------------------

	private void moveItems()
	{
		//take items from the inventory and remove the amounts from the needed materials
		List<GameTypes.ItemType> rtypes = new List<GameTypes.ItemType>();
		foreach ( Item item in getInventory().mItems )
		{
			if ( mNeededItemsDict.ContainsKey(item.getType()) )
			{
				mNeededItemsDict[item.getType()] = mNeededItemsDict[item.getType()] - item.getAmount();
				if ( mNeededItemsDict[item.getType()] <= 0 )
				{
					rtypes.Add(item.getType());
				}
				item.setAmount(0);
			}
		}
		//delete the empty entries in material map
		foreach (GameTypes.ItemType type in rtypes)
		{
			if (mNeededItemsDict.ContainsKey(type))
				mNeededItemsDict.Remove(type);
		}
	}

	private void setupType()
	{
		//sets up the type info in case it isnt initalised correct and so I can remember what it should be
		switch (mType)
		{
		case GameTypes.BuildingType.SpearWorkshop:
			getInventory().mCapacity = 10;
			mUnitInventory.mCapacity = 2;
			mCreateItemType = Globals.SPEARWORKSHOP_PRODUCTION_ITEM;
			mWorkers.mCapacity = 1;
			mCreateItemAmount = 1;
			mMaterialsNeededToMakeItem = Globals.STONESPEAR_PRODUCTION_MATERIALS;
			break;
		default:
			Debug.LogError(string.Format("WorkedProdBuilding type {0} not recognised",mType.ToString()));
			break;

		}
		mNeededItemsDict = new Dictionary<GameTypes.ItemType,int>(mMaterialsNeededToMakeItem);
	}

}
