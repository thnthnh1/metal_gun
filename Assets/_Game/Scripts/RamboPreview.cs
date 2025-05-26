using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RamboPreview : MonoBehaviour
{
	public SkeletonAnimation skeletonAnimation;

	public SkeletonRenderer skeletonRenderer;

	[SpineAnimation("", "", true, false)]
	public string shoot;

	[SpineAnimation("", "", true, false)]
	public string throwGrenade;

	[SpineAnimation("", "", true, false)]
	public string aim;

	[SpineAnimation("", "", true, false)]
	public string meleeAttack;

	[SpineAnimation("", "", true, false)]
	public string knife;

	[SpineAnimation("", "", true, false)]
	public string pan;

	[SpineAnimation("", "", true, false)]
	public string guitar;

	[SpineEvent("", "", true, false)]
	public string eventThrowGrenade;

	[SpineBone("", "", true, false)]
	public string equipGunBoneName;

	[SpineBone("", "", true, false)]
	public string equipMeleeWeaponBoneName;

	[SpineBone("", "", true, false)]
	public string effectWindBoneName;

	public Vector2 throwGrenadeDirection;

	public Transform throwGrenadePoint;

	private float lastTimeMeleeAttack;

	private float timerFire;

	private float timerThrow;

	[SerializeField]
	private bool isReadyAttack;

	[SerializeField]
	private bool isUsingMeleeWeapon;

	[SerializeField]
	private bool flagMeleeAttack;

	[SerializeField]
	private bool flagThrowGrenade;

	private string readyAttackMethodName = "ReadyAttack";

	private BaseGunPreview gun;

	private BaseGrenadePreview grenade;

	private BaseMeleeWeaponPreview meleeWeapon;

	private Dictionary<int, BaseGunPreview> guns = new Dictionary<int, BaseGunPreview>();

	private Dictionary<int, BaseGrenadePreview> grenades = new Dictionary<int, BaseGrenadePreview>();

	private Dictionary<int, BaseMeleeWeaponPreview> meleeWeapons = new Dictionary<int, BaseMeleeWeaponPreview>();

	private void Start()
	{
		this.skeletonAnimation.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.HandleAnimationStart);
		this.skeletonAnimation.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.HandleAnimationCompleted);
		this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationEvent);
	}

	private void Update()
	{
		if (this.isUsingMeleeWeapon)
		{
			if (this.meleeWeapon && this.isReadyAttack && !this.flagMeleeAttack && !this.flagThrowGrenade)
			{
				float time = Time.time;
				if (time - this.lastTimeMeleeAttack > 1f / this.meleeWeapon.baseStats.AttackTimePerSecond)
				{
					this.lastTimeMeleeAttack = time;
					this.flagMeleeAttack = true;
					this.PlayAnimationMeleeAttack();
				}
			}
		}
		else
		{
			if (this.gun && this.isReadyAttack)
			{
				this.timerFire += Time.deltaTime;
				if (this.timerFire >= 1f / this.gun.baseStats.AttackTimePerSecond)
				{
					this.timerFire = 0f;
					this.PlayAnimationShoot();
					this.gun.Fire();
				}
			}
			if (this.grenade)
			{
				this.timerThrow += Time.deltaTime;
				if (this.timerThrow >= 3f)
				{
					this.timerThrow = 0f;
					this.ShowGun(false);
					this.isReadyAttack = false;
					this.flagThrowGrenade = true;
					this.PlayAnimationThrowGrenade();
				}
			}
		}
	}

	private void ShowGun(bool isShow)
	{
		if (this.gun)
		{
			this.gun.gameObject.SetActive(isShow);
		}
	}

	private void ShowMeleeWeapon(bool isShow)
	{
		if (this.meleeWeapon)
		{
			this.meleeWeapon.gameObject.SetActive(isShow);
		}
	}

	private void HandleAnimationStart(TrackEntry entry)
	{
		if (string.Compare(entry.animation.name, this.meleeAttack) == 0)
		{
			this.meleeWeapon.ActiveEffect(true);
		}
	}

	private void HandleAnimationCompleted(TrackEntry entry)
	{
		if (string.Compare(entry.animation.name, this.throwGrenade) == 0)
		{
			this.flagThrowGrenade = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
			this.ActiveAim(true);
			base.Invoke(this.readyAttackMethodName, 1f);
		}
		if (string.Compare(entry.animation.name, this.meleeAttack) == 0)
		{
			this.flagMeleeAttack = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
			this.meleeWeapon.ActiveEffect(false);
		}
	}

	private void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventThrowGrenade) == 0)
		{
			float d = UnityEngine.Random.Range(2f, 2.5f);
			Vector2 throwForce = this.throwGrenadeDirection * d;
			if (this.grenade)
			{
				this.grenade.Active(this.throwGrenadePoint.position, throwForce, base.transform);
			}
		}
	}

	private void ActiveAim(bool isActive)
	{
	}

	public void EquipGun(int id)
	{
		this.timerFire = 0f;
		this.isReadyAttack = false;
		this.isUsingMeleeWeapon = false;
		this.grenade = null;
		if (this.guns.ContainsKey(id))
		{
			this.ActiveGun(id);
		}
		else
		{
			this.CreateGunPreview(id);
		}
		this.ShowMeleeWeapon(false);
		base.Invoke(this.readyAttackMethodName, 0.5f);
	}

	private void CreateGunPreview(int id)
	{
		string str = string.Format("gun_preview_{0}", id);
		BaseGunPreview original = Resources.Load<BaseGunPreview>("Gun Preview/" + str);
		BaseGunPreview baseGunPreview = UnityEngine.Object.Instantiate<BaseGunPreview>(original, base.transform);
		BoneFollower boneFollower = baseGunPreview.gameObject.AddComponent<BoneFollower>();
		if (boneFollower != null)
		{
			boneFollower.skeletonRenderer = this.skeletonRenderer;
			boneFollower.boneName = this.equipGunBoneName;
		}
		baseGunPreview.gameObject.name = id.ToString();
		this.guns.Add(id, baseGunPreview);
		this.ActiveGun(id);
	}

	private void ActiveGun(int id)
	{
		foreach (KeyValuePair<int, BaseGunPreview> current in this.guns)
		{
			if (current.Key == id)
			{
				current.Value.gameObject.SetActive(true);
				this.gun = current.Value;
			}
			else
			{
				current.Value.gameObject.SetActive(false);
			}
		}
	}

	private void PlayAnimationShoot()
	{
		TrackEntry trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(0, this.shoot, false);
		trackEntry.AttachmentThreshold = 1f;
		trackEntry.MixDuration = 0f;
		TrackEntry trackEntry2 = this.skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0.5f, 0.1f);
		trackEntry2.AttachmentThreshold = 1f;
	}

	public void EquipGrenade(int id)
	{
		this.timerThrow = 2f;
		this.isUsingMeleeWeapon = false;
		if (this.grenades.ContainsKey(id))
		{
			this.ActiveGrenade(id);
		}
		else
		{
			this.CreateGrenadePreview(id);
		}
	}

	private void CreateGrenadePreview(int id)
	{
		string str = string.Format("grenade_preview_{0}", id);
		BaseGrenadePreview original = Resources.Load<BaseGrenadePreview>("Grenade Preview/" + str);
		BaseGrenadePreview baseGrenadePreview = UnityEngine.Object.Instantiate<BaseGrenadePreview>(original, base.transform);
		baseGrenadePreview.gameObject.name = string.Format("grenade_{0}", id);
		baseGrenadePreview.gameObject.SetActive(false);
		this.grenades.Add(id, baseGrenadePreview);
		this.ActiveGrenade(id);
	}

	private void ActiveGrenade(int id)
	{
		foreach (KeyValuePair<int, BaseGrenadePreview> current in this.grenades)
		{
			if (current.Key == id)
			{
				this.grenade = current.Value;
			}
		}
	}

	private void PlayAnimationThrowGrenade()
	{
		this.ActiveAim(false);
		this.skeletonAnimation.AnimationState.SetAnimation(1, this.throwGrenade, false);
	}

	private void ReadyAttack()
	{
		this.ShowMeleeWeapon(this.isUsingMeleeWeapon);
		this.ShowGun(!this.isUsingMeleeWeapon);
		this.isReadyAttack = true;
	}

	public void EquipMeleeWeapon(int id)
	{
		this.isReadyAttack = false;
		this.isUsingMeleeWeapon = true;
		this.grenade = null;
		if (this.meleeWeapons.ContainsKey(id))
		{
			this.ActiveMeleeWeapon(id);
		}
		else
		{
			this.CreateMeleeWeaponPreview(id);
		}
		this.ShowGun(false);
		base.Invoke(this.readyAttackMethodName, 0.5f);
	}

	private void CreateMeleeWeaponPreview(int id)
	{
		string str = string.Format("melee_weapon_preview_{0}", id);
		BaseMeleeWeaponPreview original = Resources.Load<BaseMeleeWeaponPreview>("Melee Weapon Preview/" + str);
		BaseMeleeWeaponPreview baseMeleeWeaponPreview = UnityEngine.Object.Instantiate<BaseMeleeWeaponPreview>(original, base.transform);
		BoneFollower boneFollower = baseMeleeWeaponPreview.gameObject.AddComponent<BoneFollower>();
		if (boneFollower != null)
		{
			boneFollower.skeletonRenderer = this.skeletonRenderer;
			boneFollower.boneName = this.equipMeleeWeaponBoneName;
		}
		baseMeleeWeaponPreview.gameObject.name = id.ToString();
		baseMeleeWeaponPreview.InitEffect(this.skeletonAnimation, this.effectWindBoneName);
		this.meleeWeapons.Add(id, baseMeleeWeaponPreview);
		this.ActiveMeleeWeapon(id);
	}

	private void ActiveMeleeWeapon(int id)
	{
		foreach (KeyValuePair<int, BaseMeleeWeaponPreview> current in this.meleeWeapons)
		{
			if (current.Key == id)
			{
				current.Value.gameObject.SetActive(true);
				this.meleeWeapon = current.Value;
			}
			else
			{
				current.Value.gameObject.SetActive(false);
			}
		}
	}

	private void PlayAnimationMeleeAttack()
	{
		if (this.meleeWeapon)
		{
			MeleeWeaponType type = this.meleeWeapon.type;
			if (type != MeleeWeaponType.Knife)
			{
				if (type != MeleeWeaponType.Pan)
				{
					if (type == MeleeWeaponType.Guitar)
					{
						this.meleeAttack = this.guitar;
					}
				}
				else
				{
					this.meleeAttack = this.pan;
				}
			}
			else
			{
				this.meleeAttack = this.knife;
			}
		}
		this.skeletonAnimation.AnimationState.SetAnimation(1, this.meleeAttack, false);
	}
}
