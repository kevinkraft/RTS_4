
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS;

//class for a menu for user selection of what to exchange between EntityActions

//Notes:
// * The ExchangeAction gives a target and an acter and this menu returns an ExchangeList dictionary
// * Remebmer, a positive amount value moves items from the acter to the target
// * User doesn't have to press enter when inputting the number

public class ExchangeMenu : Menu
{
    //public methods
    public Dropdown mItemDropdown;
    public InputField mNumberField;
    public Text mDisplayText;
    public bool mInUse = false;

    //private members
    //private EntityAction mActer;
    //private EntityAction mTarget;
    private bool mMadeSelection = false;
    private bool mPopulated = false;
    private Dictionary<GameTypes.ItemType,int> mExchangeList = new Dictionary<GameTypes.ItemType, int>();
    private int mUserAmount;
    private bool mCancelled = false;
    private List<string> mOptStrings;
    private Dictionary<GameTypes.ItemType, KeyValuePair<int, int>> mOptDict;
    private GameTypes.ItemType mChoiceType = GameTypes.ItemType.Unknown;
    private int mChoiceMin;
    private int mChoiceMax;
    private int mChoiceValue;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void acceptChoice()
    {
        //accept the current exchange list and proceed with the action
        mMadeSelection = true;
    }

    public void addChoice()
    {
        //Debug.Log(mChoiceValue.ToString());
        //Debug.Log(mChoiceType.ToString());
        //check if an item of this type has already been added
        if (mExchangeList.ContainsKey(mChoiceType))
            Debug.Log("This Item type has already been added to the selection. If you want to change the amount, remove it first.");
        else
        {
            mExchangeList.Add(mChoiceType, mChoiceValue);
            updateSelection();
        }
        //call set number filed so that if it hasnt already been set and defaulted to the max value, it will now default
        //to something else
        numberFieldSet("0");

    }

    public void cancelled()
    {
        mExchangeList = new Dictionary<GameTypes.ItemType, int>();
        mMadeSelection = true;
        mCancelled = true;
        clear();
    }

    public void clear()
    {
        mMadeSelection = false;
        mPopulated = false;
        mItemDropdown.ClearOptions();
        mExchangeList.Clear();
        mDisplayText.text = "";
        transform.SetAsFirstSibling();
        gameObject.SetActive(false);
        mInUse = false;
    }

    public void dropdownChanged()
    {
        //get the chosen string
        int index = mItemDropdown.value;
        string choice = mOptStrings[index];
        //get the type from the chosen string
        string[] splitString = choice.Split('(');
        if (splitString.Length != 2)
            Debug.LogError("Reading of the choice string failed.");
        choice = splitString[0];
        //get type from the string
        GameTypes.ItemType type = (GameTypes.ItemType) System.Enum.Parse(typeof(GameTypes.ItemType), choice);
        if (type == 0)
            Debug.LogError("Reading of the choise string as an Item type failed.");
        //set chosen type
        mChoiceType = type;
        //set the min and max
        if ( !mOptDict.ContainsKey(type) )
            Debug.LogError("The options dictionary does not contain the chosen option?");
        mChoiceMin = mOptDict[type].Key;
        mChoiceMax = mOptDict[type].Value;
        //reset the number field
        mNumberField.text = "";
        //Debug.Log(mChoiceType.ToString());
        //Debug.Log(mChoiceMin.ToString());
        //Debug.Log(mChoiceMax.ToString());

    }

    public Dictionary<GameTypes.ItemType, int> getExchangeList()
    {
        //return a cloned dictionary 
        Dictionary<GameTypes.ItemType, int> fdict = new Dictionary<GameTypes.ItemType, int>();
        //Debug.Log("size of exchange list in menu is " + mExchangeList.Count.ToString());
        if (mMadeSelection)
            fdict = mExchangeList;
        Dictionary<GameTypes.ItemType, int> cdict = new Dictionary<GameTypes.ItemType, int>(fdict);
        //clear();
        return cdict;

    }

    public bool hasMadeSelection()
    {
        return mMadeSelection;
    }

    public bool isCancelled()
    {
        return mCancelled;
    }

    public bool isPopulated()
    {
        return mPopulated;
    }

    /*public void numberFieldChanged(string new_num)
    {
        float num = float.Parse(new_num);
        Debug.Log(string.Format("Given number {0}",num));
        float clamped = Mathf.Clamp(num, mChoiceMin, mChoiceMax);
        Debug.Log(string.Format("Clamped number {0}", clamped));
        //reset the text
        mNumberField.text = clamped.ToString();
    }*/
	
