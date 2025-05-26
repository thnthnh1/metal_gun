using System;
using UnityEngine;

public class BaseGun : BaseWeapon
{
	public AudioSource audioSource;

	[Header("STATS")]
	public SO_GunStats baseStats;

	[Header("BASE GUN PROPERTIES")]
	public GunType gunType;

	public BaseBullet bulletPrefab;

	public BaseMuzzle muzzlePrefab;

	public Transform firePoint;

	public Transform muzzlePoint;

	public ParticleSystem bulletTrash;

	protected float bulletSpeed;

	protected int bulletPerShoot;

	protected bool isInfinityAmmo = true;

	[SerializeField]
	protected bool hasCartouche = true;

	protected int numberCrossBullet = 3;

	protected BaseMuzzle muzzle;

	private AudioClip soundCartouche;

	private bool isInitialized;

	public int ammo;

	protected virtual void Awake()
	{
		if (SoundManager.Instance != null)
		{
			Debug.Log("Gun Fire Is play Sound Null");
			this.soundCartouche = SoundManager.Instance.GetAudioClip("sfx_cartouche");
		}
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, delegate(Component sender, object param)
		{
			if (this && base.gameObject.activeInHierarchy && !(bool)param)
			{
				Debug.Log("Gun Fire Is play Awake");
				this.PlaySoundCartouche();
			}
		});
	}

	public override void LoadScriptableObject()
	{
	}

	public override void Init(int level)
	{
		base.Init(level);
		if (!this.isInitialized)
		{
			this.isInitialized = true;
			this.ammo = GameData.playerGuns.GetGunAmmo(this.id);
		}
		this.damage = this.baseStats.Damage;
		this.attackTimePerSecond = this.baseStats.AttackTimePerSecond;
		this.criticalRate = this.baseStats.CriticalRate;
		this.criticalDamageBonus = this.baseStats.CriticalDamageBonus;
		this.hasCartouche = this.baseStats.HasCartouche;
		this.bulletSpeed = this.baseStats.BulletSpeed;
		this.bulletPerShoot = this.baseStats.BulletPerShoot;
		StaticGunData data = GameData.staticGunData.GetData(this.id);
		this.isInfinityAmmo = (data == null || !data.isSpecialGun);
	}

	public override void Attack(AttackData attackData)
	{
		if (this.bulletTrash != null)
		{
			this.bulletTrash.Play();
		}
		this.ReleaseBullet(attackData);
		this.PlaySoundAttack();
	}

	public override void PlaySoundAttack()
	{
		if (this.attackSounds.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, this.attackSounds.Length);
			this.audioSource.PlayOneShot(this.attackSounds[num]);
		}
	}

	protected virtual void PlaySoundCartouche()
	{
		Debug.Log("Gun Fire Is play Method");
		if (this.hasCartouche)
		{
			Debug.Log("Gun Fire Is play If");
			if (UIController.Instance.btnAutoFire.image.sprite == UIController.Instance.sprAutoFireOn)
			{
				Debug.Log("Gun Fire Is play If in Sprite IF");
				// SoundManager.Instance.PlaySfx(this.soundCartouche, -15f);
			}
			else if (UIController.Instance.btnAutoFire.image.sprite == UIController.Instance.sprAutoFireOff)
			{
				Debug.Log("Gun Fire Is play If in Sprite Else IF");
				SoundManager.Instance.PlaySfx(this.soundCartouche, -15f);
			}
			// SoundManager.Instance.PlaySfx(this.soundCartouche, -15f);
		}
	}

	protected virtual void ActiveMuzzle()
	{
		if (this.muzzlePrefab)
		{
			if (this.muzzle == null)
			{
				this.muzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.muzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.parent);
			}
			this.muzzle.Active();
		}
	}

	public virtual void ConsumeAmmo(int amount = 1)
	{
		if (!this.isInfinityAmmo)
		{
			if (this.ammo <= 0)
			{
				this.ammo = 0;
				EventDispatcher.Instance.PostEvent(EventID.OutOfAmmo);
				return;
			}
			this.ammo -= amount;
			Singleton<UIController>.Instance.UpdateGunTypeText(false, this.ammo);
		}
	}

	protected virtual void ReleaseBullet(AttackData attackData)
	{
		this.ConsumeAmmo(1);
	}

	public virtual void ReleaseCrossBullets(AttackData attackData, Transform crossFirePoint, bool isFacingRight)
	{
		this.ConsumeAmmo(this.numberCrossBullet);
	}
}
