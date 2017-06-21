using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wrapper script for all menu types

public class Menu : MonoBehaviour
{
    //public member
    public string mName;

    //private members
    public Canvas mCanvas;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    protected void Start()
    {
        //get the parent canvas, or dint give a warning if its the menu instance stored in GameObjectList
        mCanvas = GetComponentInParent<Canvas>();
        if ( !mCanvas && !transform.parent.GetComponentInParent<GameObjectList>() )
            Debug.LogError("Menu canvas is null. Can't find Menu canvas");
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    /*public void setAnchorsPivots(float aminx, float aminy, float amaxx, float amaxy, float pivotx, float pivoty)
    {

    }*/

    public virtual void setPositions(float left, float right, float top, float bottom, float aminx=0f, float aminy=0f, float amaxx=1f, float amaxy=1f, float pivotx=0.5f, float pivoty=0.5f)
    {
        //sets the position, anchors and pivots 
        //get the rect
        RectTransform rt = (RectTransform)transform;
        //set the anchor and pivots
        rt.anchorMin = new Vector2(aminx, aminy);
        rt.anchorMax = new Vector2(amaxx, amaxy);
        rt.pivot = new Vector2(pivotx, pivoty);
        //set the offsets
        rt.offsetMin = new Vector2(left, bottom);
        rt.offsetMax = new Vector2(right, -top);
    }
    public void setPositions(float x, float y, float w, float h)
    {
        //set position for x,y screen coords, width and height, pivot is set to top left of menu,
        //anchors set to the bottom left of the canvas, usually a canvas covering the whole screen
        RectTransform rt = (RectTransform)transform;
        setPositions(0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        rt.sizeDelta = new Vector2(w, h);
        rt.anchoredPosition = new Vector2(x, y);
    }

}
