using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Resource : Entity
{

    //private members
    public GameTypes.ItemType mType;
    public int mAmount;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Update()
    {
        if (!isInstantiated)
            return;
        base.Update();
        //it its empty, set it as dead
        if ( mAmount <= 0 )
        {
            mDead = true;
        }
    }

}
