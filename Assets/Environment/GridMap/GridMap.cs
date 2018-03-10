using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//a class to contain the grid of map (biome?) types in right locations

//Notes:
// * done with a dictionary
// * The grid can be populated from already initiated Regions as the
//   Regions need to Start themselves first

public class GridMap : MonoBehaviour 
{

	//private members
	private Dictionary< Vector2, GameTypes.MapType> mGrid = Globals.DEFAULT_MAP_GRID_START;
	private WorldManager mWorldManager;
	private bool mInitiated = false;

	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------
	void Awake()
	{
		//get the parent world manager
		mWorldManager = GetComponentInParent<WorldManager>();
		if (!mWorldManager)
			Debug.LogError ("No WorldManager found as parent.");
	}
	void Start()
	{
		//make copy of the grid so you can modify it
		Dictionary<Vector2, GameTypes.MapType> grid = new Dictionary<Vector2, GameTypes.MapType>(mGrid);
		//check the already instantiated Regions to see if their properties match the default grid
		foreach (Region reg in mWorldManager.getRegions()) 
		{
			Vector2 rgp = reg.getGridPos();
			GameTypes.MapType mt = reg.mType;
			if ( mGrid.ContainsKey(rgp) ) 
			{
				//the grid knows about this Region
				mGrid[rgp] = mt;
				grid.Remove(rgp);
			} 
			else 
			{
				//the grid doesnt know about this region	
				mGrid.Add(rgp, mt);
			}
		}
		//make other regions defined in the default
		foreach (KeyValuePair<Vector2,GameTypes.MapType> kp in grid)
		{
			//make a new region
			ObjectManager.initRegion(kp.Key, kp.Value, mWorldManager, mWorldManager.getRandomSeed() );
		}
			
	}

	//-------------------------------------------------------------------------------------------------
	// public methods
	//-------------------------------------------------------------------------------------------------

	public void addGridEntry(Vector2 loc, GameTypes.MapType mtype)
	{
		if (hasGridEntry(loc))
		{
			Debug.LogError("This entry already exists in the GridMap");
		}
		mGrid[loc] = mtype;
	}

	public GameTypes.MapType getGridEntry(Vector2 loc)
	{
		//returns type at given grid location
		if (hasGridEntry(loc))
		{
			return mGrid[loc];
		}
		else return GameTypes.MapType.Unknown;
	}

	public bool hasGridEntry(Vector2 loc)
	{
		//returns true if an entry exists for this grid position in the map
		if (mGrid.ContainsKey(loc))
			return true;	
		else return false;	
	}







}
