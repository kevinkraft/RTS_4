using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RTS;

public class UserInput : MonoBehaviour 
{

	//public members
	public Player player;

    //private members
    private float mClickDelay = 0.2f;
    private float mClickTime = 0f;
    private Vector2 mHoldMouseStart;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------

    // Use this for initialization
    void Start ()
    {
        //player = transform.root.GetComponentInChidren<Player>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
        if (player.mHuman)
        {
            moveCamera();
            rotateCamera();
            mouseActivity();
        }

	}

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

    private GameObject findHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.collider.gameObject;
        return null;
    }

    private Vector3 findHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return Globals.InvalidPosition;
    }
    private void leftMouseClick()
    {
        //Debug.Log("left mouse click");
        if (player.mHud.mouseInBounds())
        {
            GameObject hitObject = findHitObject();
            Vector3 hitPoint = findHitPoint();
            if (hitObject && hitPoint != Globals.InvalidPosition)
            {
                if (player.mSelectedList.Count != 0)
                {
                    //clear the selection, unless a popmenu is open, then only close the popmenu if its not clicked in
                    if (player.mHud.mPopMenu.isActive())
                        player.mHud.mPopMenu.setActive(false);
                    else
                        player.clearSelection();
                }
                if (hitObject.name != "Ground")
                {
                    Entity ent = hitObject.transform.GetComponentInParent<Entity>();
                    if (ent)
                    {
                        //we already know the player has no selected object
                        player.mSelectedList = new List<Entity>() { ent };
                    }
                }
            }
        }
    }

    private void leftMouseHeld()
    {
        //Debug.Log("mouse held");
        SelectionBox sb = player.mHud.mSelectionBox;
        if (sb.isActive() == false)
        {
            sb.setActive(true);
            sb.setTopLeftCorner(mHoldMouseStart);
        }
        else
        {
            sb.setBottonRightCorner(Input.mousePosition);
        }
    }
    private void leftMouseHoldRelease()
    {
        //Debug.Log("hold released");
        SelectionBox sb = player.mHud.mSelectionBox;
        player.mSelectedList = sb.getSelection();
        sb.setActive(false);
    }
    private void mouseActivity()
    {
        if (EventSystem.current.IsPointerOverGameObject() && !player.mHud.mSelectionBox.isActive() )
            return;

        //press down the button to start the timer
        if (Input.GetMouseButtonDown(0))
        {
            mClickTime = Time.time;
            mHoldMouseStart = Input.mousePosition;
        }
        //Release the mouse button
        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - mClickTime <= mClickDelay)
                leftMouseClick();
            else if (Time.time - mClickTime > mClickDelay)
                leftMouseHoldRelease();
            
        }
        //Holding down the mouse button
        else if (Input.GetMouseButton(0))
        {
            if (Time.time - mClickTime > mClickDelay)
                leftMouseHeld();
        }
        else if (Input.GetMouseButtonUp(1)) rightMouseClick();
    }

    private void moveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0, 0, 0);
        bool mouseScroll = false;
        
        //scroll faster if the camear is higher
        float scrollspeed = Globals.ScrollSpeed * Camera.main.transform.position.y;
        float zoomspeed = Globals.ScrollSpeed * Mathf.Pow(Camera.main.transform.position.y,2);
        //Debug.Log(string.Format("scroll speed: {0}",scrollspeed));

        //horizontal camera movement
        if (xpos >= 0 && xpos < Globals.ScrollWidth)
        {
            movement.x -= scrollspeed;
            //player.hud.SetCursorState(CursorState.PanLeft);
            mouseScroll = true;
        }
        else if (xpos <= Screen.width && xpos > Screen.width - Globals.ScrollWidth)
        {
            movement.x += scrollspeed;
            //player.hud.SetCursorState(CursorState.PanRight);
            mouseScroll = true;
        }

        //vertical camera movement
        if (ypos >= 0 && ypos < Globals.ScrollWidth)
        {
            movement.z -= scrollspeed;
            //player.hud.SetCursorState(CursorState.PanDown);
            mouseScroll = true;
        }
        else if (ypos <= Screen.height && ypos > Screen.height - Globals.ScrollWidth)
        {
            movement.z += scrollspeed;
            //player.hud.SetCursorState(CursorState.PanUp);
            mouseScroll = true;
        }

        //make sure movement is in the direction the camera is pointing
        //but ignore the vertical tilt of the camera to get sensible scrolling
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        //away from ground movement
        if (Camera.main.orthographic == false)
        {
            float zoom_amount = Input.GetAxis("Mouse ScrollWheel");
            //if the camera is at min or max hight dont zoom
            if (Camera.main.transform.position.y >= Globals.MaxCameraHeight && zoom_amount <= 0 ||
                Camera.main.transform.position.y <= Globals.MinCameraHeight && zoom_amount >= 0)
            {
                //pass
            }
            else
            {
                //get the facing direction of the camera and normalise
                Vector3 facing = Camera.main.transform.forward;
                facing.Normalize();
                float scale = scrollspeed * zoom_amount;
                Vector3 newpos = facing * scale;
                movement.x += newpos.x;
                movement.y += newpos.y;
                movement.z += newpos.z;
            }
        }
        else
        {
            //ortho
            Camera.main.orthographicSize -= scrollspeed * Input.GetAxis("Mouse ScrollWheel");
        }
       
        //calculate desired camera position based on received input
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        //limit away from ground movement to be between a minimum and maximum distance
        if (destination.y > Globals.MaxCameraHeight)
        {
            destination.y = Globals.MaxCameraHeight;
        }
        else if (destination.y < Globals.MinCameraHeight)
        {
            destination.y = Globals.MinCameraHeight;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            if ( Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                //Debug.Log("zooming");
                Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * zoomspeed);
            }
            else
            {
                //Debug.Log("panning");
                Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * scrollspeed);
            }
        }

        if (!mouseScroll)
        {
            //player.hud.SetCursorState(CursorState.Select);
        }

    }

    private void rightMouseClick()
    {
        //Debug.Log("Need to fix right click so the PopMenu works with multiple units");
        /*Entity sel = player.mSelectedObject;
        if (player.mHud.mouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && sel )
        {
            
            if ( sel.GetType() == typeof(Unit) )
            {
                GameObject hitObject = findHitObject();
                Vector3 hitPoint = findHitPoint();
                //make a popmenu
                player.mHud.populatePopMenu(sel, hitObject, hitPoint);
            }
        }*/
        //Entity sel = player.mSelectedObject;
        if (player.mHud.mouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.mSelectedList.Count != 0 )
        {
            if ( player.mSelectedList[0].GetType() == typeof(Unit) )
            {
                GameObject hitObject = findHitObject();
                Vector3 hitPoint = findHitPoint();
                //make a popmenu
                player.mHud.populatePopMenu(player.mSelectedList[0], hitObject, hitPoint);
            }
        }
    }

    private void rotateCamera()
    {

        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        //detect rotation amount if ALT is being held and the Right mouse button is down
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * Globals.RotateAmount;
            destination.y += Input.GetAxis("Mouse X") * Globals.RotateAmount;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * Globals.RotateSpeed);
        }


    }





    /*





    private void MouseHover()
    {
        if (player.hud.MouseInBounds())
        {
            GameObject hoverObject = FindHitObject();
            if (hoverObject)
            {
                if (player.SelectedObject) player.SelectedObject.SetHoverState(hoverObject);
                else if (hoverObject.name != "Ground")
                {
                    Player owner = hoverObject.transform.root.GetComponent<Player>();
                    if (owner)
                    {
                        Unit unit = hoverObject.transform.parent.GetComponent<Unit>();
                        Building building = hoverObject.transform.parent.GetComponent<Building>();
                        if (owner.username == player.username && (unit || building)) player.hud.SetCursorState(CursorState.Select);
                    }
                }
            }
        }
    }*/

}
