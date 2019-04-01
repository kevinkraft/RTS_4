using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

//class for units

//Notes:
// * When the unit is Garrisoned, just put it below the map (no this is bad, turn off its object)
// * Does setting the game object when garrisoning make it null in other scripts?
//   * this doesnt work because then the units actions arent cycled, i.e. the garrison action isnt deleted
//   * turn off the model object instead?

public class Unit : EntityAction
{
	//private members
	public GameTypes.GenderType mGender;

    //private member
    //private float mSpeed = UNIT_SPEED;
    //private float mRotateSpeed = UNIT_ROTATE_SPEED;
    private bool mGarrisoned = false;
    public Building mGarrison;
    private bool mPregnant = false;
    private float mPregnancyProgress = 0f;
    private System.Random mRandomGen = new System.Random();
	[SerializeField]
    private float mHunger = 0f;
	private UnitStats mStats;

    //-------------------------------------------------------------------------------------------------
    // unity methods
    //-------------------------------------------------------------------------------------------------
	public override void Awake()
	{
		base.Awake();
		mStats = GetComponentInChildren<UnitStats>();
		if ( !mStats )
			Debug.LogError("UnitStats is null.");
		//overwrite the ItemGropu of mInventory with EquipmentInventory
		mInventory = GetComponentInChildren<EquipmentInventory>();
		if ( !mInventory )
			Debug.LogError("EquiptmentInventory is null.");
	}

    public override void Update()
    {
        if (!isInstantiated)
            return;
        base.Update();
        //update pregnancy
        updatePregnancy();
        updateHunger();
		//make sure the inventory capacity is the same as that in UnitStats
		if ( getInventoryCapacity() != mStats.getIntValue(GameTypes.UnitStatType.InventoryCapacity)  )
		{
			//the inventory capacity in UnitStats has changed
			getInventory().mCapacity = mStats.getIntValue(GameTypes.UnitStatType.InventoryCapacity);
		}
    }

    //-------------------------------------------------------------------------------------------------
    // public methods
    //-------------------------------------------------------------------------------------------------

	public void addToStat(GameTypes.UnitStatType stype, float cval)
	{
		//add a value to the unit stats, use a minus to subtract.
		mStats.addToValue(stype, cval);
	}

	public void addToStat(GameTypes.UnitStatType stype, int cval)
	{
		//add a value to the unit stats, use a minus to subtract.
		mStats.addToValue(stype, cval);
	}

	public void attack(EntityHP target)
    {
        //make a new attack instance as a child of the action group object
        Attack att = Instantiate(ObjectManager.getAction("Attack"), mActions.transform).GetComponent<Attack>();
        //set target
        att.setTarget(target);
        //add the action to the action group component
        mActions.clearAddAction(att);
    }

	public void changeTown(Town ntown)
	{
		//change the units town, and more the unit game object to the correct place
		//remove it from its current town
		Town otown = getTown();
		otown.removeEntity("units", this);
		//add it to the new town, this should also move its game object to a child of the new town
		ntown.addEntity("units", this);



	}

    public void collectResource(Resource res, bool prepend = false)
    {
        //make a new collect instance as a child of the action group object
        Collect col = ObjectManager.initCollect(mActions.transform);
        //set target
        col.setTarget(res);
        //add the action to the action group component
        if (prepend)
        {
            col.setOneRound(true);
            mActions.prependAction(col);
        }
        else
        {
            mActions.clearAddAction(col);
        }
    }

	public override void dropInventory()
	{
		//drop the units inventory on the ground
		//To Do: Implement Items that are visible on the map
		//Currently I'm just deleting them
		EquipmentInventory einv = _getEquipmentInventory();
		if (!einv) return;
		einv.wipe();
	}

    public void dropOrDumpInventory()
    {
        //drops the inventory if the stockpile is full, or dumps it to the stockpile
        if (getStockpile().isInventoryFull())
            dropInventory();
        else
            dumpInventory();
    }

	public void dumpInventory()
	{
		//drop everything in the inventory into the stockpile
		/*Exchange ex = ObjectManager.initExchange(transform);
		ex.setTarget(getStockpile());
		ex.setExchangeList(getInventoryDictionary());
		mActions.prependAction(ex);*/
		foreach (KeyValuePair<GameTypes.ItemType,int> it in getInventoryDictionary() )
		{
			Item item = getItemOfType(it.Key);
			EquipItem ei = item as EquipItem;
			if (ei)
			{
				//if its equipped, dont drop it
				if ( ei.isEquipped() == true ) continue;
			}
			exchangeWith(getStockpile(), it.Key, it.Value);
		}
	}

