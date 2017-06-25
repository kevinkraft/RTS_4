using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To Do:
// * Attack, take the model bounds into account so the unit doesnt have to walk into the building to attack it(done)
//   * this must be quite general as it applies to almost all actions, move and attack and others.(done)
//   * need to make sure the unit can attack when its mIntrRange from the bounds (done)
//     * I think bounds will have to be a default entity attribute (no it doesnt, needs to change )
//     * also the calculation of displacement between two bounds needs to be a general entity feature(done)
//       * then the modified destination can be passed to all the action classes, so none of them should contain 
//          functions that deal with bounds (done)
//     * the difference between entities needs to account for the bounds (done)
// * Inventories(done)
//   * I need to make some sort of UI to display info and stop relying on the inspector(done)
// * Need to make a pop menu(done)
// * Add an action menu to the side bar(done)
// * Exchange action(done)
//   * make the base part of the exchange action(done)
//   * also need an ExchangeMenu for user defined exchanges(done)
//     * needs a drop down menu for choosing the action(done)
//     * needs a number input box which is bounde by a min and max value(done)
//     * need a display for items already added(done)
//     * need an add button(done)
//     * need a clear or remove button(done)
// * Finish resources(do collect and see whats needed)
// * Collect(done)
//   * Need to give the units a building they can bring their collect resources back to(done)
//     * Implement stockpile(done)
//       * make model(done)
//       * need to setup the building types system(done)
//       * need to make Town(done)
//     * Will also need to implement the Town heirarchy so that the units know which stockpile to use(done)
// * Implement Town(done)
//   * For this I might need EntityContainer and EntityGroup(didnt need EntityGroup, done)
//   * need to decide how the heirarchy will work(done)
//     * probably the EntityContainer will reach into its children and find EntityContainers and EntityGroups(yes, done) 
//   * Town needs to have a unit group and a building group and needs to choose a stockpile(done)
//   * the entities need to know which town they are in(done)
//     * only buildings and units need Towns so make it an EntityHP atribute, can always change it if you add other child classes(done)
// * Procreate(done)
//   * Done in a building(done)
//     * Need to units to be in the building with the procreate action active(done)
//     * there needs to be a progress bar and a precreate speed(done)
//   * implement garrison(done)
//     * Need to ungarrison the building if its dead(done)
//     * buildings need a unit inventory(done)
//     * unit needs to know where its garrisoned(done)
//     * building info menu needs an ungarrison button(done)
//     * unit needs an ungarrison button(done)
//     * need to check if the unit is garrisoned in the move action(done)
//     * for now just add the number of garrisoned units to the building info menu(done)
// * Eat Action
//   * bas action(done)
//   * need to implement Region, for checking available food
// * Wait action(done)
//   * Unit goes to a location and goes back there unless its Action queue is cleared(done)
//   * Can be used to make units stay where you want them to(done)
// * Message Bar
//   * A UI device which gives the user useful info
//     * Tell user:
//       * When a unit dies
//       * When a unit is born
//       * When the stockpile is full
// * A clear button in the ActionMenu to wipe all actions
//   * I can see it being quite handy

//Problems
// * Sometimes the OK button in the ExchangeMenu gets stuck and does nothing
//   * I've reproduced the problem twice but not sure how I reproduced it.
// * The units dont get very close to the resources when you tell them to move to them(fixed)
//   * problem with Entity.pointOfTouchingBounds(fixed)
//     * need a better way of telling them where to go so that they dont collide(fixed)
//   * the resources models weren't centered in their game object position(ok)
// * If a move action is interrupted by another action, the unit finished that action and goes back to moving it is facing the wrong direction(ok)
//   * This is because the units Move action mRotating bool has already been set to true, the unit has already finished the ratation part of its move(ok)
//     so it doesnt need to rotate again when it goes back to the move action.(fixed)
//   * I reset the destination rotation in each cycle of Movement Update.(ok)
// * MINOR
//   * Buildings mTown is never set(fixed)
//     * this doesnt even matter for now(fixed)
//     * there was no call to base.Start() in building (ok)
//   * When garrisoned in a building, if you tell the unit to garrison in another building it 
//     walks into the othe buildings model, it doesnt take the bounds into account when it moves to
//     the other building

public class Entity : Selectable
{

    //public members


   //protected members
    protected bool mDead;
    protected Model mModel;
    protected bool isInstantiated = false;


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
            Destroy(this.gameObject);
        }
    }

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
