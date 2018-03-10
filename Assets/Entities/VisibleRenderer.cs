using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wrapper script for getting OnBecomeInvisible for an Entity Model

public class VisibleRenderer : MonoBehaviour
{

    //public members
    public bool mVisible = false;

    //private members
    private Model mModel;
    

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    private void Awake()
    {
        Model m = GetComponentInParent<Model>();
        if (!m)
            Debug.LogError("Can't find parent Model");
        mModel = m;

    }
    private void OnBecameInvisible()
    {
        mVisible = false;
    }
    private void OnBecameVisible()
    {
        mVisible = true;
    }


}