    public void numberFieldSet(string new_num)
    {
		//Debug.Log(new_num);
        int num = int.Parse(new_num);
        //Debug.Log(string.Format("Given number {0}", num));
        //Debug.Log(string.Format("min number {0}", mChoiceMin));
        //Debug.Log(string.Format("max number {0}", mChoiceMax));
        //clamp number
        if (num < mChoiceMin)
            num = mChoiceMin;
        else if (num > mChoiceMax)
            num = mChoiceMax;
        //Debug.Log(string.Format("Clamped number {0}", num));
        //reset the text
        mNumberField.text = num.ToString();
        mChoiceValue = num;
    }

    public void populate(EntityAction acter, EntityAction target)
    {
        gameObject.SetActive(true);
        mCancelled = false;
        mInUse = true;
        //add options to the acter drop down
        //List<string> act_opts = new List<string> { "Item1", "Item2", "Item3", "Item4" };
        //mItemDropdown.AddOptions(act_opts);
        //make  lsit of potential options
        Dictionary<GameTypes.ItemType, int> act_items = acter.getInventoryDictionary();
        Dictionary<GameTypes.ItemType, int> tar_items = target.getInventoryDictionary();
        Dictionary<GameTypes.ItemType, KeyValuePair<int, int>> opts_dict = new Dictionary<GameTypes.ItemType, KeyValuePair<int, int>>();
        KeyValuePair<int, int> temp_kp = new KeyValuePair<int, int>();
        foreach (KeyValuePair<GameTypes.ItemType, int> it in act_items )
        {
            if ( !opts_dict.ContainsKey(it.Key) )
            {
                //the opt dict doesnt already contain this key
                //get the max value which is how much the acter can give
                temp_kp = new KeyValuePair<int, int>(0, it.Value);
                opts_dict.Add(it.Key, temp_kp);
            }
            else
            {
                Debug.LogError("Opts Dict already contains the ItemType key. This shoudln't be possible if the inventory only has one of each item as it should.");
            }
        }
        //now fill the min value with the target items
        foreach (KeyValuePair<GameTypes.ItemType, int> it in tar_items)
        {
            if (opts_dict.ContainsKey(it.Key))
            {
                //the acter also has this item type
                opts_dict[it.Key] = new KeyValuePair<int, int>(-1*it.Value, opts_dict[it.Key].Value);
            }
            else
            {
                //the acter didnt contain this item and we need to make a ew entry
                temp_kp = new KeyValuePair<int, int>(-1*it.Value, 0);
                opts_dict.Add(it.Key, temp_kp);
            }
        }
        //now make a string for each entry
        List<string> opt_strings = new List<string>();
        GameTypes.ItemType first_opt = GameTypes.ItemType.Unknown;
        int i = 0;
        foreach (GameTypes.ItemType type in opts_dict.Keys)
        {
            opt_strings.Add( string.Format("{0}({1}:{2})",type.ToString(), opts_dict[type].Key, opts_dict[type].Value) );
            if (i==0)
                first_opt = type;
            i++;
        }
        mOptDict = opts_dict;
        mOptStrings = opt_strings;
        mItemDropdown.AddOptions(opt_strings);

        //set the first option as the chosen option
        if (first_opt != GameTypes.ItemType.Unknown)
        {
            mChoiceType = first_opt;
            mChoiceMin = mOptDict[first_opt].Key;
            mChoiceMax = mOptDict[first_opt].Value;
            mChoiceValue = mChoiceMax;
        }

        //bring to front of the sidebar
        transform.SetAsLastSibling();
        mPopulated = true;
    }

    public void removeChoice()
    {
        //remove the last element from the choice dict
        GameTypes.ItemType rmtype = GameTypes.ItemType.Unknown;
        foreach (KeyValuePair<GameTypes.ItemType,int> entry in mExchangeList)
        {
            rmtype = entry.Key;
        }
        if (rmtype != GameTypes.ItemType.Unknown)
        {
            mExchangeList.Remove(rmtype);
            updateSelection();
        }
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void updateSelection()
    {
        //looks at the ExchangeList and updates the display accordingly
        string display_text = "";
        foreach (KeyValuePair<GameTypes.ItemType,int> entry in mExchangeList)
        {
            float eval = entry.Value;
            if (eval >= 0)
                display_text += string.Format("Give {0} {1}\n", eval, entry.Key.ToString());
            else if (eval < 0)
                display_text += string.Format("Take {0} {1}\n", -1*eval, entry.Key.ToString());
        }
        mDisplayText.text = display_text;
    }




}
