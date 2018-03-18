using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//Version 0.8

//To Do:
// * I think removing the Player->Nation->Region->Town->Units/Buildings hierarchy would be good
//   * Keep the Region->Town->Units/Buildings hierarchy and have a list of Regions and towns controlled by the Nation and player
//     that is kept as an attribute of the Nation or Player.
// * Implement Trade
//   * Add Stone, so there is something to trade(done)
//     * add drop rates for adding various Resources when a new region is made(done)
//     * Add a desert type region(not yet)
//     * Add new Resource types(done)
//       * Copper and Tin(done)
//   * Add a TradeDepot, which needs to be a separate class from WorkedBuilding
//     * Add TradeBuilding
//       * Has two job types
//         * Traders move to and from other TradeDepots to exchange goods
//         * Stockers just move goods around in the local Town
// * Vehicles
//   * Implement Vehicles
//   * Add a Cart class
//   * Add a WorkedProdBuilding for making carts
// * Need to add Town to the GameObjectsList so that they can be instantiated
//   * Currently there is no way to start a new town
// * Add a home Town attribute to Units
//   * In this way we can always check why they are loyal to, when Loyalty is implemented
//   * We can always check if they will answer commands from the user and from the leader of what ever
//     town or region they are in
// * Equipment
//   * Visual for when something is equipped?
//   * I'd like to add something simpler than a weapon, but for now I dont know what else to add that is simpler.
// * QUALITY OF LIFE
//   * Icons
//     * to show hungry units
//     * to show units taking damage
//   * Show number of selected units with rectangle select
//     * Maybe add this to the message bar when you make it
//   * Message Bar
//     * A UI device which gives the user useful info
//       * Tell user:
//         * When a unit dies
//         * When a unit is born
//         * When the stockpile is full
//   * A clear button in the ActionMenu to wipe all actions
//     * I can see it being quite handy
//   * Ledger
//     * A menu with info about everything
//     * It will have multiple tabs
//       * Make a multitab menu, dont change the already existing TabMenu
//     * It will have tabs for Nation, Region(Province?), Towns, etc. everything.
//   * Idle unit button
//     * put it on the side bar
//     * it makes idle units go to the Town main building.
//   * Add randomness to Wait and Move so the units dont always stand in the same spot
//     * Only add the randomness for rectangle select
//     * Have to make sure the untis dont stand inside buildings
//   * Change the construct action when holding Ctrl, so it doesn't wipe previous actions
//     * In this way, you can string together multiple constructions
//     * pretty sure you can already do this, but need to press ctrl when selecing the building to construct
//   * Display the total population of your nation
//     * need a top tool bar for this
//   * Make farming more efficient
//     * Units need to clear their inventory before working, so they can carry the items back(done)
//     * Could also allow more units to work the farms
//     * or make farms produce more food
//   * A button to dump a units inventory would be useful

//Problems
// * PERFORMANCE
//   * After playing for awhile the game gets impossibly slow
//     * I havent seen this problem in a ling time.
//     * Maybe its because the other towns stockpile was destroyed? (maybe, added a fix to deal with this)
//     * I'm fixing dead entities in the EntityContainers first (done)
//       * Removed dead entities from EntityContainer, but not tested(fixed, removed properly)
//          what im doing doesnt work because im looping over something and then changing it (fixed it)
//       * need to do something with Town if it has no buildings and/or Units(done)
//         * delete town if empty, delete units if there are units but no buildings and no other town(done)
//     * I played the game for ages with a few hundred units and it didnt slow down(ok)
// * GENERAL: 
//   * The icon for a worked building with not enough workers is not being shown.(fixed, was a problem with a farm with its gem object off)
//   * The Eat action is taking far too long.(fixed)
//     * The problem was caused by rounding as I had changed item numbers to ints (ok)
//   * Two units can explore the same Region such that two Regions are created i nthe same place
//     * should be easy enough to fix, just add a check when finishing the Explore that the Region hasn already been explored.
//   * Rectangle select stopped working after I selected all units
//   * Town isn't setting stockpile to be a building of type Stockpile
//   * When constructing, Unit get stuck at the stockpile, removng and adding an item to their inventories
//     * Its something to do with the Construction no longer needing any items
//     * I can reproduce but its hard to
//     * It may also be due to Exchange
//     * Ive neer seen it happen when the unit ic collecting wood, so it must be an exchange problem
//     * I think it was to do with small amounts oscillating around zero, so the items were being removed and added again
//       * I may have fixed it by setting the 0.01 threshold to 0.07 instead, couldnt reproduce the bug again
//       * This may have fixed the problem, but it caused problems elsewhere so I had to change it back
//   * Sometimes the OK button in the ExchangeMenu gets stuck and does nothing
//     * I've reproduced the problem twice but not sure how I reproduced it.
//     * Something to do with switching the selected entity, or clicking out of the ExchangeMenu before its finished
//   * Garrison/Procreate bug
//     * Units don't return to procreate action after eating
//     * They also dont take account of the stockpile bounding box
 //      * this is because the garrisoned units have zero bounds as their models dont actually exist
