using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class HUD : MonoBehaviour
{

    //public members
    public GUISkin mSelectBoxSkin;
    public PopMenu mPopMenu;
    public ExchangeMenu mExchangeMenu;
    public SelectionBox mSelectionBox;

    //private members
    private Rect mPlayingArea;
    private SideBar mSideBar;
	private List<IconPanel> mIcons = new List<IconPanel>();
	private Canvas mScreenBarsCanvas;
	private Canvas mCameraSpaceCanvas;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
        UI.StoreSelectBoxSkin(mSelectBoxSkin);
        setPlayingArea();
        mSideBar = GetComponentInChildren<SideBar>();
        if (!mSideBar)
            Debug.LogError("Can't find SideBar in HUD");
        mSelectionBox = GetComponentInChildren<SelectionBox>();
        if (!mSelectionBox)
            Debug.Log("Can't find SelectionBox in HUD. I disabled it.");
        mPopMenu = GetComponentInChildren<PopMenu>();
        if (!mPopMenu)
            Debug.LogError("Can't find PopMenu in HUD");
        //mPopMenu.gameObject.SetActive(false);
        mExchangeMenu = GetComponentInChildren<ExchangeMenu>();
        if (!mExchangeMenu)
            Debug.LogError("Can't find ExchangeMenu in HUD");
        mExchangeMenu.gameObject.SetActive(false);
		//get the canvases
		List<Canvas> clist = new List<Canvas>(GetComponentsInChildren<Canvas>());
		foreach (Canvas c in clist)
		{
			if ( c.gameObject.name == "ScreenBarsCanvas" )
				mScreenBarsCanvas = c;
			else if ( c.gameObject.name == "CameraSpaceCanvas" )
				mCameraSpaceCanvas = c;
			else
				Debug.LogWarning("Canvas with name "+c.gameObject.name+" not recognised");
		}
    }

    void Start ()
    {
    }
	
	void Update ()
    {
		//manage the current icons
		manageIcons();
	}

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    public void clearSelection()
    {
        //clear the hud of parts relating to the selected entity
        mSideBar.clearSelection();
        if (mPopMenu.isActive())
            mPopMenu.setActive(false);  
    }
  
    public void drawSelectionBox(Entity ent)
    {
		if (ent)
		{
        	GUI.skin = mSelectBoxSkin;
        	Rect selectBox = calculateSelectionBox(ent);
        	//Draw the selection box around the currently selected object, within the bounds of the playing area
        	GUI.BeginGroup(mPlayingArea);
        	GUI.Box(selectBox, "");
        	//GUI.Label(new Rect(selectBox.x, selectBox.y - 7, selectBox.width * healthPercentage, 5), "", healthStyle);
        	GUI.EndGroup();
		}
    }
    public void drawSelectionBox(List<Entity> ents)
    {
        foreach(Entity ent in ents)
            drawSelectionBox(ent);
    }
    public void drawSideBar(Entity ent)
    {
        mSideBar.populate(ent);
    }

	public void makeIconIfNeeded(Entity ent)
	{
		//make an icon of the given type for the given entity if its necessary
		if ( ent.hasIcon() == true )
			return;
		GameTypes.IconType itype = needsIcon(ent);
		if ( itype != GameTypes.IconType.Unknown )
		{
			makeIcon(ent, itype);
		}
	}

    public bool mouseInBounds()
    {
        //Screen coordinates start in the lower-left corner of the screen
        //not the top-left of the screen like the drawing coordinates do
        Vector3 mousePos = Input.mousePosition;
        /*bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - ORDERS_BAR_WIDTH;
        bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;*/
        bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width;
        bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height;
        return insideWidth && insideHeight;
    }
    public void populatePopMenu(Entity sel, GameObject obj, Vector3 hitPoint)
    {
		//REMOVED THIS FOR EXPLORE ACTION
        //if (!obj && hitPoint == Globals.InvalidPosition)
        //    return;
		Entity ent = null;
		if (obj)
        	ent = obj.GetComponentInParent<Entity>();
		EntityAction sel_act = (EntityAction)sel;
        if (sel_act)
        {
            //populate the menu
            mPopMenu.populate(sel_act, ent, hitPoint);
        }

    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

    private Rect calculateSelectionBox(Entity ent)
    {
        //get bounds
        Bounds selectionBounds = ent.calculateBounds();

        //shorthand for the coordinates of the centre of the selection bounds
        float cx = selectionBounds.center.x;
        float cy = selectionBounds.center.y;
        float cz = selectionBounds.center.z;
        //shorthand for the coordinates of the extents of the selection bounds
        float ex = selectionBounds.extents.x;
        float ey = selectionBounds.extents.y;
        float ez = selectionBounds.extents.z;

        //Determine the screen coordinates for the corners of the selection bounds
        List<Vector3> corners = new List<Vector3>();
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy + ey, cz + ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy + ey, cz - ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy - ey, cz + ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy + ey, cz + ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy - ey, cz - ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy - ey, cz + ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy + ey, cz - ez)));
        corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy - ey, cz - ez)));

        //Determine the bounds on screen for the selection bounds
        Bounds screenBounds = new Bounds(corners[0], Vector3.zero);
        for (int i = 1; i < corners.Count; i++)
        {
            screenBounds.Encapsulate(corners[i]);
        }

        //Screen coordinates start in the bottom left corner, rather than the top left corner
        //this correction is needed to make sure the selection box is drawn in the correct place
        float selectBoxTop = mPlayingArea.height - (screenBounds.center.y + screenBounds.extents.y);
        float selectBoxLeft = screenBounds.center.x - screenBounds.extents.x;
        float selectBoxWidth = 2 * screenBounds.extents.x;
        float selectBoxHeight = 2 * screenBounds.extents.y;

        return new Rect(selectBoxLeft, selectBoxTop, selectBoxWidth, selectBoxHeight);
    }

	private void makeIcon(Entity ent, GameTypes.IconType itype)
	{
		IconPanel ip = Instantiate(ObjectManager.getMenu("IconPanel"), mCameraSpaceCanvas.transform).GetComponent<IconPanel>();
		ip.mEntity = ent;
		ip.mIconType = itype;
		ip.mActive = true;
		//add it to the list to keep track
		mIcons.Add(ip);
	}
	private void manageIcons()
	{
		//remove icons if no longer necessary
		for (int i = mIcons.Count - 1; i >= 0; i--)
		{
			if ( needsIcon(mIcons[i].mEntity) == GameTypes.IconType.Unknown )
			{
				IconPanel pan = mIcons[i];
				mIcons.RemoveAt(i);
				Destroy(pan.gameObject);
			}
		}

	}
	private GameTypes.IconType needsIcon(Entity ent)
	{
		//check conditions for displaying certain icons above entities to convey information
		Unit unit = ent as Unit;
		WorkedBuilding wb = ent as WorkedBuilding;
		if ( unit )
		{
			if ( unit.isIdle() )
				return GameTypes.IconType.Caution;
		}
		else if ( wb )
		{
			if ( wb.needsWorkers() )
				return GameTypes.IconType.Caution;
		}
		return GameTypes.IconType.Unknown;
	}

    private void setPlayingArea()
    {
        mPlayingArea = new Rect(0, 0, Screen.width, Screen.height);
    }



}
