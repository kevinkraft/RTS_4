using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class SelectionBox : MonoBehaviour
{

    //public members
    public bool mHeld = false;

    //private members
    private bool mActive = false;
    private float mPosX1 = 100f;
    private float mPosY1 = 100f;
    private float mPosX2 = 500f;
    private float mPosY2 = 500f;
    private List<Entity> mSelection = new List<Entity>();
    private Vector3 mTL;
    private Vector3 mTR;
    private Vector3 mBL;
    private Vector3 mBR;
    private bool mMadeSelection = false;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------

    void OnGUI()
    {
        //Note that OnGUI is called multiple times in a frame.
        //need to handle mouse input here as well as in UserInput in case the mouse is over the selection box
        if (Input.GetMouseButtonUp(0) && mHeld == true)
        {
            setBottonRightCorner(Input.mousePosition);
            setCorners(mPosX1, mPosY1, mPosX2, mPosY2);
            makeSelection();
        }
        else if (Input.GetMouseButton(0))
        {
            setBottonRightCorner(Input.mousePosition);
        }

        if (mActive && mHeld)
            setCorners(mPosX1, mPosY1, mPosX2, mPosY2);
        else
        {
            setActive(false);
        }
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public bool isActive()
    {
        return mActive;
    }
    public List<Entity> getSelection()
    {
        //Debug.Log("getting selection");
        if (mMadeSelection)
        {
            //Debug.Log("returning selection");
            setActive(false);
            return mSelection;
        }
        else
        {
            //Debug.Log("returning a null selection");
            setActive(false);
            return new List<Entity>();
        }
    }
    public void setActive(bool b)
    {
        mActive = b;
        mHeld = b;
        if ( b == false )
            clear();
    }
    public void setBottonRightCorner(Vector2 v)
    {
        mPosX2 = v.x;
        mPosY2 = v.y;
    }
    public void setTopLeftCorner(Vector2 v)
    {
        mPosX1 = v.x;
        mPosY1 = v.y;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void clear()
    {
        mActive = false;
        setPositions(0, 0);
        mMadeSelection = false;
    }
    private bool isWithinPolygon(Vector3 unitPos)
    {
        //Is a unit within a polygon determined by 4 corners
        bool isWithinPolygon = false;

        //The polygon forms 2 triangles, so we need to check if a point is within any of the triangles
        //Triangle 1: TL - BL - TR
        if (IsWithinTriangle(unitPos, mTL, mBL, mTR))
        {
            return true;
        }

        //Triangle 2: TR - BL - BR
        if (IsWithinTriangle(unitPos, mTR, mBL, mBR))
        {
            return true;
        }

        return isWithinPolygon;
    }
    private bool IsWithinTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //Is a point within a triangle
        //From http://totologic.blogspot.se/2014/01/accurate-point-in-triangle-test.html
        bool isWithinTriangle = false;

        //Need to set z -> y because of other coordinate system
        float denominator = ((p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z));

        float a = ((p2.z - p3.z) * (p.x - p3.x) + (p3.x - p2.x) * (p.z - p3.z)) / denominator;
        float b = ((p3.z - p1.z) * (p.x - p3.x) + (p1.x - p3.x) * (p.z - p3.z)) / denominator;
        float c = 1 - a - b;

        //The point is within the triangle if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
        if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
        {
            isWithinTriangle = true;
        }

        return isWithinTriangle;
    }
    private void makeSelection()
    {
        //Debug.Log("Made selection");
        mMadeSelection = true;
        //mouse has been released, check whats in the net
        mSelection.Clear();
        //raycast the 2D corners to 3D points
        raycastCorners();
        //loop over the list of visible entities and check if in net
        //if there is a unit only select units, otherwise select anything
        bool unitSel = false;
        List<Entity> tsel = new List<Entity>();
        foreach (Entity ent in ObjectManager.getVisibleEntities())
        {
            if (isWithinPolygon(ent.transform.position))
            {
                Unit unit = ent as Unit;
                if (unit)
                    unitSel = true;
                tsel.Add(ent);
            }
        }
        //is there at least one unit in the sel? then only select units
        mSelection = new List<Entity>(tsel);
        if (unitSel)
        {
            //remove the non units
            foreach (Entity ent in tsel)
            {
                Unit unit = ent as Unit;
                if (!unit)
                    mSelection.Remove(ent);
            }
        }
        //Debug.Log("sel len is " + mSelection.Count.ToString());
    }
    private void raycastCorners()
    {
        //The problem is that the corners in the 2d square is not the same as in 3d space
        //To get corners, we have to fire a ray from the screen
        //We have 2 of the corner positions, but we don't know which,  
        //so we can figure it out or fire 4 raycasts
        Vector2 startp = new Vector2(mPosX1, mPosY1);
        Vector2 endp = new Vector2(mPosX2, mPosY2);
        Vector2 trp = new Vector2(mPosX2, mPosY1);
        Vector2 blp = new Vector2(mPosX1, mPosY2);
        float sizeX = Mathf.Abs(startp.x - endp.x);
        float sizeY = Mathf.Abs(startp.y - endp.y);
        Vector2 middle =  startp + endp;


        /*Debug.Log("TR:");
        Debug.Log(trp);
        Debug.Log("BL:");
        Debug.Log(blp);
        Debug.Log("BR:");
        Debug.Log(endp);*/

        /*mTL = new Vector3(middle.x - sizeX / 2f, middle.y + sizeY / 2f, 0f);
        mTR = new Vector3(middle.x + sizeX / 2f, middle.y + sizeY / 2f, 0f);
        mBL = new Vector3(middle.x - sizeX / 2f, middle.y - sizeY / 2f, 0f);
        mBR = new Vector3(middle.x + sizeX / 2f, middle.y - sizeY / 2f, 0f);*/
        mTL = Vector3.zero;
        mTR = Vector3.zero;
        mBL = Vector3.zero;
        mBR = Vector3.zero;

        //Debug.Log("TL screen box:");
        //Debug.Log(startp);

        //From screen to world
        RaycastHit hit;
        int i = 0;
        //Fire ray from camera
        if (Physics.Raycast(Camera.main.ScreenPointToRay(startp), out hit))
            {
            mTL = hit.point;
            i++;
            //Debug.Log("TL game pos:");
            //Debug.Log(mTL);
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(trp), out hit))
            {
            mTR = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(blp), out hit))
        {
            mBL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(endp), out hit))
        {
            mBR = hit.point;
            i++;
        }
    }
    private void setCorners(float x1, float y1, float x2, float y2)
    {
        //flip the coordinates to you cant also select up and to the left
        float xt = x2;
        float yt = y2;
        if ( x1 > x2)
        {
            x2 = x1;
            x1 = xt;
        }
        if (y2 > y1)
        {
            y2 = y1;
            y1 = yt;
        }
        float w = x2 - x1;
        float h = 1 * (y1 - y2);
        setPositions(x1, y1, w, h);
    }
    private void setPositions(float x, float y, float width=100f, float height=100f, float aminx = 0f, float aminy = 0f, float amaxx = 0f, float amaxy = 0f, float pivotx = 0f, float pivoty = 1f)
    {
        //sets the position, anchors and pivots 
        //get the rect
        RectTransform rt = (RectTransform)transform;
        //set the anchor and pivots
        rt.anchorMin = new Vector2(aminx, aminy);
        rt.anchorMax = new Vector2(amaxx, amaxy);
        rt.pivot = new Vector2(pivotx, pivoty);
        //set the offsets
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 0);
        //set the top corner pos
        rt.sizeDelta = new Vector2(width, height);
        rt.anchoredPosition = new Vector2(x, y);
    }

}

