using System;
using UnityEngine;

public class PoolingController : Singleton<PoolingController>
{
	public Transform groupBullet;

	public Transform groupGrenade;

	public Transform groupEffect;

	public Transform groupText;

	public ObjectPooling<BulletUzi> poolBulletUzi;

	public ObjectPooling<BulletMachineGunM4> poolBulletMachineGunM4;

	public ObjectPooling<BulletScarHGun> poolBulletScarHGun;

	public ObjectPooling<BulletAWP> poolBulletAWP;

	public ObjectPooling<BulletShotgun> poolBulletShotgun;

	public ObjectPooling<BulletP100> poolBulletP100;

	public ObjectPooling<BulletBullpup> poolBulletBullpup;

	public ObjectPooling<BulletSniperRifle> poolBulletSniperRifle;

	public ObjectPooling<BulletTeslaMini> poolBulletTeslaMini;

	public ObjectPooling<BulletSpreadGun> poolBulletSpreadGun;

	public ObjectPooling<BulletRocketChaser> poolBulletRocketChaser;

	public ObjectPooling<BulletFamasGun> poolBulletFamasGun;

	public ObjectPooling<BulletSplitGun> poolBulletSplitGun;

	public ObjectPooling<FireBall> poolBulletFireBall;

	public ObjectPooling<BulletKamePower> poolBulletKamePower;

	public ObjectPooling<BombSupportSkill> poolBombSupportSkill;

	public ObjectPooling<BombSupportSurvival> poolBombSupportSurvival;

	public ObjectPooling<BulletRifle> poolBulletRifle;

	public ObjectPooling<BulletPistol> poolBulletPistol;

	public ObjectPooling<BulletSniper> poolBulletSniper;

	public ObjectPooling<BulletTank> poolBulletTank;

	public ObjectPooling<BulletTankCannon> poolBulletTankCannon;

	public ObjectPooling<Bomb> poolBulletBomb;

	public ObjectPooling<HomingMissile> poolHomingMissile;

	public ObjectPooling<BulletPlasma> poolBulletPlasma;

	public ObjectPooling<Torpedo> poolTorpedo;

	public ObjectPooling<BulletSpider> poolBulletSpider;

	public ObjectPooling<BulletBazooka> poolBulletBazooka;

	public ObjectPooling<BulletBossMegatron> poolBulletBossMegatron;

	public ObjectPooling<RocketBossMegatank> poolRocketBossMegatank;

	public ObjectPooling<RocketBossSubmarine> poolRocketBossSubmarine;

	public ObjectPooling<BulletPoisonBossVenom> poolBulletPoisonBossVenom;

	public ObjectPooling<BulletBossProfessor> poolBulletBossProfessor;

	public ObjectPooling<StoneBossMonkey> poolStoneBossMonkey;

	public ObjectPooling<StoneBossMonkeyMinion> poolStoneBossMonkeyMinion;

	public ObjectPooling<BaseGrenade> poolBaseGrenade;

	public ObjectPooling<BaseGrenadeEnemy> poolBaseGrenadeEnemy;

	public ObjectPooling<EnemyRifle> poolEnemyRifle;

	public ObjectPooling<EnemyGeneral> poolEnemyGeneral;

	public ObjectPooling<EnemyGrenade> poolEnemyGrenade;

	public ObjectPooling<EnemyKnife> poolEnemyKnife;

	public ObjectPooling<EnemyHelicopter> poolEnemyHelicopter;

	public ObjectPooling<EnemyTank> poolEnemyTank;

	public ObjectPooling<EnemyTankCannon> poolEnemyTankCannon;

	public ObjectPooling<EnemyBomber> poolEnemyBomber;

	public ObjectPooling<EnemySniper> poolEnemySniper;

	public ObjectPooling<EnemyFire> poolEnemyFire;

	public ObjectPooling<EnemyMarine> poolEnemyMarine;

	public ObjectPooling<EnemyParachutist> poolEnemyParachutist;

	public ObjectPooling<EnemyBazooka> poolEnemyBazooka;

	public ObjectPooling<EnemyMonkey> poolEnemyMonkey;

	public ObjectPooling<EnemySpider> poolEnemySpider;

	public ObjectPooling<BossMonkeyMinion> poolBossMonkeyMinion;

	public ObjectPooling<TextDamage> poolTextDamageTMP;

	public ObjectPooling<EffectTextBANG> poolTextBANG;

	public ObjectPooling<EffectTextCRIT> poolTextCRIT;

	public ObjectPooling<EffectTextWHAM> poolTextWHAM;

	public ObjectPooling<ItemDropHealth> poolItemDropHealth;

	public ObjectPooling<ItemDropCoin> poolItemDropCoin;

	public ObjectPooling<ItemDropGun> poolItemDropGun;

	public ObjectPooling<PoisonTrap> poolPoisonTrap;

	public ObjectPooling<Spike> poolSpike;

