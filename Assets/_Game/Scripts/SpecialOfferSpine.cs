using Spine.Unity;
using System;
using UnityEngine;

public class SpecialOfferSpine : MonoBehaviour
{
	public SpecialOffer type;

	public SkeletonGraphic pack;

	[SpineSkin("", "", true, false)]
	public string skinPackEverybodyFavorite;

	[SpineSkin("", "", true, false)]
	public string skinPackDragonBreath;

	[SpineSkin("", "", true, false)]
	public string skinPackLetThereBeFire;

	[SpineSkin("", "", true, false)]
	public string skinPackSnippingForDummies;

	[SpineSkin("", "", true, false)]
	public string skinPackTaserLaser;

	[SpineSkin("", "", true, false)]
	public string skinPackShockingSale;

	[SpineSkin("", "", true, false)]
	public string skinPackEnthusiast;

	private bool isSetSkinDone;

	public void Show()
	{
		if (!this.isSetSkinDone)
		{
			this.isSetSkinDone = true;
			this.SetSkin(this.type);
		}
	}

	public void SetSkin(DayOfWeek day)
	{
		switch (day)
		{
		case DayOfWeek.Sunday:
			this.pack.Skeleton.SetSkin(this.skinPackEnthusiast);
			break;
		case DayOfWeek.Monday:
			this.pack.Skeleton.SetSkin(this.skinPackEverybodyFavorite);
			break;
		case DayOfWeek.Tuesday:
			this.pack.Skeleton.SetSkin(this.skinPackDragonBreath);
			break;
		case DayOfWeek.Wednesday:
			this.pack.Skeleton.SetSkin(this.skinPackLetThereBeFire);
			break;
		case DayOfWeek.Thursday:
			this.pack.Skeleton.SetSkin(this.skinPackSnippingForDummies);
			break;
		case DayOfWeek.Friday:
			this.pack.Skeleton.SetSkin(this.skinPackTaserLaser);
			break;
		case DayOfWeek.Saturday:
			this.pack.Skeleton.SetSkin(this.skinPackShockingSale);
			break;
		}
	}

	public void SetSkin(SpecialOffer packageType)
	{
		switch (packageType)
		{
		case SpecialOffer.LetThereBeFire:
			this.pack.Skeleton.SetSkin(this.skinPackLetThereBeFire);
			break;
		case SpecialOffer.EveryBodyFavorite:
			this.pack.Skeleton.SetSkin(this.skinPackEverybodyFavorite);
			break;
		case SpecialOffer.DragonBreath:
			this.pack.Skeleton.SetSkin(this.skinPackDragonBreath);
			break;
		case SpecialOffer.SnippingForDummies:
			this.pack.Skeleton.SetSkin(this.skinPackSnippingForDummies);
			break;
		case SpecialOffer.TaserLaser:
			this.pack.Skeleton.SetSkin(this.skinPackTaserLaser);
			break;
		case SpecialOffer.ShockingSale:
			this.pack.Skeleton.SetSkin(this.skinPackShockingSale);
			break;
		case SpecialOffer.UpgradeEnthusiast:
			this.pack.Skeleton.SetSkin(this.skinPackEnthusiast);
			break;
		}
	}
}
