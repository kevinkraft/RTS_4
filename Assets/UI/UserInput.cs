using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RTS;

public class UserInput : MonoBehaviour {

    //private members
    private Player player;
    
    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------

    // Use this for initialization
    void Start ()
    {

        player = transform.root.GetComponent<Player>();
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
        if (player.mHud.mouseInBounds())
        {
            //Debug.Log("Mouse in bound after left click.");
            GameObject hitObject = findHitObject();
            //if (hitObject)
            //    Debug.Log("Clicked on a valid game object.");
            Vector3 hitPoint = findHitPoint();
            if (hitObject && hitPoint != Globals.InvalidPosition)
            {
                if (player.mSelectedObject)
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
                        player.mSelectedObject = ent;
                    }
                }
            }
        }
    }

    private void mouseActivity()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (Input.GetMouseButtonDown(0)) leftMouseClick();
        else if (Input.GetMouseButtonDown(1)) rightMouseClick();
        //MouseHover();

    }

    private void moveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0, 0, 0);

        bool mouseScroll = false;

        //horizontal camera movement
        if (xpos >= 0 && xpos < Globals.ScrollWidth)
        {
            movement.x -= Globals.ScrollSpeed;
            //player.hud.SetCursorState(CursorState.PanLeft);
            mouseScroll = true;
        }
        else if (xpos <= Screen.width && xpos > Screen.width - Globals.ScrollWidth)
        {
            movement.x += Globals.ScrollSpeed;
            //player.hud.SetCursorState(CursorState.PanRight);
            mouseScroll = true;
        }

        //vertical camera movement
        if (ypos >= 0 && ypos < Globals.ScrollWidth)
        {
            movement.z -= Globals.ScrollSpeed;
            //player.hud.SetCursorState(CursorState.PanDown);
            mouseScroll = true;
        }
        else if (ypos <= Screen.height && ypos > Screen.height - Globals.ScrollWidth)
        {
            movement.z += Globals.ScrollSpeed;
            //player.hud.SetCursorState(CursorState.PanUp);
            mouseScroll = true;
        }

        //make sure movement is in the direction the camera is pointing
        //but ignore the vertical tilt of the camera to get sensible scrolling
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        //away from ground movement
        movement.y -= Globals.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

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
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * Globals.ScrollSpeed);
        }

        if (!mouseScroll)
        {
            //player.hud.SetCursorState(CursorState.Select);
        }

    }

    private void rightMouseClick()
    {
        Entity sel = player.mSelectedObject;
        if (player.mHud.mouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && sel )
        {
            
            if ( sel.GetType() == typeof(Unit) )
            {
                Unit unit = (Unit) sel;
                GameObject hitObject = findHitObject();
                EntityHP ent_hp = hitObject.transform.GetComponentInParent<EntityHP>();
                Vector3 hitPoint = findHitPoint();
               /* //attack if its an EntityHP
                if (ent_hp)
                {
                    unit.attack(ent_hp);
                }
                //move if its valid empty space
                else if (hitPoint != Globals.InvalidPosition)
                {
                    unit.moveTo(hitPoint);
                }*/

                //make a popmenu
                player.mHud.populatePopMenu(sel, hitObject, hitPoint);
                
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
