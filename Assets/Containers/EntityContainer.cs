using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for things that contain entities and other groups of entities

//Notes
// * Searches its immediate children to fill a dict for entities and a dict 
//   for other entity containers
// * The base class doesn't automatically do this as you have to specify what the
//   string key in the dicts will be

public class EntityContainer : Selectable
{

    //protected members
    protected Dictionary<string, List<Entity>> mGroupMap = new Dictionary<string, List<Entity>>();
    protected Dictionary<string, List<EntityContainer>> mContainerMap = new Dictionary<string, List<EntityContainer>>();

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
    public override void Awake()
    {
        base.Awake();
    }
    public override void Update()
    {
        base.Update();
        removeDeadEntities();
    }
    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------
    public virtual void addEntity(string group_name, Entity ent)
    {
        /*if ( !mGroupMap.ContainsKey(group_name) )
        {
            List<Entity> lempty = new List<Entity>();
            mGroupMap.Add(group_name, lempty);
        }*/
        newGroup(group_name);
        mGroupMap[group_name].Add(ent);
    }

    public void addContainer(string cont_name, EntityContainer cent)
    {
        if (!mContainerMap.ContainsKey(cont_name))
        {
            List<EntityContainer> lempty = new List<EntityContainer>();
            mContainerMap.Add(cont_name, lempty);
        }
        mContainerMap[cont_name].Add(cent);
    }
    public void clearGroup(string gname)
    {
        //clears the group but doesnt remove it from the map
        if (mGroupMap.ContainsKey(gname))
        {
            mGroupMap[gname].Clear();
        }

    }
    public List<Entity> getGroup(string gname)
    {
        if (mGroupMap.ContainsKey(gname))
        {
            return mGroupMap[gname];
        }
        else
            return new List<Entity>();
    }
    public void removeContainer(string cname, EntityContainer ec)
    {
        //removes ec from container map group named cname if ec is in that group
        bool contains_ec = false;
        if (mContainerMap.ContainsKey(cname))
        {
            foreach (EntityContainer lec in mContainerMap[cname])
            {
                if (lec == ec)
                {
                    contains_ec = true;
                    break;
                }
            }
            if (contains_ec)
            {
                mContainerMap[cname].Remove(ec);
            }
        }
    }
    public void removeEntity(string gname, Entity ent)
    {
        //removes ent from group named gname if ent is in that group
        bool contains_ent = false;
        if ( mGroupMap.ContainsKey(gname) )
        {
            foreach (Entity lent in mGroupMap[gname])
            {
                if (lent == ent)
                {
                    contains_ent = true;
                    break;
                }
            }
            if (contains_ent)
            {
                mGroupMap[gname].Remove(ent);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------
    // protected methods
    //-------------------------------------------------------------------------------------------------
	protected void newContainerGroup(string cname)
	{
		if (!mContainerMap.ContainsKey(cname))
		{
			List<EntityContainer> lempty = new List<EntityContainer>();
			mContainerMap.Add(cname, lempty);
		}
	}
	protected void newGroup(string gname)
    {
        if (!mGroupMap.ContainsKey(gname))
        {
            List<Entity> lempty = new List<Entity>();
            mGroupMap.Add(gname, lempty);
        }
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
    private void removeDeadEntities()
    {
        Dictionary<string, List<Entity>> gm = new Dictionary<string, List<Entity>>();
        foreach (KeyValuePair<string, List<Entity>> pair in mGroupMap)
        {
            List<Entity> elist = new List<Entity>(pair.Value);
            foreach (Entity ent in pair.Value)
            {
                if (!ent)
                    elist.Remove(ent);
            }
            gm[pair.Key] = elist;
        }
        mGroupMap = gm;

    }

}
