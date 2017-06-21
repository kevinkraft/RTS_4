﻿using System.Collections;
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
// * Exchange action
//   * make the base part of the exchange action(done)
//   * also need an ExchangeMenu for user defined exchanges
//     * needs a drop down menu for choosing the action
//     * needs a number input box which is bounde by a min and max value
//     * need a display for items already added
//     * need an add button
//     * need a clear or remove button
// * Finish resources
// * Collect

public class Entity : Selectable
{

    //public members


   //protected members
    protected bool mDead;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
        mDead = false;
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
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
        respoint.y = ent.transform.position.y;
        return respoint;
    }

    public virtual void mouseClick(GameObject hitObject, Vector3 hitPoint)
    {
        //process a left mouse click while this entity is selected by the player
        Debug.LogError("This function should be overwritten.");
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
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
