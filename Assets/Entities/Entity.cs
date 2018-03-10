using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//To Do:
// * Implement Trade
//   * Add Stone, so there is something to trade
//     * add drop rates for adding various Resources when a new region is made
//     * Add a desert type region
//     * Add new Resource types
//       * Copper and Tin
//   * Add a TradeDepot, which needs to be a separate class from WorkedBuilding
//     * Add TradeBuilding
//       * Has two job types
//         * Traders move to and from other TradeDepots to exchange goods
//         * Stockers just move goods around in the local Town
// * WorkedProdBuildings
//   * Add a child class of WorkedBulding, WorkedProdBuilding, that takes in items and produces something
// * Vehicles
//   * Implement Vehicles
//   * Add a Cart class
//   * Add a WorkedProdBuilding for making carts
// * Eat Action
//   * base action(done)
//   * need to implement Region, for checking available food(done)
//   * If there is no food in the stockpile the units can do anything as the Eat action is stuck at the top(done)
//     * Need to implement region and Resource list(done)
// * Explore Action(done)
//   * When the user click an invalid position, i.e outside the map the Unit should go to the(done)
//     nearest map section without a region(done)
//   * Need to implement a GridMap, which is not part of Player that knows which Region spaces(done) 
//     are empty and where to send the Unit(done)
//     * This will need to be a WorldManager(done)
//   * When the Unit enters the Empty region a new Region is made(done)
// * Need to add Town to the GameObjectsList so that they can be instantiated
//   * Currently there is no way to start a new town
// * Add a home Town attribute to Units
//   * In this way we can always check why they are loyal to, when Loyalty is implemented
//   * We can always check if they will answer commands from the user and from the leader of what ever
//     town or region they are in
// * QUALITY OF LIFE
//   * Implement rectangle select(done)
//     * Giving the same action to each entity selected(done)
//     * implement the actual selection(done)
//       * need list of visible entities (its in ObjectManager, done)
//       * need a UI box for selection (done)
//       * Need to loop through the visible entities list and see if any are in the box(done)
//       * There might be an issue with selecting through a menu, cant test this until I (ok)
//         fix the left and right clicking (yes you can, it might be ok)
//     * Will need a list of selected entities(done)
//     * Will need to cancel the info menu display(done)
//       * maybe show a new menu which gives options for the group(nothing to add yet)
//     * Will need to loop through the EntitiyActions and make the right(done)
//       action for each one(done)
//     * probably need to prevent multiple units doing an exchange(menus are made in order, ok)
//   * Icons
//     * to show idle units(done)
//     * to show hungry units
//     * to show units taking damage
//     * Icons above worked buildings to show that no one is working them(done)
//       * for this we need to fix the worked buildings(done)
//         * the building needs to know who is working it(done)
//           * and needs to know when they stop working it(done)
//         * the building needs to know how many workers it can have(ok)
//         * The workers need to know where they work, I think this is already part of the work action(done)
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
//     * Maybe its because the other towns stockpile was destroyed? (maybe, added a fix to deal with this)
//     * I'm fixing dead entities in the EntityContainers first (done)
//       * Removed dead entities from EntityContainer, but not tested(fixed, removed properly)
//          what im doing doesnt work because im looping over something and then changing it (fixed it)
//       * need to do something with Town if it has no buildings and/or Units(done)
//         * delete town if empty, delete units if there are units but no buildings and no other town(done)
//     * I played the game for ages with a few hundred units and it didnt slow down(ok)
// * GENERAL: 
//   * The same unit that initially explored a region then explores the same region again and it overlaps(fixed)
//   * Rectangle select stopped working after I selected all units
//   * Town isn't setting stockpile to be a building of type Stockpile
//   * The cancel button on the ExchangeMenu stops it from working entirely(fixed)
//     * Can reproduce, happens as soon as you press the cancel button(fixed)
//     * I had to set mCancelled = false when populating the menu (ok)
//   * When constructing, Unit get stuck at the stockpile, removng and adding an item to their inventories
//     * Its something to do with the Construction no longer needing any items
//     * I can reproduce but its hard to
//     * It may also be due to Exchange
//     * Ive neer seen it happen when the unit ic collecting wood, so it must be an exchange problem
//     * I think it was to do with small amounts oscillating around zero, so the items were being removed and added again
//       * I may have fixed it by setting the 0.01 threshold to 0.07 instead, couldnt reproduce the bug again
//       * This may have fixed the problem, but it caused problems elsewhere so I had to change it back
//   * The PopMenu doesnt appear properly when you first right click(fixed)
//     * Its fine after the first time(fixed)
//     * The problem was with setting the gameObject to false at Start(ok)
//   * Sometimes the OK button in the ExchangeMenu gets stuck and does nothing
//     * I've reproduced the problem twice but not sure how I reproduced it.
//     * Something to do with switching the selected entity, or clicking out of the ExchangeMenu before its finished
//   * The units dont get very close to the resources when you tell them to move to them(fixed)
//     * problem with Entity.pointOfTouchingBounds(fixed)
//       * need a better way of telling them where to go so that they dont collide(fixed)
//     * the resources models weren't centered in their game object position(ok)
//   * If a move action is interrupted by another action, the unit finished that action and goes back to moving it is facing the wrong direction(ok)
//     * This is because the units Move action mRotating bool has already been set to true, the unit has already finished the ratation part of its move(ok)
//       so it doesnt need to rotate again when it goes back to the move action.(fixed)
//     * I reset the destination rotation in each cycle of Movement Update.(ok)
//   * Garrison/Procreate bug
//     * Units don't return to procreate action after eating
//     * They also dont take account of the stockpile bounding box
 //      * this is because the garrisoned units have zero bounds as their models dont actually exist
// * MINOR
//   * Buildings mTown is never set(fixed)
//     * this doesnt even matter for now(fixed)
//     * there was no call to base.Start() in building (ok)
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
