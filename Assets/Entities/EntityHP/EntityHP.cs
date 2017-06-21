using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHP : Entity
{

    //public members
    public float mHP;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------

    public override void Update()
    {
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

    public void setHP(float hp)
    {
        mHP = hp;
    }
    public float getHP()
    {
        return mHP;
    }

}



