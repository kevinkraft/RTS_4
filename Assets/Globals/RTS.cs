using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{

    public static class Globals
    {
        //scroll and camera
        public static float ScrollSpeed { get { return 35; } }
        public static float RotateSpeed { get { return 100; } }
        public static int ScrollWidth { get { return 15; } }
        public static float MinCameraHeight { get { return 10; } }
        public static float MaxCameraHeight { get { return 40; } }
        public static float RotateAmount { get { return 10; } }

        //mouse clicks
        private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
        public static Vector3 InvalidPosition { get { return invalidPosition; } }

        //popmenu 
        public static float POP_MENU_BUTTON_HEIGHT { get { return 35; } }
        public static float POP_MENU_WIDTH { get { return 80; } }
        public static float POP_MENU_BUTTON_BORDER { get { return 5; } }

    }

    public static class UI
    {
        //selection box
        private static GUISkin selectBoxSkin;
        public static GUISkin SelectBoxSkin { get { return selectBoxSkin; } }
        public static void StoreSelectBoxSkin(GUISkin skin)
        {
            selectBoxSkin = skin;
        }

    }

    public static class ObjectManager
    {
        //private members
        private static GameObjectList mGameObjectList;

        //get functions
        public static void setGameObjectList(GameObjectList objectList)
        {
            mGameObjectList = objectList;
        }

        public static GameObject getAction(string act_name)
        {
            return mGameObjectList.getAction(act_name);
        }
        public static GameObject getItem(string item_name)
        {
            return mGameObjectList.getItem(item_name);
        }
        public static GameObject getMenu(string menu_name)
        {
            return mGameObjectList.getMenu(menu_name);
        }

        //init action functions
        public static Attack initAttack(Transform parent)
        {
            Attack move = GameObject.Instantiate(getAction("Attack"), parent).GetComponent<Attack>();
            return move;
        }
        public static Movement initMove(Transform parent)
        {
            Movement move = GameObject.Instantiate(getAction("Movement"),parent).GetComponent<Movement>();
            return move;
        }
        public static Exchange initExchange(Transform parent)
        {
            Exchange ex = GameObject.Instantiate(getAction("Exchange"), parent).GetComponent<Exchange>();
            return ex;
        }

        //init item function
        public static Item initItem(GameTypes.ItemType type, Transform parent)
        {
            Item item = GameObject.Instantiate(getItem(type.ToString()), parent).GetComponent<Item>();
            item.mAmount = 0f;
            item.mType = type;
            return item;

        }

    }

    public static class GameTypes
    {

        //unknown must be first?
        public enum ItemType { Unknown, Food, Wood }

    }

}