	public void equipItem(EquipItem eitem)
	{
		EquipmentInventory einv = _getEquipmentInventory();
		if (!einv)
		{
			Debug.LogError("Unit has invalid EquipmentInventory");
		}
		einv.equipItem(eitem);
	}

    public void exchangeWith(EntityAction target, GameTypes.ItemType type, int amount)
    {
        Exchange ex = ObjectManager.initExchange(transform);
        ex.setTarget(target);
        ex.setExchangeItem(type, amount);
        mActions.prependAction(ex);
    }
	 
    public void garrison(Building b)
    {
        b.addUnit(this);
        mGarrisoned = true;
        if (!b)
            Debug.LogError("Building to garrison in is null");
        mGarrison = b;
        //transform.position = new Vector3(transform.position.x, -10f, transform.position.z);
        //gameObject.SetActive(false);
        setModelActive(false);
    }

    public void garrisonIn(Building b)
    {
        //make a new garrison instance as a child of the action group object
        Garrison gar = ObjectManager.initGarrison(transform);
        //set building to garrison in
        gar.setTarget(b);
        //add the action to the action group component
        mActions.prependAction(gar);
    }

	public float getAttack()
	{
		return mStats.getValue(GameTypes.UnitStatType.Attack);
	}

	public float getConstructSpeed()
	{
		return mStats.getValue(GameTypes.UnitStatType.ConstructSpeed);
	}

	public List<EquipItem> getAvailableEquipItems()
	{
		List<EquipItem> alist = new List<EquipItem>();
		EquipmentInventory einv = _getEquipmentInventory();
		if (!einv) return alist;
		alist = einv.getAvailableEquipItems();
		return alist;
	}

	public List<EquipItem> getEquippedItems()
	{
		List<EquipItem> elist = new List<EquipItem>();
		EquipmentInventory einv = _getEquipmentInventory();
		if (!einv) return elist;
		elist = einv.getEquippedItems();
		return elist;
	}
		
	public float getExchangeSpeed()
	{
		return mStats.getValue(GameTypes.UnitStatType.ExchangeSpeed);
	}

    public Building getGarrison()
    {
        return mGarrison;
    }

	public GameTypes.GenderType getGender()
	{
		return mGender;
	}

    public float getHunger()
    {
        return mHunger;
    }

	public float getIntrRange()
	{
		return mStats.getValue(GameTypes.UnitStatType.InteractionRange);
	}

    public float getMoveSpeed()
    {
		return mStats.getValue(GameTypes.UnitStatType.MoveSpeed);
    }

    public float getPregnancyProgress()
    {
        return mPregnancyProgress;
    }

	public float getProcreateChance()
	{
		return mStats.getValue(GameTypes.UnitStatType.ProcreateChance);
	}

    public bool getResource(GameTypes.ItemType type, int amount=-1)
    {
        //tells the unit to get some of a resource. Chceks the unit inventory, if the
        //unit has none of the item it checks the stockpile and tells them to get the item from there
        //if theres none in the stockpile it tells them to go and find the resource.
        //amount set to -1 means fill the inventory

        //what amount do we need?
        int invcap = getInventory().mCapacity;
        int invsize = getInventorySize();
        if (amount == -1 || invcap < invsize + amount)
            amount = invcap - invsize;
        
        //check the inventory and do nothing if the unit has the item
        Item item = getItemOfType(type);
        if (item)
		{
            //unit has it, do nothing, if it has the desired amount
			if ( item.getAmount() >= amount ) return true;
        }
        //does the stockpile have some
        Building sp = getStockpile();
        item = sp.getItemOfType(type);
        if (item)
        {
            //is the inventory full
            if (isInventoryFull())
            {
                //is the stockpile full
                if (sp.isInventoryFull())
                    dropInventory();
                else
                    dumpInventory();
            }
            //stockpile has it, send Unit to get it
            exchangeWith(sp, type, -1*amount);
            return false;
        }
        //stockpile doesn't have it, get the closest resource of this type
        Resource res = mTown.getRegion().getClosestResourceOfType(type, this);
        if (res)
        {
            //Debug.Log(res.transform.position);
            //go and collect this resource for one cycle
            collectResource(res, true);
        }
        return false;
    }

