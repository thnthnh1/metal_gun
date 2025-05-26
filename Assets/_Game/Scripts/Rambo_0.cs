using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rambo_0 : Rambo
{
	private sealed class _CoroutineCooldownGrenade_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float cooldown;

		internal float _newCooldown___0;

		internal float _count___0;

		internal float _percentCooldown___1;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineCooldownGrenade_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._newCooldown___0 = this.cooldown;
				if (this._this.Skills.decreaseCooldownGrenade.level > 0)
				{
					this._newCooldown___0 = this.cooldown * (1f - this._this.Skills.decreaseCooldownGrenade.value / 100f);
				}
				this._this.isCooldownGrenade = true;
				Singleton<UIController>.Instance.SetCooldownButtonGrenade(false);
				this._count___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._this.isCooldownGrenade)
			{
				this._count___0 += Time.deltaTime;
				this._this.isCooldownGrenade = (this._count___0 < this._newCooldown___0);
				this._percentCooldown___1 = Mathf.Clamp01(this._count___0 / this._newCooldown___0);
				Singleton<UIController>.Instance.imageCooldownGrenade.fillAmount = this._percentCooldown___1;
				Singleton<UIController>.Instance.textCooldownGrenade.text = string.Format("{0:f1}", this._newCooldown___0 - this._count___0);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isCooldownGrenade = false;
			Singleton<UIController>.Instance.SetCooldownButtonGrenade(true);
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _CoroutineRegen_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _timer___0;

		internal float _hp___0;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineRegen_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timer___0 = 0;
				this._hp___0 = this._this.stats.MaxHp * (this._this.Skills.recoverAtLowHP.value / 100f);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < 5)
			{
				this._this.RestoreHP(this._hp___0, false);
				this._timer___0++;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.StartCoroutine(this._this.CoroutineCooldownRegen());
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _CoroutineCooldownRegen_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _timer___0;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineCooldownRegen_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timer___0 = 0;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.cooldownRegen)
			{
				this._timer___0++;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isReadyRegen = true;
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _CoroutineReflect_c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _duration___0;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineReflect_c__Iterator3()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.isReflect = true;
				this._this.isImmortal = true;
				this._this.effectImmortal.SetActive(true);
				this._duration___0 = (int)this._this.Skills.createReflectShield.value;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._duration___0 > 0)
			{
				this._duration___0--;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isReflect = false;
			this._this.isImmortal = false;
			this._this.effectImmortal.SetActive(false);
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _CoroutineCooldownReflect_c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _timer___0;

		internal float _percent___1;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineCooldownReflect_c__Iterator4()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timer___0 = 0;
				Singleton<UIController>.Instance.EnableSkill(false);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.cooldownReflect)
			{
				this._percent___1 = Mathf.Clamp01((float)this._timer___0 / (float)this._this.cooldownReflect);
				Singleton<UIController>.Instance.SetCooldownSkill(this._percent___1);
				Singleton<UIController>.Instance.SetTextCooldownSkill((float)(this._this.cooldownReflect - this._timer___0));
				this._timer___0++;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			Singleton<UIController>.Instance.EnableSkill(true);
			this._this.isReadyReflect = true;
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _CoroutineBomb_c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _duration___0;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineBomb_c__Iterator5()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._duration___0 = this._this.Skills.activeBomb.value;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._duration___0 > 0f)
			{
				this._duration___0 -= 0.3f;
				this._this.ReleaseBomb();
				this._current = this._this.bombInterval;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _CoroutineCooldownBomb_c__Iterator6 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _timer___0;

		internal float _percent___1;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineCooldownBomb_c__Iterator6()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timer___0 = 0;
				Singleton<UIController>.Instance.EnableSkill(false);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.cooldownBomb)
			{
				this._percent___1 = Mathf.Clamp01((float)this._timer___0 / (float)this._this.cooldownBomb);
				Singleton<UIController>.Instance.SetCooldownSkill(this._percent___1);
				Singleton<UIController>.Instance.SetTextCooldownSkill((float)(this._this.cooldownBomb - this._timer___0));
				this._timer___0++;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			Singleton<UIController>.Instance.EnableSkill(true);
			this._this.isReadyBomb = true;
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _CoroutineRage_c__Iterator7 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _duration___0;

		internal float _value___0;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineRage_c__Iterator7()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.isRage = true;
				this._this.effectRage.SetActive(false);
				this._this.effectRage.SetActive(true);
				this._this.ActiveSkinRage(true);
				this._duration___0 = 15;
				this._value___0 = this._this.Skills.rage.value / 100f;
				this._this.IncreaseStats(this._value___0);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._duration___0 > 0 && !this._this.isDead)
			{
				this._duration___0--;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isRage = false;
			this._this.effectRage.SetActive(false);
			this._this.ActiveSkinRage(false);
			this._this.RemoveStatsBonus(this._value___0);
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _CoroutineCooldownRage_c__Iterator8 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _timer___0;

		internal float _percent___1;

		internal Rambo_0 _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineCooldownRage_c__Iterator8()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timer___0 = 0;
				Singleton<UIController>.Instance.EnableSkill(false);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.cooldownRage)
			{
				this._percent___1 = Mathf.Clamp01((float)this._timer___0 / (float)this._this.cooldownRage);
				Singleton<UIController>.Instance.SetCooldownSkill(this._percent___1);
				Singleton<UIController>.Instance.SetTextCooldownSkill((float)(this._this.cooldownRage - this._timer___0));
				this._timer___0++;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			Singleton<UIController>.Instance.EnableSkill(true);
			this._this.isReadyRage = true;
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	[Header("RAMBO JOHN")]
	public GameObject effectRage;

	[SpineAnimation("", "", true, false)]
	public string rage;

	[SpineSkin("", "", true, false)]
	public string skinDefault;

	[SpineSkin("", "", true, false)]
	public string skinRage;

	private bool isReadyRegen = true;

	private int cooldownRegen = 60;

	private bool isReadyReflect = true;

	private bool isReflect;

	private int cooldownReflect = 60;

	public BombSupportSkill bombPrefab;

	private bool isReadyBomb = true;

	private int cooldownBomb = 60;

	private WaitForSeconds bombInterval = new WaitForSeconds(0.05f);

	private bool isReadyRage = true;

	private bool isRage;

	private int cooldownRage = 60;

	public SkillTreeRambo_0 Skills
	{
		get
		{
			return (SkillTreeRambo_0)this.skillTree;
		}
	}

	protected override void Start()
	{
		base.Start();
		EventDispatcher.Instance.RegisterListener(EventID.ActiveReflectShield, delegate(Component sender, object param)
		{
			this.ActiveReflectShield();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ActiveBomb, delegate(Component sender, object param)
		{
			this.ActiveBomb();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ActiveRage, delegate(Component sender, object param)
		{
			this.ActiveRage();
		});
	}

	public override void Renew()
	{
		base.Renew();
		this.isReflect = false;
		this.effectRage.SetActive(false);
		this.ActiveSkinRage(false);
	}

	public override AttackData GetMeleeWeaponAttackData()
	{
		AttackData meleeWeaponAttackData = base.GetMeleeWeaponAttackData();
		if (this.Skills.increaseMeleeWeaponDamage.level > 0)
		{
			float damage = meleeWeaponAttackData.damage * (1f + this.Skills.increaseMeleeWeaponDamage.value / 100f);
			meleeWeaponAttackData.damage = damage;
		}
		return meleeWeaponAttackData;
	}

	public override AttackData GetGunAttackData()
	{
		AttackData gunAttackData = base.GetGunAttackData();
		if (gunAttackData.weapon == WeaponType.NormalGun)
		{
			if (this.Skills.increaseNormalGunDamage.level > 0)
			{
				float damage = gunAttackData.damage * (1f + this.Skills.increaseNormalGunDamage.value / 100f);
				gunAttackData.damage = damage;
			}
		}
		else if (gunAttackData.weapon == WeaponType.SpecialGun && this.Skills.increaseSpecialGunDamage.level > 0)
		{
			float damage2 = gunAttackData.damage * (1f + this.Skills.increaseSpecialGunDamage.value / 100f);
			gunAttackData.damage = damage2;
		}
		if (this.Skills.increaseDamageUnitMuchHP.level > 0)
		{
			DebuffData item = new DebuffData(DebuffType.TakeMoreDamageWhenHighHP, 0f, this.Skills.increaseDamageUnitMuchHP.value);
			if (gunAttackData.debuffs == null)
			{
				gunAttackData.debuffs = new List<DebuffData>
				{
					item
				};
			}
			else
			{
				gunAttackData.debuffs.Add(item);
			}
		}
		return gunAttackData;
	}

	public override AttackData GetGrenadeAttackData(BaseGrenade grenade)
	{
		AttackData grenadeAttackData = base.GetGrenadeAttackData(grenade);
		if (this.Skills.grenadeStun.level > 0)
		{
			DebuffData item = new DebuffData(DebuffType.Stun, this.Skills.grenadeStun.value, 0f);
			if (grenadeAttackData.debuffs == null)
			{
				grenadeAttackData.debuffs = new List<DebuffData>
				{
					item
				};
			}
			else
			{
				grenadeAttackData.debuffs.Add(item);
			}
		}
		return grenadeAttackData;
	}

	protected override float GetReviveImmortalDuration()
	{
		if (this.Skills.increaseImmortalDuration.level > 0)
		{
			return this.Skills.increaseImmortalDuration.value;
		}
		return base.GetReviveImmortalDuration();
	}

	protected override void CalculateBaseStatsIncrease()
	{
		base.CalculateBaseStatsIncrease();
		if (this.Skills.increaseHP.level > 0)
		{
			float value = this.baseStats.HP * (this.Skills.increaseHP.value / 100f);
			this.stats.AdjustStats(value, StatsType.MaxHp);
		}
		if (this.Skills.increaseSpeed.level > 0)
		{
			float value2 = this.baseStats.MoveSpeed * (this.Skills.increaseSpeed.value / 100f);
			this.stats.AdjustStats(value2, StatsType.MoveSpeed);
		}
	}

	public override void TakeDamage(AttackData attackData)
	{
		if (this.isDead || attackData.attacker.isDead)
		{
			return;
		}
		if (this.isImmortal)
		{
			EffectController arg_5D_0 = EffectController.Instance;
			Vector2 position = this.bodyCenterPoint.position;
			Color color = Color.yellow;
			string content = "BLOCK";
			Transform groupText = Singleton<PoolingController>.Instance.groupText;
			arg_5D_0.SpawnTextTMP(position, color, content, 3.5f, groupText);
			if (this.isReflect)
			{
				float num = attackData.damage * 0.15f;
				DebuffData item = new DebuffData(DebuffType.Reflect, 0f, num);
				List<DebuffData> list = new List<DebuffData>
				{
					item
				};
				float damage = num;
				List<DebuffData> debuffs = list;
				AttackData attackData2 = new AttackData(this, damage, 0f, false, WeaponType.NormalGun, -1, debuffs);
				attackData.attacker.TakeDamage(attackData2);
			}
		}
		else
		{
			if (this.Skills.evade.level > 0)
			{
				bool flag = (float)UnityEngine.Random.Range(1, 101) <= this.Skills.evade.value;
				if (flag)
				{
					EffectController arg_143_0 = EffectController.Instance;
					Vector2 position = this.bodyCenterPoint.position;
					Color color = Color.gray;
					string content = "EVADE";
					Transform groupText = Singleton<PoolingController>.Instance.groupText;
					arg_143_0.SpawnTextTMP(position, color, content, 3.5f, groupText);
					return;
				}
			}
			if (this.Skills.reduceDamageTaken.level > 0)
			{
				float num2 = attackData.damage * Mathf.Clamp01(this.Skills.reduceDamageTaken.value / 100f);
				attackData.damage -= num2;
			}
			this.EffectTakeDamage();
			this.ShowTextDamageTaken(attackData.damage);
			this.stats.AdjustStats(-attackData.damage, StatsType.Hp);
			this.UpdateHealthBar(false);
			if (this.HpPercent < 0.2f)
			{
				base.ActiveSoundLowHp(true);
			}
			if (this.stats.HP <= 0f)
			{
				this.Die();
				if (GameData.mode == GameMode.Campaign && ((BaseEnemy)attackData.attacker).isFinalBoss)
				{
					EventLogger.LogEvent("N_KilledByFinalBoss", new object[]
					{
						string.Format("ID={0},{1}-{2}", ((BaseEnemy)attackData.attacker).id, GameData.currentStage.id, GameData.currentStage.difficulty)
					});
				}
			}
			else if ((double)this.HpPercent <= 0.2 && this.Skills.recoverAtLowHP.level > 0 && this.isReadyRegen)
			{
				this.isReadyRegen = false;
				base.StartCoroutine(this.CoroutineRegen());
			}
		}
	}

	public override void TakeDamage(float damage)
	{
		if (!this.isDead)
		{
			if (this.isImmortal)
			{
				EffectController arg_4C_0 = EffectController.Instance;
				Vector2 position = this.bodyCenterPoint.position;
				Color color = Color.yellow;
				string content = "BLOCK";
				Transform groupText = Singleton<PoolingController>.Instance.groupText;
				arg_4C_0.SpawnTextTMP(position, color, content, 3.5f, groupText);
			}
			else
			{
				if (this.Skills.evade.level > 0)
				{
					bool flag = (float)UnityEngine.Random.Range(1, 101) <= this.Skills.evade.value;
					if (flag)
					{
						EffectController arg_C9_0 = EffectController.Instance;
						Vector2 position = this.bodyCenterPoint.position;
						Color color = Color.gray;
						string content = "EVADE";
						Transform groupText = Singleton<PoolingController>.Instance.groupText;
						arg_C9_0.SpawnTextTMP(position, color, content, 3.5f, groupText);
						return;
					}
				}
				if (this.Skills.reduceDamageTaken.level > 0)
				{
					float num = damage * Mathf.Clamp01(this.Skills.reduceDamageTaken.value / 100f);
					damage -= num;
				}
				this.EffectTakeDamage();
				this.ShowTextDamageTaken(damage);
				this.stats.AdjustStats(-damage, StatsType.Hp);
				this.UpdateHealthBar(false);
				if (this.stats.HP <= 0f)
				{
					this.Die();
				}
				else if ((double)this.HpPercent <= 0.2 && this.Skills.recoverAtLowHP.level > 0 && this.isReadyRegen)
				{
					this.isReadyRegen = false;
					base.StartCoroutine(this.CoroutineRegen());
				}
			}
		}
	}

	protected override void OnFinishStage(float delayEndGame)
	{
		this.isImmortal = true;
		this.StartDelayAction(delegate
		{
			SoundManager.Instance.PlaySfx("sfx_voice_victory", 0f);
			base.PlayAnimationVictory();
			if (this.Skills.bonusCoin.level > 0)
			{
				EventDispatcher.Instance.PostEvent(EventID.BonusCoinCollected, this.Skills.bonusCoin.value);
			}
			if (this.Skills.bonusExp.level > 0)
			{
				EventDispatcher.Instance.PostEvent(EventID.BonusExpWin, this.Skills.bonusExp.value);
			}
		}, delayEndGame);
	}

	protected override void RestoreHP(float value, bool isFromItemDrop)
	{
		if (isFromItemDrop)
		{
			float num = (this.stats.MaxHp - this.stats.HP) * 0.5f;
			if (num < 500f)
			{
				num = 500f;
			}
			if (this.Skills.bonusItemHealthValue.level > 0)
			{
				num *= 1f + this.Skills.bonusItemHealthValue.value / 100f;
			}
			this.stats.AdjustStats(num, StatsType.Hp);
			if (this.stats.HP > this.stats.MaxHp)
			{
				this.stats.SetStats(this.stats.MaxHp, StatsType.Hp);
			}
			this.UpdateHealthBar(false);
			base.ActiveSoundLowHp(this.HpPercent < 0.2f);
			this.effectRestoreHP.Play();
			int num2 = Mathf.RoundToInt(num * 10f);
			EffectController arg_120_0 = EffectController.Instance;
			Vector2 position = base.BodyCenterPoint.position;
			Color green = Color.green;
			string content = string.Format("+{0} HP", num2);
			Transform groupText = Singleton<PoolingController>.Instance.groupText;
			arg_120_0.SpawnTextTMP(position, green, content, 3.5f, groupText);
			SoundManager.Instance.PlaySfx(this.soundRevive, 0f);
		}
		else
		{
			base.RestoreHP(value, false);
		}
	}

	protected override IEnumerator CoroutineCooldownGrenade(float cooldown)
	{
		Rambo_0._CoroutineCooldownGrenade_c__Iterator0 _CoroutineCooldownGrenade_c__Iterator = new Rambo_0._CoroutineCooldownGrenade_c__Iterator0();
		_CoroutineCooldownGrenade_c__Iterator.cooldown = cooldown;
		_CoroutineCooldownGrenade_c__Iterator._this = this;
		return _CoroutineCooldownGrenade_c__Iterator;
	}

	private IEnumerator CoroutineRegen()
	{
		Rambo_0._CoroutineRegen_c__Iterator1 _CoroutineRegen_c__Iterator = new Rambo_0._CoroutineRegen_c__Iterator1();
		_CoroutineRegen_c__Iterator._this = this;
		return _CoroutineRegen_c__Iterator;
	}

	private IEnumerator CoroutineCooldownRegen()
	{
		Rambo_0._CoroutineCooldownRegen_c__Iterator2 _CoroutineCooldownRegen_c__Iterator = new Rambo_0._CoroutineCooldownRegen_c__Iterator2();
		_CoroutineCooldownRegen_c__Iterator._this = this;
		return _CoroutineCooldownRegen_c__Iterator;
	}

	private void ActiveReflectShield()
	{
		if (this.isReadyReflect)
		{
			this.isReadyReflect = false;
			base.StartCoroutine(this.CoroutineReflect());
			base.StartCoroutine(this.CoroutineCooldownReflect());
		}
	}

	private IEnumerator CoroutineReflect()
	{
		Rambo_0._CoroutineReflect_c__Iterator3 _CoroutineReflect_c__Iterator = new Rambo_0._CoroutineReflect_c__Iterator3();
		_CoroutineReflect_c__Iterator._this = this;
		return _CoroutineReflect_c__Iterator;
	}

	private IEnumerator CoroutineCooldownReflect()
	{
		Rambo_0._CoroutineCooldownReflect_c__Iterator4 _CoroutineCooldownReflect_c__Iterator = new Rambo_0._CoroutineCooldownReflect_c__Iterator4();
		_CoroutineCooldownReflect_c__Iterator._this = this;
		return _CoroutineCooldownReflect_c__Iterator;
	}

	private void ActiveBomb()
	{
		if (this.isReadyBomb)
		{
			this.isReadyBomb = false;
			base.StartCoroutine(this.CoroutineBomb());
			base.StartCoroutine(this.CoroutineCooldownBomb());
		}
	}

	private void ReleaseBomb()
	{
		BombSupportSkill bombSupportSkill = Singleton<PoolingController>.Instance.poolBombSupportSkill.New();
		if (bombSupportSkill == null)
		{
			bombSupportSkill = UnityEngine.Object.Instantiate<BombSupportSkill>(this.bombPrefab);
		}
		Vector2 position = Singleton<CameraFollow>.Instance.top.position;
		position.y += 1.5f;
		position.x = UnityEngine.Random.Range(Singleton<CameraFollow>.Instance.left.position.x, Singleton<CameraFollow>.Instance.right.position.x);
		bombSupportSkill.Active(position, UnityEngine.Random.Range(50f, 80f), null);
	}

	private IEnumerator CoroutineBomb()
	{
		Rambo_0._CoroutineBomb_c__Iterator5 _CoroutineBomb_c__Iterator = new Rambo_0._CoroutineBomb_c__Iterator5();
		_CoroutineBomb_c__Iterator._this = this;
		return _CoroutineBomb_c__Iterator;
	}

	private IEnumerator CoroutineCooldownBomb()
	{
		Rambo_0._CoroutineCooldownBomb_c__Iterator6 _CoroutineCooldownBomb_c__Iterator = new Rambo_0._CoroutineCooldownBomb_c__Iterator6();
		_CoroutineCooldownBomb_c__Iterator._this = this;
		return _CoroutineCooldownBomb_c__Iterator;
	}

	private void ActiveRage()
	{
		if (this.isReadyRage)
		{
			this.isReadyRage = false;
			base.StartCoroutine(this.CoroutineRage());
			base.StartCoroutine(this.CoroutineCooldownRage());
		}
	}

	private void IncreaseStats(float percent)
	{
		this.AddModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, percent));
		this.AddModifier(new ModifierData(StatsType.AttackTimePerSecond, ModifierType.AddPercentBase, percent));
		this.AddModifier(new ModifierData(StatsType.CriticalRate, ModifierType.AddPercentBase, percent));
		this.ReloadStats();
	}

	private void RemoveStatsBonus(float percent)
	{
		this.RemoveModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, percent));
		this.RemoveModifier(new ModifierData(StatsType.AttackTimePerSecond, ModifierType.AddPercentBase, percent));
		this.RemoveModifier(new ModifierData(StatsType.CriticalRate, ModifierType.AddPercentBase, percent));
		this.ReloadStats();
	}

	private IEnumerator CoroutineRage()
	{
		Rambo_0._CoroutineRage_c__Iterator7 _CoroutineRage_c__Iterator = new Rambo_0._CoroutineRage_c__Iterator7();
		_CoroutineRage_c__Iterator._this = this;
		return _CoroutineRage_c__Iterator;
	}

	private IEnumerator CoroutineCooldownRage()
	{
		Rambo_0._CoroutineCooldownRage_c__Iterator8 _CoroutineCooldownRage_c__Iterator = new Rambo_0._CoroutineCooldownRage_c__Iterator8();
		_CoroutineCooldownRage_c__Iterator._this = this;
		return _CoroutineCooldownRage_c__Iterator;
	}

	private void ActiveSkinRage(bool isActive)
	{
		string skin = (!isActive) ? this.skinDefault : this.skinRage;
		this.skeletonAnimation.Skeleton.SetSkin(skin);
	}
}
