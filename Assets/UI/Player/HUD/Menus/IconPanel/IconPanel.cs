using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS;

//Notes
// * Class for displaying icons above entities
// * This class knows which entity it belongs to and follows that entities position
// * It lives in the CamareSpaceCanvas in the Hierarchy, like the PopMenu
// * It has an image to display

public class IconPanel : Menu 
{

	//public members
	public Sprite mCautionIcon;
	public Sprite mActiveIcon;
	public GameTypes.IconType mIconType;
	public bool mActive;
	public Entity mEntity;

	//private
	private Image mImage;
	private float mWidth;
	private float mOffset;

	//-------------------------------------------------------------------------------------------------
	// unity methods
	//-------------------------------------------------------------------------------------------------
	public void Awake()
	{
		//get the icon image
		mImage = GetComponentInChildren<Image>();
		mActive = false;
		mIconType = GameTypes.IconType.Unknown;
	}

	public void Start()
	{
		//set the width and offset
		if ( mEntity )
		{
			mEntity.setHasIcon(true);
			Unit unit = mEntity as Unit;
			if ( unit )
			{
				mWidth = Globals.ICON_PANEL_UNIT_WIDTH;
				mOffset = Globals.ICON_PANEL_UNIT_OFFSET;
			}
			else
			{
				mWidth = Globals.ICON_PANEL_BUILDING_WIDTH;
				mOffset = Globals.ICON_PANEL_BUILDING_OFFSET;
			}
		}
		else
		{
			mActive = false;
		}
		//choose the active icon
		if ( mIconType == GameTypes.IconType.Caution )
			mActiveIcon = mCautionIcon;
		//set the correct image
		mImage.sprite = mActiveIcon;
	}

	private void OnGUI()
	{
		//update the postion on the screen
		if (mActive)
		{	
			setScreenPosition();
		}
	}

	private void OnDestroy()
	{
		if (mEntity)
			mEntity.setHasIcon(false);
	}


	//-------------------------------------------------------------------------------------------------
	// private methods
	//-------------------------------------------------------------------------------------------------
	private void setScreenPosition()
	{
		//sets the screen position of the IconPanel using the entity position
		if ( !mEntity )
		{
			Debug.LogWarning("Entity is null.");
			mActive = false;
			return;
		}
		Vector3 screenpos = Camera.main.WorldToScreenPoint(mEntity.transform.position);
		setPositions(screenpos.x - mWidth/2, screenpos.x + mWidth/2, -1*(mOffset + screenpos.y + mWidth/2), mOffset + screenpos.y - mWidth/2, 0f, 0f, 0f, 0f, 0.5f, 0.5f);
	}




}
