using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
	public static string nextScene = "Login";

	public Text textTip;

	public Text loadingPercent;

	public Image imgGuide;

	public Sprite[] sprGuides;

	private AsyncOperation async;

	private float timerLoading;

	private string formatPercent = "{0}%";

	private List<string> tips;

	private void Start()
	{
		SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (this.async == null)
		{
			return;
		}
		float num = Mathf.Clamp01(this.async.progress / 0.9f);
		if (num >= 0.99f && Time.timeSinceLevelLoad - this.timerLoading >= 3f && !this.async.allowSceneActivation)
		{
			this.async.allowSceneActivation = true;
			this.async = null;
		}
	}

	private void OnEnable()
	{
		this.RandomGuide();
		this.timerLoading = Time.timeSinceLevelLoad;
		this.async = SceneManager.LoadSceneAsync(Loading.nextScene);
		this.async.allowSceneActivation = false;
	}

	public void Show()
	{
		SoundManager.Instance.StopMusic();
		base.gameObject.SetActive(true);
	}

	private void RandomTips()
	{
		if (this.tips == null)
		{
			TextAsset textAsset = Resources.Load<TextAsset>("JSON/Mix/tips");
			this.tips = JsonConvert.DeserializeObject<List<string>>(textAsset.text);
		}
		if (this.tips.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, this.tips.Count);
			this.textTip.text = string.Format("TIPS: {0}", this.tips[index].ToUpper());
		}
		else
		{
			this.textTip.text = string.Empty;
		}
	}

	private void RandomGuide()
	{
		int num = UnityEngine.Random.Range(0, this.sprGuides.Length);
		this.imgGuide.sprite = this.sprGuides[num];
		this.imgGuide.SetNativeSize();
	}
}
