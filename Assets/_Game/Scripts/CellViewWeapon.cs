using EnhancedUI.EnhancedScroller;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CellViewWeapon : EnhancedScrollerCellView
{
	public Text weaponName;

	public Image bg;

	public Sprite bgSelected;

	public Sprite bgUnselected;

	public Sprite bgLock;

	public Image imageWeapon;

	public GameObject labelEquipped;

	public GameObject labelSelected;

	public GameObject labelNew;

	public SkeletonGraphic effectUpgrade;

	public Color32 colorWeaponNameSelected;

	public Color32 colorWeaponNameUnselected;

	public Color32 colorWeaponNameLocked;

	public GameObject[] levelStars;

	private CellViewWeaponData _data;

	public void SetData(CellViewWeaponData data)
	{
		this._data = data;
		this.UpdateInformation();
	}

	private void UpdateInformation()
	{
		this.imageWeapon.sprite = this._data.weaponImage;
		this.imageWeapon.SetNativeSize();
		this.weaponName.text = this._data.weaponName;
		this.labelEquipped.SetActive(this._data.isEquipped);
		this.labelSelected.SetActive(this._data.isSelected);
		this.labelNew.SetActive(this._data.isNew);
		if (this._data.isSelected)
		{
			this.bg.sprite = this.bgSelected;
			this.weaponName.color = this.colorWeaponNameSelected;
		}
		else
		{
			this.bg.sprite = ((!this._data.isLock) ? this.bgUnselected : this.bgLock);
			this.weaponName.color = ((!this._data.isLock) ? this.colorWeaponNameUnselected : this.colorWeaponNameLocked);
		}
		for (int i = 0; i < this.levelStars.Length; i++)
		{
			this.levelStars[i].SetActive(i < this._data.level);
		}
	}

	public override void RefreshCellView()
	{
		base.RefreshCellView();
		this.UpdateInformation();
		if (this._data.isUpgrading)
		{
			this._data.isUpgrading = false;
			this.effectUpgrade.gameObject.SetActive(true);
			this.effectUpgrade.AnimationState.SetAnimation(0, "animation", false);
		}
	}

	public void SelectWeaponCellView()
	{
		this.labelNew.SetActive(false);
		EventDispatcher.Instance.PostEvent(EventID.SelectWeaponCellView, this._data);
		if (GameData.isShowingTutorial)
		{
			if (this._data.id == 0)
			{
				EventDispatcher.Instance.PostEvent(EventID.SubStepSelectUzi);
			}
			else if (this._data.id == 107)
			{
				EventDispatcher.Instance.PostEvent(EventID.SubStepSelectKame);
			}
		}
	}
}
