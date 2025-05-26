using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameController : Singleton<GameController>
{
	public BaseModeController modeController;

	public ItemDropController itemDropController;

	public Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();

	private BaseUnit _Player_k__BackingField;

	private int _PlayTime_k__BackingField;

	private bool _IsGameStarted_k__BackingField;

	private bool _IsPaused_k__BackingField;

	public ModeControllerWrapper modeControllerWrapper;

	public bool photonNetworkScene = false;

	public Map CampaignMap
	{
		get
		{
			return ((CampaignModeController)this.modeController).Map;
		}
	}

	public MapSurvival SurvivalMap
	{
		get
		{
			return ((SurvivalModeController)this.modeController).Map;
		}
	}

	public BaseUnit Player
	{
		get;
		set;
	}

	public int PlayTime
	{
		get;
		set;
	}

	public bool IsGameStarted
	{
		get;
		set;
	}

	public bool IsPaused
	{
		get;
		set;
	}

	public CampaignModeController campaignModeController
	{
		get
		{
			return this.modeControllerWrapper.campaignModeController;
		}
	}

	public SurvivalModeController survivalModeController
	{
		get
		{
			return this.modeControllerWrapper.survivalModeController;
		}
	}

	public CnControls.SimpleJoystick joystick;
	public CnControls.NinputJoystick aimJoystick;

	private void Awake()
	{
		Singleton<PoolingController>.Instance.InitPool();
		GameMode mode = GameData.mode;

		if (photonNetworkScene)
		{
			return;
		}
		if (mode != GameMode.Campaign)
		{
			if (mode == GameMode.Survival)
			{
				this.modeController = this.survivalModeController;
				UnityEngine.Object.Destroy(this.campaignModeController.gameObject);
			}
		}
		else
		{
			this.modeController = this.campaignModeController;
			UnityEngine.Object.Destroy(this.survivalModeController.gameObject);
		}
		this.modeController.enabled = true;
		this.modeController.InitMode();
        Application.targetFrameRate = 60;
	}

	private void Start()
	{
		this.IsPaused = true;
		this.IsGameStarted = false;
		GameData.isUseRevive = false;
	}

	private void OnApplicationPause(bool pause)
	{
		if (!this.IsPaused)
		{
			if (this.IsGameStarted)
			{
				Singleton<UIController>.Instance.hudPause.Open();
			}
			if (Singleton<CameraFollow>.Instance.slowMotion.IsShowing)
			{
				Time.timeScale = 0.2f;
			}
		}
	}

	public void CreatePlayer(Vector2 position, Transform parent = null)
	{
		Rambo ramboPrefab = GameResourcesUtils.GetRamboPrefab(ProfileManager.UserProfile.ramboId);
		Rambo rambo = UnityEngine.Object.Instantiate<Rambo>(ramboPrefab);
		int id = ProfileManager.UserProfile.ramboId;
		int ramboLevel = GameData.playerRambos.GetRamboLevel(id);
		rambo.Active(id, ramboLevel, position);
		rambo.Rigid.simulated = true;
		Singleton<GameController>.Instance.Player = rambo;
		Singleton<CameraFollow>.Instance.SetTarget(this.Player.transform);
		Singleton<GameController>.Instance.AddUnit(this.Player.gameObject, this.Player);
		rambo.transform.parent = parent;
	}

	public void AddUnit(GameObject obj, BaseUnit unit)
	{
		if (this.activeUnits.ContainsKey(obj))
		{
		}
		this.activeUnits[obj] = unit;
	}

	public void RemoveUnit(GameObject obj)
	{
		if (this.activeUnits.ContainsKey(obj))
		{
			this.activeUnits.Remove(obj);
		}
	}

	public void SetActiveAllUnits(bool isActive)
	{
		List<BaseUnit> list = new List<BaseUnit>(this.activeUnits.Values);
		foreach (BaseUnit current in list)
		{
			if (!(current is RubberBoat))
			{
				current.enabled = isActive;
			}
		}
	}

	public void DeactiveEnemies()
	{
		List<BaseUnit> list = new List<BaseUnit>(this.activeUnits.Values);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].transform.root.CompareTag("Enemy"))
			{
				list[i].Deactive();
			}
		}
	}

	public BaseUnit GetUnit(GameObject obj)
	{
		if (this.activeUnits.ContainsKey(obj))
		{
			return this.activeUnits[obj];
		}
		return null;
	}

	public BaseEnemy GetEnemyPrefab(int id)
	{
		return this.modeController.GetEnemyPrefab(id);
	}
}