	public void InitPool()
	{
		this.poolBulletUzi = new ObjectPooling<BulletUzi>(8);
		this.poolBulletMachineGunM4 = new ObjectPooling<BulletMachineGunM4>(8);
		this.poolBulletScarHGun = new ObjectPooling<BulletScarHGun>(8);
		this.poolBulletAWP = new ObjectPooling<BulletAWP>(8);
		this.poolBulletShotgun = new ObjectPooling<BulletShotgun>(8);
		this.poolBulletP100 = new ObjectPooling<BulletP100>(8);
		this.poolBulletBullpup = new ObjectPooling<BulletBullpup>(8);
		this.poolBulletSniperRifle = new ObjectPooling<BulletSniperRifle>(8);
		this.poolBulletTeslaMini = new ObjectPooling<BulletTeslaMini>(8);
		this.poolBulletSpreadGun = new ObjectPooling<BulletSpreadGun>(8);
		this.poolBulletRocketChaser = new ObjectPooling<BulletRocketChaser>(8);
		this.poolBulletFamasGun = new ObjectPooling<BulletFamasGun>(8);
		this.poolBulletSplitGun = new ObjectPooling<BulletSplitGun>(8);
		this.poolBulletFireBall = new ObjectPooling<FireBall>(8);
		this.poolBulletKamePower = new ObjectPooling<BulletKamePower>(8);
		this.poolBombSupportSkill = new ObjectPooling<BombSupportSkill>(8);
		this.poolBombSupportSurvival = new ObjectPooling<BombSupportSurvival>(8);
		this.poolBulletRifle = new ObjectPooling<BulletRifle>(8);
		this.poolBulletPistol = new ObjectPooling<BulletPistol>(8);
		this.poolBulletSniper = new ObjectPooling<BulletSniper>(8);
		this.poolBulletTank = new ObjectPooling<BulletTank>(8);
		this.poolBulletTankCannon = new ObjectPooling<BulletTankCannon>(8);
		this.poolBulletBomb = new ObjectPooling<Bomb>(8);
		this.poolHomingMissile = new ObjectPooling<HomingMissile>(8);
		this.poolBulletPlasma = new ObjectPooling<BulletPlasma>(8);
		this.poolTorpedo = new ObjectPooling<Torpedo>(8);
		this.poolBulletSpider = new ObjectPooling<BulletSpider>(8);
		this.poolBulletBazooka = new ObjectPooling<BulletBazooka>(8);
		this.poolBulletBossMegatron = new ObjectPooling<BulletBossMegatron>(8);
		this.poolRocketBossMegatank = new ObjectPooling<RocketBossMegatank>(8);
		this.poolRocketBossSubmarine = new ObjectPooling<RocketBossSubmarine>(8);
		this.poolBulletPoisonBossVenom = new ObjectPooling<BulletPoisonBossVenom>(8);
		this.poolBulletBossProfessor = new ObjectPooling<BulletBossProfessor>(8);
		this.poolStoneBossMonkey = new ObjectPooling<StoneBossMonkey>(8);
		this.poolStoneBossMonkeyMinion = new ObjectPooling<StoneBossMonkeyMinion>(8);
		this.poolBaseGrenade = new ObjectPooling<BaseGrenade>(8);
		this.poolBaseGrenadeEnemy = new ObjectPooling<BaseGrenadeEnemy>(8);
		this.poolEnemyRifle = new ObjectPooling<EnemyRifle>(8);
		this.poolEnemyGeneral = new ObjectPooling<EnemyGeneral>(8);
		this.poolEnemyGrenade = new ObjectPooling<EnemyGrenade>(8);
		this.poolEnemyKnife = new ObjectPooling<EnemyKnife>(8);
		this.poolEnemyHelicopter = new ObjectPooling<EnemyHelicopter>(8);
		this.poolEnemyTank = new ObjectPooling<EnemyTank>(8);
		this.poolEnemyTankCannon = new ObjectPooling<EnemyTankCannon>(8);
		this.poolEnemyBomber = new ObjectPooling<EnemyBomber>(8);
		this.poolEnemySniper = new ObjectPooling<EnemySniper>(8);
		this.poolEnemyFire = new ObjectPooling<EnemyFire>(8);
		this.poolEnemyMarine = new ObjectPooling<EnemyMarine>(8);
		this.poolEnemyParachutist = new ObjectPooling<EnemyParachutist>(8);
		this.poolEnemyBazooka = new ObjectPooling<EnemyBazooka>(8);
		this.poolEnemyMonkey = new ObjectPooling<EnemyMonkey>(8);
		this.poolEnemySpider = new ObjectPooling<EnemySpider>(8);
		this.poolBossMonkeyMinion = new ObjectPooling<BossMonkeyMinion>(8);
		this.poolTextDamageTMP = new ObjectPooling<TextDamage>(8);
		this.poolTextBANG = new ObjectPooling<EffectTextBANG>(8);
		this.poolTextCRIT = new ObjectPooling<EffectTextCRIT>(8);
		this.poolTextWHAM = new ObjectPooling<EffectTextWHAM>(8);
		this.poolItemDropHealth = new ObjectPooling<ItemDropHealth>(8);
		this.poolItemDropCoin = new ObjectPooling<ItemDropCoin>(8);
		this.poolItemDropGun = new ObjectPooling<ItemDropGun>(8);
		this.poolPoisonTrap = new ObjectPooling<PoisonTrap>(8);
		this.poolSpike = new ObjectPooling<Spike>(8);
	}
}
