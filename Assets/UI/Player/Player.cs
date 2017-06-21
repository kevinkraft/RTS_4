using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Player : MonoBehaviour {

    public string mUsername;
    public bool mHuman;
    public HUD mHud;
    public Entity mSelectedObject { get; set; }
    // public int startMoney, startMoneyLimit, startPower, startPowerLimit;


    //private members
    //private Dictionary<ResourceType, int> resources, resourceLimits;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
        mHud = GetComponentInChildren<HUD>();
        //resources = InitResourceList();
        //resourceLimits = InitResourceList();
    }

    // Use this for initialization
    void Start ()
    {
        
        //AddStartResourceLimits();
        //AddStartResources();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (mSelectedObject == null)
            mHud.clearSelection();
        //check if there is a popmenu and if it has an outcome
        if (mHud.mPopMenu.isActive())
        {
            if (mHud.mPopMenu.mOutcome == true)
            {
                EntityAction ent_act = (EntityAction)mSelectedObject;
                //if control is held down queue the action
                if (ent_act)
                    if (Input.GetKey(KeyCode.LeftControl))
                        ent_act.addAction(mHud.mPopMenu.mOutcomeAction, "append");
                    else
                        ent_act.addAction(mHud.mPopMenu.mOutcomeAction);
                mHud.mPopMenu.setActive(false);
            }
        }
    }

    void OnGUI()
    {
        if ( mHuman )
        {
            if (mSelectedObject)
            {
                mHud.drawSelectionBox(mSelectedObject);
                mHud.drawSideBar(mSelectedObject);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    public void clearSelection()
    {
        mSelectedObject = null;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------


    /*private Dictionary<ResourceType, int> InitResourceList()
    {
        Dictionary<ResourceType, int> list = new Dictionary<ResourceType, int>();
        list.Add(ResourceType.Money, 0);
        list.Add(ResourceType.Power, 0);
        return list;
    }*/

    /*private void AddStartResourceLimits()
    {
        IncrementResourceLimit(ResourceType.Money, startMoneyLimit);
        IncrementResourceLimit(ResourceType.Power, startPowerLimit);
    }

    private void AddStartResources()
    {
        AddResource(ResourceType.Money, startMoney);
        AddResource(ResourceType.Power, startPower);
    }

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
    }

    public void IncrementResourceLimit(ResourceType type, int amount)
    {
        resourceLimits[type] += amount;
    }

    public void AddUnit(string unitName, Vector3 spawnPoint, Vector3 rallyPoint, Quaternion rotation, Building creator)
    {
        Units units = GetComponentInChildren<Units>();
        GameObject newUnit = (GameObject)Instantiate(Globals.GetUnit(unitName), spawnPoint, rotation);
        newUnit.transform.parent = units.transform;
        Unit unitObject = newUnit.GetComponent<Unit>();
        if (unitObject)
        {
            unitObject.Init(creator);
            if (spawnPoint != rallyPoint) unitObject.StartMove(rallyPoint);
        }
    }*/

}
