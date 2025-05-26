using System;
using UnityEngine;

public class GunKamePower : BaseGun
{
	public GameObject chargeEffect;

	public AudioSource audioSourceCharge;

	private float timerCharge;

	private bool isCharging;

	private bool isReadyShoot;

	private BaseUnit shooter;

	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Kame Power/gun_kame_power_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunKamePowerStats>(path);
	}

	protected override void Awake()
	{
		this.shooter = base.transform.root.GetComponent<BaseUnit>();
		this.chargeEffect.SetActive(false);
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, delegate(Component sender, object param)
		{
			this.Shoot((bool)param);
		});
	}

	private void Update()
	{
		if (this.isCharging)
		{
			this.timerCharge += Time.deltaTime;
			if (this.timerCharge >= ((SO_GunKamePowerStats)this.baseStats).ChargeTime)
			{
				this.timerCharge = 0f;
				AttackData curentAttackData = ((Rambo)this.shooter).GetCurentAttackData();
				this.ReleaseBullet(curentAttackData, 1f);
			}
		}
	}

	public override void Attack(AttackData attackData)
	{
		this.isReadyShoot = true;
	}

	private void ReleaseBullet(AttackData attackData, float percentCharge)
	{
		if (this.isReadyShoot)
		{
			this.isReadyShoot = false;
			base.ReleaseBullet(attackData);
			if (!this.isInfinityAmmo && this.ammo <= 0)
			{
				return;
			}
			BulletKamePower bulletKamePower = Singleton<PoolingController>.Instance.poolBulletKamePower.New();
			if (bulletKamePower == null)
			{
				bulletKamePower = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletKamePower);
			}
			attackData.damage *= percentCharge;
			float num = ((SO_GunKamePowerStats)this.baseStats).BulletSpeed;
			num *= percentCharge;
			bulletKamePower.Active(attackData, this.firePoint, num, percentCharge, null);
			this.ActiveMuzzle();
			this.PlaySoundAttack();
		}
	}

	private void Shoot(bool isFire)
	{
		if (this && base.gameObject.activeInHierarchy)
		{
			if (isFire)
			{
				this.isCharging = true;
				this.timerCharge = 0f;
				this.chargeEffect.SetActive(true);
				this.audioSourceCharge.Play();
			}
			else
			{
				this.isCharging = false;
				this.chargeEffect.SetActive(false);
				this.audioSourceCharge.Stop();
				float percentCharge = Mathf.Clamp(this.timerCharge / ((SO_GunKamePowerStats)this.baseStats).ChargeTime, 0.5f, 1f);
				AttackData curentAttackData = ((Rambo)this.shooter).GetCurentAttackData();
				this.ReleaseBullet(curentAttackData, percentCharge);
			}
		}
	}
}
