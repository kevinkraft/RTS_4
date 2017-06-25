using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for what is displayed in the sidebar

//Notes:
// * should be a child of a canvas, when you add new menus they get the parent canvas
//   automatically
// * this should be part of the player prefab

public class SideBar : MonoBehaviour
{

    //public members


    //private members
    public TabMenu mTabMenu;
    public InfoMenu mInfoMenu;
    public InventoryMenu mInventoryMenu;
    public ActionMenu mActionMenu;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Start()
    {
        //instantiate tab menu and set its position
        mTabMenu = Instantiate( ObjectManager.getMenu("TabMenu"), transform ).GetComponent<TabMenu>();
        mTabMenu.setPositions(0, 0, 0, 0, 0, 0.1f, 1, 0.9f);
        //make the sub menus
        mInfoMenu = Instantiate(ObjectManager.getMenu("InfoMenu"), mTabMenu.transform).GetComponent<InfoMenu>();
        mInventoryMenu = Instantiate(ObjectManager.getMenu("InventoryMenu"), mTabMenu.transform).GetComponent<InventoryMenu>();
        mActionMenu = Instantiate(ObjectManager.getMenu("ActionMenu"), mTabMenu.transform).GetComponent<ActionMenu>();
        //add submenus as tabs
        mTabMenu.addMenu(0, mInfoMenu, "INFO");
        mTabMenu.addMenu(1, mInventoryMenu, "ITEMS");
        mTabMenu.addMenu(2, mActionMenu, "ACTIONS");

    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

    public void clearSelection()
    {
        mInfoMenu.clear();
        mInventoryMenu.clear();
        mActionMenu.clear();
    }

    public void populate(Entity ent)
    {
        mInfoMenu.populate(ent);
        mInventoryMenu.populate(ent);
        mActionMenu.populate(ent);
    }

}