    public float getRotateSpeed()
    {
		return mStats.getValue(GameTypes.UnitStatType.RotateSpeed);
    }

    /*public override void mouseClick(GameObject hitObject, Vector3 hitPoint)
    {
        //process a left mouse click while this entity is selected by the player
        Debug.Log("in unit, this function does nothing yet");
    }*/

	public float getWorkSpeed()
	{
		return mStats.getValue(GameTypes.UnitStatType.WorkSpeed);
	}

	public override bool hasEquipItem()
	{
		return mInventory.hasEquipItem();
	}

    public bool isGarrisoned()
    {
        return mGarrisoned;
    }
		
    public bool isPregnant()
    {
        return mPregnant;
    }

	public bool makeInventorySpaceFor(GameTypes.ItemType itype)
	{
		//dumps everything in the inventory except items of a certain type
		//return true if unit needs to return to stockpile, false other wise
		Dictionary<GameTypes.ItemType, int> dict = getInventoryDictionary();
		Building sp = getStockpile();
		bool new_action = false;
		foreach ( KeyValuePair<GameTypes.ItemType,int> pair in dict )
		{
			//skip- if the item is equipped
			Item it = getItemOfType(pair.Key);
			EquipItem ei = it as EquipItem;
			if (ei)
			{
				if (ei.isEquipped()) continue;
			}
			//skip if its the item we want
			if ( pair.Key != itype )
			{
				//make an exchange with the stockpile
				exchangeWith(sp, pair.Key, pair.Value);
				new_action = true;
			}
		}
		return new_action;
	}

    public void makePregnant()
    {
        if ( mGender == GameTypes.GenderType.Female && mPregnant == false)
        {
            mPregnant = true;
            mPregnancyProgress = 0f;
        }
    }

    public void moveTo( Vector3 destination, bool clear=true )
    {
        //make a new move instance as a child of the action group object
        //Movement move = Instantiate(ObjectManager.getAction("Movement"), mActions.transform).GetComponent<Movement>();
        Movement move = ObjectManager.initMove(mActions.transform);
        //set destination
        move.setDestination(destination);
        //add the action to the action group component
        if ( clear )
            mActions.clearAddAction(move);
        else
            mActions.prependAction(move);
    }

    public void moveTo(Entity ent, bool clear = true)
    {
        //make a new move instance as a child of the action group object
        //Movement move = Instantiate(ObjectManager.getAction("Movement"), mActions.transform).GetComponent<Movement>();
        Movement move = ObjectManager.initMove(mActions.transform);
        //set destination
        move.setDestination(ent);
        //add the action to the action group component
        if (clear)
            mActions.clearAddAction(move);
        else
            mActions.prependAction(move);
    }

	public bool moveToInRange(Entity ent, bool clear = true)
	{
		//move to the entity taking bounds into accout.
		//returns true if its in range.
		//is target in range?
		float dist = Vector3.Distance(transform.position, ent.transform.position);
		if (dist < getIntrRange()) return true;
		else
		{
			//is it in range of the bounds?
			if ( calculateExtentsDistance(ent) < getIntrRange() ) return true;
		}
		//move to the target taking the bounds into account
		Vector3 direction = pointOfTouchingBounds( ent );
		moveTo(direction, false);
		return false;
	}

	public string printStats()
	{
		//return a string containing info about the Units stats
		return mStats.print();
	}

	public void retrieveItemsAndGiveToTarget(KeyValuePair<GameTypes.ItemType,int> needed, EntityAction target)
	{
		//resources are needed by an EntityAction, send Unit to get them and return them to the EntityAction
		if (this.getResource(needed.Key, needed.Value))
		{
			//unit has the resource, give the right amount to the WorkedProdBuilding
			Item item = this.getItemOfType(needed.Key);
			int amount = 0;
			if (!item)
				Debug.LogError("The acter doesn't have the Item. This shouldn't be possible.");
			//if the unit has more than is needed then set to the needed amount
			if ( item.getAmount() > needed.Value )
			{
				amount = needed.Value;
			}
			else
			{
				amount = item.getAmount();
			}
			this.exchangeWith(target, needed.Key, amount);
			return;
		}
		else
		{
			//unit has been given actions to go and get the resource
			return;
		}
	}

