using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Player : MonoBehaviour 
{

    public string mUsername;
    public bool mHuman;
    public HUD mHud;
    //public Entity mSelectedObject { get; set; }
    public List<Entity> mSelectedList = new List<Entity>(); 
	//public WorldManager mWorldManager; //dont need this, just use transform.root

	//private members
	private Nation mNation; 

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
        mHud = GetComponentInChildren<HUD>();
		mNation = GetComponentInChildren<Nation>();
		if ( !mNation && mUsername != "NeutralPlayer" )
			Debug.LogError("Nation not found in player");
		/*//get the parent world manager
		mWorldManager = GetComponentInParent<WorldManager>();
		if (!mWorldManager && !transform.parent.GetComponentInParent<GameObjectList>() )
			Debug.LogError ("No WorldManager found as parent.");*/
    }
    void Start ()
    {
    }
	void Update ()
    {
		if (mHuman)
		{
        	if (mSelectedList.Count == 0)
            	mHud.clearSelection();
            //check if there is a popmenu and if it has an outcome
	        if (mHuman && mHud.mPopMenu.isActive())
	        {
	            //Debug.Log("Need to fix PopMenu to work with multiple selections");
	            /*if (mHud.mPopMenu.mOutcome == true)
	            {
	                EntityAction ent_act = (EntityAction)mSelectedObject;
	                //if control is held down queue the action
	                if (ent_act)
	                    if (Input.GetKey(KeyCode.LeftControl))
	                        ent_act.addAction(mHud.mPopMenu.mOutcomeAction, "append");
	                    else
	                        ent_act.addAction(mHud.mPopMenu.mOutcomeAction);
	                mHud.mPopMenu.setActive(false);
	            }*/
	            if (mHud.mPopMenu.mOutcome == true)
	            {
	                foreach (Entity ent in mSelectedList)
	                {
	                    EntityAction ent_act = (EntityAction)ent;
	                    //if control is held down queue the action
	                    if (ent_act)
	                    {
	                        if (Input.GetKey(KeyCode.LeftControl))
	                            ent_act.addAction(Instantiate(mHud.mPopMenu.mOutcomeAction, ent_act.transform), "append");
	                        else
	                            ent_act.addAction(Instantiate(mHud.mPopMenu.mOutcomeAction, ent_act.transform));
	                    }
	                }
	                 mHud.mPopMenu.setActive(false);
	             }
			}
			//make icons to display info to the user
			setIdleUnitIcons();

		}
    }

    void OnGUI()
    {
        if ( mHuman )
        {
            if ( mSelectedList.Count != 0 )
            {
                //mHud.drawSelectionBox(mSelectedObject);
                //mHud.drawSideBar(mSelectedObject);
                mHud.drawSelectionBox(mSelectedList);
                if (mSelectedList.Count == 1)
                    mHud.drawSideBar(mSelectedList[0]);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    public void clearSelection()
    {
        //mSelectedObject = null;
        mSelectedList.Clear();
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
	private List<Building> getAllBuildings()
	{
		//get all the buildings in this players nation
		return mNation.getAllBuildings();
	}
	private List<Unit> getAllUnits()
	{
		//get all the units in this players nation
		return mNation.getAllUnits();
	}

	private void setIdleUnitIcons()
	{
		//add caution icons to to Units that are idle
		//first get all the units controlled by this player
		List<Unit> ulist = getAllUnits();
		foreach (Unit unit in ulist)
		{
			mHud.makeIconIfNeeded(unit);
		}
		List<Building> blist = getAllBuildings();
		foreach (Building b in blist)
		{
			mHud.makeIconIfNeeded(b);
		}



	}


}
