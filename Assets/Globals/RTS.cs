﻿using System.Collections;
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
        public static float POP_MENU_LIST_WIDTH { get { return 100f; } }

        //units
        public static float UNIT_PREGNANCY_CYCLE_PROGRESS { get { return 0.05f; } }
        //public static float UNIT_HUNGER_CYCLE_INCREASE { get { return 0.001f; } } //too low
        public static float UNIT_HUNGER_CYCLE_INCREASE { get { return 0.01f; } } //this is a good value with hunger_value = 10f, default
        //public static float UNIT_HUNGER_CYCLE_INCREASE { get { return 0.05f; } } //for testing
        //public static float UNIT_HUNGER_CYCLE_INCREASE { get { return 0.1f; } } //for testing
        public static float UNIT_HUNGRY_DAMAGE_TAKEN { get { return 0.05f; } } // default
        //public static float UNIT_HUNGRY_DAMAGE_TAKEN { get { return 0.1f; } } //for testing
        //the value of 1 Food in terms of unit hunger
        public static float UNIT_FOOD_HUNGER_VALUE { get { return 10f; } } 

        //Materials needed for Construction types
        public static Dictionary<GameTypes.ItemType, float> MAINHUT_CONSTRUCTION_MATERIALS { get { return new Dictionary<GameTypes.ItemType, float>()
        {
            {GameTypes.ItemType.Wood, 50}
        }; } } //default =50
        public static Dictionary<GameTypes.ItemType, float> STOCKPILE_CONSTRUCTION_MATERIALS { get { return new Dictionary<GameTypes.ItemType, float>()
        {
            {GameTypes.ItemType.Wood, 100} //default=100
        }; }}
        public static Dictionary<GameTypes.ItemType, float> FARM_CONSTRUCTION_MATERIALS { get { return new Dictionary<GameTypes.ItemType, float>()
        {
            {GameTypes.ItemType.Wood, 30} //default=30
        }; }}



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
        public static GameObject getResource(string res_name)
        {
            return mGameObjectList.getResource(res_name);
        }
        public static GameObject getUnit(string unit_name)
        {
            return mGameObjectList.getUnit(unit_name);
        }
        public static GameObject getBuilding(string b_name)
        {
            GameObject go = mGameObjectList.getBuilding(b_name);
            if (!go)
                Debug.LogError(string.Format("Building name {0} not found", b_name));
            return go;
        }
        public static GameObject getConstruction(string c_name)
        {
            return mGameObjectList.getConstruction(c_name);
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
        public static Wait initWait(Transform parent)
        {
            Wait wt = GameObject.Instantiate(getAction("Wait"), parent).GetComponent<Wait>();
            return wt;
        }
        public static Exchange initExchange(Transform parent)
        {
            Exchange ex = GameObject.Instantiate(getAction("Exchange"), parent).GetComponent<Exchange>();
            return ex;
        }
        public static Garrison initGarrison(Transform parent)
        {
            Garrison gar = GameObject.Instantiate(getAction("Garrison"), parent).GetComponent<Garrison>();
            return gar;
        }
        public static Procreate initProcreate(Transform parent)
        {
            Procreate proc = GameObject.Instantiate(getAction("Procreate"), parent).GetComponent<Procreate>();
            return proc;
        }
        public static Collect initCollect(Transform parent)
        {
            Collect coll = GameObject.Instantiate(getAction("Collect"), parent).GetComponent<Collect>();
            return coll;
        }
        public static Construct initConstruct(Transform parent)
        {
            Construct constr = GameObject.Instantiate(getAction("Construct"), parent).GetComponent<Construct>();
            return constr;
        }
        public static   Eat initEat(Transform parent)
        {
            Eat eat = GameObject.Instantiate(getAction("Eat"), parent).GetComponent<Eat>();
            return eat;
        }
        public static Work initWork(Transform parent)
        {
            Work wr = GameObject.Instantiate(getAction("Work"), parent).GetComponent<Work>();
            return wr;
        }

        //init item function
        public static Item initItem(GameTypes.ItemType type, Transform parent)
        {
            Item item = GameObject.Instantiate(getItem(type.ToString()), parent).GetComponent<Item>();
            item.mAmount = 0f;
            item.mType = type;
            return item;

        }

        //init unit functions
        public static Unit initUnit( Vector3 pos, GameTypes.GenderTypes gender, Town town)
        {
            Unit unit = GameObject.Instantiate(getUnit("Unit"), town.gameObject.transform).GetComponent<Unit>();
            unit.mGender = gender;
            unit.gameObject.transform.position = pos;
            town.addEntity("units", unit);
            return unit;
        }
        
        //init building functions
        public static Building initBuilding(Vector3 pos, GameTypes.BuildingType bt, Town town)
        {
            Building b = GameObject.Instantiate(getBuilding(bt.ToString()), town.gameObject.transform).GetComponent<Building>();
            b.gameObject.transform.position = pos;
            b.mType = bt;
            town.addEntity("buildings", b);
            return b;
        }

        //init Construction function
        public static Construction initConstruction(Vector3 pos, GameTypes.BuildingType bt, Town town)
        {
            Construction constro = GameObject.Instantiate(getConstruction("Construction"), town.gameObject.transform).GetComponent<Construction>();
            constro.gameObject.transform.position = pos;
            constro.mType = bt;
            town.addEntity("constructions", constro);
            return constro;
        }


    }

    public static class GameTypes
    {

        //unknown must be first
        public enum ItemType { Unknown, Food, Wood };

        //unknown must be first
        public enum BuildingType { Unknown, MainHut, Stockpile, Farm };

        //unknown must be first
        public enum GenderTypes { Unknown, Male, Female };
    }

}
