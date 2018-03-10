using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wrapper script for entity models so they can be turned on and off and to access visible objects

public class Model : MonoBehaviour
{

    //public members
    public VisibleRenderer mVisibleRenderer;

    //private members
    private Entity mEntity;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------

    private void Awake()
    {
        VisibleRenderer vr = GetComponentInChildren<VisibleRenderer>();
        if (!vr)
            Debug.LogError("Can't find VisibleRenderer in Entity Model");
        mVisibleRenderer = vr;
        Entity ent = GetComponentInParent<Entity>();
        if (!ent)
            Debug.LogError("Can't find Entity as parent of Entity Model");
        mEntity = ent;

    }

    /*private void Update()
    {
        //check if visible
        if (mVisibleRenderer.mVisible)
        {
            //tell the Entity to add itself to the visible list
            Debug.Log("Is visible");

        }
    }*/


}
