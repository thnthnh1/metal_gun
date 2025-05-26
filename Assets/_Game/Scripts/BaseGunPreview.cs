using System;
using UnityEngine;

public class BaseGunPreview : MonoBehaviour
{
	public int id;

	public SO_GunStats baseStats;

	public BaseBulletPreview bulletPrefab;

	public BaseMuzzle muzzlePrefab;

	public Transform firePoint;

	public Transform muzzlePoint;

	public ParticleSystem bulletTrash;

	public AudioClip soundAttack;

	protected BaseMuzzle muzzle;

	public virtual void Fire()
	{
		if (this.bulletTrash)
		{
			this.bulletTrash.Play();
		}
		this.ReleaseBullet();
	}

	public virtual void ActiveMuzzle()
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

	protected virtual void ReleaseBullet()
	{
	}

	protected virtual void PlaySoundAttack()
	{
		if (this.soundAttack)
		{
			SoundManager.Instance.PlaySfx(this.soundAttack, 0f);
		}
	}
}
