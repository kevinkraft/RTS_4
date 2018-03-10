using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for holding the Terrain and randomly assigning Resources to the terrain in a Region

public class Map : Selectable
{

    //public members
    public GameTypes.MapType mType = GameTypes.MapType.Unknown;
    public int mSeed;

    //private members
    private GameObject mGround;
    public Region mRegion;
    private System.Random mRandomGen;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
        //turn on the ground
        Ground g = gameObject.GetComponentInChildren(typeof(Ground), true) as Ground;
        mGround = g.gameObject;
        if (!mGround)
        {
            Debug.LogError("Can't find Ground of Map");
        }
        else if (!GetComponentInParent<GameObjectList>())
        {
            mGround.SetActive(true);
        }
    }

    public override void Start()
    {
        //get the region
        mRegion = GetComponentInParent<Region>();
        if (!mRegion && !GetComponentInParent<GameObjectList>())
        {
            Debug.LogError("Can't find Maps Region");
            return;
        }
        else if (GetComponentInParent<GameObjectList>())
            return;

        float width = mRegion.getWidth();
        //set the terrain position and resolution to match what is in Region
        Terrain terr = GetComponentInChildren<Terrain>();
        terr.terrainData.size = new Vector3(width, 2 * width, width);
        terr.transform.position = new Vector3(mRegion.transform.position.x + -1f * width / 2f, 0f, mRegion.transform.position.z + -1f * width / 2f);

        //make the random generator
        mRandomGen = new System.Random(mSeed);

        //populate the resources
        populateResources();

    }
    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void populateResources()
    {
        //make 4 groups of Wood and Food, each with 7 instances
        //need a position and sigma of spread for each group

        //set constants
        float width = mRegion.getWidth();
        float hwidth = mRegion.getWidth()/2f;
        float n_per_food_group = Mathf.Ceil(width / 40f); //about 8 for 300 width Region
        float n_per_wood_group = Mathf.Ceil(width / 10f); //about 8 for 300 width Region
        float n_groups = Mathf.Ceil(width / 74f); //about 4 for 300 width Region
        float spread_max_food = width / 15f;
        float spread_max_wood = width / 7f;
        List<GameTypes.ItemType> types = new List<GameTypes.ItemType>() { GameTypes.ItemType.Food, GameTypes.ItemType.Wood };

        float spread;
        float n_per_group = 1;
        float spread_max = 5;
        foreach (GameTypes.ItemType type in types)
        {
            //loop to make groups
            for (int i = 0; i < n_groups; i++)
            {
                //set group pos
				float gposx = mRegion.gameObject.transform.position.x + getRand(-hwidth, hwidth);
				float gposz = mRegion.gameObject.transform.position.z + getRand(-hwidth, hwidth);
                //differences for food and wood
                if (type == GameTypes.ItemType.Food)
                {
                    n_per_group = n_per_food_group;
                    spread_max = spread_max_food;
                }
                else if (type == GameTypes.ItemType.Wood)
                {
                    n_per_group = n_per_wood_group;
                    spread_max = spread_max_wood;
                }

                //set the group spread
                spread = getRand(5, spread_max);

                //loop to make Resource instances
                for (int j=0; j < n_per_group; j++)
                {
                    //set the individual pos
                    Vector3 pos = new Vector3(gposx + getRand(-spread, spread), 0f, gposz + getRand(-spread, spread));
                    //instantiate resources
                    Resource res = ObjectManager.initResource(pos, type, mRegion);
                }


            }

        }

    }

    private float getRand(float minv=0, float maxv=1)
    { 
        return (float)mRandomGen.NextDouble() * (maxv - minv) + minv;
    }
   

}
