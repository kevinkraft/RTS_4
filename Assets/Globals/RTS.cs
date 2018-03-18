using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{

    public static class Globals
    {
        //scroll and camera
        public static float ScrollSpeed { get { return 2; } }
        public static float RotateSpeed { get { return 200; } }
        public static int ScrollWidth { get { return 20; } }
        public static float MinCameraHeight { get { return 10; } }
        //public static float MaxCameraHeight { get { return 150; } } //default
		public static float MaxCameraHeight { get { return 450; } } //testing
        public static float RotateAmount { get { return 20; } }

        //mouse clicks
        private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
        public static Vector3 InvalidPosition { get { return invalidPosition; } }

        //popmenu 
        public static float POP_MENU_BUTTON_HEIGHT { get { return 35; } }
        public static float POP_MENU_WIDTH { get { return 80; } }
        public static float POP_MENU_BUTTON_BORDER { get { return 5; } }
        public static float POP_MENU_LIST_WIDTH { get { return 100f; } }

		//IconPanel
		public static float ICON_PANEL_UNIT_OFFSET { get { return 30; } }
		public static float ICON_PANEL_UNIT_WIDTH { get { return 20; } }
		public static float ICON_PANEL_BUILDING_OFFSET { get { return 45; } }
		public static float ICON_PANEL_BUILDING_WIDTH { get { return 45; } }

        //units
        public static float UNIT_PREGNANCY_CYCLE_PROGRESS { get { return 0.05f; } }
        //public static float UNIT_HUNGER_CYCLE_INCREASE { get { return 0.001f; } } //too low
        public static float UNIT_HUNGER_CYCLE_INCREASE { get { return 0.01f; } } //this is a good value with hunger_value = 10f, default
        //public static float UNIT_HUNGER_CYCLE_INCREASE { get { return 0.05f; } } //for testing
        //public static float UNIT_HUNGER_CYCLE_INCREASE { get { return 0.1f; } } //for testing
        //public static float UNIT_HUNGRY_DAMAGE_TAKEN { get { return 0.0f; } } // for testing
        public static float UNIT_HUNGRY_DAMAGE_TAKEN { get { return 0.05f; } } // default
        //public static float UNIT_HUNGRY_DAMAGE_TAKEN { get { return 0.1f; } } //for testing
        //the value of 1 Food in terms of unit hunger
        public static float UNIT_FOOD_HUNGER_VALUE { get { return 10f; } }
		public static Dictionary<GameTypes.UnitStatType, float> UNIT_DEFAULT_FLOAT_STATS = new Dictionary<GameTypes.UnitStatType, float>()
		{
			{GameTypes.UnitStatType.InteractionRange, 1f},
			{GameTypes.UnitStatType.Attack, 1f},
			{GameTypes.UnitStatType.ExchangeSpeed, 1f},
			{GameTypes.UnitStatType.MoveSpeed, 3f},
			{GameTypes.UnitStatType.RotateSpeed, 3f},
			{GameTypes.UnitStatType.ProcreateChance, 10f},
			{GameTypes.UnitStatType.ConstructSpeed, 1f},
			{GameTypes.UnitStatType.WorkSpeed, 1f}
		};
		public static Dictionary<GameTypes.UnitStatType, int> UNIT_DEFAULT_INT_STATS = new Dictionary<GameTypes.UnitStatType, int>()
		{
			{GameTypes.UnitStatType.InventoryCapacity, 10}
		};
		public static List<GameTypes.EquipmentSlots> DEFAULT_UNIT_EQUIPMENT_SLOTS = new List<GameTypes.EquipmentSlots>()
		{
			{GameTypes.EquipmentSlots.Head},
			{GameTypes.EquipmentSlots.Feet},
			{GameTypes.EquipmentSlots.HandR},
		};

        //maps
		public static int REGION_MAP_WIDTH { get { return 300; } } //default, for now
		//public static int REGION_MAP_WIDTH { get { return 900; } } //testing, a bit large
		public static Dictionary<Vector2, GameTypes.MapType> DEFAULT_MAP_GRID_START { get { 
				Dictionary<Vector2,GameTypes.MapType> d = new Dictionary<Vector2,GameTypes.MapType>();
				//add the locations and types here
				d.Add (new Vector2(0,0), GameTypes.MapType.GrassPlain);
				//d.Add (new Vector2(0,-1), GameTypes.MapType.GrassPlain);
				return d;
				} }

        //resources
        public static int RESOURCE_DEFAULT_AMOUNT { get { return 1000; } }
		//rates that they appear in regions
		public static Dictionary<GameTypes.ItemType, float> RESOURCE_REGION_DROP_RATES = new Dictionary<GameTypes.ItemType, float>()
		{
			{GameTypes.ItemType.Unknown, 0f},
			{GameTypes.ItemType.Food, 1f},
			{GameTypes.ItemType.Wood, 1f},
			{GameTypes.ItemType.Stone, 1f}, //default 0.5f
			{GameTypes.ItemType.Copper, 0.2f},
			{GameTypes.ItemType.Tin, 0.2f}
		};
		//number of groups of this resource
		public static Dictionary<GameTypes.ItemType, int> RESOURCE_N_GROUPS = new Dictionary<GameTypes.ItemType, int>()
		{
			{GameTypes.ItemType.Unknown, 0},
			{GameTypes.ItemType.Food, Mathf.CeilToInt(REGION_MAP_WIDTH / 74f)},
			{GameTypes.ItemType.Wood, Mathf.CeilToInt(REGION_MAP_WIDTH / 74f)},
		    {GameTypes.ItemType.Stone, Mathf.CeilToInt(REGION_MAP_WIDTH / 100f)},
			{GameTypes.ItemType.Copper, Mathf.CeilToInt(REGION_MAP_WIDTH / 100f)},
			{GameTypes.ItemType.Tin, Mathf.CeilToInt(REGION_MAP_WIDTH / 100f)}
		};
		//number of resources in a group, scales with region width
		public static Dictionary<GameTypes.ItemType, int> RESOURCE_N_PER_GROUPS = new Dictionary<GameTypes.ItemType, int>()
		{
			{GameTypes.ItemType.Unknown, 0},
			{GameTypes.ItemType.Food, Mathf.CeilToInt(REGION_MAP_WIDTH / 40f)},
			{GameTypes.ItemType.Wood, Mathf.CeilToInt(REGION_MAP_WIDTH / 10f)},
			{GameTypes.ItemType.Stone, Mathf.CeilToInt(REGION_MAP_WIDTH / 60f)},
			{GameTypes.ItemType.Copper, Mathf.CeilToInt(REGION_MAP_WIDTH / 60f)},
			{GameTypes.ItemType.Tin, Mathf.CeilToInt(REGION_MAP_WIDTH / 60f)},
		};
		//the max spread of the groups of resources
		public static Dictionary<GameTypes.ItemType, float> RESOURCE_GROUP_MAX_SPREADS = new Dictionary<GameTypes.ItemType, float>()
		{
			{GameTypes.ItemType.Unknown, 0f},
			{GameTypes.ItemType.Food, REGION_MAP_WIDTH / 15f},
			{GameTypes.ItemType.Wood, REGION_MAP_WIDTH / 7f},
			{GameTypes.ItemType.Stone, REGION_MAP_WIDTH / 30f},
			{GameTypes.ItemType.Copper, REGION_MAP_WIDTH / 30f},
			{GameTypes.ItemType.Tin, REGION_MAP_WIDTH / 30f}
		};
		

        //Materials needed for Construction types
        public static Dictionary<GameTypes.ItemType, int> MAINHUT_CONSTRUCTION_MATERIALS { get { return new Dictionary<GameTypes.ItemType, int>()
        {
            {GameTypes.ItemType.Wood, 50}
        }; } } //default =50
        public static Dictionary<GameTypes.ItemType, int> STOCKPILE_CONSTRUCTION_MATERIALS { get { return new Dictionary<GameTypes.ItemType, int>()
        {
            {GameTypes.ItemType.Wood, 100}
        }; }}
        public static Dictionary<GameTypes.ItemType, int> FARM_CONSTRUCTION_MATERIALS { get { return new Dictionary<GameTypes.ItemType, int>()
        {
            {GameTypes.ItemType.Wood, 30}
        }; }}
		public static Dictionary<GameTypes.ItemType, int> SPEARWORKSHOP_CONSTRUCTION_MATERIALS { get { return new Dictionary<GameTypes.ItemType, int>()
		{
			{GameTypes.ItemType.Wood, 30}
		}; }}

		//Materials needed for Item Production types
		public static Dictionary<GameTypes.ItemType, int> STONESPEAR_PRODUCTION_MATERIALS = new Dictionary<GameTypes.ItemType, int>()
		{
			{GameTypes.ItemType.Wood, 1},
			{GameTypes.ItemType.Stone, 1}
		};


		//Equip item globals
		//the slot that the item goes into
		public static GameTypes.EquipmentSlots EQUIP_ITEM_MAGICHAT_SLOT = GameTypes.EquipmentSlots.Head;
		public static GameTypes.EquipmentSlots EQUIP_ITEM_MAGICBOOTS_SLOT = GameTypes.EquipmentSlots.Feet;
		public static GameTypes.EquipmentSlots EQUIP_ITEM_MAGICCLUB_SLOT = GameTypes.EquipmentSlots.HandR;
		public static GameTypes.EquipmentSlots EQUIP_ITEM_MAGICSWORD_SLOT = GameTypes.EquipmentSlots.HandR;
		public static GameTypes.EquipmentSlots EQUIP_ITEM_STONESPEAR_SLOT = GameTypes.EquipmentSlots.HandR;
		//the equip attributes of the item when equipped
		public static Dictionary<GameTypes.UnitStatType, float> EQUIP_ITEM_MAGICHAT_EQUIP_EFFECTS = new Dictionary<GameTypes.UnitStatType, float>()
		{
			{GameTypes.UnitStatType.WorkSpeed, 50},
			{GameTypes.UnitStatType.ProcreateChance, -100f}
		};
		public static Dictionary<GameTypes.UnitStatType, float> EQUIP_ITEM_MAGICCLUB_EQUIP_EFFECTS = new Dictionary<GameTypes.UnitStatType, float>()
		{
			{GameTypes.UnitStatType.Attack, 50},
			{GameTypes.UnitStatType.ProcreateChance, -100f}
		};
		public static Dictionary<GameTypes.UnitStatType, float> EQUIP_ITEM_MAGICBOOTS_EQUIP_EFFECTS = new Dictionary<GameTypes.UnitStatType, float>()
		{
			{GameTypes.UnitStatType.MoveSpeed, 50},
			{GameTypes.UnitStatType.ProcreateChance, -100f}
		};
		public static Dictionary<GameTypes.UnitStatType, float> EQUIP_ITEM_MAGICSWORD_EQUIP_EFFECTS = new Dictionary<GameTypes.UnitStatType, float>()
		{
			{GameTypes.UnitStatType.Attack, 50},
			{GameTypes.UnitStatType.ProcreateChance, -100f}
		};
		public static Dictionary<GameTypes.UnitStatType, float> EQUIP_ITEM_STONESPEAR_EQUIP_EFFECTS = new Dictionary<GameTypes.UnitStatType, float>()
		{ {GameTypes.UnitStatType.Attack, 4} };
		//mapping between unit stat type and the display text 
		public static Dictionary<GameTypes.UnitStatType, string> UNIT_STATS_DISPLAY_TEXT = new Dictionary<GameTypes.UnitStatType, string>()
		{
			{GameTypes.UnitStatType.Unknown, "Unknw"},
			{GameTypes.UnitStatType.InteractionRange, "Rng"},
			{GameTypes.UnitStatType.Attack, "Att"},
			{GameTypes.UnitStatType.ExchangeSpeed, "ExchSpd"},
			{GameTypes.UnitStatType.MoveSpeed, "Spd"},
			{GameTypes.UnitStatType.RotateSpeed, "RotSpd"},
			{GameTypes.UnitStatType.ProcreateChance, "Sx%"},
			{GameTypes.UnitStatType.ConstructSpeed, "ConstrSpd"},
			{GameTypes.UnitStatType.InventoryCapacity, "InvCap"},
			{GameTypes.UnitStatType.WorkSpeed, "WrkSpd"},
		};


		//PRODUCTION BUILDING CREATE ITEMS
		public static GameTypes.ItemType SPEARWORKSHOP_PRODUCTION_ITEM = GameTypes.ItemType.StoneSpear;


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

        //visible entity list
        private static List<Entity> mVisibleEntities = new List<Entity>();

        public static void addVisibleEntity(Entity ent)
        {
            foreach (Entity inent in mVisibleEntities)
            {
                if (inent == ent)
                {
                    Debug.LogError("This entity is already in the visible list.");
                    return;
                }
            }
            mVisibleEntities.Add(ent);
        }

        public static void removeVisibleEntity(Entity ent)
        {
            bool in_list = false;
            foreach (Entity inent in mVisibleEntities)
            {
                if (inent == ent)
                    in_list = true;
            }
            if (in_list == false)
            {
                Debug.LogError("This entity is not in the visible list.");
                return;
            }
            mVisibleEntities.Remove(ent);
        }

        public static List<Entity> getVisibleEntities()
        {
            return mVisibleEntities;
        }

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

        public static GameObject getMap(string m_name)
        {
            return mGameObjectList.getMap(m_name);
        }

		public static GameObject getRegion(string r_name)
		{
			return mGameObjectList.getRegion(r_name);
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
		public static Explore initExplore(Transform parent)
		{
			Explore exp = GameObject.Instantiate(getAction("Explore"),parent).GetComponent<Explore>();
			return exp;
		}

        //init item function
        public static Item initItem(GameTypes.ItemType type, Transform parent)
        {
            Item item = GameObject.Instantiate(getItem(type.ToString()), parent).GetComponent<Item>();
			item.setAmount(0);
			item.setType(type);
            return item;

        }

        //init unit functions
        public static Unit initUnit( Vector3 pos, GameTypes.GenderType gender, Town town)
        {
            Unit unit = GameObject.Instantiate(getUnit("Unit"), town.gameObject.transform).GetComponent<Unit>();
			unit.setGender(gender);
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

        //init map function
        public static Map initMap(Vector3 pos, GameTypes.MapType mt, Region reg, int seed)
        {
            Map map = GameObject.Instantiate(getMap(mt.ToString()), reg.gameObject.transform).GetComponent<Map>();
            map.gameObject.transform.position = pos;
            map.mType = mt;
            reg.addMap(map);
            map.mSeed = seed;
            return map;
        }
		//init Region function
		public static Region initRegion(Vector2 grid_pos, GameTypes.MapType mt, WorldManager wm, int seed)
		{
			Region reg = GameObject.Instantiate(getRegion("Region"), wm.gameObject.transform).GetComponent<Region>();
			reg.setGridPos(grid_pos);
			reg.mType = mt;
			reg.mSeed = seed;
			//wm.addRegion(reg);
			return reg;
		}
        //init Resource function
        public static Resource initResource(Vector3 pos, GameTypes.ItemType type, Region reg)
        {
            Resource res = GameObject.Instantiate(getResource(type.ToString()), reg.getResourceObject().transform).GetComponent<Resource>();
            res.gameObject.transform.position = pos;
            res.mType = type;
            reg.addEntity("resources",res);
            res.mAmount = Globals.RESOURCE_DEFAULT_AMOUNT;
            return res;
        }



    }

    public static class GameTypes
    {

        //unknown must be first
		public enum ItemType { Unknown, Food, Wood, Stone, Copper, Tin, MagicHat, MagicBoots, MagicClub, MagicSword, StoneSpear };
		//public enum ItemType {Unknown, Food};

        //unknown must be first
        public enum BuildingType { Unknown, MainHut, Stockpile, Farm, SpearWorkshop };

        //unknown must be first
        public enum GenderType { Unknown, Male, Female };

        //unknown must be first
        public enum MapType { Unknown, GrassPlain};

		//unknown must be first
		public enum IconType { Unknown, Caution};

		//unknown must be first
		public enum EquipmentSlots { Unknown, Head, Feet, HandR };

		//unknown must be first
		public enum UnitStatType { Unknown, InteractionRange, Attack, ExchangeSpeed, MoveSpeed, RotateSpeed, ProcreateChance, ConstructSpeed, InventoryCapacity, WorkSpeed };

    }

}
