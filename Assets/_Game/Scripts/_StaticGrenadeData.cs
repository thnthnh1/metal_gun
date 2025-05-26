using System;
using System.Collections.Generic;
using UnityEngine;

public class _StaticGrenadeData : Dictionary<int, StaticGrenadeData>
{
	public StaticGrenadeData GetData(int id)
	{
		if (base.ContainsKey(id))
		{
			return base[id];
		}
		return null;
	}

	public SO_GrenadeStats GetBaseStats(int id, int level)
	{
		StaticGrenadeData data = this.GetData(id);
		string path = string.Format(data.statsPath, level);
		return Resources.Load<SO_GrenadeStats>(path);
	}

	public float GetBattlePower(int id, int level)
	{
		SO_GrenadeStats baseStats = this.GetBaseStats(id, level);
		float damage = baseStats.Damage;
		float num = baseStats.CriticalRate / 100f;
		float num2 = 1f + baseStats.CriticalDamageBonus / 100f;
		return ((1f - num) * damage + num * num2 * damage) * (1f / baseStats.Cooldown);
	}

	public WeaponStatsGrade GetGradeDamage(float damage)
	{
		if (damage >= 100f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (damage >= 50f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (damage >= 25f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (damage >= 15f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}

	public WeaponStatsGrade GetGradeRadius(float radius)
	{
		if (radius >= 3f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (radius >= 2.5f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (radius >= 2f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (radius >= 1f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}

	public WeaponStatsGrade GetGradeCooldown(float rate)
	{
		if (rate <= 0.5f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (rate <= 1.5f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (rate <= 3f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (rate <= 4f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}
}
