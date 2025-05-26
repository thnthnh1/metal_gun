using System;
using System.Collections.Generic;
using UnityEngine;

public class _StaticGunData : Dictionary<int, StaticGunData>
{
	public StaticGunData GetData(int id)
	{
		if (base.ContainsKey(id))
		{
			return base[id];
		}
		return null;
	}

	public SO_GunStats GetBaseStats(int id, int level)
	{
		StaticGunData data = this.GetData(id);
		string path = string.Format(data.statsPath, level);
		return Resources.Load<SO_GunStats>(path);
	}

	public float GetFireRate(int id, int level)
	{
		SO_GunStats baseStats = this.GetBaseStats(id, level);
		float result;
		if (id == 108)
		{
			result = 1f / ((SO_GunFlameStats)baseStats).TimeApplyDamage;
		}
		else if (id == 103)
		{
			result = 1f / ((SO_GunLaserStats)baseStats).TimeApplyDamage;
		}
		else
		{
			if (id != 106)
			{
				return baseStats.AttackTimePerSecond;
			}
			result = 1f / ((SO_GunTeslaStats)baseStats).TimeApplyDamage;
		}
		return result;
	}

	public float GetBattlePower(int id, int level)
	{
		SO_GunStats baseStats = this.GetBaseStats(id, level);
		float num = baseStats.Damage;
		float num2 = baseStats.CriticalRate / 100f;
		float num3 = 1f + baseStats.CriticalDamageBonus / 100f;
		float result;
		if (id == 107)
		{
			num *= 0.75f;
			result = ((1f - num2) * num + num2 * num3 * num) * baseStats.AttackTimePerSecond * (1f / ((SO_GunKamePowerStats)baseStats).ChargeTime);
		}
		else if (id == 108)
		{
			result = ((1f - num2) * num + num2 * num3 * num) * (1f / ((SO_GunFlameStats)baseStats).TimeApplyDamage);
		}
		else if (id == 103)
		{
			result = ((1f - num2) * num + num2 * num3 * num) * (1f / ((SO_GunLaserStats)baseStats).TimeApplyDamage);
		}
		else if (id == 105)
		{
			result = ((1f - num2) * (num * (2f / baseStats.BulletSpeed) / ((SO_GunFireBallStats)baseStats).TimeApplyDamage) + num2 * num3 * num * (2f / baseStats.BulletSpeed) / ((SO_GunFireBallStats)baseStats).TimeApplyDamage) * baseStats.AttackTimePerSecond;
		}
		else if (id == 100)
		{
			num *= 3f;
			result = ((1f - num2) * num + num2 * num3 * num) * baseStats.AttackTimePerSecond;
		}
		else if (id == 106)
		{
			num *= 1.3f;
			result = ((1f - num2) * num + num2 * num3 * num) * (1f / ((SO_GunTeslaStats)baseStats).TimeApplyDamage);
		}
		else if (id == 101)
		{
			num *= 1.2f;
			result = ((1f - num2) * num + num2 * num3 * num) * baseStats.AttackTimePerSecond;
		}
		else
		{
			result = ((1f - num2) * num + num2 * num3 * num) * baseStats.AttackTimePerSecond;
		}
		return result;
	}

	public WeaponStatsGrade GetGradeDamage(float damage)
	{
		if (damage >= 40f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (damage >= 25f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (damage >= 10f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (damage >= 3.5f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}

	public WeaponStatsGrade GetGradeFireRate(float rate)
	{
		if (rate >= 6.5f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (rate >= 5f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (rate >= 2.2f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (rate >= 1f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}

	public WeaponStatsGrade GetGradeCritRate(float rate)
	{
		if (rate >= 20f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (rate >= 15f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (rate >= 10f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (rate >= 5f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}

	public WeaponStatsGrade GetGradeCritDamage(float damage)
	{
		if (damage > 100f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (damage >= 80f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (damage >= 60f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (damage >= 50f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}
}
