using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//wrapper class for tab panes in TabMenu

public class TabPane : Menu
{
    //public members
    public Menu mSubMenu;

    //private members
    private Button mButton;
    private Text mButtonText;
    

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
        //get the button script
        mButton = GetComponentInChildren<Button>();
        if (!mButton)
            Debug.LogError("TabPane Buton is missing");
        mButtonText = mButton.GetComponentInChildren<Text>();
    }


    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public void setMenu(Menu menu)
    {
        mSubMenu = menu;
        menu.transform.SetParent(this.transform);
    }

    public void setButtonText(string tab_name)
    {
        mButtonText.text = tab_name;
    }

}
