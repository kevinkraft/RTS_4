using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Notes:
// * class for a Nation
// * Holds Regions beloning to this Nation
// * Knows which player controls it (is this necessary?)
// * Nothing more than a container for now

public class Nation : EntityContainer
{

	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------
	private void Awake()
	{
		base.Awake();
	}

	void Start()
	{
		//search for entities in children and put them in the right group
		foreach (Transform child in transform)
		{
			//if its a Region add it to region group
			Region r = child.GetComponent<Region>();
			if ( r )
			{
				addContainer("regions",r);
			}
		}

	}
	//-------------------------------------------------------------------------------------------------
	// public methods
	//-------------------------------------------------------------------------------------------------
	public List<Building> getAllBuildings()
	{
		//make a list of all the buildings in the Regions
		List<Building> blist = new List<Building>();
		foreach ( Region r in mContainerMap["regions"] )
		{
			blist.AddRange(r.getAllBuildings());
		}
		return blist;
	}
	public List<Unit> getAllUnits()
	{
		//make a list of all the units in the Regions
		List<Unit> ulist = new List<Unit>();
		foreach ( Region r in mContainerMap["regions"] )
		{
			ulist.AddRange(r.getAllUnits());
		}
		return ulist;
	}



}
