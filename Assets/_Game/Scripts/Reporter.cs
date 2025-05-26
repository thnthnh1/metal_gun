using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reporter : MonoBehaviour
{
	public enum _LogType
	{
		Assert = 1,
		Error = 0,
		Exception = 4,
		Log = 3,
		Warning = 2
	}

	public class Sample
	{
		public float time;

		public byte loadedScene;

		public float memory;

		public float fps;

		public string fpsText;

		public static float MemSize()
		{
			return 13f;
		}

		public string GetSceneName()
		{
			if ((int)this.loadedScene == -1)
			{
				return "AssetBundleScene";
			}
			return Reporter.scenes[(int)this.loadedScene];
		}
	}

	public class Log
	{
		public int count = 1;

		public Reporter._LogType logType;

		public string condition;

		public string stacktrace;

		public int sampleId;

		public Reporter.Log CreateCopy()
		{
			return (Reporter.Log)base.MemberwiseClone();
		}

		public float GetMemoryUsage()
		{
			return (float)(8 + this.condition.Length * 2 + this.stacktrace.Length * 2 + 4);
		}
	}

	private enum ReportView
	{
		None,
		Logs,
		Info,
		Snapshot
	}

	private enum DetailView
	{
		None,
		StackTrace,
		Graph
	}

	private sealed class _readInfo_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string _prefFile___0;

		internal string _url___0;

		internal WWW _www___0;

		internal Reporter _this;

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

		public _readInfo_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._prefFile___0 = "build_info.txt";
				this._url___0 = this._prefFile___0;
				if (this._prefFile___0.IndexOf("://") == -1)
				{
					string text = Application.streamingAssetsPath;
					if (text == string.Empty)
					{
						text = Application.dataPath + "/StreamingAssets/";
					}
					this._url___0 = Path.Combine(text, this._prefFile___0);
				}
				if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.WebGLPlayer && !this._url___0.Contains("://"))
				{
					this._url___0 = "file://" + this._url___0;
				}
				this._www___0 = new WWW(this._url___0);
				this._current = this._www___0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (!string.IsNullOrEmpty(this._www___0.error))
				{
					UnityEngine.Debug.LogError(this._www___0.error);
				}
				else
				{
					this._this.buildDate = this._www___0.text;
				}
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

	private List<Reporter.Sample> samples = new List<Reporter.Sample>(216000);

	private List<Reporter.Log> logs = new List<Reporter.Log>();

	private List<Reporter.Log> collapsedLogs = new List<Reporter.Log>();

	private List<Reporter.Log> currentLog = new List<Reporter.Log>();

	private MultiKeyDictionary<string, string, Reporter.Log> logsDic = new MultiKeyDictionary<string, string, Reporter.Log>();

	private Dictionary<string, string> cachedString = new Dictionary<string, string>();

	[HideInInspector]
	public bool show;

	private bool collapse;

	private bool clearOnNewSceneLoaded;

	private bool showTime;

	private bool showScene;

	private bool showMemory;

	private bool showFps;

	private bool showGraph;

	private bool showLog = true;

	private bool showWarning = true;

	private bool showError = true;

	private int numOfLogs;

	private int numOfLogsWarning;

	private int numOfLogsError;

	private int numOfCollapsedLogs;

	private int numOfCollapsedLogsWarning;

	private int numOfCollapsedLogsError;

	private bool showClearOnNewSceneLoadedButton = true;

	private bool showTimeButton = true;

	private bool showSceneButton = true;

	private bool showMemButton = true;

	private bool showFpsButton = true;

	private bool showSearchText = true;

	private string buildDate;

	private string logDate;

	private float logsMemUsage;

	private float graphMemUsage;

	private float gcTotalMemory;

	public string UserData = string.Empty;

	public float fps;

	public string fpsText;

	private Reporter.ReportView currentView = Reporter.ReportView.Logs;

	private static bool created;

	public Images images;

	private GUIContent clearContent;

	private GUIContent collapseContent;

	private GUIContent clearOnNewSceneContent;

	private GUIContent showTimeContent;

	private GUIContent showSceneContent;

	private GUIContent userContent;

	private GUIContent showMemoryContent;

	private GUIContent softwareContent;

	private GUIContent dateContent;

	private GUIContent showFpsContent;

	private GUIContent infoContent;

	private GUIContent searchContent;

	private GUIContent closeContent;

	private GUIContent buildFromContent;

	private GUIContent systemInfoContent;

	private GUIContent graphicsInfoContent;

	private GUIContent backContent;

	private GUIContent logContent;

	private GUIContent warningContent;

	private GUIContent errorContent;

	private GUIStyle barStyle;

	private GUIStyle buttonActiveStyle;

	private GUIStyle nonStyle;

	private GUIStyle lowerLeftFontStyle;

	private GUIStyle backStyle;

	private GUIStyle evenLogStyle;

	private GUIStyle oddLogStyle;

	private GUIStyle logButtonStyle;

	private GUIStyle selectedLogStyle;

	private GUIStyle selectedLogFontStyle;

	private GUIStyle stackLabelStyle;

	private GUIStyle scrollerStyle;

	private GUIStyle searchStyle;

	private GUIStyle sliderBackStyle;

	private GUIStyle sliderThumbStyle;

	private GUISkin toolbarScrollerSkin;

	private GUISkin logScrollerSkin;

	private GUISkin graphScrollerSkin;

	public Vector2 size = new Vector2(32f, 32f);

	public float maxSize = 20f;

	public int numOfCircleToShow = 1;

	private static string[] scenes;

	private string currentScene;

	private string filterText = string.Empty;

	private string deviceModel;

	private string deviceType;

	private string deviceName;

	private string graphicsMemorySize;

	private string maxTextureSize;

	private string systemMemorySize;

	public bool Initialized;

	private Rect screenRect;

	private Rect toolBarRect;

	private Rect logsRect;

	private Rect stackRect;

	private Rect graphRect;

	private Rect graphMinRect;

	private Rect graphMaxRect;

	private Rect buttomRect;

	private Vector2 stackRectTopLeft;

	private Rect detailRect;

	private Vector2 scrollPosition;

	private Vector2 scrollPosition2;

	private Vector2 toolbarScrollPosition;

	private Reporter.Log selectedLog;

	private float toolbarOldDrag;

	private float oldDrag;

	private float oldDrag2;

	private float oldDrag3;

	private int startIndex;

	private Rect countRect;

	private Rect timeRect;

	private Rect timeLabelRect;

	private Rect sceneRect;

	private Rect sceneLabelRect;

	private Rect memoryRect;

	private Rect memoryLabelRect;

	private Rect fpsRect;

	private Rect fpsLabelRect;

	private GUIContent tempContent = new GUIContent();

	private Vector2 infoScrollPosition;

	private Vector2 oldInfoDrag;

	private Rect tempRect;

	private float graphSize = 4f;

	private int startFrame;

	private int currentFrame;

	private Vector3 tempVector1;

	private Vector3 tempVector2;

	private Vector2 graphScrollerPos;

	private float maxFpsValue;

	private float minFpsValue;

	private float maxMemoryValue;

	private float minMemoryValue;

	private List<Vector2> gestureDetector = new List<Vector2>();

	private Vector2 gestureSum = Vector2.zero;

	private float gestureLength;

	private int gestureCount;

	private float lastClickTime = -1f;

	private Vector2 startPos;

	private Vector2 downPos;

	private Vector2 mousePosition;

	private int frames;

	private bool firstTime = true;

	private float lastUpdate;

	private const int requiredFrames = 10;

	private const float updateInterval = 0.25f;

	private List<Reporter.Log> threadedLogs = new List<Reporter.Log>();

	public float TotalMemUsage
	{
		get
		{
			return this.logsMemUsage + this.graphMemUsage;
		}
	}

	private void Awake()
	{
		if (!this.Initialized)
		{
			this.Initialize();
		}
		UnityEngine.Object.Destroy(this);
	}

	private void OnEnable()
	{
		if (this.logs.Count == 0)
		{
			this.clear();
		}
	}

	private void OnDisable()
	{
	}

	private void addSample()
	{
		Reporter.Sample sample = new Reporter.Sample();
		sample.fps = this.fps;
		sample.fpsText = this.fpsText;
		sample.loadedScene = (byte)SceneManager.GetActiveScene().buildIndex;
		sample.time = Time.realtimeSinceStartup;
		sample.memory = this.gcTotalMemory;
		this.samples.Add(sample);
		this.graphMemUsage = (float)this.samples.Count * Reporter.Sample.MemSize() / 1024f / 1024f;
	}

	public void Initialize()
	{
		if (!Reporter.created)
		{
			try
			{
				base.gameObject.SendMessage("OnPreStart");
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			Reporter.scenes = new string[SceneManager.sceneCountInBuildSettings];
			this.currentScene = SceneManager.GetActiveScene().name;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Application.logMessageReceivedThreaded += new Application.LogCallback(this.CaptureLogThread);
			Reporter.created = true;
			this.clearContent = new GUIContent(string.Empty, this.images.clearImage, "Clear logs");
			this.collapseContent = new GUIContent(string.Empty, this.images.collapseImage, "Collapse logs");
			this.clearOnNewSceneContent = new GUIContent(string.Empty, this.images.clearOnNewSceneImage, "Clear logs on new scene loaded");
			this.showTimeContent = new GUIContent(string.Empty, this.images.showTimeImage, "Show Hide Time");
			this.showSceneContent = new GUIContent(string.Empty, this.images.showSceneImage, "Show Hide Scene");
			this.showMemoryContent = new GUIContent(string.Empty, this.images.showMemoryImage, "Show Hide Memory");
			this.softwareContent = new GUIContent(string.Empty, this.images.softwareImage, "Software");
			this.dateContent = new GUIContent(string.Empty, this.images.dateImage, "Date");
			this.showFpsContent = new GUIContent(string.Empty, this.images.showFpsImage, "Show Hide fps");
			this.infoContent = new GUIContent(string.Empty, this.images.infoImage, "Information about application");
			this.searchContent = new GUIContent(string.Empty, this.images.searchImage, "Search for logs");
			this.closeContent = new GUIContent(string.Empty, this.images.closeImage, "Hide logs");
			this.userContent = new GUIContent(string.Empty, this.images.userImage, "User");
			this.buildFromContent = new GUIContent(string.Empty, this.images.buildFromImage, "Build From");
			this.systemInfoContent = new GUIContent(string.Empty, this.images.systemInfoImage, "System Info");
			this.graphicsInfoContent = new GUIContent(string.Empty, this.images.graphicsInfoImage, "Graphics Info");
			this.backContent = new GUIContent(string.Empty, this.images.backImage, "Back");
			this.logContent = new GUIContent(string.Empty, this.images.logImage, "show or hide logs");
			this.warningContent = new GUIContent(string.Empty, this.images.warningImage, "show or hide warnings");
			this.errorContent = new GUIContent(string.Empty, this.images.errorImage, "show or hide errors");
			this.currentView = (Reporter.ReportView)PlayerPrefs.GetInt("Reporter_currentView", 1);
			this.show = (PlayerPrefs.GetInt("Reporter_show") == 1);
			this.collapse = (PlayerPrefs.GetInt("Reporter_collapse") == 1);
			this.clearOnNewSceneLoaded = (PlayerPrefs.GetInt("Reporter_clearOnNewSceneLoaded") == 1);
			this.showTime = (PlayerPrefs.GetInt("Reporter_showTime") == 1);
			this.showScene = (PlayerPrefs.GetInt("Reporter_showScene") == 1);
			this.showMemory = (PlayerPrefs.GetInt("Reporter_showMemory") == 1);
			this.showFps = (PlayerPrefs.GetInt("Reporter_showFps") == 1);
			this.showGraph = (PlayerPrefs.GetInt("Reporter_showGraph") == 1);
			this.showLog = (PlayerPrefs.GetInt("Reporter_showLog", 1) == 1);
			this.showWarning = (PlayerPrefs.GetInt("Reporter_showWarning", 1) == 1);
			this.showError = (PlayerPrefs.GetInt("Reporter_showError", 1) == 1);
			this.filterText = PlayerPrefs.GetString("Reporter_filterText");
			this.size.x = (this.size.y = PlayerPrefs.GetFloat("Reporter_size", 32f));
			this.showClearOnNewSceneLoadedButton = (PlayerPrefs.GetInt("Reporter_showClearOnNewSceneLoadedButton", 1) == 1);
			this.showTimeButton = (PlayerPrefs.GetInt("Reporter_showTimeButton", 1) == 1);
			this.showSceneButton = (PlayerPrefs.GetInt("Reporter_showSceneButton", 1) == 1);
			this.showMemButton = (PlayerPrefs.GetInt("Reporter_showMemButton", 1) == 1);
			this.showFpsButton = (PlayerPrefs.GetInt("Reporter_showFpsButton", 1) == 1);
			this.showSearchText = (PlayerPrefs.GetInt("Reporter_showSearchText", 1) == 1);
			this.initializeStyle();
			this.Initialized = true;
			if (this.show)
			{
				this.doShow();
			}
			this.deviceModel = SystemInfo.deviceModel.ToString();
			this.deviceType = SystemInfo.deviceType.ToString();
			this.deviceName = SystemInfo.deviceName.ToString();
			this.graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();
			this.maxTextureSize = SystemInfo.maxTextureSize.ToString();
			this.systemMemorySize = SystemInfo.systemMemorySize.ToString();
			return;
		}
		UnityEngine.Debug.LogWarning("tow manager is exists delete the second");
		UnityEngine.Object.DestroyImmediate(base.gameObject, true);
	}

	private void initializeStyle()
	{
		int num = (int)(this.size.x * 0.2f);
		int num2 = (int)(this.size.y * 0.2f);
		this.nonStyle = new GUIStyle();
		this.nonStyle.clipping = TextClipping.Clip;
		this.nonStyle.border = new RectOffset(0, 0, 0, 0);
		this.nonStyle.normal.background = null;
		this.nonStyle.fontSize = (int)(this.size.y / 2f);
		this.nonStyle.alignment = TextAnchor.MiddleCenter;
		this.lowerLeftFontStyle = new GUIStyle();
		this.lowerLeftFontStyle.clipping = TextClipping.Clip;
		this.lowerLeftFontStyle.border = new RectOffset(0, 0, 0, 0);
		this.lowerLeftFontStyle.normal.background = null;
		this.lowerLeftFontStyle.fontSize = (int)(this.size.y / 2f);
		this.lowerLeftFontStyle.fontStyle = FontStyle.Bold;
		this.lowerLeftFontStyle.alignment = TextAnchor.LowerLeft;
		this.barStyle = new GUIStyle();
		this.barStyle.border = new RectOffset(1, 1, 1, 1);
		this.barStyle.normal.background = this.images.barImage;
		this.barStyle.active.background = this.images.button_activeImage;
		this.barStyle.alignment = TextAnchor.MiddleCenter;
		this.barStyle.margin = new RectOffset(1, 1, 1, 1);
		this.barStyle.clipping = TextClipping.Clip;
		this.barStyle.fontSize = (int)(this.size.y / 2f);
		this.buttonActiveStyle = new GUIStyle();
		this.buttonActiveStyle.border = new RectOffset(1, 1, 1, 1);
		this.buttonActiveStyle.normal.background = this.images.button_activeImage;
		this.buttonActiveStyle.alignment = TextAnchor.MiddleCenter;
		this.buttonActiveStyle.margin = new RectOffset(1, 1, 1, 1);
		this.buttonActiveStyle.fontSize = (int)(this.size.y / 2f);
		this.backStyle = new GUIStyle();
		this.backStyle.normal.background = this.images.even_logImage;
		this.backStyle.clipping = TextClipping.Clip;
		this.backStyle.fontSize = (int)(this.size.y / 2f);
		this.evenLogStyle = new GUIStyle();
		this.evenLogStyle.normal.background = this.images.even_logImage;
		this.evenLogStyle.fixedHeight = this.size.y;
		this.evenLogStyle.clipping = TextClipping.Clip;
		this.evenLogStyle.alignment = TextAnchor.UpperLeft;
		this.evenLogStyle.imagePosition = ImagePosition.ImageLeft;
		this.evenLogStyle.fontSize = (int)(this.size.y / 2f);
		this.oddLogStyle = new GUIStyle();
		this.oddLogStyle.normal.background = this.images.odd_logImage;
		this.oddLogStyle.fixedHeight = this.size.y;
		this.oddLogStyle.clipping = TextClipping.Clip;
		this.oddLogStyle.alignment = TextAnchor.UpperLeft;
		this.oddLogStyle.imagePosition = ImagePosition.ImageLeft;
		this.oddLogStyle.fontSize = (int)(this.size.y / 2f);
		this.logButtonStyle = new GUIStyle();
		this.logButtonStyle.fixedHeight = this.size.y;
		this.logButtonStyle.clipping = TextClipping.Clip;
		this.logButtonStyle.alignment = TextAnchor.UpperLeft;
		this.logButtonStyle.fontSize = (int)(this.size.y / 2f);
		this.logButtonStyle.padding = new RectOffset(num, num, num2, num2);
		this.selectedLogStyle = new GUIStyle();
		this.selectedLogStyle.normal.background = this.images.selectedImage;
		this.selectedLogStyle.fixedHeight = this.size.y;
		this.selectedLogStyle.clipping = TextClipping.Clip;
		this.selectedLogStyle.alignment = TextAnchor.UpperLeft;
		this.selectedLogStyle.normal.textColor = Color.white;
		this.selectedLogStyle.fontSize = (int)(this.size.y / 2f);
		this.selectedLogFontStyle = new GUIStyle();
		this.selectedLogFontStyle.normal.background = this.images.selectedImage;
		this.selectedLogFontStyle.fixedHeight = this.size.y;
		this.selectedLogFontStyle.clipping = TextClipping.Clip;
		this.selectedLogFontStyle.alignment = TextAnchor.UpperLeft;
		this.selectedLogFontStyle.normal.textColor = Color.white;
		this.selectedLogFontStyle.fontSize = (int)(this.size.y / 2f);
		this.selectedLogFontStyle.padding = new RectOffset(num, num, num2, num2);
		this.stackLabelStyle = new GUIStyle();
		this.stackLabelStyle.wordWrap = true;
		this.stackLabelStyle.fontSize = (int)(this.size.y / 2f);
		this.stackLabelStyle.padding = new RectOffset(num, num, num2, num2);
		this.scrollerStyle = new GUIStyle();
		this.scrollerStyle.normal.background = this.images.barImage;
		this.searchStyle = new GUIStyle();
		this.searchStyle.clipping = TextClipping.Clip;
		this.searchStyle.alignment = TextAnchor.LowerCenter;
		this.searchStyle.fontSize = (int)(this.size.y / 2f);
		this.searchStyle.wordWrap = true;
		this.sliderBackStyle = new GUIStyle();
		this.sliderBackStyle.normal.background = this.images.barImage;
		this.sliderBackStyle.fixedHeight = this.size.y;
		this.sliderBackStyle.border = new RectOffset(1, 1, 1, 1);
		this.sliderThumbStyle = new GUIStyle();
		this.sliderThumbStyle.normal.background = this.images.selectedImage;
		this.sliderThumbStyle.fixedWidth = this.size.x;
		GUISkin reporterScrollerSkin = this.images.reporterScrollerSkin;
		this.toolbarScrollerSkin = UnityEngine.Object.Instantiate<GUISkin>(reporterScrollerSkin);
		this.toolbarScrollerSkin.verticalScrollbar.fixedWidth = 0f;
		this.toolbarScrollerSkin.horizontalScrollbar.fixedHeight = 0f;
		this.toolbarScrollerSkin.verticalScrollbarThumb.fixedWidth = 0f;
		this.toolbarScrollerSkin.horizontalScrollbarThumb.fixedHeight = 0f;
		this.logScrollerSkin = UnityEngine.Object.Instantiate<GUISkin>(reporterScrollerSkin);
		this.logScrollerSkin.verticalScrollbar.fixedWidth = this.size.x * 2f;
		this.logScrollerSkin.horizontalScrollbar.fixedHeight = 0f;
		this.logScrollerSkin.verticalScrollbarThumb.fixedWidth = this.size.x * 2f;
		this.logScrollerSkin.horizontalScrollbarThumb.fixedHeight = 0f;
		this.graphScrollerSkin = UnityEngine.Object.Instantiate<GUISkin>(reporterScrollerSkin);
		this.graphScrollerSkin.verticalScrollbar.fixedWidth = 0f;
		this.graphScrollerSkin.horizontalScrollbar.fixedHeight = this.size.x * 2f;
		this.graphScrollerSkin.verticalScrollbarThumb.fixedWidth = 0f;
		this.graphScrollerSkin.horizontalScrollbarThumb.fixedHeight = this.size.x * 2f;
	}

	private void Start()
	{
		this.logDate = DateTime.Now.ToString();
		base.StartCoroutine("readInfo");
	}

	private void clear()
	{
		this.logs.Clear();
		this.collapsedLogs.Clear();
		this.currentLog.Clear();
		this.logsDic.Clear();
		this.selectedLog = null;
		this.numOfLogs = 0;
		this.numOfLogsWarning = 0;
		this.numOfLogsError = 0;
		this.numOfCollapsedLogs = 0;
		this.numOfCollapsedLogsWarning = 0;
		this.numOfCollapsedLogsError = 0;
		this.logsMemUsage = 0f;
		this.graphMemUsage = 0f;
		this.samples.Clear();
		GC.Collect();
		this.selectedLog = null;
	}

	private void calculateCurrentLog()
	{
		bool flag = !string.IsNullOrEmpty(this.filterText);
		string value = string.Empty;
		if (flag)
		{
			value = this.filterText.ToLower();
		}
		this.currentLog.Clear();
		if (this.collapse)
		{
			for (int i = 0; i < this.collapsedLogs.Count; i++)
			{
				Reporter.Log log = this.collapsedLogs[i];
				if (log.logType != Reporter._LogType.Log || this.showLog)
				{
					if (log.logType != Reporter._LogType.Warning || this.showWarning)
					{
						if (log.logType != Reporter._LogType.Error || this.showError)
						{
							if (log.logType != Reporter._LogType.Assert || this.showError)
							{
								if (log.logType != Reporter._LogType.Exception || this.showError)
								{
									if (flag)
									{
										if (log.condition.ToLower().Contains(value))
										{
											this.currentLog.Add(log);
										}
									}
									else
									{
										this.currentLog.Add(log);
									}
								}
							}
						}
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < this.logs.Count; j++)
			{
				Reporter.Log log2 = this.logs[j];
				if (log2.logType != Reporter._LogType.Log || this.showLog)
				{
					if (log2.logType != Reporter._LogType.Warning || this.showWarning)
					{
						if (log2.logType != Reporter._LogType.Error || this.showError)
						{
							if (log2.logType != Reporter._LogType.Assert || this.showError)
							{
								if (log2.logType != Reporter._LogType.Exception || this.showError)
								{
									if (flag)
									{
										if (log2.condition.ToLower().Contains(value))
										{
											this.currentLog.Add(log2);
										}
									}
									else
									{
										this.currentLog.Add(log2);
									}
								}
							}
						}
					}
				}
			}
		}
		if (this.selectedLog != null)
		{
			int num = this.currentLog.IndexOf(this.selectedLog);
			if (num == -1)
			{
				Reporter.Log item = this.logsDic[this.selectedLog.condition][this.selectedLog.stacktrace];
				num = this.currentLog.IndexOf(item);
				if (num != -1)
				{
					this.scrollPosition.y = (float)num * this.size.y;
				}
			}
			else
			{
				this.scrollPosition.y = (float)num * this.size.y;
			}
		}
	}

	private void DrawInfo()
	{
		GUILayout.BeginArea(this.screenRect, this.backStyle);
		Vector2 drag = this.getDrag();
		if (drag.x != 0f && this.downPos != Vector2.zero)
		{
			this.infoScrollPosition.x = this.infoScrollPosition.x - (drag.x - this.oldInfoDrag.x);
		}
		if (drag.y != 0f && this.downPos != Vector2.zero)
		{
			this.infoScrollPosition.y = this.infoScrollPosition.y + (drag.y - this.oldInfoDrag.y);
		}
		this.oldInfoDrag = drag;
		GUI.skin = this.toolbarScrollerSkin;
		this.infoScrollPosition = GUILayout.BeginScrollView(this.infoScrollPosition, new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.buildFromContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.buildDate, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.systemInfoContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.deviceModel, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.deviceType, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.deviceName, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.graphicsInfoContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(SystemInfo.graphicsDeviceName, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.graphicsMemorySize, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.maxTextureSize, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Space(this.size.x);
		GUILayout.Space(this.size.x);
		GUILayout.Label("Screen Width " + Screen.width, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label("Screen Height " + Screen.height, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.showMemoryContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.systemMemorySize + " mb", this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Space(this.size.x);
		GUILayout.Space(this.size.x);
		GUILayout.Label("Mem Usage Of Logs " + this.logsMemUsage.ToString("0.000") + " mb", this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label("GC Memory " + this.gcTotalMemory.ToString("0.000") + " mb", this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.softwareContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(SystemInfo.operatingSystem, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.dateContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(DateTime.Now.ToString(), this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Label(" - Application Started At " + this.logDate, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.showTimeContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(Time.realtimeSinceStartup.ToString("000"), this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.showFpsContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.fpsText, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.userContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.UserData, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.showSceneContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label(this.currentScene, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Box(this.showSceneContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.Label("Unity Version = " + Application.unityVersion, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		this.drawInfo_enableDisableToolBarButtons();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Label("Size = " + this.size.x.ToString("0.0"), this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		float num = GUILayout.HorizontalSlider(this.size.x, 16f, 64f, this.sliderBackStyle, this.sliderThumbStyle, new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width * 0.5f)
		});
		if (this.size.x != num)
		{
			this.size.x = (this.size.y = num);
			this.initializeStyle();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		if (GUILayout.Button(this.backContent, this.barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.currentView = Reporter.ReportView.Logs;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private void drawInfo_enableDisableToolBarButtons()
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		GUILayout.Label("Hide or Show tool bar buttons", this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.Space(this.size.x);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Space(this.size.x);
		if (GUILayout.Button(this.clearOnNewSceneContent, (!this.showClearOnNewSceneLoadedButton) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showClearOnNewSceneLoadedButton = !this.showClearOnNewSceneLoadedButton;
		}
		if (GUILayout.Button(this.showTimeContent, (!this.showTimeButton) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showTimeButton = !this.showTimeButton;
		}
		this.tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(this.tempRect, Time.realtimeSinceStartup.ToString("0.0"), this.lowerLeftFontStyle);
		if (GUILayout.Button(this.showSceneContent, (!this.showSceneButton) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showSceneButton = !this.showSceneButton;
		}
		this.tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(this.tempRect, this.currentScene, this.lowerLeftFontStyle);
		if (GUILayout.Button(this.showMemoryContent, (!this.showMemButton) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showMemButton = !this.showMemButton;
		}
		this.tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(this.tempRect, this.gcTotalMemory.ToString("0.0"), this.lowerLeftFontStyle);
		if (GUILayout.Button(this.showFpsContent, (!this.showFpsButton) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showFpsButton = !this.showFpsButton;
		}
		this.tempRect = GUILayoutUtility.GetLastRect();
		GUI.Label(this.tempRect, this.fpsText, this.lowerLeftFontStyle);
		if (GUILayout.Button(this.searchContent, (!this.showSearchText) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showSearchText = !this.showSearchText;
		}
		this.tempRect = GUILayoutUtility.GetLastRect();
		GUI.TextField(this.tempRect, this.filterText, this.searchStyle);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void DrawReport()
	{
		this.screenRect.x = 0f;
		this.screenRect.y = 0f;
		this.screenRect.width = (float)Screen.width;
		this.screenRect.height = (float)Screen.height;
		GUILayout.BeginArea(this.screenRect, this.backStyle);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Select Photo", this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Coming Soon", this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Height(this.size.y)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(this.backContent, this.barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		}))
		{
			this.currentView = Reporter.ReportView.Logs;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void drawToolBar()
	{
		this.toolBarRect.x = 0f;
		this.toolBarRect.y = 0f;
		this.toolBarRect.width = (float)Screen.width;
		this.toolBarRect.height = this.size.y * 2f;
		GUI.skin = this.toolbarScrollerSkin;
		Vector2 drag = this.getDrag();
		if (drag.x != 0f && this.downPos != Vector2.zero && this.downPos.y > (float)Screen.height - this.size.y * 2f)
		{
			this.toolbarScrollPosition.x = this.toolbarScrollPosition.x - (drag.x - this.toolbarOldDrag);
		}
		this.toolbarOldDrag = drag.x;
		GUILayout.BeginArea(this.toolBarRect);
		this.toolbarScrollPosition = GUILayout.BeginScrollView(this.toolbarScrollPosition, new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(this.barStyle, new GUILayoutOption[0]);
		if (GUILayout.Button(this.clearContent, this.barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.clear();
		}
		if (GUILayout.Button(this.collapseContent, (!this.collapse) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.collapse = !this.collapse;
			this.calculateCurrentLog();
		}
		if (this.showClearOnNewSceneLoadedButton && GUILayout.Button(this.clearOnNewSceneContent, (!this.clearOnNewSceneLoaded) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.clearOnNewSceneLoaded = !this.clearOnNewSceneLoaded;
		}
		if (this.showTimeButton && GUILayout.Button(this.showTimeContent, (!this.showTime) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showTime = !this.showTime;
		}
		if (this.showSceneButton)
		{
			this.tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(this.tempRect, Time.realtimeSinceStartup.ToString("0.0"), this.lowerLeftFontStyle);
			if (GUILayout.Button(this.showSceneContent, (!this.showScene) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x * 2f),
				GUILayout.Height(this.size.y * 2f)
			}))
			{
				this.showScene = !this.showScene;
			}
			this.tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(this.tempRect, this.currentScene, this.lowerLeftFontStyle);
		}
		if (this.showMemButton)
		{
			if (GUILayout.Button(this.showMemoryContent, (!this.showMemory) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x * 2f),
				GUILayout.Height(this.size.y * 2f)
			}))
			{
				this.showMemory = !this.showMemory;
			}
			this.tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(this.tempRect, this.gcTotalMemory.ToString("0.0"), this.lowerLeftFontStyle);
		}
		if (this.showFpsButton)
		{
			if (GUILayout.Button(this.showFpsContent, (!this.showFps) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x * 2f),
				GUILayout.Height(this.size.y * 2f)
			}))
			{
				this.showFps = !this.showFps;
			}
			this.tempRect = GUILayoutUtility.GetLastRect();
			GUI.Label(this.tempRect, this.fpsText, this.lowerLeftFontStyle);
		}
		if (this.showSearchText)
		{
			GUILayout.Box(this.searchContent, this.barStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x * 2f),
				GUILayout.Height(this.size.y * 2f)
			});
			this.tempRect = GUILayoutUtility.GetLastRect();
			string a = GUI.TextField(this.tempRect, this.filterText, this.searchStyle);
			if (a != this.filterText)
			{
				this.filterText = a;
				this.calculateCurrentLog();
			}
		}
		if (GUILayout.Button(this.infoContent, this.barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.currentView = Reporter.ReportView.Info;
		}
		GUILayout.FlexibleSpace();
		string text = " ";
		if (this.collapse)
		{
			text += this.numOfCollapsedLogs;
		}
		else
		{
			text += this.numOfLogs;
		}
		string text2 = " ";
		if (this.collapse)
		{
			text2 += this.numOfCollapsedLogsWarning;
		}
		else
		{
			text2 += this.numOfLogsWarning;
		}
		string text3 = " ";
		if (this.collapse)
		{
			text3 += this.numOfCollapsedLogsError;
		}
		else
		{
			text3 += this.numOfLogsError;
		}
		GUILayout.BeginHorizontal((!this.showLog) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[0]);
		if (GUILayout.Button(this.logContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showLog = !this.showLog;
			this.calculateCurrentLog();
		}
		if (GUILayout.Button(text, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showLog = !this.showLog;
			this.calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((!this.showWarning) ? this.barStyle : this.buttonActiveStyle, new GUILayoutOption[0]);
		if (GUILayout.Button(this.warningContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showWarning = !this.showWarning;
			this.calculateCurrentLog();
		}
		if (GUILayout.Button(text2, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showWarning = !this.showWarning;
			this.calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((!this.showError) ? this.nonStyle : this.buttonActiveStyle, new GUILayoutOption[0]);
		if (GUILayout.Button(this.errorContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showError = !this.showError;
			this.calculateCurrentLog();
		}
		if (GUILayout.Button(text3, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.showError = !this.showError;
			this.calculateCurrentLog();
		}
		GUILayout.EndHorizontal();
		if (GUILayout.Button(this.closeContent, this.barStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x * 2f),
			GUILayout.Height(this.size.y * 2f)
		}))
		{
			this.show = false;
			ReporterGUI component = base.gameObject.GetComponent<ReporterGUI>();
			UnityEngine.Object.DestroyImmediate(component);
			try
			{
				base.gameObject.SendMessage("OnHideReporter");
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private void DrawLogs()
	{
		GUILayout.BeginArea(this.logsRect, this.backStyle);
		GUI.skin = this.logScrollerSkin;
		Vector2 drag = this.getDrag();
		if (drag.y != 0f && this.logsRect.Contains(new Vector2(this.downPos.x, (float)Screen.height - this.downPos.y)))
		{
			this.scrollPosition.y = this.scrollPosition.y + (drag.y - this.oldDrag);
		}
		this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, new GUILayoutOption[0]);
		this.oldDrag = drag.y;
		int num = (int)((float)Screen.height * 0.75f / this.size.y);
		int count = this.currentLog.Count;
		num = Mathf.Min(num, count - this.startIndex);
		int num2 = 0;
		int num3 = (int)((float)this.startIndex * this.size.y);
		if (num3 > 0)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Height((float)num3)
			});
			GUILayout.Label("---", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		int num4 = this.startIndex + num;
		num4 = Mathf.Clamp(num4, 0, count);
		bool flag = num < count;
		int num5 = this.startIndex;
		while (this.startIndex + num2 < num4)
		{
			if (num5 >= this.currentLog.Count)
			{
				break;
			}
			Reporter.Log log = this.currentLog[num5];
			if (log.logType != Reporter._LogType.Log || this.showLog)
			{
				if (log.logType != Reporter._LogType.Warning || this.showWarning)
				{
					if (log.logType != Reporter._LogType.Error || this.showError)
					{
						if (log.logType != Reporter._LogType.Assert || this.showError)
						{
							if (log.logType != Reporter._LogType.Exception || this.showError)
							{
								if (num2 >= num)
								{
									break;
								}
								GUIContent content;
								if (log.logType == Reporter._LogType.Log)
								{
									content = this.logContent;
								}
								else if (log.logType == Reporter._LogType.Warning)
								{
									content = this.warningContent;
								}
								else
								{
									content = this.errorContent;
								}
								GUIStyle gUIStyle = ((this.startIndex + num2) % 2 != 0) ? this.oddLogStyle : this.evenLogStyle;
								if (log == this.selectedLog)
								{
									gUIStyle = this.selectedLogStyle;
								}
								this.tempContent.text = log.count.ToString();
								float num6 = 0f;
								if (this.collapse)
								{
									num6 = this.barStyle.CalcSize(this.tempContent).x + 3f;
								}
								this.countRect.x = (float)Screen.width - num6;
								this.countRect.y = this.size.y * (float)num5;
								if (num3 > 0)
								{
									this.countRect.y = this.countRect.y + 8f;
								}
								this.countRect.width = num6;
								this.countRect.height = this.size.y;
								if (flag)
								{
									this.countRect.x = this.countRect.x - this.size.x * 2f;
								}
								Reporter.Sample sample = this.samples[log.sampleId];
								this.fpsRect = this.countRect;
								if (this.showFps)
								{
									this.tempContent.text = sample.fpsText;
									num6 = gUIStyle.CalcSize(this.tempContent).x + this.size.x;
									this.fpsRect.x = this.fpsRect.x - num6;
									this.fpsRect.width = this.size.x;
									this.fpsLabelRect = this.fpsRect;
									this.fpsLabelRect.x = this.fpsLabelRect.x + this.size.x;
									this.fpsLabelRect.width = num6 - this.size.x;
								}
								this.memoryRect = this.fpsRect;
								if (this.showMemory)
								{
									this.tempContent.text = sample.memory.ToString("0.000");
									num6 = gUIStyle.CalcSize(this.tempContent).x + this.size.x;
									this.memoryRect.x = this.memoryRect.x - num6;
									this.memoryRect.width = this.size.x;
									this.memoryLabelRect = this.memoryRect;
									this.memoryLabelRect.x = this.memoryLabelRect.x + this.size.x;
									this.memoryLabelRect.width = num6 - this.size.x;
								}
								this.sceneRect = this.memoryRect;
								if (this.showScene)
								{
									this.tempContent.text = sample.GetSceneName();
									num6 = gUIStyle.CalcSize(this.tempContent).x + this.size.x;
									this.sceneRect.x = this.sceneRect.x - num6;
									this.sceneRect.width = this.size.x;
									this.sceneLabelRect = this.sceneRect;
									this.sceneLabelRect.x = this.sceneLabelRect.x + this.size.x;
									this.sceneLabelRect.width = num6 - this.size.x;
								}
								this.timeRect = this.sceneRect;
								if (this.showTime)
								{
									this.tempContent.text = sample.time.ToString("0.000");
									num6 = gUIStyle.CalcSize(this.tempContent).x + this.size.x;
									this.timeRect.x = this.timeRect.x - num6;
									this.timeRect.width = this.size.x;
									this.timeLabelRect = this.timeRect;
									this.timeLabelRect.x = this.timeLabelRect.x + this.size.x;
									this.timeLabelRect.width = num6 - this.size.x;
								}
								GUILayout.BeginHorizontal(gUIStyle, new GUILayoutOption[0]);
								if (log == this.selectedLog)
								{
									GUILayout.Box(content, this.nonStyle, new GUILayoutOption[]
									{
										GUILayout.Width(this.size.x),
										GUILayout.Height(this.size.y)
									});
									GUILayout.Label(log.condition, this.selectedLogFontStyle, new GUILayoutOption[0]);
									if (this.showTime)
									{
										GUI.Box(this.timeRect, this.showTimeContent, gUIStyle);
										GUI.Label(this.timeLabelRect, sample.time.ToString("0.000"), gUIStyle);
									}
									if (this.showScene)
									{
										GUI.Box(this.sceneRect, this.showSceneContent, gUIStyle);
										GUI.Label(this.sceneLabelRect, sample.GetSceneName(), gUIStyle);
									}
									if (this.showMemory)
									{
										GUI.Box(this.memoryRect, this.showMemoryContent, gUIStyle);
										GUI.Label(this.memoryLabelRect, sample.memory.ToString("0.000") + " mb", gUIStyle);
									}
									if (this.showFps)
									{
										GUI.Box(this.fpsRect, this.showFpsContent, gUIStyle);
										GUI.Label(this.fpsLabelRect, sample.fpsText, gUIStyle);
									}
								}
								else
								{
									if (GUILayout.Button(content, this.nonStyle, new GUILayoutOption[]
									{
										GUILayout.Width(this.size.x),
										GUILayout.Height(this.size.y)
									}))
									{
										this.selectedLog = log;
									}
									if (GUILayout.Button(log.condition, this.logButtonStyle, new GUILayoutOption[0]))
									{
										this.selectedLog = log;
									}
									if (this.showTime)
									{
										GUI.Box(this.timeRect, this.showTimeContent, gUIStyle);
										GUI.Label(this.timeLabelRect, sample.time.ToString("0.000"), gUIStyle);
									}
									if (this.showScene)
									{
										GUI.Box(this.sceneRect, this.showSceneContent, gUIStyle);
										GUI.Label(this.sceneLabelRect, sample.GetSceneName(), gUIStyle);
									}
									if (this.showMemory)
									{
										GUI.Box(this.memoryRect, this.showMemoryContent, gUIStyle);
										GUI.Label(this.memoryLabelRect, sample.memory.ToString("0.000") + " mb", gUIStyle);
									}
									if (this.showFps)
									{
										GUI.Box(this.fpsRect, this.showFpsContent, gUIStyle);
										GUI.Label(this.fpsLabelRect, sample.fpsText, gUIStyle);
									}
								}
								if (this.collapse)
								{
									GUI.Label(this.countRect, log.count.ToString(), this.barStyle);
								}
								GUILayout.EndHorizontal();
								num2++;
							}
						}
					}
				}
			}
			num5++;
		}
		int num7 = (int)((float)(count - (this.startIndex + num)) * this.size.y);
		if (num7 > 0)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Height((float)num7)
			});
			GUILayout.Label(" ", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		this.buttomRect.x = 0f;
		this.buttomRect.y = (float)Screen.height - this.size.y;
		this.buttomRect.width = (float)Screen.width;
		this.buttomRect.height = this.size.y;
		if (this.showGraph)
		{
			this.drawGraph();
		}
		else
		{
			this.drawStack();
		}
	}

	private void drawGraph()
	{
		this.graphRect = this.stackRect;
		this.graphRect.height = (float)Screen.height * 0.25f;
		GUI.skin = this.graphScrollerSkin;
		Vector2 drag = this.getDrag();
		if (this.graphRect.Contains(new Vector2(this.downPos.x, (float)Screen.height - this.downPos.y)))
		{
			if (drag.x != 0f)
			{
				this.graphScrollerPos.x = this.graphScrollerPos.x - (drag.x - this.oldDrag3);
				this.graphScrollerPos.x = Mathf.Max(0f, this.graphScrollerPos.x);
			}
			Vector2 lhs = this.downPos;
			if (lhs != Vector2.zero)
			{
				this.currentFrame = this.startFrame + (int)(lhs.x / this.graphSize);
			}
		}
		this.oldDrag3 = drag.x;
		GUILayout.BeginArea(this.graphRect, this.backStyle);
		this.graphScrollerPos = GUILayout.BeginScrollView(this.graphScrollerPos, new GUILayoutOption[0]);
		this.startFrame = (int)(this.graphScrollerPos.x / this.graphSize);
		if (this.graphScrollerPos.x >= (float)this.samples.Count * this.graphSize - (float)Screen.width)
		{
			this.graphScrollerPos.x = this.graphScrollerPos.x + this.graphSize;
		}
		GUILayout.Label(" ", new GUILayoutOption[]
		{
			GUILayout.Width((float)this.samples.Count * this.graphSize)
		});
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		this.maxFpsValue = 0f;
		this.minFpsValue = 100000f;
		this.maxMemoryValue = 0f;
		this.minMemoryValue = 100000f;
		int num = 0;
		while ((float)num < (float)Screen.width / this.graphSize)
		{
			int num2 = this.startFrame + num;
			if (num2 >= this.samples.Count)
			{
				break;
			}
			Reporter.Sample sample = this.samples[num2];
			if (this.maxFpsValue < sample.fps)
			{
				this.maxFpsValue = sample.fps;
			}
			if (this.minFpsValue > sample.fps)
			{
				this.minFpsValue = sample.fps;
			}
			if (this.maxMemoryValue < sample.memory)
			{
				this.maxMemoryValue = sample.memory;
			}
			if (this.minMemoryValue > sample.memory)
			{
				this.minMemoryValue = sample.memory;
			}
			num++;
		}
		if (this.currentFrame != -1 && this.currentFrame < this.samples.Count)
		{
			Reporter.Sample sample2 = this.samples[this.currentFrame];
			GUILayout.BeginArea(this.buttomRect, this.backStyle);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Box(this.showTimeContent, this.nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x),
				GUILayout.Height(this.size.y)
			});
			GUILayout.Label(sample2.time.ToString("0.0"), this.nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(this.size.x);
			GUILayout.Box(this.showSceneContent, this.nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x),
				GUILayout.Height(this.size.y)
			});
			GUILayout.Label(sample2.GetSceneName(), this.nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(this.size.x);
			GUILayout.Box(this.showMemoryContent, this.nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x),
				GUILayout.Height(this.size.y)
			});
			GUILayout.Label(sample2.memory.ToString("0.000"), this.nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(this.size.x);
			GUILayout.Box(this.showFpsContent, this.nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x),
				GUILayout.Height(this.size.y)
			});
			GUILayout.Label(sample2.fpsText, this.nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(this.size.x);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		this.graphMaxRect = this.stackRect;
		this.graphMaxRect.height = this.size.y;
		GUILayout.BeginArea(this.graphMaxRect);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Box(this.showMemoryContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Label(this.maxMemoryValue.ToString("0.000"), this.nonStyle, new GUILayoutOption[0]);
		GUILayout.Box(this.showFpsContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Label(this.maxFpsValue.ToString("0.000"), this.nonStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		this.graphMinRect = this.stackRect;
		this.graphMinRect.y = this.stackRect.y + this.stackRect.height - this.size.y;
		this.graphMinRect.height = this.size.y;
		GUILayout.BeginArea(this.graphMinRect);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Box(this.showMemoryContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Label(this.minMemoryValue.ToString("0.000"), this.nonStyle, new GUILayoutOption[0]);
		GUILayout.Box(this.showFpsContent, this.nonStyle, new GUILayoutOption[]
		{
			GUILayout.Width(this.size.x),
			GUILayout.Height(this.size.y)
		});
		GUILayout.Label(this.minFpsValue.ToString("0.000"), this.nonStyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void drawStack()
	{
		if (this.selectedLog != null)
		{
			Vector2 drag = this.getDrag();
			if (drag.y != 0f && this.stackRect.Contains(new Vector2(this.downPos.x, (float)Screen.height - this.downPos.y)))
			{
				this.scrollPosition2.y = this.scrollPosition2.y + (drag.y - this.oldDrag2);
			}
			this.oldDrag2 = drag.y;
			GUILayout.BeginArea(this.stackRect, this.backStyle);
			this.scrollPosition2 = GUILayout.BeginScrollView(this.scrollPosition2, new GUILayoutOption[0]);
			Reporter.Sample sample = null;
			try
			{
				sample = this.samples[this.selectedLog.sampleId];
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(this.selectedLog.condition, this.stackLabelStyle, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.Space(this.size.y * 0.25f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(this.selectedLog.stacktrace, this.stackLabelStyle, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.Space(this.size.y);
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUILayout.BeginArea(this.buttomRect, this.backStyle);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Box(this.showTimeContent, this.nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x),
				GUILayout.Height(this.size.y)
			});
			GUILayout.Label(sample.time.ToString("0.000"), this.nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(this.size.x);
			GUILayout.Box(this.showSceneContent, this.nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x),
				GUILayout.Height(this.size.y)
			});
			GUILayout.Label(sample.GetSceneName(), this.nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(this.size.x);
			GUILayout.Box(this.showMemoryContent, this.nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x),
				GUILayout.Height(this.size.y)
			});
			GUILayout.Label(sample.memory.ToString("0.000"), this.nonStyle, new GUILayoutOption[0]);
			GUILayout.Space(this.size.x);
			GUILayout.Box(this.showFpsContent, this.nonStyle, new GUILayoutOption[]
			{
				GUILayout.Width(this.size.x),
				GUILayout.Height(this.size.y)
			});
			GUILayout.Label(sample.fpsText, this.nonStyle, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		else
		{
			GUILayout.BeginArea(this.stackRect, this.backStyle);
			GUILayout.EndArea();
			GUILayout.BeginArea(this.buttomRect, this.backStyle);
			GUILayout.EndArea();
		}
	}

	public void OnGUIDraw()
	{
		if (!this.show)
		{
			return;
		}
		this.screenRect.x = 0f;
		this.screenRect.y = 0f;
		this.screenRect.width = (float)Screen.width;
		this.screenRect.height = (float)Screen.height;
		this.getDownPos();
		this.logsRect.x = 0f;
		this.logsRect.y = this.size.y * 2f;
		this.logsRect.width = (float)Screen.width;
		this.logsRect.height = (float)Screen.height * 0.75f - this.size.y * 2f;
		this.stackRectTopLeft.x = 0f;
		this.stackRect.x = 0f;
		this.stackRectTopLeft.y = (float)Screen.height * 0.75f;
		this.stackRect.y = (float)Screen.height * 0.75f;
		this.stackRect.width = (float)Screen.width;
		this.stackRect.height = (float)Screen.height * 0.25f - this.size.y;
		this.detailRect.x = 0f;
		this.detailRect.y = (float)Screen.height - this.size.y * 3f;
		this.detailRect.width = (float)Screen.width;
		this.detailRect.height = this.size.y * 3f;
		if (this.currentView == Reporter.ReportView.Info)
		{
			this.DrawInfo();
		}
		else if (this.currentView == Reporter.ReportView.Logs)
		{
			this.drawToolBar();
			this.DrawLogs();
		}
	}

	private bool isGestureDone()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touches.Length != 1)
			{
				this.gestureDetector.Clear();
				this.gestureCount = 0;
			}
			else if (Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended)
			{
				this.gestureDetector.Clear();
			}
			else if (Input.touches[0].phase == TouchPhase.Moved)
			{
				Vector2 position = Input.touches[0].position;
				if (this.gestureDetector.Count == 0 || (position - this.gestureDetector[this.gestureDetector.Count - 1]).magnitude > 10f)
				{
					this.gestureDetector.Add(position);
				}
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			this.gestureDetector.Clear();
			this.gestureCount = 0;
		}
		else if (Input.GetMouseButton(0))
		{
			Vector2 vector = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
			if (this.gestureDetector.Count == 0 || (vector - this.gestureDetector[this.gestureDetector.Count - 1]).magnitude > 10f)
			{
				this.gestureDetector.Add(vector);
			}
		}
		if (this.gestureDetector.Count < 10)
		{
			return false;
		}
		this.gestureSum = Vector2.zero;
		this.gestureLength = 0f;
		Vector2 rhs = Vector2.zero;
		for (int i = 0; i < this.gestureDetector.Count - 2; i++)
		{
			Vector2 vector2 = this.gestureDetector[i + 1] - this.gestureDetector[i];
			float magnitude = vector2.magnitude;
			this.gestureSum += vector2;
			this.gestureLength += magnitude;
			float num = Vector2.Dot(vector2, rhs);
			if (num < 0f)
			{
				this.gestureDetector.Clear();
				this.gestureCount = 0;
				return false;
			}
			rhs = vector2;
		}
		int num2 = (Screen.width + Screen.height) / 4;
		if (this.gestureLength > (float)num2 && this.gestureSum.magnitude < (float)(num2 / 2))
		{
			this.gestureDetector.Clear();
			this.gestureCount++;
			if (this.gestureCount >= this.numOfCircleToShow)
			{
				return true;
			}
		}
		return false;
	}

	private bool isDoubleClickDone()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touches.Length != 1)
			{
				this.lastClickTime = -1f;
			}
			else if (Input.touches[0].phase == TouchPhase.Began)
			{
				if (this.lastClickTime == -1f)
				{
					this.lastClickTime = Time.realtimeSinceStartup;
				}
				else
				{
					if (Time.realtimeSinceStartup - this.lastClickTime < 0.2f)
					{
						this.lastClickTime = -1f;
						return true;
					}
					this.lastClickTime = Time.realtimeSinceStartup;
				}
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			if (this.lastClickTime == -1f)
			{
				this.lastClickTime = Time.realtimeSinceStartup;
			}
			else
			{
				if (Time.realtimeSinceStartup - this.lastClickTime < 0.2f)
				{
					this.lastClickTime = -1f;
					return true;
				}
				this.lastClickTime = Time.realtimeSinceStartup;
			}
		}
		return false;
	}

	private Vector2 getDownPos()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Began)
			{
				this.downPos = Input.touches[0].position;
				return this.downPos;
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			this.downPos.x = UnityEngine.Input.mousePosition.x;
			this.downPos.y = UnityEngine.Input.mousePosition.y;
			return this.downPos;
		}
		return Vector2.zero;
	}

	private Vector2 getDrag()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touches.Length != 1)
			{
				return Vector2.zero;
			}
			return Input.touches[0].position - this.downPos;
		}
		else
		{
			if (Input.GetMouseButton(0))
			{
				this.mousePosition = UnityEngine.Input.mousePosition;
				return this.mousePosition - this.downPos;
			}
			return Vector2.zero;
		}
	}

	private void calculateStartIndex()
	{
		this.startIndex = (int)(this.scrollPosition.y / this.size.y);
		this.startIndex = Mathf.Clamp(this.startIndex, 0, this.currentLog.Count);
	}

	private void doShow()
	{
		this.show = true;
		this.currentView = Reporter.ReportView.Logs;
		base.gameObject.AddComponent<ReporterGUI>();
		try
		{
			base.gameObject.SendMessage("OnShowReporter");
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void Update()
	{
		this.fpsText = this.fps.ToString("0.000");
		this.gcTotalMemory = (float)GC.GetTotalMemory(false) / 1024f / 1024f;
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		if (buildIndex != -1 && string.IsNullOrEmpty(Reporter.scenes[buildIndex]))
		{
			Reporter.scenes[SceneManager.GetActiveScene().buildIndex] = SceneManager.GetActiveScene().name;
		}
		this.calculateStartIndex();
		if (!this.show && this.isGestureDone())
		{
			this.doShow();
		}
		if (this.threadedLogs.Count > 0)
		{
			object obj = this.threadedLogs;
			lock (obj)
			{
				for (int i = 0; i < this.threadedLogs.Count; i++)
				{
					Reporter.Log log = this.threadedLogs[i];
					this.AddLog(log.condition, log.stacktrace, (LogType)log.logType);
				}
				this.threadedLogs.Clear();
			}
		}
		if (this.firstTime)
		{
			this.firstTime = false;
			this.lastUpdate = Time.realtimeSinceStartup;
			this.frames = 0;
			return;
		}
		this.frames++;
		float num = Time.realtimeSinceStartup - this.lastUpdate;
		if (num > 0.25f && this.frames > 10)
		{
			this.fps = (float)this.frames / num;
			this.lastUpdate = Time.realtimeSinceStartup;
			this.frames = 0;
		}
	}

	private void CaptureLog(string condition, string stacktrace, LogType type)
	{
		this.AddLog(condition, stacktrace, type);
	}

	private void AddLog(string condition, string stacktrace, LogType type)
	{
		float num = 0f;
		string text = string.Empty;
		if (this.cachedString.ContainsKey(condition))
		{
			text = this.cachedString[condition];
		}
		else
		{
			text = condition;
			this.cachedString.Add(text, text);
			num += (float)((!string.IsNullOrEmpty(text)) ? (text.Length * 2) : 0);
			num += (float)IntPtr.Size;
		}
		string text2 = string.Empty;
		if (this.cachedString.ContainsKey(stacktrace))
		{
			text2 = this.cachedString[stacktrace];
		}
		else
		{
			text2 = stacktrace;
			this.cachedString.Add(text2, text2);
			num += (float)((!string.IsNullOrEmpty(text2)) ? (text2.Length * 2) : 0);
			num += (float)IntPtr.Size;
		}
		bool flag = false;
		this.addSample();
		Reporter.Log log = new Reporter.Log
		{
			logType = (Reporter._LogType)type,
			condition = text,
			stacktrace = text2,
			sampleId = this.samples.Count - 1
		};
		num += log.GetMemoryUsage();
		this.logsMemUsage += num / 1024f / 1024f;
		if (this.TotalMemUsage > this.maxSize)
		{
			this.clear();
			UnityEngine.Debug.Log("Memory Usage Reach" + this.maxSize + " mb So It is Cleared");
			return;
		}
		bool flag2;
		if (this.logsDic.ContainsKey(text, stacktrace))
		{
			flag2 = false;
			this.logsDic[text][stacktrace].count++;
		}
		else
		{
			flag2 = true;
			this.collapsedLogs.Add(log);
			this.logsDic[text][stacktrace] = log;
			if (type == LogType.Log)
			{
				this.numOfCollapsedLogs++;
			}
			else if (type == LogType.Warning)
			{
				this.numOfCollapsedLogsWarning++;
			}
			else
			{
				this.numOfCollapsedLogsError++;
			}
		}
		if (type == LogType.Log)
		{
			this.numOfLogs++;
		}
		else if (type == LogType.Warning)
		{
			this.numOfLogsWarning++;
		}
		else
		{
			this.numOfLogsError++;
		}
		this.logs.Add(log);
		if (!this.collapse || flag2)
		{
			bool flag3 = false;
			if (log.logType == Reporter._LogType.Log && !this.showLog)
			{
				flag3 = true;
			}
			if (log.logType == Reporter._LogType.Warning && !this.showWarning)
			{
				flag3 = true;
			}
			if (log.logType == Reporter._LogType.Error && !this.showError)
			{
				flag3 = true;
			}
			if (log.logType == Reporter._LogType.Assert && !this.showError)
			{
				flag3 = true;
			}
			if (log.logType == Reporter._LogType.Exception && !this.showError)
			{
				flag3 = true;
			}
			if (!flag3 && (string.IsNullOrEmpty(this.filterText) || log.condition.ToLower().Contains(this.filterText.ToLower())))
			{
				this.currentLog.Add(log);
				flag = true;
			}
		}
		if (flag)
		{
			this.calculateStartIndex();
			int count = this.currentLog.Count;
			int num2 = (int)((float)Screen.height * 0.75f / this.size.y);
			if (this.startIndex >= count - num2)
			{
				this.scrollPosition.y = this.scrollPosition.y + this.size.y;
			}
		}
		try
		{
			base.gameObject.SendMessage("OnLog", log);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private void CaptureLogThread(string condition, string stacktrace, LogType type)
	{
		Reporter.Log item = new Reporter.Log
		{
			condition = condition,
			stacktrace = stacktrace,
			logType = (Reporter._LogType)type
		};
		object obj = this.threadedLogs;
		lock (obj)
		{
			this.threadedLogs.Add(item);
		}
	}

	private void OnLevelWasLoaded()
	{
		if (this.clearOnNewSceneLoaded)
		{
			this.clear();
		}
		this.currentScene = SceneManager.GetActiveScene().name;
		UnityEngine.Debug.Log("Scene " + SceneManager.GetActiveScene().name + " is loaded");
	}

	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("Reporter_currentView", (int)this.currentView);
		PlayerPrefs.SetInt("Reporter_show", (!this.show) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_collapse", (!this.collapse) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_clearOnNewSceneLoaded", (!this.clearOnNewSceneLoaded) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showTime", (!this.showTime) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showScene", (!this.showScene) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showMemory", (!this.showMemory) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showFps", (!this.showFps) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showGraph", (!this.showGraph) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showLog", (!this.showLog) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showWarning", (!this.showWarning) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showError", (!this.showError) ? 0 : 1);
		PlayerPrefs.SetString("Reporter_filterText", this.filterText);
		PlayerPrefs.SetFloat("Reporter_size", this.size.x);
		PlayerPrefs.SetInt("Reporter_showClearOnNewSceneLoadedButton", (!this.showClearOnNewSceneLoadedButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showTimeButton", (!this.showTimeButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showSceneButton", (!this.showSceneButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showMemButton", (!this.showMemButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showFpsButton", (!this.showFpsButton) ? 0 : 1);
		PlayerPrefs.SetInt("Reporter_showSearchText", (!this.showSearchText) ? 0 : 1);
		PlayerPrefs.Save();
	}

	private IEnumerator readInfo()
	{
		Reporter._readInfo_c__Iterator0 _readInfo_c__Iterator = new Reporter._readInfo_c__Iterator0();
		_readInfo_c__Iterator._this = this;
		return _readInfo_c__Iterator;
	}
}
