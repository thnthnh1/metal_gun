using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewbieController : MonoBehaviour
{
	enum RemoteConfigsStatus
	{
		ShouldFetch,
		Fetching,
		FetchedSuccess
	}

	const int DURATION_THROTTLED_WAIT = 30 * 60;
	const string KEY_REMOTE_CONFIGS_EXISTED = "REMOTE_CONFIGS_EXISTED";

	const string KEY_NEWBIE_CODE = "Newbie";
	[SerializeField] GameObject _popupView;
	[SerializeField] TMP_InputField _inputCode;
	[SerializeField] Button _btnClaim;

	public GameObject View => _popupView;
	
	string _targetCode = KEY_NEWBIE_CODE;
	int _idGun = 4;
	int _coinReward = 40000;
	bool _isRemoteConfigsExisted = false;
	bool _shouldApplyFetchedResult;
	int _durationCacheExpired;
	long _shouldWaitForNextFetchTime;
	int _durationRetryNextFetch;
	bool _isInitFirebase = false;

	RemoteConfigsStatus _remoteConfigsStatus = RemoteConfigsStatus.ShouldFetch;

	bool IsRemoteConfigsExisted
	{
		get { return _isRemoteConfigsExisted; }
		set
		{
			if (value != _isRemoteConfigsExisted)
			{
				_isRemoteConfigsExisted = value;
				PlayerPrefs.SetInt(KEY_REMOTE_CONFIGS_EXISTED, value ? 1 : 0);
				PlayerPrefs.Save();
			}
		}
	}

	void Awake()
	{
		_durationCacheExpired = 3600;
		_isRemoteConfigsExisted = PlayerPrefs.GetInt(KEY_REMOTE_CONFIGS_EXISTED, 0) > 0 ? true : false;

		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
		{
			var dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available)
			{
				Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
				Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Warning;
				Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

				_isInitFirebase = true;
			}
			else
			{
				UnityEngine.Debug.LogError(System.String.Format(
				  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
				// Firebase Unity SDK is not safe to use here.
			}
		});
		_btnClaim.interactable = false;
	}

	IEnumerator Start()
	{
		while (!_isInitFirebase) yield return null;

		Dictionary<string, object> defaults = new Dictionary<string, object>();
		defaults.Add(KEY_NEWBIE_CODE, "metalgun4");
		Initialize(defaults);
	}

	public void Initialize(Dictionary<string, object> defaultConfigs)
	{
		if (_isRemoteConfigsExisted)
		{
			Debug.LogWarning("[RemoteConfigs] Activate last fetched configs!");
			_shouldApplyFetchedResult = true;
			CheckApplyLastFetchedConfigs();
		}
		else
		{
			Debug.LogWarning("[RemoteConfigs] No remote configs existed - Let apply default configs!");
			FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaultConfigs).ContinueWithOnMainThread(setupTask =>
			{
				ShouldApplyFetchedConfigs();
				CheckApplyLastFetchedConfigs();
			});
		}

		Run();
	}

	public void CheckApplyLastFetchedConfigs()
	{
		if (_isRemoteConfigsExisted && _shouldApplyFetchedResult)
		{
			Debug.Log("[RemoteConfigs] Apply last fetched remote configs!");
			ShouldApplyFetchedConfigs();
			_shouldApplyFetchedResult = false;
		}
	}

	void ShouldApplyFetchedConfigs()
	{
		_targetCode = FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_NEWBIE_CODE).StringValue;
	}

	public void Run()
	{
		StartCoroutine(RemoteConfigsTask());
	}

	IEnumerator RemoteConfigsTask()
	{
		while (true)
		{
			switch (_remoteConfigsStatus)
			{
				case RemoteConfigsStatus.ShouldFetch:
					{
						var now = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds();
						var dt = _shouldWaitForNextFetchTime - now;
						if (dt > 0)
						{
							Debug.LogFormat("[RemoteConfigs] Waiting for next fetch time: {0}s", dt);
							yield return new WaitForSeconds(dt);
						}

						_remoteConfigsStatus = RemoteConfigsStatus.Fetching;
						FetchRemoteConfigsAsync();
					}

					break;

				case RemoteConfigsStatus.FetchedSuccess:
					{
						Debug.LogFormat("[RemoteConfigs] Waiting for cached expired: {0}s", _durationCacheExpired);
						yield return new WaitForSeconds(_durationCacheExpired);

						// Ready for next fetch
						_remoteConfigsStatus = RemoteConfigsStatus.ShouldFetch;
					}
					break;
			}

			yield return null;
		}
	}

	Task FetchRemoteConfigsAsync()
	{
		Debug.Log("[RemoteConfigs] FetchRemotConfigsAsync");

		// FetchAsync only fetches new data if the current data is older than the provided
		// timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
		// By default the timespan is 12 hours, and for production apps, this is a good
		// number.  For this example though, it's set to a timespan of zero, so that
		// changes in the console will always show up immediately.
		var cacheExpiration = new System.TimeSpan(0, 0, _durationCacheExpired);
		System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(cacheExpiration);
		return fetchTask.ContinueWithOnMainThread(FetchRemoteConfigsComplete);
	}

	void FetchRemoteConfigsComplete(Task fetchTask)
	{
		if (fetchTask.IsCompleted)
		{
			var info = FirebaseRemoteConfig.DefaultInstance.Info;
			if (info.LastFetchStatus == LastFetchStatus.Success)
			{
				Debug.Log("[RemoteConfigs] FetchRemoteConfigsComplete => Activating...!");

				FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task =>
				{
					Debug.Log(string.Format("[RemoteConfigs] Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
					IsRemoteConfigsExisted = true;

					_remoteConfigsStatus = RemoteConfigsStatus.FetchedSuccess;
					_shouldApplyFetchedResult = true;

					CheckApplyLastFetchedConfigs();
				});
			}
			else if (info.LastFetchStatus == LastFetchStatus.Failure)
			{
				_remoteConfigsStatus = RemoteConfigsStatus.ShouldFetch;
				var waitForNextFetchDuration = _durationRetryNextFetch;
				if (info.LastFetchFailureReason == FetchFailureReason.Throttled)
				{
					waitForNextFetchDuration = DURATION_THROTTLED_WAIT;
				}
				_shouldWaitForNextFetchTime = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds() + waitForNextFetchDuration;

				Debug.LogFormat("[RemoteConfigs] FetchRemoteConfigs failed with reason: {0}", info.LastFetchFailureReason);
			}
			else
			{
				Debug.LogFormat("[RemoteConfigs] Fetch status: {0} ({1})", info.LastFetchStatus, info.LastFetchFailureReason);
			}
		}
		else
		{
			_remoteConfigsStatus = RemoteConfigsStatus.ShouldFetch;
			_shouldWaitForNextFetchTime = new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeSeconds() + _durationRetryNextFetch;
		}
	}

	public void OnCodeChanged()
	{
		_btnClaim.interactable = _inputCode.text == _targetCode;
	}

	public void Open()
	{
		_popupView.SetActive(true);
		SoundManager.Instance.PlaySfxClick();
	}

	public void ClaimReward()
	{
		Close();
		SoundManager.Instance.PlaySfxClick();
		ProfileManager.UserProfile.gunNormalId.Set(_idGun);
		GameData.playerResources.ReceiveCoin(_coinReward);
		ProfileManager.UserProfile.isClaimNewbiePack.Set(true);
		GameData.playerGuns.ReceiveNewGun(_idGun);
		EventDispatcher.Instance.PostEvent(EventID.ClaimNewbiePackage);
	}

	public void Close()
	{
		_popupView.SetActive(false);
		SoundManager.Instance.PlaySfxClick();
	}
}
