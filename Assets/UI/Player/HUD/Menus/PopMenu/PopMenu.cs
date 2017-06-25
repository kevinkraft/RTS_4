using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS;

public class PopMenu : Menu
{
    //public members
    public bool mOutcome;

    //private members
    private Button mDefaultButton;
    public List<Button> mButtons = new List<Button>();
    private Vector3 mHitPoint;
    private float mButtonHeight;
    private float mHeight;
    private float mWidth;
    private float mButtonBorder;
    private bool mActive;
    private EntityAction mSelection;
    private Entity mHitObject;
    public Action mOutcomeAction;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
        //populate list of buttons with th eone button that is already instantiated
        mDefaultButton = GetComponentInChildren<Button>();
        if (!mDefaultButton)
            Debug.Log("Can't find PopMenu default button.");
        mButtonHeight = Globals.POP_MENU_BUTTON_HEIGHT;
        mWidth = Globals.POP_MENU_WIDTH;
        mButtonBorder = Globals.POP_MENU_BUTTON_BORDER;
        mActive = false;
        mOutcome = false;
    }
    private void OnGUI()
    {
        //update the postion of the menu on the screen using the hitpoint
        if (mActive)
            setScreenPosition();
    }
    void Update()
    {
        if (!mActive)
            clear();
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public bool isActive()
    {
        return mActive;
    }
    public void populate(EntityAction selection, Entity hit_ent, Vector3 hitPoint)
        /*Get number of actions avaialble. Make the button for each action and set 
        the positions and sizes correctly*/
    {
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
        /*List<string> titles = new List<string>();
        EntityHP ent_hp = (EntityHP) mHitObject;
        if (ent_hp)
        {
            titles.Add("Move");
            titles.Add("Attack");
        }
        else if (hitPoint != Globals.InvalidPosition)
            titles.Add("Move");
        EntityAction ent_act = (EntityAction) mHitObject;
        if (ent_act)
            titles.Add("Exchange");*/
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
        switch (oname)
        {
            case "Move":
                //Debug.Log(string.Format("Action keyword Move chosen", oname));
                Movement move= ObjectManager.initMove(mSelection.transform);
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
            default:
                Debug.LogError(string.Format("Action keyword {0} not recoginised.",oname));
                break;
        }
        mOutcome = true;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void clear()
    {
        clearButtons();
    }
    private void clearButtons()
    {
        foreach (Button b in mButtons)
            Destroy(b.gameObject);
        mButtons.Clear();
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
        setPositions(screenpos.x,screenpos.y, mWidth, mHeight);
    }

}