    public void returnToStockpile(GameTypes.ItemType type)
    {
        Item item = getItemOfType(type);
        if (!item)
        {
            //this means the unit doesnt contain any of this item, which means the inventory is already full
            //so dump the inventory
            //Debug.Log("No item of type, the inventory must already be full. Dumping inventory");
            dumpInventory();
            return;
        }
		exchangeWith(getStockpile(), type, item.getAmount());
    }

	public void setGender(GameTypes.GenderType gender)
	{
		mGender = gender;
	}

    public void setHunger(float h)
    {
        mHunger = h;
    }

	public void unequipItem(EquipItem eitem)
	{
		EquipmentInventory einv = _getEquipmentInventory();
		if (!einv)
		{
			Debug.LogError("Unit has invalid EquipmentInventory");
		}
		einv.unequipItem(eitem);
	}

    public void ungarrisonSelf()
    {
        //DO NOT CALL THIS FROM BUILDING
        if (!mGarrison)
            Debug.LogError("Garrison Building not set. unit name "+mName);
        mGarrison.removeUnitByUnit(this);
        /*mGarrisoned = false;
        mGarrison = null;
        //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        //gameObject.SetActive(true);
        setModelActive(true);*/
        ungarrison();
    }

    public void ungarrisonByBuilding()
    {
        //DO NOT CALL THIS FROM UNIT
        /*mGarrisoned = false;
        mGarrison = null;
        //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        //gameObject.SetActive(true);
        setModelActive(true);
        */
        ungarrison();
    }

    public void waitAt(Vector3 destination, bool clear = true)
    {
        //make a new wait instance as a child of the action group object
        Wait wt = ObjectManager.initWait(mActions.transform);
        //set destination
        wt.setDestination(destination);
        //add the action to the action group component
        if (clear)
            mActions.clearAddAction(wt);
        else
            mActions.prependAction(wt);
    }

    public void waitAt(Entity ent, bool clear = true)
    {
        //make a new wait instance as a child of the action group object
        Wait wt = ObjectManager.initWait(mActions.transform);
        //set destination
        wt.setDestination(ent);
        //add the action to the action group component
        if (clear)
            mActions.clearAddAction(wt);
        else
            mActions.prependAction(wt);
    }

    //-------------------------------------------------------------------------------------------------
    // private methods
    //-------------------------------------------------------------------------------------------------
	private EquipmentInventory _getEquipmentInventory()
	{
		EquipmentInventory einv = mInventory as EquipmentInventory;
		if (!einv)
		{
			Debug.LogError("Unit does not have an equippment inventory.");
			return null;
		}
		return einv;
	}

	private void ungarrison()
    {
        mGarrisoned = false;
        mGarrison = null;
        setModelActive(true);
    }

    private void updateHunger()
    {
        mHunger += Globals.UNIT_HUNGER_CYCLE_INCREASE;
        if (mHunger >= 100f)
        {
            mHunger = 100f;
            mHP -= Globals.UNIT_HUNGRY_DAMAGE_TAKEN;
        }
        if ( mHunger > 80f && !mActions.hasActionOfType("Eat") )
        {
            //go and eat
            //Debug.Log("Adding Eat Action");
            Eat eat = ObjectManager.initEat(mActions.transform);
            mActions.prependAction(eat);
        }
    }

    private void updatePregnancy()
    {
        if (mPregnant == true && mGender == GameTypes.GenderType.Female)
        {
            //Debug.Log("processing pregnancy");
            if (mPregnancyProgress >= 100)
            {
                //reset pregnancy
                mPregnant = false;
                mPregnancyProgress = 0f;
                //make new unit instance with random gender
                GameTypes.GenderType[] genders = new[] { GameTypes.GenderType.Female, GameTypes.GenderType.Male };
                GameTypes.GenderType gender = genders[mRandomGen.Next(genders.Length)];
                //gender is always the same, so change the generator
                int temp = mRandomGen.Next(0,10);
                Unit unit = ObjectManager.initUnit(transform.position, gender, mTown);
                //send new units to the main hut, so the user knows they are new
                unit.waitAt(mTown.mMainBuilding);
            }
            else
            {
                mPregnancyProgress += Globals.UNIT_PREGNANCY_CYCLE_PROGRESS;
            }
        }
    }


}
