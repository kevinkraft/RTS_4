using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGroup : MonoBehaviour
{
    //This can't be a blank wrapper as the order of the actions matters

    //public members
    public List<Action> mActions = new List<Action>();

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void appendAction(Action act)
    {
        mActions.Add(act);
        act.transform.SetParent(this.transform);
    }
    public void clearAddAction(Action act)
    {
        clear();
        mActions.Add(act);
        act.transform.SetParent(this.transform);
    }
    public bool hasActionOfType(string act_name)
    {
        foreach (Action act in mActions )
        {
            if (act.mName == act_name)
                return true;
        }
        return false;
    }
    public void prependAction(Action act)
    {
        if (mActions.Count >= 1)
            if (mActions[0].enabled == true)
                mActions[0].enabled = false;
        mActions.Insert(0, act);
        act.transform.SetParent(this.transform);
    }
    public string print()
    {
        //returns a string with one item on each line showing the name and amount
        string text = "";
        if (mActions.Count == 0)
            return "Empty";
        foreach (Action act in mActions)
        {
            text += act.print();
        }
        return text;
    }
    public void remove(int el)
    //remove a certain element
    {
        mActions.RemoveAt(el);
    }
    public int size()
    {
        return mActions.Count;
    }


    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void clear()
    {
        foreach (Action act in mActions)
        {
            Destroy(act.gameObject);
        }
        mActions = new List<Action>();
    }



}
