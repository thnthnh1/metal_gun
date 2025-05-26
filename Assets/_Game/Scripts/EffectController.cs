using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectController : MonoBehaviour
{
	private static EffectController _Instance_k__BackingField;

	public TextDamage textDamageTMP;

	public EffectTextCRIT textCRIT;

	public ParticleSystem fxExplosiveGas;

	public ParticleSystem fxExplosiveBomb;

	public ParticleSystem fxExplosiveC4;

	public ParticleSystem fxBulletImpact;

	public ParticleSystem fxBulletImpactExplodeSmall;

	public ParticleSystem fxBulletImpactExplodeMedium;

	public ParticleSystem fxBulletImpactExplodeLarge;

	public ParticleSystem fxBulletImpactSplitGun;

	public ParticleSystem fxBulletImpactTeslaMini;

	public ParticleSystem fxWoodBoxBroken;

	public ParticleSystem fxExplosiveMultiple;

	public ParticleSystem fxStoneRainExplosion;

	public ParticleSystem fxStoneBrokenSmall;

	public ParticleSystem fxStoneBrokenMedium;

	public ParticleSystem fxGroundSmoke;

	private ParticleSystem.EmitParams bulletHitParam;

	public static EffectController Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (EffectController.Instance == null)
		{
			EffectController.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		this.bulletHitParam.velocity = Vector3.zero;
		this.bulletHitParam.startSize = 0.1f;
		this.bulletHitParam.startLifetime = 0.2f;
		this.bulletHitParam.startColor = Color.white;
	}

	public void SpawnParticleEffect(EffectObjectName effectName, Vector3 position)
	{
		switch (effectName)
		{
		case EffectObjectName.BulletImpactNormal:
			this.fxBulletImpact.transform.position = position;
			this.fxBulletImpact.Play();
			break;
		case EffectObjectName.BulletImpactExplodeSmall:
			this.fxBulletImpactExplodeSmall.transform.position = position;
			this.fxBulletImpactExplodeSmall.Play();
			break;
		case EffectObjectName.BulletImpactExplodeMedium:
			this.fxBulletImpactExplodeMedium.transform.position = position;
			this.fxBulletImpactExplodeMedium.Play();
			break;
		case EffectObjectName.ExplosionBomb:
			this.fxExplosiveBomb.transform.position = position;
			this.fxExplosiveBomb.Play();
			break;
		case EffectObjectName.ExplosionGas:
			this.fxExplosiveGas.transform.position = position;
			this.fxExplosiveGas.Play();
			break;
		case EffectObjectName.WoodBoxBroken:
			this.fxWoodBoxBroken.transform.position = position;
			this.fxWoodBoxBroken.Play();
			break;
		case EffectObjectName.BulletImpactExplodeLarge:
			this.fxBulletImpactExplodeLarge.transform.position = position;
			this.fxBulletImpactExplodeLarge.Play();
			break;
		case EffectObjectName.ExplosionC4:
			this.fxExplosiveC4.transform.position = position;
			this.fxExplosiveC4.Play();
			break;
		case EffectObjectName.ExplosionMultiple:
			this.fxExplosiveMultiple.transform.position = position;
			this.fxExplosiveMultiple.Play();
			break;
		case EffectObjectName.StoneRainExplosion:
			this.fxStoneRainExplosion.transform.position = position;
			this.fxStoneRainExplosion.Play();
			break;
		case EffectObjectName.StoneBrokenSmall:
			this.fxStoneBrokenSmall.transform.position = position;
			this.fxStoneBrokenSmall.Play();
			break;
		case EffectObjectName.StoneBrokenMedium:
			this.fxStoneBrokenMedium.transform.position = position;
			this.fxStoneBrokenMedium.Play();
			break;
		case EffectObjectName.GroundSmoke:
			this.fxGroundSmoke.transform.position = position;
			this.fxGroundSmoke.Play();
			break;
		case EffectObjectName.BulletImpactSplitGun:
			this.fxBulletImpactSplitGun.transform.position = position;
			this.fxBulletImpactSplitGun.Play();
			break;
		case EffectObjectName.BulletImpactTeslaMini:
			this.fxBulletImpactTeslaMini.transform.position = position;
			this.fxBulletImpactTeslaMini.Play();
			break;
		}
	}

	public void SpawnTextTMP(Vector2 position, Color color, string content, float fontSize = 0.3f, Transform parent = null)
	{
		TextDamage textDamage = Singleton<PoolingController>.Instance.poolTextDamageTMP.New();
		if (textDamage == null)
		{
			textDamage = UnityEngine.Object.Instantiate<TextDamage>(this.textDamageTMP);
		}
		textDamage.Active(position, content, color, fontSize, parent, true);
	}

	public void SpawnTextDamageTMP(Vector2 position, AttackData attackData, Transform parent = null)
	{
		TextDamage textDamage = Singleton<PoolingController>.Instance.poolTextDamageTMP.New();
		if (textDamage == null)
		{
			textDamage = UnityEngine.Object.Instantiate<TextDamage>(this.textDamageTMP);
		}
		textDamage.Active(position, attackData, parent, true);
	}

	public void SpawnTextCRIT(Vector2 position, Transform parent = null)
	{
		EffectTextCRIT effectTextCRIT = Singleton<PoolingController>.Instance.poolTextCRIT.New();
		if (effectTextCRIT == null)
		{
			effectTextCRIT = UnityEngine.Object.Instantiate<EffectTextCRIT>(this.textCRIT);
		}
		effectTextCRIT.Active(position, parent);
		Singleton<CameraFollow>.Instance.AddShake(0.1f, 0.1f);
		SoundManager.Instance.PlaySfx("sfx_critical_hit", 0f);
	}
}
