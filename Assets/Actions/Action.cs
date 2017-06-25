using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Notes
// * base actions class
// * actions must be stored in "EntityAction > Actions >" as it will look for the EntityAction as its parent.
// * action is disabled until its at the front of the EntityAction's ActionGroup it is then enabled.
// *  when finished mComplete is set to true and the action gets deleted.

public class Action : MonoBehaviour
{

    //public members
    public string mName;

    //protected members
    protected bool mComplete = false;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public virtual void Awake()
    {
        enabled = false;
    }
    public virtual void Start()
    {
        Debug.LogError("Action::startAction: This function should be overwritten.");
    }
    public virtual void Update()
    {
        Debug.LogError("Action::startAction: This function should be overwritten.");
        mComplete = true;
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    /*public virtual Action init()
    {
        //init an action
        Debug.LogError("This function should be overwritten.");
        Action a = new Action();
        return a;
    }*/
    public bool isComplete()
    {
        return mComplete;
    }
    /*public virtual void setActer(EntityAction acter)
    {
        Debug.LogError("Action::setActer: This function should be overwritten.");
    }*/
    public virtual string print()
    {
        return "Base Action\n";
    }
    public virtual void setComplete(bool b)
    {
        Debug.Log("in action set complete");
        mComplete = b;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void getActer()
    {
        Debug.LogError("Action::getActer: This function should be overwritten.");
    }
};