// * MINOR
//   * When garrisoned in a building, if you tell the unit to garrison in another building it 
//     walks into the othe buildings model, it doesnt take the bounds into account when it moves to
//     the other building
//     * This is because the Unit has bounds of zero when garrisoned.
//     * Can fix by saving the bounds before garrisoning
//   * When you zoom out all the way the camera jumps back a little bit so that when you zoom in again
//     it's not in the exact same place as it was previously
//     * This is do to the if statements around zooming in UI
//   * It's possible to tilt the camera so much that the world gets clipped by the camera planes
//     * Fix the camera tilt so this cant happen. set a max for the view rotation

public class Entity : Selectable
{

    //public members


   //protected members
    protected bool mDead;
    protected Model mModel;
    protected bool isInstantiated = false;
    private bool mVisible = false;
	private bool mHasIcon;


    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
        mDead = false;
        mModel = GetComponentInChildren<Model>();
        //dont run the update function for an Entity in GameObjectList
        GameObjectList gol = GetComponentInParent<GameObjectList>();
        if (gol)
            isInstantiated = false;
        else
            isInstantiated = true;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (!isInstantiated)
            return;
        base.Update();
        if (mDead)
        {
            if (mModel.mVisibleRenderer.mVisible == true)
                ObjectManager.removeVisibleEntity(this);
            Destroy(this.gameObject);
        }
        //see if Entity is visible on screen
        checkModelVisible();
    }

    /*public void OnBecameVisible()
    {
        Debug.Log(mName + " is visible");
    }*/

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    public Bounds calculateBounds()
    {
        Bounds selectionBounds = new Bounds(this.transform.position, Vector3.zero);
        foreach (Renderer r in this.GetComponentsInChildren<Renderer>())
        {
           selectionBounds.Encapsulate(r.bounds);
        }
        return selectionBounds;
    }

    public float calculateExtentsDistance(Entity ent)
    {
        //calculates the distance between two entities taking their bounds into account.
        //calculate number of unit vectors from centre to edge of bounds
        int unitShift = this.unitsToBoundsEdge();
        int targetShift = ent.unitsToBoundsEdge();
        return (ent.transform.position - this.transform.position).magnitude - unitShift - targetShift;
    }
	public bool hasIcon()
	{
		return mHasIcon;
	}
    public Vector3 pointOfTouchingBounds(Entity ent)
    {
        //determines the vector to the point where this entity can go so that it's bounds
        //are just touching the targets bounds.
        //calculate number of unit vectors from centre to edge of bounds
        int unitShift = this.unitsToBoundsEdge();
        int targetShift = ent.unitsToBoundsEdge();

        //calculate number of unit vectors between unit centre and destination centre with bounds just touching
        int shiftAmount = targetShift + unitShift;

        //calculate direction unit needs to travel to reach destination in straight line and normalize to unit vector
        Vector3 respoint = ent.transform.position;
        Vector3 origin = transform.position;
        Vector3 direction = new Vector3(respoint.x - origin.x, 0.0f, respoint.z - origin.z);
        direction.Normalize();

        //destination = center of destination - number of unit vectors calculated above
        //this should give us a destination where the unit will not quite collide with the target
        //giving the illusion of moving to the edge of the target and then stopping
        for (int i = 0; i < shiftAmount; i++) respoint -= direction;

        //this is often too far from the target, so add a little bit more
        respoint += 1f * direction;

        respoint.y = ent.transform.position.y;
        return respoint;
    }

    public virtual void mouseClick(GameObject hitObject, Vector3 hitPoint)
    {
        //process a left mouse click while this entity is selected by the player
        Debug.LogError("This function should be overwritten.");
    }
    public void setDead(bool b)
    {
        mDead = b;
    }
	public void setHasIcon(bool b)
	{
		mHasIcon = b;
	}

    //-------------------------------------------------------------------------------------------------
    // protected methods
    //-------------------------------------------------------------------------------------------------
    protected void setModelActive(bool b)
    {
        mModel.gameObject.SetActive(b);
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void checkModelVisible()
    {
        ///what size is the visible entity list?
        //Debug.Log(string.Format("len visible entities {0}",ObjectManager.getVisibleEntities().Count));
        if (mModel.mVisibleRenderer.mVisible == true && mVisible == false)
        {
            mVisible = true;
            ObjectManager.addVisibleEntity(this);
        }
        else if (mModel.mVisibleRenderer.mVisible == false && mVisible == true)
        {
            mVisible = false;
            ObjectManager.removeVisibleEntity(this);
        }
    }
    private int unitsToBoundsEdge()
    {
        //calculates the number of unit vectors from the entity model position to the edge of 
        //the entity bounds
        Bounds acterBounds = this.calculateBounds();
        Vector3 originalExtents = acterBounds.extents;
        Vector3 normalExtents = originalExtents;
        normalExtents.Normalize();
        float numberOfExtents = originalExtents.x / normalExtents.x;
        int unitShift = Mathf.FloorToInt(numberOfExtents);
        return unitShift;
    }
}
