using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Construction : EntityAction
{

    //public members
    public GameTypes.BuildingType mType = GameTypes.BuildingType.Unknown;

    //private members
    private Dictionary<GameTypes.ItemType, int> mMaterialsMap = new Dictionary<GameTypes.ItemType, int>();
    private float mProgress = 0;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    private void Start()
    {
        base.Start();
        //setup the type
        if (isInstantiated)
            setupType();
    }

    private void Update()
    {
        if (!isInstantiated)
            return;
        base.Update();
        //move items from inventory to materials map if the materials map is not empty
        if (mMaterialsMap.Count != 0)
            moveMaterials();
        //is the progress bar full?
        //Debug.Log("CHECKING PROGRESS BAR");
        if ( mProgress >= 100 )
        {
            makeBuilding();
        }
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public float getProgress()
    {
        return mProgress;
    }

    public KeyValuePair<GameTypes.ItemType, int> neededResource()
    {
        if (mMaterialsMap.Count == 0)
            return new KeyValuePair<GameTypes.ItemType, int>();
        else
        {
            foreach (KeyValuePair<GameTypes.ItemType,int> pair in mMaterialsMap)
            {
                return pair;
            }
        }
        return new KeyValuePair<GameTypes.ItemType, int>();
    }

    public string printMaterialsMap()
    {
        //add entries for the items needed
        string rtext = "";
        foreach (KeyValuePair<GameTypes.ItemType, int> pair in mMaterialsMap)
        {
            rtext += string.Format("{0} Needed : {1}\n",pair.Key.ToString(),pair.Value);
        }
        return rtext;
    }

    public void setProgress(float p)
    {
        mProgress = p;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void makeBuilding()
    {
        //make building
        if (mDead == true)
            return;
        //Debug.Log("The building is made here.");
		//if we are building a TownHall, we want to make a new Town
		Building b = null;
		if ( mType == GameTypes.BuildingType.TownHall )
		{
			//need to determine the region that this new town is in by asking the world manager
			WorldManager wm = this.transform.root.GetComponentInChildren<WorldManager>();
			Region reg = wm.getRegionFromPosition(new Vector3(transform.position.x, 0, transform.position.y) );
			if (!reg) 
			{
				Debug.LogError("Unable to find the region corresponding to the new Town.");
			}
			//make a new town
			Town ntown = ObjectManager.initTown(reg);
			b = ObjectManager.initBuilding(gameObject.transform.position, mType, ntown);
		}
		else
		{
			b = ObjectManager.initBuilding(gameObject.transform.position, mType, mTown);
		}
        
        //now remove the construction site
        mDead = true;
    }

    private void moveMaterials()
    {
        //take items from the inventory and remove the amounts from the material map
        List<GameTypes.ItemType> rtypes = new List<GameTypes.ItemType>();
        foreach ( Item item in getInventory().mItems )
        {
			if ( mMaterialsMap.ContainsKey(item.getType()) )
            {
                //Debug.Log(string.Format("Item amount {0}", item.mAmount));
                //Debug.Log("Decreasing item and materials map amounts");
                //Debug.Log(string.Format("{0}", mMaterialsMap[item.mType]));
				mMaterialsMap[item.getType()] = mMaterialsMap[item.getType()] - item.getAmount();
				if ( mMaterialsMap[item.getType()] <= 0 )
                {
                    //Debug.Log("adding item to the list of items to remove.");
					rtypes.Add(item.getType());
                }
				item.setAmount(0);
                //Debug.Log(string.Format("{0}", mMaterialsMap[item.mType]));
            }
        }
        //delete the empty entries in material map
        foreach (GameTypes.ItemType type in rtypes)
        {
            if (mMaterialsMap.ContainsKey(type))
                mMaterialsMap.Remove(type);
        }
    }

    private void setupType()
    {
        //set the materials map based on the type
        switch (mType)
        {
            case GameTypes.BuildingType.Unknown:
                Debug.LogError("Unknown Building type");
                break;
			case GameTypes.BuildingType.TownHall:
                mMaterialsMap = Globals.TOWNHALL_CONSTRUCTION_MATERIALS;
                mName = "TownHall Construct";
                break;
			case GameTypes.BuildingType.House:
				mMaterialsMap = Globals.HOUSE_CONSTRUCTION_MATERIALS;
				mName = "House Construct";
				break;
            case GameTypes.BuildingType.Stockpile:
                mMaterialsMap = Globals.STOCKPILE_CONSTRUCTION_MATERIALS;
                mName = "Stockpile Construct";
                break;
            case GameTypes.BuildingType.Farm:
                mMaterialsMap = Globals.FARM_CONSTRUCTION_MATERIALS;
                mName = "Farm Construct";
                break;
			case GameTypes.BuildingType.SpearWorkshop:
				mMaterialsMap = Globals.SPEARWORKSHOP_CONSTRUCTION_MATERIALS;
				mName = "Spear Workshop Construct";
				break;
			case GameTypes.BuildingType.HandCartWorkshop:
				mMaterialsMap = Globals.HANDCARTWORKSHOP_CONSTRUCTION_MATERIALS;
				mName = "HandCart Workshop Construct";
				break;
            default:
                Debug.LogError("Construction(Building) type not recognised");
                break;

        }

    }


}
