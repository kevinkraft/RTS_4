using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for managing the world

//Notes
// * Managers players and the MapGrip
// * Is used to make new regions
// * Neutral player is for holding Regon that arent owned by a nation (not fully implemented)

public class WorldManager : MonoBehaviour 
{

	//public members
	public Player mNeutralPlayer;

	//private members
	private List<Player> mPlayers = new List<Player>();
	private List<Region> mRegions = new List<Region>();
	private GridMap mGridMap;
	private System.Random mRandomGen;

	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------
	void Awake()
	{
		//get the currently instantiated players
		mPlayers = new List<Player>(GetComponentsInChildren<Player>());
		if (mPlayers.Count == 0)
			Debug.LogError ("No Players found as children of WorldManager.");
		//get the currently instantiated Regions
		mRegions = new List<Region>(GetComponentsInChildren<Region>());
		if (mRegions.Count == 0)
			Debug.LogError ("No Regions found as children of WorldManager.");
		//get the GridMap
		mGridMap = GetComponentInChildren<GridMap>();
		if (!mGridMap)
			Debug.LogError ("No GridMap found as child of WorldManager.");
		//set up the random generator and choose a seed
		setupRandom();
		//Debug.Log( string.Format("WorldManager has {0} regions",(mRegions.Count).ToString()) );
	}
		
	//-------------------------------------------------------------------------------------------------
	// public methods
	//-------------------------------------------------------------------------------------------------

	public List<Vector2> getGridCoordsOfSurroundingEmptyRegions(Region reg)
	{
		//returns a list of grid coords around a given region that are empty
		return getGridCoordsOfSurroundingRegionsCondition(reg, false);
	}

	public List<Vector2> getGridCoordsOfSurroundingRegions(Region reg)
	{
		//returns a list of grid coords around a given region that are empty
		//NEVER USED?
		return getGridCoordsOfSurroundingRegionsCondition(reg, true);
	}

	public int getRandomSeed()
	{
		return mRandomGen.Next(0,100);
	}

	public Region getRegionFromGridCoord(Vector2 coord)
	{
		//gets the region with given grid coords
		//NEVER ACTUALLY USED
		foreach (Region reg in mRegions)
		{
			if (reg.getGridPos() == coord) return reg;
		}
		//no grid found
		return null;
	}

	public List<Region> getRegions()
	{
		return mRegions;
	}

	public Region getRegionFromPosition(Vector3 pos)
	{
		//returns the region in which the given game coordinates resides
		//the region coords define the centre of the region so need to account for width
		Vector2 npos = new Vector2(pos.x,pos.z); 
		//Debug.Log("unit pos:");
		//Debug.Log(pos);
		//Debug.Log( string.Format("WorldManager has {0} regions",(mRegions.Count).ToString()) );
		foreach (Region reg in mRegions)
		{
			//get the four coords
			List<Vector2> fcs = reg.getFourCorners();
			/*//TEMP
			Debug.Log("four corners");
			foreach (Vector2 fc in fcs)
				Debug.Log(fc);*/
			//check if point in the box
			if ( npos.x >= fcs[0].x && npos.x < fcs[1].x 
				&& npos.y <= fcs[1].y && npos.y >= fcs[2].y )
			{
				//Debug.Log(reg.getGridPos());
				return reg;
			}
		}
		//not in any region
		Debug.LogError("Given position is not in any Region");
		return null;
	}

	public void makeNewRegion(Vector2 gcoord)
	{
		//makes a new region at the given grid coords
		if (mGridMap.hasGridEntry(gcoord))
			Debug.LogError("The given grid coordinate already has a region");
		//init region
		int seed = (int) getRand(0,1000);
		Region reg = ObjectManager.initRegion(gcoord, GameTypes.MapType.GrassPlain, this, seed);
		//add to the list
		addRegion(reg);
	}

	//-------------------------------------------------------------------------------------------------
	// private methods
	//-------------------------------------------------------------------------------------------------
	private void addRegion(Region reg)
	{
		if (mGridMap.hasGridEntry(reg.getGridPos()))
		{
			Debug.LogError("A region already exists at this GridMap location");
			return;
		}
		//add to grid map
		mGridMap.addGridEntry(reg.getGridPos(), reg.getType());
		//for now put every new region in the NeutralPlayer
		Player p = mNeutralPlayer;
		//add region to the game, this gets called from initRegion
		mRegions.Add(reg);
		reg.gameObject.transform.SetParent(mNeutralPlayer.gameObject.transform);
	}

	private void setupRandom()
	{
		//sets up the random generator with a time seed
		mRandomGen = new System.Random(new System.Random().Next());
	}

	private List<Vector2> getGridCoordsOfSurroundingRegionsCondition(Region reg, bool cond=true)
	{
		//returns a list of the grid coords around a given region that are initiated if true
		//and empty if false
		List<Vector2> ret_list = new List<Vector2>();
		Vector2 rp = reg.getGridPos();
		for (int i=-1; i < 2; i++) //x coord
		{
			for (int j=-1; j < 2; j++ ) //z coord
			{
				if (i==0 && j==0) continue; //this region
				Vector2 testv = new Vector2(rp.x+i, rp.y+j);
				if ( mGridMap.hasGridEntry(testv) == cond )
				{
					ret_list.Add(testv);
				}
			}
		}
		return ret_list;
	}

	private float getRand(float minv=0, float maxv=1)
	{ 
		return (float)mRandomGen.NextDouble() * (maxv - minv) + minv;
	}

}
