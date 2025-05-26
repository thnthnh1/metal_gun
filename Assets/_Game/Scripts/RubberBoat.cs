using CnControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RubberBoat : Vehicle
{
	private sealed class _DelayRenew_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal RubberBoat _this;

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

		public _DelayRenew_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this.isEmergencyStop = false;
				this._current = StaticValue.waitTwoSec;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			case 2u:
				this._this.isImmortal = false;
				this._PC = -1;
				break;
			}
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

	[Header("RUBBER BOAT")]
	public Animation anim;

	public ParticleSystem moveSplash;

	public ParticleSystem dropSplash;

	public ParticleSystem jumpSplash;

	public string moveAnimName;

	public string jumpAnimName;

	public string idleAnimName;

	public string knockBackAnimName;

	public bool autoMove;

	public bool canAttack;

	public Transform spawnHeroPosition;

	public Transform playerStopPoint;

	public AudioClip soundMove;

	public AudioClip soundJump;

	private float moveForce = 280f;

	private bool breakMove;

	private bool isJumping;

	private bool isKnocking;

	private float timerKnocking = 1f;

	private float obstacleCrashDamage = 30f;

	private float bossCrashDamage = 50f;

	private Rambo player;

	private bool isEmergencyStop;

	public override BaseUnit Player
	{
		get
		{
			return this.player;
		}
	}

	public override float HpPercent
	{
		get
		{
			return this.player.HpPercent;
		}
	}

	public override bool IsFacingRight
	{
		get
		{
			return this.player.IsFacingRight;
		}
	}

	private void Start()
	{
		if (GameData.currentStage.difficulty == Difficulty.Hard)
		{
			this.obstacleCrashDamage = 50f;
			this.bossCrashDamage = 80f;
		}
		else if (GameData.currentStage.difficulty == Difficulty.Crazy)
		{
			this.obstacleCrashDamage = 70f;
			this.bossCrashDamage = 100f;
		}
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonJump, delegate(Component sender, object param)
		{
			this.Jump();
		});
		EventDispatcher.Instance.RegisterListener(EventID.PlayerDie, new Action<Component, object>(this.EmergencyStop));
		EventDispatcher.Instance.RegisterListener(EventID.ReviveByAds, delegate(Component sender, object param)
		{
			this.Renew();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReviveByGem, delegate(Component sender, object param)
		{
			this.Renew();
		});
		if (this.autoMove)
		{
			this.moveSplash.Play();
			this.anim.Play(this.moveAnimName);
		}
		else
		{
			this.Idle();
		}
		this.isOnVehicle = true;
	}

	private void Update()
	{
		if (this.breakMove)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.playerStopPoint.position, Time.deltaTime);
			if (Vector3.Distance(base.transform.position, this.playerStopPoint.position) <= 0.1f)
			{
				base.transform.position = this.playerStopPoint.position;
				this.rigid.bodyType = RigidbodyType2D.Static;
				this.Idle();
				this.GetOut();
			}
		}
	}

	private void FixedUpdate()
	{
		if (!this.breakMove)
		{
			this.Move();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Get out Vehicle"))
		{
			if (!this.breakMove)
			{
				this.breakMove = true;
				this.rigid.velocity = Vector2.zero;
				this.rigid.angularVelocity = 0f;
				this.rigid.isKinematic = true;
				EventDispatcher.Instance.PostEvent(EventID.BoatStop);
			}
		}
		else if (other.CompareTag("Enemy"))
		{
			this.AddForce(base.transform.right, 2.5f, ForceMode2D.Impulse);
			this.TakeDamage(this.bossCrashDamage);
		}
		else if (other.CompareTag("Destructible Obstacle"))
		{
			this.TakeDamage(this.obstacleCrashDamage);
			other.gameObject.SetActive(false);
			if (!this.isEmergencyStop)
			{
				Singleton<CameraFollow>.Instance.AddShake(0.1f, 1f);
				EventDispatcher.Instance.PostEvent(EventID.BoatTriggerObstacle);
			}
		}
	}

	public void StartMove()
	{
		this.autoMove = true;
		this.moveSplash.Play();
		this.anim.Play(this.moveAnimName);
	}

	public void BoatTriggerWater()
	{
		this.dropSplash.Play();
		this.moveSplash.Play();
		this.isJumping = false;
		EventDispatcher.Instance.PostEvent(EventID.BoatTriggerWater, base.transform.position);
		SoundManager.Instance.PlaySfx("sfx_trigger_water", -15f);
	}

	private void ActiveSoundMove(bool isActive)
	{
		if (isActive)
		{
			if (this.audioSource.clip == null)
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.soundMove;
				this.audioSource.Play();
			}
		}
		else
		{
			this.audioSource.Stop();
			this.audioSource.clip = null;
		}
	}

	public override void TakeDamage(AttackData attackData)
	{
		if (!this.isImmortal && this.player)
		{
			this.player.TakeDamage(attackData);
		}
	}

	public override void TakeDamage(float damage)
	{
		if (!this.isImmortal && this.player)
		{
			this.player.TakeDamage(damage);
		}
	}

	protected override void Move()
	{
		if (this.isKnocking)
		{
			this.timerKnocking = Mathf.MoveTowards(this.timerKnocking, 0f, Time.deltaTime);
			this.rigid.velocity = Vector3.Lerp(this.rigid.velocity, Vector2.right * this.moveForce * Time.deltaTime, Time.deltaTime);
			this.isKnocking = (this.timerKnocking != 0f);
		}
		else if (this.autoMove)
		{
			if (this.isEmergencyStop)
			{
				this.rigid.velocity = Vector3.Lerp(this.rigid.velocity, Vector2.zero, 5f * Time.deltaTime);
				this.player.lastDiePosition = this.spawnHeroPosition.position;
			}
			else
			{
				this.rigid.velocity = Vector2.right * 180f * Time.deltaTime;
			}
		}
		else if (this.isEmergencyStop)
		{
			this.player.lastDiePosition = this.spawnHeroPosition.position;
		}
		else
		{
			float axis = CnInputManager.GetAxis("Horizontal");
			if (axis < -0.2f || axis > 0.2f)
			{
				this.rigid.velocity = Vector2.right * axis * this.moveForce * Time.deltaTime;
			}
		}
	}

	protected override void Jump()
	{
		if (this.breakMove)
		{
			return;
		}
		this.moveSplash.Stop();
		this.moveSplash.Clear();
		this.dropSplash.Stop();
		this.dropSplash.Clear();
		if (!this.isJumping)
		{
			this.jumpSplash.Play();
		}
		this.anim.Play(this.jumpAnimName, PlayMode.StopAll);
		this.anim.PlayQueued(this.moveAnimName);
		this.isJumping = true;
		SoundManager.Instance.PlaySfx(this.soundJump, -15f);
	}

	public override void Idle()
	{
		this.anim.CrossFade(this.idleAnimName, 0.06f);
		this.moveSplash.Stop();
		this.dropSplash.Stop();
		this.jumpSplash.Stop();
	}

	public override void GetIn(Rambo rambo)
	{
		base.tag = "Player";
		rambo.SetLookDir(true);
		rambo.Rigid.isKinematic = true;
		rambo.Rigid.simulated = false;
		rambo.transform.parent = this.spawnHeroPosition;
		rambo.transform.localPosition = Vector3.zero;
		rambo.transform.localEulerAngles = Vector3.zero;
		rambo.enableMoving = false;
		rambo.enableJumping = false;
		rambo.enableFlipX = false;
		rambo.isOnVehicle = true;
		this.player = rambo;
		this.ActiveSoundMove(true);
		base.enabled = true;
	}

	public override void GetOut()
	{
		base.enabled = false;
		base.tag = "Untagged";
		if (this.player)
		{
			this.player.Rigid.isKinematic = false;
			this.player.Rigid.simulated = true;
			this.player.transform.parent = null;
			this.player.enableMoving = true;
			this.player.enableJumping = true;
			this.player.enableFlipX = true;
			this.player.isOnVehicle = false;
			this.player.enabled = true;
			this.player.transform.eulerAngles = Vector3.zero;
			this.player = null;
		}
		this.ActiveSoundMove(false);
	}

	public override void AddForce(Vector3 dir, float force, ForceMode2D forceMode = ForceMode2D.Impulse)
	{
		if (!this.isEmergencyStop)
		{
			base.AddForce(dir, force, forceMode);
			this.isKnocking = true;
			this.timerKnocking = 1f;
			this.anim.Play(this.knockBackAnimName);
			this.anim.PlayQueued(this.moveAnimName);
			this.dropSplash.Play();
			this.moveSplash.Play();
		}
	}

	public override void Renew()
	{
		base.StartCoroutine(this.DelayRenew());
	}

	private void EmergencyStop(Component arg1, object arg2)
	{
		this.isImmortal = true;
		this.isEmergencyStop = true;
	}

	private IEnumerator DelayRenew()
	{
		RubberBoat._DelayRenew_c__Iterator0 _DelayRenew_c__Iterator = new RubberBoat._DelayRenew_c__Iterator0();
		_DelayRenew_c__Iterator._this = this;
		return _DelayRenew_c__Iterator;
	}
}
