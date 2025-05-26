using System;
using System.Collections.Generic;
using UnityEngine;

public class _StaticMeleeWeaponData : Dictionary<int, StaticMeleeWeaponData>
{
	public StaticMeleeWeaponData GetData(int id)
	{
		if (base.ContainsKey(id))
		{
			return base[id];
		}
		return null;
	}

	public SO_MeleeWeaponStats GetBaseStats(int id, int level)
	{
		StaticMeleeWeaponData data = this.GetData(id);
		string path = string.Format(data.statsPath, level);
		return Resources.Load<SO_MeleeWeaponStats>(path);
	}

	public float GetBattlePower(int id, int level)
	{
		SO_MeleeWeaponStats baseStats = this.GetBaseStats(id, level);
		float damage = baseStats.Damage;
		float num = baseStats.CriticalRate / 100f;
		float num2 = 1f + baseStats.CriticalDamageBonus / 100f;
		return ((1f - num) * damage + num * num2 * damage) * baseStats.AttackTimePerSecond;
	}

	public WeaponStatsGrade GetGradeDamage(float damage)
	{
		if (damage >= 100f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (damage >= 70f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (damage >= 50f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (damage >= 30f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}

	public WeaponStatsGrade GetGradeAttackSpeed(float atkSpeed)
	{
		if (atkSpeed >= 2f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (atkSpeed >= 1f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (atkSpeed >= 0.5f)
		{
			return WeaponStatsGrade.Grade_A;
		}
		if (atkSpeed >= 0.25f)
		{
			return WeaponStatsGrade.Grade_B;
		}
		return WeaponStatsGrade.Grade_C;
	}

	public WeaponStatsGrade GetGradeCritRate(float rate)
	{
		if (rate >= 50f)
		{
			return WeaponStatsGrade.Grade_SS;
		}
		if (rate >= 35f)
		{
			return WeaponStatsGrade.Grade_S;
		}
		if (rate >= 15f)
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
