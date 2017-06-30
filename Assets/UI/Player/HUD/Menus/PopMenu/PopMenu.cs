using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS;

//class for the menu that appears at the click position for selection actions

//Notes:
// * All the actions are instantiate here after they are chosen
// * Also has a feature for selecting from a list of objects, namely building type 
//   to construct

public class PopMenu : Menu
{
    //public members
    public bool mOutcome;
    public string mOutcomeString;
    public Dropdown mListDropdown;
    public Button mDefaultButton;

    //private members
    public List<Button> mButtons = new List<Button>();
    private Vector3 mHitPoint;
    private float mButtonHeight = Globals.POP_MENU_BUTTON_HEIGHT;
    private float mHeight;
    private float mWidth = Globals.POP_MENU_WIDTH;
    private float mButtonBorder;
    private float mListWidth;
    private bool mActive;
    private EntityAction mSelection;
    private Entity mHitObject;
    public Action mOutcomeAction;
    private bool mListActive = false;
    private List<string> mListOptStrings = new List<string>(); 
    

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
        //populate list of buttons with th eone button that is already instantiated
        //mDefaultButton = GetComponentInChildren<Button>();
        if (!mDefaultButton)
            Debug.Log("Can't find PopMenu default button.");
        mButtonHeight = Globals.POP_MENU_BUTTON_HEIGHT;
        mButtonBorder = Globals.POP_MENU_BUTTON_BORDER;
        mWidth = Globals.POP_MENU_WIDTH;
        mActive = false;
        mOutcome = false;
    }
    private void Start()
    {
        base.Start();
        mWidth = Globals.POP_MENU_WIDTH;
        mListWidth = Globals.POP_MENU_LIST_WIDTH;
    }
    private void OnGUI()
    {
        //update the postion of the menu on the screen using the hitpoint
        if (mActive)
        //{
        //    if (mBuildListActive)
        //        setBuildListPosition();
        //    else
                setScreenPosition();
        //}
    }
    void Update()
    {
        //if (!mActive)
        //    clear();
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public bool isActive()
    {
        return mActive;
    }
    public void listOutcome()
    {
        Debug.Log("List has outcome");
        //get the chosen string
        int index = mListDropdown.value;
        if (index == 0)
            return;
        string choice = mListOptStrings[index];
        switch (choice)
        {
            case "":
                setActive(false);
                return;
            case "Cancel":
                setActive(false);
                return;
            default:
                break;
        }
        //get the type from the string
        GameTypes.BuildingType type = (GameTypes.BuildingType)System.Enum.Parse(typeof(GameTypes.BuildingType), choice);
        if (type == 0)
            Debug.LogError(string.Format("Choice type {0} not recognised", choice));
        //make the new action and anything it needs
        switch (mOutcomeString)
        {
            case "Construct":
                Construction c = ObjectManager.initConstruction(mHitPoint, type, mSelection.mTown);
                Construct constr = ObjectManager.initConstruct(mSelection.transform);
                constr.setTarget(c);
                mOutcomeAction = constr;
                break;
            default:
                Debug.LogError("Action choice not recognised");
                break;
        }
        //mListDropdown.RefreshShownValue();
        mOutcome = true;
    }
    public void populate(EntityAction selection, Entity hit_ent, Vector3 hitPoint)
    /*Get number of actions avaialble. Make the button for each action and set 
    the positions and sizes correctly*/
    {
        //Debug.Log(string.Format("width: {0}", mWidth));
        //set members
        mHitPoint = hitPoint;
        mHitObject = hit_ent;
        setActive(true);
        mOutcome = false;
        mSelection = selection;
        //clear any old buttons
        clearButtons();
        //how many available actions?
        List<string> titles = getAvailableActionStrings();
        //calculate dimensional properties
        int nbs = titles.Count;
        float bh = (nbs * mButtonHeight - (nbs + 1) * mButtonBorder) / nbs;
        float bw = mWidth - 2 * mButtonBorder;
        //loop over each action and make the buttons 
        int i = 0;
        foreach (string title in titles)
        {
            Button b = Instantiate(mDefaultButton, transform);
            Text btext = b.GetComponentInChildren<Text>();
            btext.text = title;
            setButtonAnchorPivots(b);
            RectTransform rt = (RectTransform)b.transform;
            //set the offsets/positons
            rt.offsetMin = new Vector2(mButtonBorder, -(bh * (i + 1) + mButtonBorder * (i + 1)));
            rt.offsetMax = new Vector2(mButtonBorder + bw, -mButtonBorder - bh * i - mButtonBorder * i);
            //set the function to call
            b.onClick.AddListener(() => { setOutcome(title); });
            //set it active
            b.gameObject.SetActive(true);
            mButtons.Add(b);
            i++;
        }
        mHeight = mButtons.Count * mButtonHeight;
        //set the menu position
        setScreenPosition();
    }
    public void populateListMenu()
    {
        //clear the popmenu (it doesnt clear the mOutcomeString)
        clearDropdownList();
        //turn on the relevant bools
        mListActive = true;
        mOutcome = false;
        //make the dropdown choices, for now it only BuildingTypes
        mListOptStrings = new List<string>() { "","Cancel" };
        foreach (GameTypes.BuildingType bt in System.Enum.GetValues(typeof(GameTypes.BuildingType)))
        {
            if (bt != GameTypes.BuildingType.Unknown )
                mListOptStrings.Add(bt.ToString());
        }
        mListDropdown.AddOptions(mListOptStrings);
        mListDropdown.value = 0; //must be after AddOptions
        RectTransform rt = (RectTransform)mListDropdown.transform;
        rt.sizeDelta = new Vector2(mListWidth-2*mButtonBorder, rt.sizeDelta.y);
        clearButtons();
        setScreenPosition();
    }

    public void setActive(bool b)
    {
        if (b)
        {
            mActive = true;
        }
        else
        {
            clear();
            mActive = false;
            setPositions(0, 0, mWidth, mHeight);
        }
    }
    public void setOutcome(string oname)
    {
        //mOutcomeAction = new Action();
        //EntityHP ent_hp = (EntityHP)mHitObject;
        EntityHP ent_hp = mHitObject as EntityHP;
        //EntityAction ent_act = (EntityAction)mHitObject;
        EntityAction ent_act = mHitObject as EntityAction;
        //Entity ent = (Entity)mHitObject;
        Entity ent = mHitObject as Entity;
        Resource res = mHitObject as Resource;
        Building build = mHitObject as Building;
        Construction constro = mHitObject as Construction;
        WorkedBuilding wb = mHitObject as WorkedBuilding;
        mOutcomeString = oname;
        switch (oname)
        {
            case "Move":
                //Debug.Log(string.Format("Action keyword Move chosen", oname));
                Movement move = ObjectManager.initMove(mSelection.transform);
                if (ent)
                    move.setDestination(ent);
                else if (mHitPoint != Globals.InvalidPosition)
                    move.setDestination(mHitPoint);
                mOutcomeAction = move;
                break;
            case "Wait":
                Wait wt = ObjectManager.initWait(mSelection.transform);
                if (ent)
                    wt.setDestination(ent);
                else if (mHitPoint != Globals.InvalidPosition)
                    wt.setDestination(mHitPoint);
                mOutcomeAction = wt;
                break;
            case "Attack":
                //Debug.Log(string.Format("Action keyword Attack chosen.", oname));
                Attack att = ObjectManager.initAttack(mSelection.transform);
                if (ent_hp)
                {
                    att.setTarget(ent_hp);
                    mOutcomeAction = att;
                    break;
                }
                else
                    mOutcome = false;
                setActive(false);
                return;
            case "Exchange":
                Exchange ex = ObjectManager.initExchange(mSelection.transform);
                if (ent_act)
                {
                    ex.setTarget(ent_act);
                    mOutcomeAction = ex;
                    break;
                }
                else
                    mOutcome = false;
                setActive(false);
                return;
            case "Garrison":
                Garrison gar = ObjectManager.initGarrison(mSelection.transform);
                if (build)
                {
                    gar.setTarget(build);
                    mOutcomeAction = gar;
                    break;
                }
                else
                    mOutcome = false;
                setActive(false);
                return;
            case "Procreate":
                Procreate proc = ObjectManager.initProcreate(mSelection.transform);
                if (build)
                {
                    proc.setGarrisonBuilding(build);
                    mOutcomeAction = proc;
                    break;
                }
                else
                    mOutcome = false;
                setActive(false);
                return;
            case "Collect":
                Collect coll = ObjectManager.initCollect(mSelection.transform);
                if (res)
                {
                    coll.setTarget(res);
                    mOutcomeAction = coll;
                    break;
                }
                else
                    mOutcome = false;
                setActive(false);
                return;
            case "Work":
                Work wr = ObjectManager.initWork(mSelection.transform);
                if (wr)
                {
                    wr.setTarget(wb);
                    mOutcomeAction = wr;
                    break;
                }
                else
                    mOutcome = false;
                setActive(false);
                return;
            case "Construct":
                if (constro)
                {
                    Construct constr = ObjectManager.initConstruct(mSelection.transform);
                    constr.setTarget(constro);
                    mOutcomeAction = constr;
                }
                else if (mHitPoint != Globals.InvalidPosition)
                {
                    populateListMenu();
                    //constr.setTarget(mHitPoint);
                }
                else
                {
                    mOutcome = false;
                    setActive(false);
                    return;
                }
                
                break;
            default:
                Debug.LogError(string.Format("Action keyword {0} not recoginised.",oname));
                break;
        }
        if (!mListActive)
            mOutcome = true;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

    private void clear()
    {
        clearButtons();
        clearDropdownList();
    }

    private void clearButtons()
    {
        foreach (Button b in mButtons)
            Destroy(b.gameObject);
        mButtons.Clear();
    }
    private void clearDropdownList()
    {
        //clears the dropdown list
        //Debug.Log("clearing the list menu");
        mListDropdown.ClearOptions();
        mListActive = false;
        RectTransform rt = (RectTransform)mListDropdown.transform;
        rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
    }
    private List<string> getAvailableActionStrings()
    {
        //returns a list of strings which are the names of available actions
        List<string> titles = new List<string>();
        Entity ent = mHitObject as Entity;
        if (ent)
        {
            titles.Add("Move");
            titles.Add("Wait");
        }
        else if (mHitPoint != Globals.InvalidPosition)
        {
            titles.Add("Move");
            titles.Add("Wait");
            titles.Add("Construct");
        }
        EntityHP ent_hp = mHitObject as EntityHP;
        if (ent_hp)
            titles.Add("Attack");
        EntityAction ent_act= mHitObject as EntityAction;
        if (ent_act)
            titles.Add("Exchange");
        Building build = mHitObject as Building;
        if (build)
        {
            titles.Add("Garrison");
            titles.Add("Procreate");
        }
        Resource res = mHitObject as Resource;
        if (res)
            titles.Add("Collect");
        WorkedBuilding wb= mHitObject as WorkedBuilding;
        if (wb)
            titles.Add("Work");
        Construction constr = mHitObject as Construction;
        if (constr)
            titles.Add("Construct");


        return titles;
    }
    private void setButtonAnchorPivots(Button b)
    {
        RectTransform rt = (RectTransform)b.transform;
        //set the anchor and pivots
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
    }
    private void setScreenPosition()
    {
        //sets the screen position of the menu using the hitpoint
        Vector3 screenpos = Camera.main.WorldToScreenPoint(mHitPoint);
        if (!mListActive)
        {
            setPositions(screenpos.x, screenpos.y, mWidth, mHeight);
        }
        else
            setPositions(screenpos.x, screenpos.y, mListWidth, mButtonHeight + mButtonBorder);
    }

}
