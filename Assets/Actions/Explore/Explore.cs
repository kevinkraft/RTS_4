using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for exploring new map regions

//Notes:
// * Right clicking in an invalid position makes this available 
// * Unit goes to nearest empty GridMap postion
// * only works if at least one of the eight surrounding Regions is not yet explored 

public class Explore : Action
{
	//public members
	public Unit mActer;

	//private
	private Vector2 mGridDestination;
	private Region mCurrentRegion;
	private Vector3 mDestination;
	private WorldManager mWorldManager;

	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------
	public override void Start()
	{
		getActer();
		if ( !mActer )
			Debug.Log("No acter is defined.");
		mComplete = false;
		//get the world manager
		mWorldManager = mActer.transform.root.GetComponent<WorldManager>();
		if (!mWorldManager) mWorldManager = mActer.transform.root.GetComponentInChildren<WorldManager>();
		if (!mWorldManager) 
			{
				Debug.LogError("Can't find WorldManager for Explore Action.");
				mComplete = true;
				return;
			}
		//get the region of the current postion of the acter
		mCurrentRegion = mWorldManager.getRegionFromPosition(mActer.transform.position);
		if (!mCurrentRegion)
		{
			Debug.LogError("Not in any region");
			mComplete = true;
			return;
		}

		//Debug.Log(string.Format("Current Region: {0}", mCurrentRegion.getGridPos()));
		//make a list of the region grid coords surrounding that are not explored
		List<Vector2> empty_coords = mWorldManager.getGridCoordsOfSurroundingEmptyRegions(mCurrentRegion);
		/*//TEMP
		foreach (Vector2 ec in empty_coords)
			Debug.Log(ec);*/
		if (empty_coords.Count == 0)
		{
			Debug.Log("No unexplored grid positions around acters current Region");
			mComplete = true;
			return;
		}
		//of these, which is the acter closest to?
		mGridDestination = closestEmptyGrid(empty_coords);
		//Debug.Log("closest empty grid");
		//Debug.Log(mGridDestination);

		mDestination = determineDestination();

	}
	public override void Update()
	{
		if (mActer.transform.position != mDestination)
		{
			//make a move action
			mActer.moveTo( mDestination, false );
			return;
		}
		//at destination, so intialise a new Region in the correct grid loc
		//Debug.Log(mGridDestination);
		mWorldManager.makeNewRegion(mGridDestination);
		mComplete = true;
	}

	//-------------------------------------------------------------------------------------------------
	// public methods
	//-------------------------------------------------------------------------------------------------
	public override string print()
	{
		return string.Format("Explore: ({0:0},{1:0})\n", mGridDestination.x, mGridDestination.y);
	}

	//-------------------------------------------------------------------------------------------------
	// protected methods
	//-------------------------------------------------------------------------------------------------
	protected void getActer()
	{
		mActer = GetComponentInParent<Unit>();
	}

	//-------------------------------------------------------------------------------------------------
	// private methods
	//-------------------------------------------------------------------------------------------------
	private Vector2 closestEmptyGrid(List<Vector2> elist)
	{
		//returns the empty grid pos closest to the acter
		//essentially need to determine which corner or edge the acter is closest to of those 
		//that are not yet explored
		float smallest_dist = (4.0f)*(mCurrentRegion.getWidth()+0.0f);
		float dist = smallest_dist;
		Vector2 smallest_grid = new Vector2(0,0);
		float xdiff = 0.0f;
		float zdiff = 0.0f;
		float sm_xdiff = 0.0f;
		float sm_zdiff = 0.0f;
		List<Vector2> fcs = mCurrentRegion.getFourCorners();
		Vector2 apos = new Vector2(mActer.transform.position.x, mActer.transform.position.z);
		Vector2 greg = mCurrentRegion.getGridPos();
		foreach (Vector2 ev in elist)
		{
			xdiff = ev.x - greg.x;
			zdiff = ev.y - greg.y;
			if ( Mathf.Abs(xdiff)>1 || Mathf.Abs(zdiff)>1 )
			{
				Debug.LogError("This empty grid pos is not surrouding the Region given.");
			}
			//do all cases by hand as there isnt any nice way to do it
			//do sides first, then corners
			if (xdiff==0 && zdiff>0) dist = Mathf.Abs(apos.y - fcs[0].y);
			else if (xdiff<0 && zdiff==0) dist = Mathf.Abs(apos.x - fcs[0].x);
			else if (xdiff>0 && zdiff==0) dist = Mathf.Abs(apos.x - fcs[1].x); 
			else if (xdiff==0 && zdiff<0) dist = Mathf.Abs(apos.y - fcs[2].y);
			else if (xdiff>0 && zdiff<0) dist = (apos - fcs[2]).magnitude;
			else if (xdiff>0 && zdiff>0) dist = (apos - fcs[1]).magnitude;
			else if (xdiff<0 && zdiff>0) dist = (apos - fcs[0]).magnitude;	
			else if (xdiff<0 && zdiff<0) dist = (apos - fcs[3]).magnitude;
			dist = Mathf.Abs(dist);
			if (dist < smallest_dist) 
			{
				smallest_dist = dist;
				smallest_grid = ev;
				sm_xdiff = xdiff;
				sm_zdiff = zdiff;
			}
		}
		//Debug.Log(string.Format("small x diff is {0}",sm_xdiff.ToString()));
		//Debug.Log(string.Format("small z diff is {0}",sm_zdiff.ToString()));
		if (smallest_grid == new Vector2(0,0)) Debug.LogError("No smallest distance to any of the grids was found. This shouldn't be possible.");
		return smallest_grid;
	}

	private Vector3 determineDestination()
	{
		//determine what destination the acter should go to before 'exploring' the new Region
		Vector2 greg = mCurrentRegion.getGridPos();
		float xdiff = mGridDestination.x - greg.x;
		float zdiff = mGridDestination.y - greg.y;
		List<Vector2> fcs = mCurrentRegion.getFourCorners();
		Vector3 dest = new Vector3(0,0,0);
		Vector3 apos = mActer.transform.position;
		//check all cases
		if (xdiff>0 && zdiff>0) dest = new Vector3(fcs[1].x, 0, fcs[1].y);
		else if (xdiff==0 && zdiff>0) dest = new Vector3(apos.x, 0, fcs[0].y);
		else if (xdiff<0 && zdiff>0) dest = new Vector3(fcs[0].x, 0, fcs[0].y);	
		else if (xdiff<0 && zdiff==0) dest = new Vector3(fcs[0].x, 0, apos.z);
		else if (xdiff<0 && zdiff<0) dest = new Vector3(fcs[3].x, 0, fcs[3].y);
		else if (xdiff==0 && zdiff<0) dest = new Vector3(apos.x, 0, fcs[2].y);
		else if (xdiff>0 && zdiff<0) dest = new Vector3(fcs[2].x, 0, fcs[2].y);
		else if (xdiff>0 && zdiff==0) dest = new Vector3(fcs[1].x, 0, apos.z);
		if (dest == new Vector3(0,0,0)) Debug.LogError("No destination was determined. This shouldn't be possible.");
		return dest;
	}



}