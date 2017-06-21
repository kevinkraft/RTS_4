using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script that controls the tab menu UI object

//Notes:
// * currently it's only setup with three tabs
//   * to change this you'll need to alter the prefab, just save the TabPane prefab
//     and make as many as you need, scaling the length of th etab buttons where appropriate

public class TabMenu : Menu
{

    //private members
    public List<TabPane> mTabPanes;


    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Start()
    {
        base.Start();
        TabPane[] panes_array = GetComponentsInChildren<TabPane>();
        if ( panes_array.Length == 0 )
            Debug.LogError("TabMenu's array of TabPanes is empty");
        mTabPanes = new List<TabPane>(panes_array);
        if (mTabPanes.Count == 0)
            Debug.LogError("TabMenu's list of TabPanes is empty");
        //foreach (TabPane pane in mTabPanes)
        //    pane.gameObject.SetActive(false);
    }

    //-------------------------------------------------------------------------------------------------
    //  public methods
    //-------------------------------------------------------------------------------------------------

    public void addMenu(int index, Menu menu, string tab_name)
    {
        if (index > mTabPanes.Count - 1)
            Debug.LogError("given index is out of range of the number of TabPanes.");
        mTabPanes[index].setMenu(menu);
        mTabPanes[index].setButtonText(tab_name);
        mTabPanes[index].mSubMenu.setPositions(0, 0, 0, 0, 0, 0, 1, 0.8783909f);

    }


}
