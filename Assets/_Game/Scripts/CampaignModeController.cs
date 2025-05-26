using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CampaignModeController : BaseModeController
{
	private sealed class _OnTransportJetDone_c__AnonStorey4
	{
		internal Rambo rambo;

		internal void __m__0()
		{
			this.rambo.enabled = true;
			this.rambo.Rigid.simulated = true;
			this.rambo.PlayAnimationIdle();
			this.rambo.effectDustGround.Play();
			this.rambo.ActiveHealthBar(true);
			this.rambo.transform.rotation = Quaternion.identity;
			Singleton<GameController>.Instance.IsGameStarted = true;
			EventDispatcher.Instance.PostEvent(EventID.GameStart);
		}
	}

	private sealed class _CoroutineAutoSpawnEnemy_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _enemySpawned___0;

		internal int numberEnemy;

		internal CampaignModeController _this;

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

		public _CoroutineAutoSpawnEnemy_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._enemySpawned___0 = 0;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._enemySpawned___0 < this.numberEnemy)
			{
				if (Singleton<CameraFollow>.Instance.IsCanSpawnGroundEnemyFromLeft())
				{
					this._this.SpawnEnemyFromSide(Singleton<CameraFollow>.Instance.pointGroundSpawnLeft);
				}
				if (Singleton<CameraFollow>.Instance.IsCanSpawnGroundEnemyFromRight())
				{
					this._this.SpawnEnemyFromSide(Singleton<CameraFollow>.Instance.pointGroundSpawnRight);
				}
				this._enemySpawned___0++;
				this._current = StaticValue.waitOneSec;
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

	private sealed class _UpdateMapProgress_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal CampaignModeController _this;

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

		public _UpdateMapProgress_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				break;
			case 1u:
				if (this._this.Player)
				{
					float num2 = this._this.Player.transform.position.x - this._this.Map.playerSpawnPoint.position.x;
					float num3 = this._this.Map.mapEndPoint.position.x - this._this.Map.playerSpawnPoint.position.x;
					float percent = Mathf.Clamp01(num2 / num3);
					Singleton<UIController>.Instance.UpdateMapProgress(percent);
				}
				break;
			default:
				return false;
			}
			this._current = StaticValue.waitHalfSec;
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
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

	private sealed class _CoroutineRoundTime_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _minute___0;

		internal int _second___0;

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

		public _CoroutineRoundTime_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._minute___0 = 0;
				this._second___0 = 0;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!Singleton<GameController>.Instance.IsPaused)
			{
				this._minute___0 = Singleton<GameController>.Instance.PlayTime / 60;
				this._second___0 = Singleton<GameController>.Instance.PlayTime % 60;
				Singleton<UIController>.Instance.UpdateGameTime(this._minute___0, this._second___0);
				Singleton<GameController>.Instance.PlayTime++;
			}
			this._current = StaticValue.waitOneSec;
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
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

	private sealed class _CoroutineFadeMusic_c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _currentVolume___0;

		internal WaitForSeconds _waitFadeMusic___0;

		internal bool isWin;

		internal string _nextMusic___0;

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

		public _CoroutineFadeMusic_c__Iterator3()
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
			SoundManager.Instance.audioMusic.Stop();
			this._nextMusic___0 = ((!this.isWin) ? "music_lose" : "music_win");
			SoundManager.Instance.PlaySfx(this._nextMusic___0, 0f);
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

	public TransportJet transportJetPrefab;

	private int highestComboKill;

	private int coinCollected;

	private int coinBonusComboKill;

	private int coinFirstComplete;

	private int coinTotal;

	private int expWin;

	private TransportJet jet;

	private Map _Map_k__BackingField;

	private bool _IsAllowSpawnSideEnemy_k__BackingField;

	private static Action<ShowResult> __f__am_cache0;

	public Map Map
	{
		get;
		private set;
	}

	public bool IsAllowSpawnSideEnemy
	{
		get;
		set;
	}

	public BaseUnit Player
	{
		get
		{
			return Singleton<GameController>.Instance.Player;
		}
	}

	public override void InitMode()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ViewAdsx2CoinEndGame, delegate(Component sender, object param)
		{
			this.OnViewAdsx2CoinEndGame((bool)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.GetComboKill, delegate(Component sender, object param)
		{
			this.OnGetComboKill((int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.TransportJetDone, delegate(Component sender, object param)
		{
			this.OnTransportJetDone();
		});
		EventDispatcher.Instance.RegisterListener(EventID.SelectBoosterDone, delegate(Component sender, object param)
		{
			this.OnSelectBoosterDone();
		});
		EventDispatcher.Instance.RegisterListener(EventID.GameStart, delegate(Component sender, object param)
		{
			this.StartGame();
		});
		EventDispatcher.Instance.RegisterListener(EventID.GameEnd, delegate(Component sender, object param)
		{
			this.EndGame((bool)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.FinishStage, delegate(Component sender, object param)
		{
			this.OnFinishStage();
		});
		EventDispatcher.Instance.RegisterListener(EventID.GetItemDrop, delegate(Component sender, object param)
		{
			this.OnGetItemDrop((ItemDropData)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.BonusCoinCollected, delegate(Component sender, object param)
		{
			this.OnBonusCoinCollected((float)param);
		});
		this.IsAllowSpawnSideEnemy = false;
		this.CreateMap();
		this.CreateEnemyInZone(1);
		this.PlayBackgroundMusic();
		Singleton<CameraFollow>.Instance.SetCameraSize(4f);
		Singleton<CameraFollow>.Instance.SetInitialPoint(this.Map.cameraInitialPoint);
	}

	public override void StartGame()
	{
		this.Player.enabled = true;
		base.StartCoroutine(this.CoroutineRoundTime());
		Singleton<UIController>.Instance.ShowMissionStart();
		if (!GameData.playerTutorials.IsCompletedStep(TutorialType.ActionInGame) && string.Compare(GameData.currentStage.id, "1.1") == 0)
		{
			Singleton<UIController>.Instance.tutorialGamePlay.ShowTutorialActionIngame();
		}
	}

	public override void PauseGame()
	{
		base.PauseGame();
	}

	public override void ResumeGame()
	{
		base.ResumeGame();
	}

	public override void ReplayGame()
	{
		throw new NotImplementedException();
	}

	public override void QuitGame()
	{
		throw new NotImplementedException();
	}

	public override void EndGame(bool isWin)
	{
		List<RewardData> list = new List<RewardData>();
		this.PlayMusicWinLose(isWin);
		if (isWin)
		{
			this.SetProgressDailyQuest();
			this.coinCollected += this.Map.CoinCompleteStage;
			this.coinTotal += this.coinCollected + this.coinBonusComboKill;
			if (!MapUtils.IsStagePassed(GameData.currentStage.id, GameData.currentStage.difficulty))
			{
				list = GameData.staticCampaignStageData.GetFirstTimeRewards(GameData.currentStage.id, GameData.currentStage.difficulty);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].type == RewardType.Coin)
					{
						list[i].value += this.coinTotal;
					}
				}
			}
			else
			{
				list.Add(new RewardData
				{
					type = RewardType.Coin,
					value = this.coinTotal
				});
			}
			Singleton<UIController>.Instance.hudWin.Open(list);
			MapUtils.UnlockCampaignProgress(GameData.currentStage);
			DailyQuestTracker.Instance.Save();
			AchievementTracker.Instance.Save();
			if (!ProfileManager.UserProfile.isNoLongerRate)
			{
				if (string.Compare(this.Map.stageNameId, "1.4") == 0)
				{
					if (!ProfileManager.UserProfile.isShowRateMap1)
					{
						ProfileManager.UserProfile.isShowRateMap1.Set(true);
						Singleton<Popup>.Instance.ShowRateUs();
					}
				}
				else if (string.Compare(this.Map.stageNameId, "2.4") == 0)
				{
					if (!ProfileManager.UserProfile.isShowRateMap2)
					{
						ProfileManager.UserProfile.isShowRateMap2.Set(true);
						Singleton<Popup>.Instance.ShowRateUs();
					}
				}
				else if (string.Compare(this.Map.stageNameId, "3.4") == 0 && !ProfileManager.UserProfile.isShowRateMap3)
				{
					ProfileManager.UserProfile.isShowRateMap3.Set(true);
					Singleton<Popup>.Instance.ShowRateUs();
				}
			}
			RewardUtils.Receive(list);
		}
		else
		{
			SoundManager.Instance.PlaySfx("sfx_voice_game_over", 0f);
			Singleton<UIController>.Instance.hudLose.Open();
		}
		if (!ProfileManager.UserProfile.isRemoveAds)
		{
			// AdMob Remove
			// Singleton<AdmobController>.Instance.ManualResetTryLoadCount();
			// Singleton<AdmobController>.Instance.ShowInterstitialAd(delegate(ShowResult result)
			// {
			// 	Time.timeScale = 1f;
			// 	if (result == ShowResult.Finished)
			// 	{
			// 		UnityEngine.Debug.Log("NIk Log is the Reward Complete");
			// 		// StartCoroutine(DelayReward());
			// 		Time.timeScale = 1f;
			// 		int num = ProfileManager.UserProfile.countRewardInterstitialAds;
			// 		if (num < 10)
			// 		{
			// 			GameData.playerResources.ReceiveGem(5);
			// 			Singleton<Popup>.Instance.Show(string.Format("Get <color=cyan>{0} gems</color> for the ads", 5), "rewards", PopupType.Ok, null, null);
			// 			SoundManager.Instance.PlaySfx("sfx_get_reward", 0f);
			// 			num++;
			// 			ProfileManager.UserProfile.countRewardInterstitialAds.Set(num);
			// 		}
			// 	}else
			// 	{
			// 		UnityEngine.Debug.Log("NIk Log is the Reward Fail");
			// 	}
			// });
		}
		EventLogger.LogEvent("N_GameResult", new object[]
		{
			(!isWin) ? "Lose" : "Win",
			GameData.currentStage.id,
			GameData.currentStage.difficulty
		});
		EventLogger.LogEvent("N_UsePrimaryGun", new object[]
		{
			GameData.staticGunData[ProfileManager.UserProfile.gunNormalId].gunName.Replace(" ", "_")
		});
		if (ProfileManager.UserProfile.gunSpecialId != -1)
		{
			EventLogger.LogEvent("N_UseSpecialGun", new object[]
			{
				GameData.staticGunData[ProfileManager.UserProfile.gunSpecialId].gunName.Replace(" ", "_")
			});
		}
	}

	public override void OnPlayerDie()
	{
		throw new NotImplementedException();
	}

	public override void OnPlayerRevive()
	{
		throw new NotImplementedException();
	}

	private void OnSelectBoosterDone()
	{
		Singleton<GameController>.Instance.IsPaused = false;
		ControllerType controllerType = this.Map.controllerType;
		if (controllerType != ControllerType.Rambo)
		{
			if (controllerType == ControllerType.Boat)
			{
				CustomMapRacingBoat component = this.Map.gameObject.GetComponent<CustomMapRacingBoat>();
				if (component != null)
				{
					component.Init(this.Map);
				}
				Singleton<GameController>.Instance.IsGameStarted = true;
				EventDispatcher.Instance.PostEvent(EventID.GameStart);
			}
		}
		else if (this.Map.isRamboStartOnJet)
		{
			this.CreateTransportJet();
		}
		else
		{
			Singleton<GameController>.Instance.CreatePlayer(this.Map.playerSpawnPoint.position, null);
			Singleton<GameController>.Instance.IsGameStarted = true;
			EventDispatcher.Instance.PostEvent(EventID.GameStart);
		}
	}

	private void OnTransportJetDone()
	{
		Rambo rambo = (Rambo)this.Player;
		rambo.transform.parent = null;
		BoneFollower component = rambo.GetComponent<BoneFollower>();
		if (component)
		{
			UnityEngine.Object.Destroy(component);
		}
		rambo.PlayAnimationParachute();
		rambo.transform.DOMove(this.Map.playerSpawnPoint.position, 1f, false).SetEase(Ease.InCubic).OnComplete(delegate
		{
			rambo.enabled = true;
			rambo.Rigid.simulated = true;
			rambo.PlayAnimationIdle();
			rambo.effectDustGround.Play();
			rambo.ActiveHealthBar(true);
			rambo.transform.rotation = Quaternion.identity;
			Singleton<GameController>.Instance.IsGameStarted = true;
			EventDispatcher.Instance.PostEvent(EventID.GameStart);
		});
	}

	private void OnFinishStage()
	{
		Singleton<GameController>.Instance.IsPaused = true;
		Singleton<GameController>.Instance.DeactiveEnemies();
		this.IsAllowSpawnSideEnemy = false;
	}

	private void SetProgressDailyQuest()
	{
		if (GameData.selectingBoosters.Count > 0)
		{
			EventDispatcher.Instance.PostEvent(EventID.UseBooster);
		}
		if (GameData.selectingBoosters.Contains(BoosterType.Damage))
		{
			EventDispatcher.Instance.PostEvent(EventID.UseBoosterDamage);
		}
		if (GameData.selectingBoosters.Contains(BoosterType.Critical))
		{
			EventDispatcher.Instance.PostEvent(EventID.UseBoosterCritical);
		}
		if (GameData.selectingBoosters.Contains(BoosterType.Speed))
		{
			EventDispatcher.Instance.PostEvent(EventID.UseBoosterSpeed);
		}
		if (GameData.selectingBoosters.Contains(BoosterType.CoinMagnet))
		{
			EventDispatcher.Instance.PostEvent(EventID.UseBoosterCoinMagnet);
		}
	}

	private void OnViewAdsx2CoinEndGame(bool isWin)
	{
	}

	private void OnGetComboKill(int count)
	{
		if (count == 10)
		{
			this.coinBonusComboKill += 5;
		}
		else if (count == 20)
		{
			this.coinBonusComboKill += 25;
		}
		else if (count == 25)
		{
			this.coinBonusComboKill += 35;
		}
		else if (count >= 26)
		{
			this.coinBonusComboKill++;
		}
		if (count > this.highestComboKill)
		{
			this.highestComboKill = count;
		}
	}

	private void OnGetItemDrop(ItemDropData data)
	{
		if (data.type == ItemDropType.Coin)
		{
			this.coinCollected += (int)data.value;
			Singleton<UIController>.Instance.UpdateCoinCollectedText(this.coinCollected);
		}
	}

	private void OnBonusExpWin(float percent)
	{
		this.expWin = Mathf.RoundToInt((float)this.expWin * (1f + percent / 100f));
	}

	private void OnBonusCoinCollected(float percent)
	{
		this.coinCollected = Mathf.RoundToInt((float)this.coinCollected * (1f + percent / 100f));
	}

	public void CreateEnemyInZone(int zoneId)
	{
		if (this.Map.mapData.enemyData == null)
		{
			return;
		}
		int i = 0;
		int count = this.Map.mapData.enemyData.Count;
		while (i < count)
		{
			EnemySpawnData enemySpawnData = this.Map.mapData.enemyData[i];
			if (enemySpawnData.packId == zoneId)
			{
				BaseUnit baseUnit = this.SpawnEnemy(enemySpawnData);
			}
			i++;
		}
	}

	public void AutoSpawnEnemy()
	{
		if (this.Map.isAutoSpawnEnemy && this.IsAllowSpawnSideEnemy && this.Map.enemyAutoSpawnPrefabs.Length > 0)
		{
			base.StartCoroutine(this.CoroutineAutoSpawnEnemy(this.Map.enemyPerSpawn));
		}
	}

	private IEnumerator CoroutineAutoSpawnEnemy(int numberEnemy)
	{
		CampaignModeController._CoroutineAutoSpawnEnemy_c__Iterator0 _CoroutineAutoSpawnEnemy_c__Iterator = new CampaignModeController._CoroutineAutoSpawnEnemy_c__Iterator0();
		_CoroutineAutoSpawnEnemy_c__Iterator.numberEnemy = numberEnemy;
		_CoroutineAutoSpawnEnemy_c__Iterator._this = this;
		return _CoroutineAutoSpawnEnemy_c__Iterator;
	}

	private void SpawnEnemyFromSide(Vector2 position)
	{
		if (GameData.mode == GameMode.Campaign)
		{
			int num = UnityEngine.Random.Range(0, this.Map.enemyAutoSpawnPrefabs.Length);
			int id = this.Map.enemyAutoSpawnPrefabs[num].id;
			int levelEnemy = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);
			int level = UnityEngine.Random.Range(1, levelEnemy + 1);
			BaseEnemy baseEnemy = this.SpawnEnemy(id, level, position);
			baseEnemy.zoneId = -1;
			baseEnemy.isRunPassArea = true;
			baseEnemy.canJump = true;
			baseEnemy.DelayTargetPlayer();
		}
	}

	public override BaseEnemy GetEnemyPrefab(int id)
	{
		for (int i = 0; i < this.Map.enemyPrefabs.Length; i++)
		{
			BaseEnemy baseEnemy = this.Map.enemyPrefabs[i];
			if (baseEnemy.id == id)
			{
				return baseEnemy;
			}
		}
		return null;
	}

	private BaseEnemy SpawnEnemy(int id, int level, Vector2 position)
	{
		BaseEnemy enemyPrefab = this.GetEnemyPrefab(id);
		BaseEnemy fromPool = enemyPrefab.GetFromPool();
		fromPool.Active(id, level, position);
		Singleton<GameController>.Instance.AddUnit(fromPool.gameObject, fromPool);
		return fromPool;
	}

	private BaseEnemy SpawnEnemy(EnemySpawnData spawnData)
	{
		BaseEnemy enemyPrefab = this.GetEnemyPrefab(spawnData.id);
		BaseEnemy fromPool = enemyPrefab.GetFromPool();
		fromPool.Active(spawnData);
		Singleton<GameController>.Instance.AddUnit(fromPool.gameObject, fromPool);
		return fromPool;
	}

	public void PlayMusicWinLose(bool isWin)
	{
		base.StartCoroutine(this.CoroutineFadeMusic(isWin));
	}

	private void CreateTransportJet()
	{
		this.jet = UnityEngine.Object.Instantiate<TransportJet>(this.transportJetPrefab, this.Map.jetStartPoint.position, this.Map.jetStartPoint.rotation);
		this.jet.Active(this.Map.jetDestination.position);
		Singleton<GameController>.Instance.CreatePlayer(this.jet.playerStandPoint.position, this.jet.transform);
		Rambo rambo = (Rambo)this.Player;
		BoneFollower boneFollower = rambo.gameObject.AddComponent<BoneFollower>();
		boneFollower.skeletonRenderer = this.jet.skeletonAnimation;
		boneFollower.boneName = this.jet.boneStand;
		rambo.ActiveHealthBar(false);
		rambo.PlayAnimationIdleInJet();
		rambo.Rigid.simulated = false;
		rambo.enabled = false;
	}

	private void CreateMap()
	{
		Map mapPrefab = MapUtils.GetMapPrefab(GameData.currentStage.id);
		this.Map = UnityEngine.Object.Instantiate<Map>(mapPrefab);
		this.Map.Init();
	}

	private void PlayBackgroundMusic()
	{
		int num = int.Parse(this.Map.stageNameId.Split(new char[]
		{
			'.'
		}).First<string>());
		string musicName = string.Format("music_map_{0}", num);
		SoundManager.Instance.PlayMusicFromBeginning(musicName, 0f);
	}

	private IEnumerator UpdateMapProgress()
	{
		CampaignModeController._UpdateMapProgress_c__Iterator1 _UpdateMapProgress_c__Iterator = new CampaignModeController._UpdateMapProgress_c__Iterator1();
		_UpdateMapProgress_c__Iterator._this = this;
		return _UpdateMapProgress_c__Iterator;
	}

	private IEnumerator CoroutineRoundTime()
	{
		return new CampaignModeController._CoroutineRoundTime_c__Iterator2();
	}

	private IEnumerator CoroutineFadeMusic(bool isWin)
	{
		CampaignModeController._CoroutineFadeMusic_c__Iterator3 _CoroutineFadeMusic_c__Iterator = new CampaignModeController._CoroutineFadeMusic_c__Iterator3();
		_CoroutineFadeMusic_c__Iterator.isWin = isWin;
		return _CoroutineFadeMusic_c__Iterator;
	}
}
