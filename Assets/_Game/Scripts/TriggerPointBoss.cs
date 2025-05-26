using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TriggerPointBoss : MonoBehaviour
{
	private sealed class _CoroutineFadeMusic_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _currentVolume___0;

		internal WaitForSeconds _waitFadeMusic___0;

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

		public _CoroutineFadeMusic_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._currentVolume___0 = SoundManager.Instance.audioMusic.volume;
				this._waitFadeMusic___0 = new WaitForSeconds(0.2f);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (SoundManager.Instance.audioMusic.volume > 0f)
			{
				SoundManager.Instance.audioMusic.volume -= 0.2f;
				this._current = this._waitFadeMusic___0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			SoundManager.Instance.audioMusic.volume = this._currentVolume___0;
			SoundManager.Instance.PlayMusic("music_boss", 0f);
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

	public BaseEnemy bossPrefab;

	public int levelInNormal = 1;

	public Collider2D lockWallLeft;

	public Collider2D lockWallRight;

	public Transform lockPointTop;

	public Transform playerStandPosition;

	public Transform spawnPosition;

	public Transform basePosition;

	public bool isAutoSpawnEnemy;

	protected BoxCollider2D sensor;

	private void Awake()
	{
		this.sensor = base.GetComponent<BoxCollider2D>();
		this.lockWallLeft.gameObject.SetActive(false);
		this.lockWallRight.gameObject.SetActive(false);
		EventDispatcher.Instance.RegisterListener(EventID.WarningBossDone, delegate(Component sender, object param)
		{
			this.SpawnBoss();
		});
	}

	private void OnEnable()
	{
		this.ActiveSensor(true);
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.ActiveSensor(false);
			this.Lock();
			Singleton<UIController>.Instance.hudBoss.WarningBoss();
			if (GameData.mode == GameMode.Campaign)
			{
				((CampaignModeController)Singleton<GameController>.Instance.modeController).IsAllowSpawnSideEnemy = this.isAutoSpawnEnemy;
			}
		}
	}

	protected virtual void Lock()
	{
		this.lockWallLeft.gameObject.SetActive(true);
		this.lockWallRight.gameObject.SetActive(true);
		Singleton<CameraFollow>.Instance.SetMarginLeft(this.lockWallLeft.transform.position.x);
		Singleton<CameraFollow>.Instance.SetMarginRight(this.lockWallRight.transform.position.x);
		if (this.lockPointTop)
		{
			Singleton<CameraFollow>.Instance.SetMarginTop(this.lockPointTop.position.y);
		}
	}

	public void ActiveSensor(bool isActive)
	{
		this.sensor.enabled = isActive;
	}

	protected virtual void SpawnBoss()
	{
		if (base.gameObject.activeInHierarchy)
		{
			BaseEnemy baseEnemy = UnityEngine.Object.Instantiate<BaseEnemy>(this.bossPrefab);
			baseEnemy.basePosition = this.basePosition.position;
			int level = this.GetLevel();
			baseEnemy.Active(this.bossPrefab.id, level, this.spawnPosition.position);
			baseEnemy.SetTarget(Singleton<GameController>.Instance.Player);
			Singleton<GameController>.Instance.AddUnit(baseEnemy.gameObject, baseEnemy);
			Singleton<UIController>.Instance.hudBoss.UpdateHP(1f);
			this.SwitchMusic();
		}
	}

	protected int GetLevel()
	{
		int num = this.levelInNormal;
		if (GameData.mode == GameMode.Campaign)
		{
			if (GameData.currentStage.difficulty == Difficulty.Hard)
			{
				num += 2;
			}
			else if (GameData.currentStage.difficulty == Difficulty.Crazy)
			{
				num += 7;
			}
		}
		return Mathf.Clamp(num, 1, 20);
	}

	protected void SwitchMusic()
	{
		if (GameData.mode == GameMode.Campaign)
		{
			base.StartCoroutine(this.CoroutineFadeMusic());
		}
	}

	private IEnumerator CoroutineFadeMusic()
	{
		return new TriggerPointBoss._CoroutineFadeMusic_c__Iterator0();
	}
}
