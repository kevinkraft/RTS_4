using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class ItemGroup : MonoBehaviour
{

    //public members
    public List<Item> mItems = new List<Item>();
    public int mCapacity; //-1 is no cap

    //private members

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------

    private void Awake()
    {
        mItems = new List<Item>( GetComponentsInChildren<Item>() );
    }
    private void Update()
    {
        //remove an item if it has 0 or less
        GameTypes.ItemType remove_type = GameTypes.ItemType.Unknown;
        int i = 0;
        foreach ( Item item in mItems)
        {
            if (item.mAmount <= 0)
            {
                remove_type = item.mType;
                break;
            }
            i++;
        }
        if (remove_type != GameTypes.ItemType.Unknown)
        {
            Destroy(mItems[i].gameObject);
            mItems.RemoveAt(i);
        }
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public Dictionary<GameTypes.ItemType, int> getInventoryDictionary()
    {
        //returns a dictionary of the group
        Dictionary<GameTypes.ItemType, int> rdict = new Dictionary<GameTypes.ItemType, int>();
        foreach (Item item in mItems)
        {
            rdict.Add(item.mType, item.mAmount);
        }
        return rdict;
    }

    public bool addItem(Item item)
    {
        if (getSize() + item.mAmount > mCapacity)
        {
            Debug.Log("Not enough space in the ItemGroup for this item.");
            return false;
        }
        else
        {
            mItems.Add(item);
            return true;
        }
    }

    public int getFreeSpace()
    {
        int fs = mCapacity - getSize();
        if (fs > 0)
            return fs;
        else
			return 0;
    }

    public Item getItemOfType(GameTypes.ItemType type)
    {
        foreach (Item item in mItems)
        {
            if (item.mType == type)
                return item;    
        }
        return null;
    }

    public int getSize()
    {
        //returns the sum of the sizes of everything in the group
        int size = 0;
        foreach (Item item in mItems)
        {
            size += item.mAmount;
        }
        return size;
    }

    public bool isFull()
    {
        if ( mCapacity - getSize() <= 0)
            return true;
        else
            return false;
    }

    public string print()
    {
        //returns a string with one item on each line showing the name and amount
        string text = "";
        if (mItems.Count == 0)
            return "Empty";
        foreach ( Item item in mItems )
        {
            text += string.Format("{0} : {1}\n", item.mType.ToString(), item.mAmount);
        }
        return text;
    }

    public void wipe()
    {
        //deletes the Items, then resets the Item list to be empty
        foreach(Item item in mItems)
        {
            Destroy(item.gameObject);
        }
        mItems = new List<Item>();
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------



}