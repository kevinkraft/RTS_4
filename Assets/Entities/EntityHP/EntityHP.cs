using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Child class of Entity that has HP, base class of Building and Unit

//Notes:
// * It looks for a Town as its parent, so it should be the child of a Town
//   in the heirarchy

public class EntityHP : Entity
{

    //public members
    public float mHP;
    public Town mTown;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        //get the town, which should be the parent
        mTown = transform.parent.GetComponent<Town>();
        //mTown = GetComponentInParent<Town>();
        if (!mTown && !GetComponentInParent<GameObjectList>())
            Debug.LogError("No Town found as parent of EntityHP");
    }


    public override void Update()
    {
        if (!isInstantiated)
            return;
        base.Update();
        if (mHP < 0f)
        {
            mHP = 0;
            mDead = true;
        }
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public float getHP()
    {
        return mHP;
    }
    public Building getStockpile()
    {
        return mTown.mStockpile;
    }
    public Town getTown()
    {
        return mTown;
    }
    public void setHP(float hp)
    {
        mHP = hp;
    }

	public void setTown(Town tw)
	{
		mTown = tw;
	}


}



