using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Item : Selectable
{

    
    //public members
	[SerializeField]
    private GameTypes.ItemType mType;
	[SerializeField]
    private int mAmount;
    //public bool mHeld;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------

	//-------------------------------------------------------------------------------------------------
	// Public methods
	//-------------------------------------------------------------------------------------------------

	public int getAmount()
	{
		return mAmount;
	}

	public GameTypes.ItemType getType()
	{
		return mType;
	}

	public void setAmount(int am)
	{
		mAmount = am;
	}

	public void setType(GameTypes.ItemType t)
	{
		mType = t;
	}

}
