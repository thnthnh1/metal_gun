using System;
using UnityEngine;

public class PoolingPreviewController : Singleton<PoolingPreviewController>
{
	public Transform group;

	public ObjectPooling<BulletPreviewUzi> uzi;

	public ObjectPooling<BulletPreviewM4> m4;

	public ObjectPooling<BulletPreviewScarH> scarH;

	public ObjectPooling<BulletPreviewAWP> awp;

	public ObjectPooling<BulletPreviewShotgun> shotgun;

	public ObjectPooling<BulletPreviewP100> p100;

	public ObjectPooling<BulletPreviewBullpup> bullpup;

	public ObjectPooling<BulletPreviewSniperRifle> sniperRifle;

	public ObjectPooling<BulletPreviewTeslaMini> teslaMini;

	public ObjectPooling<BulletPreviewSpread> spread;

	public ObjectPooling<BulletPreviewFamas> famas;

	public ObjectPooling<BulletPreviewRocketChaser> rocketChaser;

	public ObjectPooling<BulletPreviewSplit> split;

	public ObjectPooling<BulletPreviewFireball> fireball;

	public ObjectPooling<BulletPreviewKamePower> kamePower;

	public ObjectPooling<BaseGrenadePreview> grenadeBase;

	public ObjectPooling<GrenadePreviewTet> grenadeTet;

	public ObjectPooling<TextDamage> textDamage;

	private void Awake()
	{
		this.uzi = new ObjectPooling<BulletPreviewUzi>(8);
		this.m4 = new ObjectPooling<BulletPreviewM4>(8);
		this.scarH = new ObjectPooling<BulletPreviewScarH>(8);
		this.awp = new ObjectPooling<BulletPreviewAWP>(8);
		this.shotgun = new ObjectPooling<BulletPreviewShotgun>(8);
		this.p100 = new ObjectPooling<BulletPreviewP100>(8);
		this.bullpup = new ObjectPooling<BulletPreviewBullpup>(8);
		this.sniperRifle = new ObjectPooling<BulletPreviewSniperRifle>(8);
		this.teslaMini = new ObjectPooling<BulletPreviewTeslaMini>(8);
		this.spread = new ObjectPooling<BulletPreviewSpread>(8);
		this.famas = new ObjectPooling<BulletPreviewFamas>(8);
		this.rocketChaser = new ObjectPooling<BulletPreviewRocketChaser>(8);
		this.split = new ObjectPooling<BulletPreviewSplit>(8);
		this.fireball = new ObjectPooling<BulletPreviewFireball>(8);
		this.kamePower = new ObjectPooling<BulletPreviewKamePower>(8);
		this.grenadeBase = new ObjectPooling<BaseGrenadePreview>(8);
		this.grenadeTet = new ObjectPooling<GrenadePreviewTet>(8);
		this.textDamage = new ObjectPooling<TextDamage>(8);
	}
}
