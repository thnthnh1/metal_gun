using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestReporter : MonoBehaviour
{
	public int logTestCount = 100;

	public int threadLogTestCount = 100;

	public bool logEverySecond = true;

	private int currentLogTestCount;

	private Reporter reporter;

	private GUIStyle style;

	private Rect rect1;

	private Rect rect2;

	private Rect rect3;

	private Rect rect4;

	private Rect rect5;

	private Rect rect6;

	private Thread thread;

	private float elapsed;

	private void Start()
	{
		Application.runInBackground = true;
		this.reporter = (UnityEngine.Object.FindObjectOfType(typeof(Reporter)) as Reporter);
		UnityEngine.Debug.Log("test long text sdf asdfg asdfg sdfgsdfg sdfg sfgsdfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfg sdfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfg sdfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfg sdfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfg sdfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfg ssssssssssssssssssssssasdf asdf asdf asdf adsf \n dfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfasdf asdf asdf asdf adsf \n dfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfasdf asdf asdf asdf adsf \n dfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfasdf asdf asdf asdf adsf \n dfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfasdf asdf asdf asdf adsf \n dfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdf");
		this.style = new GUIStyle();
		this.style.alignment = TextAnchor.MiddleCenter;
		this.style.normal.textColor = Color.white;
		this.style.wordWrap = true;
		for (int i = 0; i < 10; i++)
		{
			UnityEngine.Debug.Log("Test Collapsed log");
			UnityEngine.Debug.LogWarning("Test Collapsed Warning");
			UnityEngine.Debug.LogError("Test Collapsed Error");
		}
		for (int j = 0; j < 10; j++)
		{
			UnityEngine.Debug.Log("Test Collapsed log");
			UnityEngine.Debug.LogWarning("Test Collapsed Warning");
			UnityEngine.Debug.LogError("Test Collapsed Error");
		}
		this.rect1 = new Rect((float)(Screen.width / 2 - 120), (float)(Screen.height / 2 - 225), 240f, 50f);
		this.rect2 = new Rect((float)(Screen.width / 2 - 120), (float)(Screen.height / 2 - 175), 240f, 100f);
		this.rect3 = new Rect((float)(Screen.width / 2 - 120), (float)(Screen.height / 2 - 50), 240f, 50f);
		this.rect4 = new Rect((float)(Screen.width / 2 - 120), (float)(Screen.height / 2), 240f, 50f);
		this.rect5 = new Rect((float)(Screen.width / 2 - 120), (float)(Screen.height / 2 + 50), 240f, 50f);
		this.rect6 = new Rect((float)(Screen.width / 2 - 120), (float)(Screen.height / 2 + 100), 240f, 50f);
		this.thread = new Thread(new ThreadStart(this.threadLogTest));
		this.thread.Start();
	}

	private void OnDestroy()
	{
		this.thread.Abort();
	}

	private void threadLogTest()
	{
		for (int i = 0; i < this.threadLogTestCount; i++)
		{
			UnityEngine.Debug.Log("Test Log from Thread");
			UnityEngine.Debug.LogWarning("Test Warning from Thread");
			UnityEngine.Debug.LogError("Test Error from Thread");
		}
	}

	private void Update()
	{
		int num = 0;
		while (this.currentLogTestCount < this.logTestCount && num < 10)
		{
			UnityEngine.Debug.Log("Test Log " + this.currentLogTestCount);
			UnityEngine.Debug.LogError("Test LogError " + this.currentLogTestCount);
			UnityEngine.Debug.LogWarning("Test LogWarning " + this.currentLogTestCount);
			num++;
			this.currentLogTestCount++;
		}
		this.elapsed += Time.deltaTime;
		if (this.elapsed >= 1f)
		{
			this.elapsed = 0f;
			UnityEngine.Debug.Log("One Second Passed");
		}
	}

	private void OnGUI()
	{
		if (this.reporter && !this.reporter.show)
		{
			GUI.Label(this.rect1, "Draw circle on screen to show logs", this.style);
			GUI.Label(this.rect2, "To use Reporter just create reporter from reporter menu at first scene your game start", this.style);
			if (GUI.Button(this.rect3, "Load ReporterScene"))
			{
				SceneManager.LoadScene("ReporterScene");
			}
			if (GUI.Button(this.rect4, "Load test1"))
			{
				SceneManager.LoadScene("test1");
			}
			if (GUI.Button(this.rect5, "Load test2"))
			{
				SceneManager.LoadScene("test2");
			}
			GUI.Label(this.rect6, "fps : " + this.reporter.fps.ToString("0.0"), this.style);
		}
	}
}
