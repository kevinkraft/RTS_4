using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for holding the values of unit statistics

public class UnitStats : MonoBehaviour 
{

	//private variables
	private Dictionary<GameTypes.UnitStatType, float> mValueMap = new Dictionary<GameTypes.UnitStatType, float>();
	private Dictionary<GameTypes.UnitStatType, int> mIntValueMap = new Dictionary<GameTypes.UnitStatType, int>();

	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------

	void Awake()
	{
		//set the default values
		mValueMap = new Dictionary<GameTypes.UnitStatType,float>( Globals.UNIT_DEFAULT_FLOAT_STATS );
		mIntValueMap = new Dictionary<GameTypes.UnitStatType,int>( Globals.UNIT_DEFAULT_INT_STATS );
	}
		
	//-------------------------------------------------------------------------------------------------
	// public methods
	//-------------------------------------------------------------------------------------------------

	public void addToValue(GameTypes.UnitStatType stype, float cval)
	{
		if ( mValueMap.ContainsKey(stype) )
		{
			mValueMap[stype] = mValueMap[stype] + cval;

		}
		else if ( mIntValueMap.ContainsKey(stype) )
		{
			mIntValueMap[stype] = mIntValueMap[stype] + (int)cval;
		}
		else
		{
			Debug.LogError("The given UnitStatsType is not in the dictionary of types and values.");
			return;
		}
	}

	public void addToValue(GameTypes.UnitStatType stype, int cval)
	{
		if ( mValueMap.ContainsKey(stype) )
		{
			mValueMap[stype] = mValueMap[stype] + (float) cval;

		}
		else if ( mIntValueMap.ContainsKey(stype) )
		{
			mIntValueMap[stype] = mIntValueMap[stype] + cval;
		}
		else
		{
			Debug.LogError("The given UnitStatsType is not in the dictionary of types and values.");
			return;
		}
	}

	public int getIntValue(GameTypes.UnitStatType stype)
	{
		//Debug.Log(mIntValueMap.Count+" "+mValueMap.Count);
		if ( mIntValueMap.ContainsKey(stype) )
		{
			return mIntValueMap[stype];
		}
		else
		{
			//Debug.LogError("The given UnitStatsType ("+stype.ToString()+") is not in the dictionary of types and values.");
			return 0;
		}
	}

	public float getValue(GameTypes.UnitStatType stype)
	{
		if ( mValueMap.ContainsKey(stype) )
		{
			return mValueMap[stype];

		}
		else if ( mIntValueMap.ContainsKey(stype) )
		{
			return (float) mIntValueMap[stype];
		}
		else
		{
			Debug.LogError("The given UnitStatsType is not in the dictionary of types and values.");
			return 0f;
		}
	}

	public string print()
	{
		string rs = "";
		//for the floats
		foreach (KeyValuePair<GameTypes.UnitStatType,float> sts in mValueMap )
		{
			rs += Globals.UNIT_STATS_DISPLAY_TEXT[sts.Key] + ":" + sts.Value.ToString() + "\n"; 
		}
		//for the ints
		foreach (KeyValuePair<GameTypes.UnitStatType,int> sts in mIntValueMap )
		{
			rs += Globals.UNIT_STATS_DISPLAY_TEXT[sts.Key] + ":" + sts.Value.ToString() + "\n"; 
		}
		return rs;
	}

	public void setValue(GameTypes.UnitStatType stype, float val)
	{
		mValueMap[stype] = val;
	}

	public void setValue(GameTypes.UnitStatType stype, int val)
	{
		mIntValueMap[stype] = val;
	}

}
