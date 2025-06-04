using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SurvivalModeController : BaseModeController
{
	private sealed class _CoroutineBomb_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal Vector2 _startPosition___0;

		internal Vector2 _endPosition___0;

		internal Vector2 _v___1;

		internal SurvivalModeController _this;

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

		public _CoroutineBomb_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._startPosition___0 = Singleton<CameraFollow>.Instance.top.position;
				this._startPosition___0.y = this._startPosition___0.y + 1.5f;
				this._startPosition___0.x = Singleton<CameraFollow>.Instance.left.position.x;
				this._endPosition___0 = this._startPosition___0;
				this._endPosition___0.x = Singleton<CameraFollow>.Instance.right.position.x;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._startPosition___0.x < this._endPosition___0.x)
			{
				this._this.ReleaseBomb(this._startPosition___0);
				this._v___1 = this._startPosition___0;
				this._v___1.x = this._v___1.x + 1f;
				this._startPosition___0 = this._v___1;
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

	private sealed class _CoroutineStartWave_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _totalWaves___0;

		internal int _curTimeIndex___0;

		internal SurvivalWaveData waveData;

		internal int _totalUnits___0;

		internal int _quantityWave_1___0;

		internal int _quantityWave_2___0;

		internal int _quantityWave_3___0;

		internal SurvivalModeController _this;

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

		public _CoroutineStartWave_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._totalWaves___0 = 3;
				this._curTimeIndex___0 = 0;
				this._totalUnits___0 = this.waveData.numberUnits;
				this._quantityWave_1___0 = Mathf.RoundToInt((float)this._totalUnits___0 / 3f);
				this._quantityWave_2___0 = Mathf.RoundToInt((float)this._totalUnits___0 / 3f);
				this._quantityWave_3___0 = this._totalUnits___0 - (this._quantityWave_1___0 + this._quantityWave_2___0);
				break;
			case 1u:
				this._this.timerPerWave++;
				this._this.UpdateWaveTime();
				break;
			default:
				return false;
			}
			if (!Singleton<GameController>.Instance.IsPaused && !this._this.isPreparingNextWave)
			{
				if (this._curTimeIndex___0 < this._totalWaves___0 && (this._this.timerPerWave == 1 || this._this.timerPerWave == 10 || this._this.timerPerWave == 20))
				{
					int num2 = 0;
					int num3 = 0;
					if (this._this.timerPerWave == 1)
					{
						num2 = 0;
						num3 = this._quantityWave_1___0;
					}
					else if (this._this.timerPerWave == 10)
					{
						num2 = this._quantityWave_1___0;
						num3 = this._quantityWave_1___0 + this._quantityWave_2___0;
					}
					else if (this._this.timerPerWave == 20)
					{
						num2 = this._quantityWave_1___0 + this._quantityWave_2___0;
						num3 = this._totalUnits___0;
					}
					for (int i = num2; i < num3; i++)
					{
						SurvivalEnemy survivalEnemy = this._this.currentWaveData.units[i];
						List<int> locationCanSpawnUnit = this._this.Map.GetLocationCanSpawnUnit(survivalEnemy);
						if (locationCanSpawnUnit.Count > 0)
						{
							int index = UnityEngine.Random.Range(0, locationCanSpawnUnit.Count);
							this._this.Map.AddUnitToSpawnLocation(survivalEnemy, locationCanSpawnUnit[index], this._this.currentWaveData.minLevelUnit, this._this.currentWaveData.maxLevelUnit);
						}
					}
					this._this.Map.Spawn();
					this._curTimeIndex___0++;
				}
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

	private sealed class _CoroutineCountDownNextWave_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _remainingTime___0;

		internal string _msg___1;

		internal SurvivalModeController _this;

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

		public _CoroutineCountDownNextWave_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._remainingTime___0 = 3;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._remainingTime___0 > 0)
			{
				this._msg___1 = string.Format("NEXT WAVE IN: {0}", this._remainingTime___0);
				Singleton<UIController>.Instance.modeSurvivalUI.ShowNotice(this._msg___1);
				this._remainingTime___0--;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.StartNextWave();
			Singleton<UIController>.Instance.modeSurvivalUI.SetTextWave(this._this.numberWave);
			Singleton<UIController>.Instance.modeSurvivalUI.HideNotice();
			this._this.coroutineCountDownNextWave = null;
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

	public MapSurvival[] mapPrefabs;

	public BaseEnemy[] enemyType_1;

	public BaseEnemy[] enemyType_2;

	public BaseEnemy[] enemyType_3;

	public BaseEnemy[] enemyType_4;

	public BaseEnemy[] enemyType_5;

	[Header("SKILL")]
	public BombSupportSurvival bombPrefab;

	private WaitForSeconds bombInterval = new WaitForSeconds(0.1f);

	private int soldierKill;

	private int vehicleKill;

	private int bossKill;

	private int highestComboKill;

	private int soldierScore;

	private int vehicleScore;

	private int bossScore;

	private int timeScore;

	private int totalScore;

	private int curWaveIndex = -1;

	private int numberWave;

	private int timerPerWave;

	private int enemyKilledInWave;

	private int totalEnemiesInWave;

	private bool isPreparingNextWave;

	private IEnumerator coroutineStartWave;

	private IEnumerator coroutineCountDownNextWave;

	private SO_SurvivalWave curWaveData;

	private SurvivalWaveData currentWaveData;

	private Dictionary<int, int> unitScoreData;

	private Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();

	private MapSurvival _Map_k__BackingField;

	private static Action<ShowResult> __f__am_cache0;

	public MapSurvival Map
	{
		get;
		private set;
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
		EventDispatcher.Instance.RegisterListener(EventID.StartFirstWave, delegate(Component sender, object param)
		{
			this.OnStartPlay();
		});
		EventDispatcher.Instance.RegisterListener(EventID.GameStart, delegate(Component sender, object param)
		{
			this.StartGame();
		});
		EventDispatcher.Instance.RegisterListener(EventID.GameEnd, delegate(Component sender, object param)
		{
			this.EndGame((bool)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.LabelWaveAnimateComplete, delegate(Component sender, object param)
		{
			this.CountDownNextWave();
		});
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
		EventDispatcher.Instance.RegisterListener(EventID.GetComboKill, delegate(Component sender, object param)
		{
			this.OnGetComboKill((int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemBomb, delegate(Component sender, object param)
		{
			this.OnUseSupportItemBomb();
		});
		EventDispatcher.Instance.RegisterListener(EventID.QuitSurvivalSession, delegate(Component sender, object param)
		{
			this.OnQuitSurvivalSession();
		});
		this.LoadScoreData();
		this.CreateMap();
		this.PlayBackgroundMusic();
	}

	public override void StartGame()
	{
		this.Player.enabled = true;
		this.CountDownNextWave();
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
		Singleton<GameController>.Instance.IsPaused = true;
		Singleton<GameController>.Instance.DeactiveEnemies();
		Singleton<UIController>.Instance.ActiveIngameUI(false);
		Singleton<UIController>.Instance.alarmRedScreen.SetActive(false);
		DailyQuestTracker.Instance.Save();
		AchievementTracker.Instance.Save();
		SurvivalResultData data = new SurvivalResultData(this.numberWave, this.soldierKill, this.soldierScore, this.vehicleKill, this.vehicleScore, this.bossKill, this.bossScore, this.timeScore, this.highestComboKill, this.totalScore);
		if (this.totalScore > 0)
		{
			GameData.playerTournamentData.score += this.totalScore;
			//TournamentData data2 = new TournamentData(AccessToken.CurrentAccessToken.UserId, GameData.playerTournamentData.score, this.GetMostUsedGunId(), false);
			//Singleton<FireBaseDatabase>.Instance.SaveTournamentData(data2, null);
		}
		Singleton<UIController>.Instance.hudSurvivalResult.Open(data);
		/*if (!ProfileManager.UserProfile.isRemoveAds)
		{
			// AdMob Remove
			Singleton<AdmobController>.Instance.ShowRewardedVideoAd(null);
		}*/
		EventDispatcher.Instance.PostEvent(EventID.CompleteSurvivalSession);
		EventLogger.LogEvent("N_CompleteSurvival", new object[]
		{
			"Wave=" + this.numberWave,
			"Score=" + this.totalScore
		});
	}

	public override void OnPlayerDie()
	{
		throw new NotImplementedException();
	}

	public override BaseEnemy GetEnemyPrefab(int id)
	{
		for (int i = 0; i < this.enemyType_1.Length; i++)
		{
			BaseEnemy baseEnemy = this.enemyType_1[i];
			if (baseEnemy.id == id)
			{
				return baseEnemy;
			}
		}
		for (int j = 0; j < this.enemyType_2.Length; j++)
		{
			BaseEnemy baseEnemy2 = this.enemyType_2[j];
			if (baseEnemy2.id == id)
			{
				return baseEnemy2;
			}
		}
		for (int k = 0; k < this.enemyType_3.Length; k++)
		{
			BaseEnemy baseEnemy3 = this.enemyType_3[k];
			if (baseEnemy3.id == id)
			{
				return baseEnemy3;
			}
		}
		for (int l = 0; l < this.enemyType_4.Length; l++)
		{
			BaseEnemy baseEnemy4 = this.enemyType_4[l];
			if (baseEnemy4.id == id)
			{
				return baseEnemy4;
			}
		}
		for (int m = 0; m < this.enemyType_5.Length; m++)
		{
			BaseEnemy baseEnemy5 = this.enemyType_5[m];
			if (baseEnemy5.id == id)
			{
				return baseEnemy5;
			}
		}
		return null;
	}

	public override void OnPlayerRevive()
	{
		throw new NotImplementedException();
	}

	private void OnStartPlay()
	{
		Singleton<GameController>.Instance.IsPaused = false;
		Singleton<GameController>.Instance.CreatePlayer(this.Map.playerSpawnPoint.position, null);
		Singleton<GameController>.Instance.IsGameStarted = true;
		EventDispatcher.Instance.PostEvent(EventID.GameStart);
	}

	private void OnQuitSurvivalSession()
	{
		if (this.totalScore > 0)
		{
			GameData.playerTournamentData.score += this.totalScore;
			//TournamentData data = new TournamentData(AccessToken.CurrentAccessToken.UserId, GameData.playerTournamentData.score, ProfileManager.UserProfile.gunNormalId, false);
			//Singleton<FireBaseDatabase>.Instance.SaveTournamentData(data, null);
		}
		// if (!ProfileManager.UserProfile.isRemoveAds)
		// {
		// 	Time.timeScale = 1f;
		// 	// AdMob Remove
		// 	Singleton<AdmobController>.Instance.ManualResetTryLoadCount();
		// 	Singleton<AdmobController>.Instance.ShowInterstitialAd(delegate(ShowResult result)
		// 	{
		// 		Time.timeScale = 1f;
		// 		Singleton<UIController>.Instance.BackToMainMenu();
		// 	});
		// }
		// else
		{
			Singleton<UIController>.Instance.BackToMainMenu();
		}
	}

	private void OnUnitDie(Component senser, object param)
	{
		UnitDieData unitDieData = (UnitDieData)param;
		BaseEnemy component = unitDieData.unit.GetComponent<BaseEnemy>();
		if (component != null)
		{
			int unitScore = this.GetUnitScore(component.id, component.level);
			this.totalScore += unitScore;
			if (component.id < 100)
			{
				this.soldierKill++;
				this.soldierScore += unitScore;
			}
			else if (component.id < 1000)
			{
				this.vehicleKill++;
				this.vehicleScore += unitScore;
			}
			Singleton<UIController>.Instance.modeSurvivalUI.SetScoreText(this.totalScore);
			this.RemoveUnit(component);
			if (this.activeUnits.Count <= 0 && this.enemyKilledInWave >= this.totalEnemiesInWave)
			{
				this.CompleteWave();
			}
		}
	}

	private void OnGetComboKill(int count)
	{
		if (count > this.highestComboKill)
		{
			this.highestComboKill = count;
		}
	}

	private void OnUseSupportItemBomb()
	{
		base.StartCoroutine(this.CoroutineBomb());
	}

	private void ReleaseBomb(Vector2 pos)
	{
		BombSupportSurvival bombSupportSurvival = Singleton<PoolingController>.Instance.poolBombSupportSurvival.New();
		if (bombSupportSurvival == null)
		{
			bombSupportSurvival = UnityEngine.Object.Instantiate<BombSupportSurvival>(this.bombPrefab);
		}
		bombSupportSurvival.Active(pos, UnityEngine.Random.Range(50f, 80f), null);
	}

	private IEnumerator CoroutineBomb()
	{
		SurvivalModeController._CoroutineBomb_c__Iterator0 _CoroutineBomb_c__Iterator = new SurvivalModeController._CoroutineBomb_c__Iterator0();
		_CoroutineBomb_c__Iterator._this = this;
		return _CoroutineBomb_c__Iterator;
	}

	public void AddUnit(BaseUnit unit)
	{
		if (!this.activeUnits.ContainsKey(unit.gameObject))
		{
			this.activeUnits.Add(unit.gameObject, unit);
		}
	}

	public void RemoveUnit(BaseUnit unit)
	{
		if (this.activeUnits.ContainsKey(unit.gameObject))
		{
			this.activeUnits.Remove(unit.gameObject);
			this.enemyKilledInWave++;
		}
	}

	private void CreateMap()
	{
		if (this.mapPrefabs.Length > 0)
		{
			MapSurvival original = this.mapPrefabs[UnityEngine.Random.Range(0, this.mapPrefabs.Length)];
			this.Map = UnityEngine.Object.Instantiate<MapSurvival>(original);
			this.Map.Init();
			// Singleton<CameraFollow>.Instance.SetCameraSize(7.5f); // original
			Singleton<CameraFollow>.Instance.SetCameraSize(6f); // change by hardik
			Singleton<CameraFollow>.Instance.SetInitialPoint(this.Map.cameraInitialPoint);
		}
	}

	private void PlayBackgroundMusic()
	{
		SoundManager.Instance.PlayMusicFromBeginning("music_survival", 0f);
	}

	private void CountDownNextWave()
	{
		if (this.coroutineCountDownNextWave != null)
		{
			base.StopCoroutine(this.coroutineCountDownNextWave);
		}
		this.coroutineCountDownNextWave = this.CoroutineCountDownNextWave();
		base.StartCoroutine(this.coroutineCountDownNextWave);
	}

	private void StartNextWave()
	{
		this.numberWave++;
		this.timerPerWave = 0;
		this.highestComboKill = 0;
		this.isPreparingNextWave = false;
		this.currentWaveData = new SurvivalWaveData();
		this.currentWaveData.waveId = this.numberWave;
		int num = this.numberWave + 9;
		int num2 = Mathf.RoundToInt((float)num * 1.2f);
		this.currentWaveData.numberUnits = UnityEngine.Random.Range(num, num2 + 1);
		int num3 = Mathf.Clamp(this.numberWave / 2, 1, 20);
		this.currentWaveData.minLevelUnit = num3;
		this.currentWaveData.maxLevelUnit = Mathf.Clamp(num3 + 1, 1, 20);
		this.currentWaveData.units = new List<SurvivalEnemy>();
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		for (int i = 0; i < this.currentWaveData.numberUnits; i++)
		{
			int num9 = UnityEngine.Random.Range(1, 1001);
			SurvivalEnemy id;
			if (num9 <= 500)
			{
				int num10 = UnityEngine.Random.Range(0, this.enemyType_1.Length);
				id = (SurvivalEnemy)this.enemyType_1[num10].id;
				num4++;
			}
			else if (num9 <= 750)
			{
				int num11 = UnityEngine.Random.Range(0, this.enemyType_2.Length);
				id = (SurvivalEnemy)this.enemyType_2[num11].id;
				num5++;
			}
			else if (num9 <= 850)
			{
				int num12 = UnityEngine.Random.Range(0, this.enemyType_3.Length);
				id = (SurvivalEnemy)this.enemyType_3[num12].id;
				num6++;
			}
			else if (num9 <= 950)
			{
				int num13 = UnityEngine.Random.Range(0, this.enemyType_4.Length);
				id = (SurvivalEnemy)this.enemyType_4[num13].id;
				num7++;
			}
			else
			{
				int num14 = UnityEngine.Random.Range(0, this.enemyType_5.Length);
				id = (SurvivalEnemy)this.enemyType_5[num14].id;
				num8++;
			}
			this.currentWaveData.units.Add(id);
		}
		this.CalculateWaveUnits(this.currentWaveData);
		if (this.coroutineStartWave != null)
		{
			base.StopCoroutine(this.coroutineStartWave);
		}
		this.coroutineStartWave = this.CoroutineStartWave(this.currentWaveData);
		base.StartCoroutine(this.coroutineStartWave);
	}

	private void CompleteWave()
	{
		int num = this.highestComboKill;
		int num2 = Mathf.Clamp(60 - this.timerPerWave, 0, 60) * 3;
		this.totalScore += num + num2;
		this.timeScore += num2;
		Singleton<UIController>.Instance.modeSurvivalUI.SetScoreText(this.totalScore);
		Singleton<UIController>.Instance.modeSurvivalUI.ShowComplete();
		this.isPreparingNextWave = true;
		EventDispatcher.Instance.PostEvent(EventID.CompleteWave, this.curWaveData);
		if (this.numberWave == 5)
		{
			EventDispatcher.Instance.PostEvent(EventID.CompleteSurvivalWave5);
		}
	}

	private void UpdateWaveTime()
	{
		int min = this.timerPerWave / 60;
		int second = this.timerPerWave % 60;
		Singleton<UIController>.Instance.UpdateGameTime(min, second);
	}

	private void CalculateWaveUnits(SurvivalWaveData waveData)
	{
		this.activeUnits.Clear();
		this.enemyKilledInWave = 0;
		this.totalEnemiesInWave = 0;
		for (int i = 0; i < waveData.units.Count; i++)
		{
			SurvivalEnemy survivalEnemy = waveData.units[i];
			if (survivalEnemy != SurvivalEnemy.Bomber)
			{
				this.totalEnemiesInWave++;
			}
		}
	}

	private IEnumerator CoroutineStartWave(SurvivalWaveData waveData)
	{
		SurvivalModeController._CoroutineStartWave_c__Iterator1 _CoroutineStartWave_c__Iterator = new SurvivalModeController._CoroutineStartWave_c__Iterator1();
		_CoroutineStartWave_c__Iterator.waveData = waveData;
		_CoroutineStartWave_c__Iterator._this = this;
		return _CoroutineStartWave_c__Iterator;
	}

	private IEnumerator CoroutineCountDownNextWave()
	{
		SurvivalModeController._CoroutineCountDownNextWave_c__Iterator2 _CoroutineCountDownNextWave_c__Iterator = new SurvivalModeController._CoroutineCountDownNextWave_c__Iterator2();
		_CoroutineCountDownNextWave_c__Iterator._this = this;
		return _CoroutineCountDownNextWave_c__Iterator;
	}

	private int GetMostUsedGunId()
	{
		int key = ProfileManager.UserProfile.gunNormalId;
		Dictionary<int, int> dictionary;
		if (string.IsNullOrEmpty(ProfileManager.UserProfile.tournamentGunProfile))
		{
			dictionary = new Dictionary<int, int>();
			dictionary.Add(key, 1);
		}
		else
		{
			dictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(ProfileManager.UserProfile.tournamentGunProfile);
			if (dictionary.ContainsKey(key))
			{
				int num = dictionary[key];
				num++;
				dictionary[key] = num;
			}
			else
			{
				dictionary.Add(key, 1);
			}
		}
		string value = JsonConvert.SerializeObject(dictionary);
		ProfileManager.UserProfile.tournamentGunProfile.Set(value);
		ProfileManager.SaveAll();
		int num2 = 0;
		int result = ProfileManager.UserProfile.gunNormalId;
		foreach (KeyValuePair<int, int> current in dictionary)
		{
			if (current.Value > num2)
			{
				num2 = current.Value;
				result = current.Key;
			}
		}
		return result;
	}

	private int GetUnitScore(int id, int level)
	{
		if (this.unitScoreData.ContainsKey(id))
		{
			int num = this.unitScoreData[id];
			return Mathf.RoundToInt((float)num * ((float)level / 2f));
		}
		return 0;
	}

	private void LoadScoreData()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("JSON/Tournament Data/tournament_survival_unit_score_data");
		this.unitScoreData = JsonConvert.DeserializeObject<Dictionary<int, int>>(textAsset.text);
	}

	public void NextWave()
	{
		this.totalEnemiesInWave = 0;
		foreach (BaseUnit current in Singleton<GameController>.Instance.activeUnits.Values)
		{
			if (current.CompareTag("Enemy"))
			{
				current.TakeDamage(1000000f);
			}
		}
	}
}
