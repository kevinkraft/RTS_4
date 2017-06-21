using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class GameObjectList : MonoBehaviour
{

    //public members
    //public GameObject[] buildings;
    //public GameObject[] units;
    public List<GameObject> mActions;
    public List<GameObject> mItems;
    public List<GameObject> mMenus;
    //public GameObject player;

    //private members
    private static bool created = false;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
       if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            mActions = getActionComponentObjects();
            mItems = getItemComponentObjects();
            mMenus = getMenuComponentObjects();
            ObjectManager.setGameObjectList(this);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public GameObject getAction(string act_name)
    {
        for (int i = 0; i < mActions.Count; i++)
        {
            Action act = mActions[i].GetComponent<Action>();
            if (act && act.mName == act_name) return mActions[i];
        }
        return null;
    }

    public GameObject getItem(string item_name)
    {
        for (int i = 0; i < mItems.Count; i++)
        {
            Item item = mItems[i].GetComponent<Item>();
            if (item && item.mName == item_name) return mItems[i];
        }
        return null;
    }

    public GameObject getMenu(string menu_name)
    {
        for (int i = 0; i < mMenus.Count; i++)
        {
            Menu menu = mMenus[i].GetComponent<Menu>();
            if (menu && menu.mName == menu_name) return mMenus[i];
        }
        return null;
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------

    private List<GameObject> getActionComponentObjects()
    {
        //returns a list of child game objects which contain an action script component 
        //this allows to just drop the action prefabs as children in GameObjectList/Actions
        List<GameObject> act_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        //Debug.Log(string.Format("count children: {0}",child_objects.Length) );
        foreach ( Component cobj in child_objects )
        {
            //Debug.Log(cobj.name);
            if ( cobj.gameObject.GetComponent<Action>() != null )
                act_objects.Add( cobj.gameObject );
        }
        //Debug.Log(string.Format("count num with actions: {0}", act_objects.Count));
        return act_objects;
    }

    private List<GameObject> getItemComponentObjects()
    {
        //returns a list of child game objects which contain an item script component 
        //this allows to just drop the item prefabs as children in GameObjectList/Items
        List<GameObject> item_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Item>() != null)
                item_objects.Add(cobj.gameObject);
        }
        return item_objects;
    }

    private List<GameObject> getMenuComponentObjects()
    {
        //returns a list of child game objects which contain a menu script component 
        //this allows to just drop the menu prefabs as children in GameObjectList/Menus
        List<GameObject> menu_objects = new List<GameObject>();
        Component[] child_objects = this.GetComponentsInChildren<Transform>();
        foreach (Component cobj in child_objects)
        {
            if (cobj.gameObject.GetComponent<Menu>() != null)
                menu_objects.Add(cobj.gameObject);
        }
        return menu_objects;
    }



}
