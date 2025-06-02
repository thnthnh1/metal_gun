// using Facebook.Unity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
// using Facebook;

public class FbController : MonoBehaviour
{
	private sealed class _InitFacebook_c__AnonStorey3
	{
		internal UnityAction callback;

		internal FbController _this;

		internal void __m__0()
		{
			this._this.OnInitComplete(this.callback);
		}
	}

	private sealed class _LoginWithReadPermission_c__AnonStorey4
	{
		internal UnityAction<bool> callback;

		internal FbController _this;

		// internal void __m__0(ILoginResult result)
		// {
		// 	this._this.LoginCallback(result, this.callback);
		// }

		internal void __m__1()
		{
			this._this.LoginWithReadPermission(this.callback);
		}
	}

	private sealed class _LoginWithPublishAction_c__AnonStorey5
	{
		internal UnityAction<bool> callback;

		internal FbController _this;

		// internal void __m__0(ILoginResult result)
		// {
		// 	this._this.LoginCallback(result, this.callback);
		// }

		internal void __m__1()
		{
			this._this.LoginWithPublishAction(this.callback);
		}
	}

	private sealed class _GetLoggedInUserInfomation_c__AnonStorey6
	{
		internal UnityAction<FbUserInfo> callback;

		internal FbController _this;

		// internal void __m__0(IGraphResult result)
		// {
		// 	this._this.GetUserInfoCallback(result, this.callback);
		// }

		internal void __m__1(bool success)
		{
			if (success)
			{
				this._this.GetLoggedInUserInfomation(this.callback);
			}
		}
	}

	private sealed class _ShareScreenshot_c__AnonStorey7
	{
		internal string message;

		internal UnityAction<bool> callback;

		internal FbController _this;

		internal void __m__0(bool loginStatus)
		{
			if (loginStatus)
			{
				this._this.ShareScreenshot(this.message, this.callback);
			}
		}

		// internal void __m__1(IGraphResult result)
		// {
		// 	this._this.ShareScreenshotCallback(result, this.callback);
		// }

		internal void __m__2(bool success)
		{
			if (success)
			{
				this._this.ShareScreenshot(this.message, this.callback);
			}
		}
	}

	private sealed class _ShareImage_c__AnonStorey8
	{
		internal string message;

		internal Texture2D image;

		internal UnityAction<bool> callback;

		internal FbController _this;

		internal void __m__0(bool loginStatus)
		{
			if (loginStatus)
			{
				this._this.ShareImage(this.message, this.image, this.callback);
			}
		}

		// internal void __m__1(IGraphResult result)
		// {
		// 	this._this.ShareScreenshotCallback(result, this.callback);
		// }

		internal void __m__2(bool success)
		{
			if (success)
			{
				this._this.ShareImage(this.message, this.image, this.callback);
			}
		}
	}

	private sealed class _FeedShare_c__AnonStorey9
	{
		internal UnityAction<bool> callback;

		internal string link;

		internal string linkName;

		internal string linkCaption;

		internal string linkDescription;

		internal string linkPicture;

		internal FbController _this;

		// internal void __m__0(IShareResult result)
		// {
		// 	this._this.FeedsShareCallback(result, this.callback);
		// }

		internal void __m__1(bool success)
		{
			if (success)
			{
				this._this.FeedShare(this.link, this.linkName, this.linkCaption, this.linkDescription, this.linkPicture, this.callback);
			}
		}
	}

	private sealed class _ShareLink_c__AnonStoreyA
	{
		internal UnityAction<bool> callback;

		internal string link;

		internal FbController _this;

		// internal void __m__0(IShareResult result)
		// {
		// 	this._this.FeedsShareCallback(result, this.callback);
		// }

		internal void __m__1(bool success)
		{
			if (success)
			{
				this._this.ShareLink(this.link, this.callback);
			}
		}
	}

	private sealed class _GetFriends_c__AnonStoreyB
	{
		internal int numberFriend;

		internal UnityAction<List<FbUserInfo>> callback;

		internal bool justFriendPlayedGame;

		internal FbController _this;

		// internal void __m__0(IGraphResult result)
		// {
		// 	this._this.GetFriendsCallback(this.numberFriend, result, this.callback);
		// }

		internal void __m__1(bool success)
		{
			if (success)
			{
				this._this.GetFriends(this.numberFriend, this.justFriendPlayedGame, this.callback);
			}
		}
	}

	private sealed class _InviteFriends_c__AnonStoreyC
	{
		internal UnityAction<int> callback;

		internal string message;

		internal List<string> friendList;

		internal string data;

		internal FbController _this;

		// internal void __m__0(IAppRequestResult result)
		// {
		// 	this._this.InviteFriendsCallback(result, this.callback);
		// }

		internal void __m__1(bool success)
		{
			if (success)
			{
				this._this.InviteFriends(this.message, this.friendList, this.data, this.callback);
			}
		}
	}

	private sealed class _GetDataFromFriendAppRequest_c__AnonStoreyD
	{
		internal UnityAction<List<FbAppRequestData>> callback;

		internal FbController _this;

		// internal void __m__0(IGraphResult result)
		// {
		// 	this._this.GetDataFromAppRequestCallback(result, this.callback);
		// }

		internal void __m__1(bool success)
		{
			if (success)
			{
				this._this.GetDataFromFriendAppRequest(this.callback);
			}
		}
	}

	private sealed class _DeleteDataFromFriendAppRequest_c__AnonStoreyE
	{
		internal UnityAction<List<string>> callback;

		internal List<FbAppRequestData> requestDataList;

		internal FbController _this;

		// internal void __m__0(IGraphResult result)
		// {
		// 	this._this.DeleteAppRequestCallback(result, this.callback);
		// }

		internal void __m__1(bool success)
		{
			if (success)
			{
				this._this.DeleteDataFromFriendAppRequest(this.requestDataList, this.callback);
			}
		}
	}

	private sealed class _SendDeleteRequest_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal List<string> requestIds;

		internal string _json___0;

		internal string _graph___0;

		internal UnityWebRequest _www___1;

		internal List<string> _deletedIds___2;

		internal UnityAction<List<string>> callback;

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

		public _SendDeleteRequest_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			bool flag = false;
			switch (num)
			{
			case 0u:
				// this._json___0 = JsonConvert.SerializeObject(this.requestIds);
				// this._graph___0 = string.Format("https://graph.facebook.com?ids={0}&method=delete&access_token={1}", this._json___0, AccessToken.CurrentAccessToken.TokenString);
				// this._www___1 = UnityWebRequest.Get(this._graph___0);
				num = 4294967293u;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			try
			{
				switch (num)
				{
				case 1u:
					this._deletedIds___2 = null;
					if (!this._www___1.isNetworkError)
					{
						this._deletedIds___2 = new List<string>();
						try
						{
							Dictionary<string, bool> dictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(this._www___1.downloadHandler.text);
							foreach (KeyValuePair<string, bool> current in dictionary)
							{
								if (current.Value)
								{
									this._deletedIds___2.Add(current.Key.Split(new char[]
									{
										':'
									})[0]);
								}
							}
						}
						catch
						{
						}
					}
					if (this.callback != null)
					{
						this.callback(this._deletedIds___2);
					}
					break;
				default:
					this._current = this._www___1.Send();
					if (!this._disposing)
					{
						this._PC = 1;
					}
					flag = true;
					return true;
				}
			}
			finally
			{
				if (!flag)
				{
					this.____Finally0();
				}
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			uint num = (uint)this._PC;
			this._disposing = true;
			this._PC = -1;
			switch (num)
			{
			case 1u:
				try
				{
				}
				finally
				{
					this.____Finally0();
				}
				break;
			}
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}

		private void ____Finally0()
		{
			if (this._www___1 != null)
			{
				((IDisposable)this._www___1).Dispose();
			}
		}
	}

	private sealed class _AsyncGetProfilePicture_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string imageURL;

		internal UnityWebRequest _www___0;

		internal Sprite _img___0;

		internal UnityAction<Sprite> callback;

		internal FbController _this;

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

		public _AsyncGetProfilePicture_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._www___0 = UnityWebRequestTexture.GetTexture(this.imageURL);
				this._current = this._www___0.Send();
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._img___0 = null;
				if (!this._www___0.isNetworkError)
				{
					Texture2D content = DownloadHandlerTexture.GetContent(this._www___0);
					this._img___0 = Sprite.Create(content, new Rect(0f, 0f, (float)content.width, (float)content.height), Vector2.zero);
					if (this.imageURL.Equals(this._this.LoggedInUserInfo.ProfileImageURL))
					{
						this._this.userProfilePicture = this._img___0;
					}
				}
				if (this.callback != null)
				{
					this.callback(this._img___0);
				}
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

	private sealed class _AsyncGetProfilePicture_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string imageURL;

		internal UnityWebRequest _www___0;

		internal Image avatar;

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

		public _AsyncGetProfilePicture_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._www___0 = UnityWebRequestTexture.GetTexture(this.imageURL);
				this._current = this._www___0.Send();
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (!this._www___0.isNetworkError)
				{
					Texture2D content = DownloadHandlerTexture.GetContent(this._www___0);
					this.avatar.sprite = Sprite.Create(content, new Rect(0f, 0f, (float)content.width, (float)content.height), Vector2.zero);
				}
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

	private sealed class _LoginCallback_c__AnonStoreyF
	{
		internal UnityAction<bool> callback;

		internal void __m__0(FbUserInfo info)
		{
			if (this.callback != null)
			{
				this.callback(true);
			}
		}
	}

	private FbUserInfo _LoggedInUserInfo_k__BackingField;

	private static FbController _instance;

	private Sprite userProfilePicture;

	private bool isSendingDataToFacebook;

	private string profileImgLinkFormat = "https://graph.facebook.com/{0}/picture?width=130&height=130";

	public static FbController Instance
	{
		get
		{
			if (FbController._instance == null)
			{
				GameObject gameObject = new GameObject("FB Controller");
				FbController._instance = gameObject.AddComponent<FbController>();
			}
			return FbController._instance;
		}
		private set
		{
		}
	}

	public FbUserInfo LoggedInUserInfo
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (FbController._instance == null)
		{
			FbController._instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
		this.InitFacebook(null);
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void InitFacebook(UnityAction callback = null)
	{
		// if (!FB.IsInitialized)
		// {
		// 	FB.Init(delegate
		// 	{
		// 		this.OnInitComplete(callback);
		// 	}, new HideUnityDelegate(this.OnHideUnity), null);
		// }
		// else
		// {
		// 	FB.ActivateApp();
		// }
	}

	public void LoginWithReadPermission(UnityAction<bool> callback = null)
	{
		// if (FB.IsInitialized)
		// {
		// 	UnityEngine.Debug.Log("Login Read permition FB");
		// 	List<string> permissions = new List<string>
		// 	{
		// 		"public_profile",
		// 		"email",
		// 		"user_friends"
		// 	};
		// 	FB.LogInWithReadPermissions(permissions, delegate(ILoginResult result)
		// 	{
		// 		UnityEngine.Debug.Log("Login Callback Read permition FB");
		// 		this.LoginCallback(result, callback);
		// 	});
		// }
		// else
		// {
		// 	this.InitFacebook(delegate
		// 	{
		// 		this.LoginWithReadPermission(callback);
		// 	});
		// }
	}

	public void LoginWithPublishAction(UnityAction<bool> callback = null)
	{
		// if (FB.IsInitialized)
		// {
		// 	FB.LogInWithPublishPermissions(new List<string>
		// 	{
		// 		"publish_actions"
		// 	}, delegate(ILoginResult result)
		// 	{
		// 		this.LoginCallback(result, callback);
		// 	});
		// }
		// else
		// {
		// 	this.InitFacebook(delegate
		// 	{
		// 		this.LoginWithPublishAction(callback);
		// 	});
		// }
	}

	public void Logout(UnityAction callback = null)
	{
		// FB.LogOut();
		// this.LoggedInUserInfo = null;
		// this.userProfilePicture = null;
		// PlayerPrefs.DeleteKey("FbUserInfo");
		// if (callback != null)
		// {
		// 	callback();
		// }
	}

	public void GetLoggedInUserInfomation(UnityAction<FbUserInfo> callback)
	{
		// if (FB.IsLoggedIn)
		// {
		// 	if (this.LoggedInUserInfo == null)
		// 	{
		// 		UnityEngine.Debug.Log("Login FB, available info");
		// 		FB.API("/me?fields=name,email,id", HttpMethod.GET, delegate (IGraphResult result) {
		// 			if (string.IsNullOrEmpty(result.Error))
		// 			{
		// 				this.GetUserInfoCallback(result, callback);
		// 				//profile_picture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
		// 			}
		// 		else
		// 		{
		// 			//Debug.LogWarning("received error=" + result.Error);
		// 		}
		// 		});

		// 		//Get Photo
		// 		FB.API("/me/picture?redirect=false", HttpMethod.GET, ProfilePhotoCallback);

		// 		//Get Facebook ID
		// 		Mp_playerSettings.instance.facebookID = Facebook.Unity.AccessToken.CurrentAccessToken.UserId;

		// 		// Mp_RewardsMenu.instance.ClaimRewards();
		// 	}
		// 	else
		// 	{
		// 		callback(this.LoggedInUserInfo);
		// 	}
		// }
		// else
		// {
		// 	this.LoginWithReadPermission(delegate(bool success)
		// 	{
		// 		if (success)
		// 		{
		// 			UnityEngine.Debug.Log("FB Login Success");
		// 			this.GetLoggedInUserInfomation(callback);
		// 		}
		// 	});
		// }
	}
	// private void ProfilePhotoCallback(IGraphResult result)
	// {
	// 	if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
	// 	{
	// 		IDictionary data = result.ResultDictionary["data"] as IDictionary;
	// 		string photoURL = data["url"] as string;

	// 		Mp_playerSettings.instance.facebookPhotoURL = photoURL;
	// 		StartCoroutine(LoadImage(photoURL));
	// 	}
	// }

	public void GetFriendProfileForHighscore(string ID) 
	{
		//FB.API("/" + ID + "?fields=name", HttpMethod.GET, FriendForHighscoreCallback); //old

		//FB.API("/" + ID + "/picture?redirect=false", HttpMethod.GET, FriendForHighscoreCallback);
		// FB.API("/" + ID + "/name?redirect=false", HttpMethod.GET, FriendForHighscoreCallback);
	}

	// public void FriendForHighscoreCallback(IGraphResult result)
	// {
	// 	UnityEngine.Debug.Log("Geting Friend for highscore table");

	// 	UnityEngine.Debug.Log(result);

	// 	//string name = result.ResultDictionary["name"].ToString();
	// 	//string picture = result.ResultDictionary["picture"].ToString();

	// 	//UnityEngine.Debug.Log("name " + name);
	// 	//UnityEngine.Debug.Log("picture " + picture);
	// }

	public void GetLoggedInUserProfilePicture(UnityAction<Sprite> callback)
	{
		if (this.LoggedInUserInfo != null)
		{
			if (this.userProfilePicture == null)
			{
				this.GetProfilePicture(this.LoggedInUserInfo.ProfileImageURL, callback);
			}
			else
			{
				callback(this.userProfilePicture);
			}
		}
	}

	public void GetProfilePicture(string imageURL, UnityAction<Sprite> callback)
	{
		base.StartCoroutine(this.AsyncGetProfilePicture(imageURL, callback));
	}

	public void GetProfilePictureById(string fbId, UnityAction<Sprite> callback)
	{
		string imageURL = string.Format(this.profileImgLinkFormat, fbId);
		this.GetProfilePicture(imageURL, callback);
	}

	public void SetProfilePicture(string imageURL, Image avatar)
	{
		base.StartCoroutine(this.AsyncGetProfilePicture(imageURL, avatar));
	}

	public void SetProfilePictureById(string fbId, Image avatar)
	{
		string imageURL = string.Format(this.profileImgLinkFormat, fbId);
		this.SetProfilePicture(imageURL, avatar);
	}

	public void TakeScreenshotForShare()
	{
		UnityEngine.ScreenCapture.CaptureScreenshot("FbCaptureImage.png");
	}

	public Texture2D GetScreenshot()
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height);
		string path = Application.persistentDataPath + "/FbCaptureImage.png";
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			texture2D.LoadImage(data);
		}
		return texture2D;
	}

	public void ShareScreenshot(string message, UnityAction<bool> callback = null)
	{
		if (this.isSendingDataToFacebook)
		{
			return;
		}
		// if (FB.IsLoggedIn)
		// {
		// 	if (!AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
		// 	{
		// 		this.LoginWithPublishAction(delegate(bool loginStatus)
		// 		{
		// 			if (loginStatus)
		// 			{
		// 				this.ShareScreenshot(message, callback);
		// 			}
		// 		});
		// 	}
		// 	else
		// 	{
		// 		this.isSendingDataToFacebook = true;
		// 		byte[] contents = this.GetScreenshot().EncodeToJPG();
		// 		WWWForm wWWForm = new WWWForm();
		// 		wWWForm.AddBinaryData("image", contents);
		// 		wWWForm.AddField("message", message);
		// 		FB.API("/me/photos", HttpMethod.POST, delegate(IGraphResult result)
		// 		{
		// 			this.ShareScreenshotCallback(result, callback);
		// 		}, wWWForm);
		// 	}
		// }
		// else
		// {
		// 	this.LoginWithReadPermission(delegate(bool success)
		// 	{
		// 		if (success)
		// 		{
		// 			this.ShareScreenshot(message, callback);
		// 		}
		// 	});
		// }
	}

	public void ShareImage(string message, Texture2D image, UnityAction<bool> callback = null)
	{
		if (this.isSendingDataToFacebook || image == null)
		{
			return;
		}
		// if (FB.IsLoggedIn)
		// {
		// 	if (!AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
		// 	{
		// 		this.LoginWithPublishAction(delegate(bool loginStatus)
		// 		{
		// 			if (loginStatus)
		// 			{
		// 				this.ShareImage(message, image, callback);
		// 			}
		// 		});
		// 	}
		// 	else
		// 	{
		// 		this.isSendingDataToFacebook = true;
		// 		byte[] contents = image.EncodeToJPG();
		// 		WWWForm wWWForm = new WWWForm();
		// 		wWWForm.AddBinaryData("image", contents);
		// 		wWWForm.AddField("message", message);
		// 		FB.API("/me/photos", HttpMethod.POST, delegate(IGraphResult result)
		// 		{
		// 			this.ShareScreenshotCallback(result, callback);
		// 		}, wWWForm);
		// 	}
		// }
		// else
		// {
		// 	this.LoginWithReadPermission(delegate(bool success)
		// 	{
		// 		if (success)
		// 		{
		// 			this.ShareImage(message, image, callback);
		// 		}
		// 	});
		// }
	}

	public void FeedShare(string link, string linkName, string linkCaption, string linkDescription, string linkPicture, UnityAction<bool> callback = null)
	{
		// if (FB.IsLoggedIn)
		// {
		// 	Uri link2 = new Uri(link);
		// 	FB.FeedShare(string.Empty, link2, linkName, linkCaption, linkDescription, new Uri(linkPicture), string.Empty, delegate(IShareResult result)
		// 	{
		// 		this.FeedsShareCallback(result, callback);
		// 	});
		// }
		// else
		// {
		// 	this.LoginWithReadPermission(delegate(bool success)
		// 	{
		// 		if (success)
		// 		{
		// 			this.FeedShare(link, linkName, linkCaption, linkDescription, linkPicture, callback);
		// 		}
		// 	});
		// }
	}

	public void ShareLink(string link, UnityAction<bool> callback = null)
	{
		// if (FB.IsLoggedIn)
		// {
		// 	FB.ShareLink(new Uri(link), string.Empty, string.Empty, null, delegate(IShareResult result)
		// 	{
		// 		this.FeedsShareCallback(result, callback);
		// 	});
		// }
		// else
		// {
		// 	this.LoginWithReadPermission(delegate(bool success)
		// 	{
		// 		if (success)
		// 		{
		// 			this.ShareLink(link, callback);
		// 		}
		// 	});
		// }
	}

	public void GetFriends(int numberFriend, bool justFriendPlayedGame, UnityAction<List<FbUserInfo>> callback)
	{
		if (numberFriend > 0)
		{
			// if (FB.IsLoggedIn)
			// {
			// 	string query = (!justFriendPlayedGame) ? "/me/invitable_friends?fields=id,name,picture.width(130).height(130)&limit=1000" : "/me/friends?fields=id,name,picture.width(130).height(130)&limit=1000";

			// 	FB.API(query, HttpMethod.GET, delegate (IGraphResult result) {
			// 		if (string.IsNullOrEmpty(result.Error))
			// 		{
			// 			this.GetFriendsCallback(numberFriend, result, callback);
			// 			//profile_picture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
			// 		}
			// 		else
			// 		{
			// 			//Debug.LogWarning("received error=" + result.Error);
			// 		}
			// 	});
			// }
			// else
			// {
			// 	this.LoginWithReadPermission(delegate(bool success)
			// 	{
			// 		if (success)
			// 		{
			// 			this.GetFriends(numberFriend, justFriendPlayedGame, callback);
			// 		}
			// 	});
			// }
		}
	}

	public void InviteFriends(string message, List<string> friendList, string data = "", UnityAction<int> callback = null)
	{
		// if (FB.IsLoggedIn)
		// {
		// 	string message2 = message;
		// 	List<string> friendList2 = friendList;
		// 	string data2 = data;
		// 	string productName = Application.productName;
		// 	FB.AppRequest(message2, friendList2, null, null, null, data2, productName, delegate(IAppRequestResult result)
		// 	{
		// 		this.InviteFriendsCallback(result, callback);
		// 	});
		// }
		// else
		// {
		// 	this.LoginWithReadPermission(delegate(bool success)
		// 	{
		// 		if (success)
		// 		{
		// 			this.InviteFriends(message, friendList, data, callback);
		// 		}
		// 	});
		// }
	}

	public void SendDataToFriendViaAppRequest(string message, List<string> friendList, string data, UnityAction<int> callback = null)
	{
		this.InviteFriends(message, friendList, data, callback);
	}

	public void GetDataFromFriendAppRequest(UnityAction<List<FbAppRequestData>> callback)
	{
		// if (FB.IsLoggedIn)
		// {
		// 	FB.API("/me/apprequests?fields=id,data,from", HttpMethod.GET, delegate (IGraphResult result) {
		// 		if (string.IsNullOrEmpty(result.Error))
		// 		{
		// 			this.GetDataFromAppRequestCallback(result, callback);
		// 			//profile_picture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
		// 		}
		// 		else
		// 		{
		// 			//Debug.LogWarning("received error=" + result.Error);
		// 		}
		// 	});
		// }
		// else
		// {
		// 	this.LoginWithReadPermission(delegate(bool success)
		// 	{
		// 		if (success)
		// 		{
		// 			this.GetDataFromFriendAppRequest(callback);
		// 		}
		// 	});
		// }
	}

	public void DeleteDataFromFriendAppRequest(List<FbAppRequestData> requestDataList, UnityAction<List<string>> callback = null)
	{
		List<string> list = new List<string>();
		int i = 0;
		int count = requestDataList.Count;
		while (i < count)
		{
			list.Add(requestDataList[i].requestId);
			i++;
		}
		if (list.Count > 0)
		{
			// if (FB.IsLoggedIn)
			// {
			// 	string arg = JsonConvert.SerializeObject(list);
			// 	string query = string.Format("?ids={0}&method=delete", arg);

			// 	FB.API(query, HttpMethod.GET, delegate (IGraphResult result) {
			// 		if (string.IsNullOrEmpty(result.Error))
			// 		{
			// 			this.DeleteAppRequestCallback(result, callback);
			// 			//profile_picture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
			// 		}
			// 		else
			// 		{
			// 			//Debug.LogWarning("received error=" + result.Error);
			// 		}
			// 	});
			// }
			// else
			// {
			// 	this.LoginWithReadPermission(delegate(bool success)
			// 	{
			// 		if (success)
			// 		{
			// 			this.DeleteDataFromFriendAppRequest(requestDataList, callback);
			// 		}
			// 	});
			// }
		}
	}

	private IEnumerator SendDeleteRequest(List<string> requestIds, UnityAction<List<string>> callback)
	{
		FbController._SendDeleteRequest_c__Iterator0 _SendDeleteRequest_c__Iterator = new FbController._SendDeleteRequest_c__Iterator0();
		_SendDeleteRequest_c__Iterator.requestIds = requestIds;
		_SendDeleteRequest_c__Iterator.callback = callback;
		return _SendDeleteRequest_c__Iterator;
	}

	private IEnumerator AsyncGetProfilePicture(string imageURL, UnityAction<Sprite> callback)
	{
		FbController._AsyncGetProfilePicture_c__Iterator1 _AsyncGetProfilePicture_c__Iterator = new FbController._AsyncGetProfilePicture_c__Iterator1();
		_AsyncGetProfilePicture_c__Iterator.imageURL = imageURL;
		_AsyncGetProfilePicture_c__Iterator.callback = callback;
		_AsyncGetProfilePicture_c__Iterator._this = this;
		return _AsyncGetProfilePicture_c__Iterator;
	}

	private IEnumerator AsyncGetProfilePicture(string imageURL, Image avatar)
	{
		FbController._AsyncGetProfilePicture_c__Iterator2 _AsyncGetProfilePicture_c__Iterator = new FbController._AsyncGetProfilePicture_c__Iterator2();
		_AsyncGetProfilePicture_c__Iterator.imageURL = imageURL;
		_AsyncGetProfilePicture_c__Iterator.avatar = avatar;
		return _AsyncGetProfilePicture_c__Iterator;
	}

	private void OnHideUnity(bool isUnityShown)
	{
	}

	private void OnInitComplete(UnityAction callback)
	{
		// if (FB.IsInitialized)
		// {
		// 	FB.ActivateApp();
		// 	UnityEngine.Debug.Log("LB In FB Is Init");
		// }
		// if (FB.IsLoggedIn)
		// {
		// 	string @string = PlayerPrefs.GetString("FbUserInfo", string.Empty);
		// 	if (string.IsNullOrEmpty(@string))
		// 	{
		// 		this.LoggedInUserInfo = null;
		// 		this.GetLoggedInUserInfomation(null);
		// 	}
		// 	else
		// 	{
		// 		this.LoggedInUserInfo = JsonConvert.DeserializeObject<FbUserInfo>(@string);
		// 	}
		// }
		// if (callback != null)
		// {
		// 	callback();
		// }
	}

	// private void LoginCallback(ILoginResult result, UnityAction<bool> callback)
	// {
	// 	UnityEngine.Debug.Log("Try Login FB");
	// 	if (!result.Cancelled && string.IsNullOrEmpty(result.Error))
	// 	{
	// 		if (this.LoggedInUserInfo == null)
	// 		{
	// 			this.GetLoggedInUserInfomation(delegate(FbUserInfo info)
	// 			{
	// 				if (callback != null)
	// 				{
	// 					UnityEngine.Debug.Log("Login FB, true");
	// 					callback(true);
	// 				}
	// 			});
	// 		}
	// 		else if (callback != null)
	// 		{
	// 			callback(true);
	// 		}
	// 	}
	// 	else if (result.Error != null && result.Error.Contains("logged in as different Facebook user"))
	// 	{
	// 		UnityEngine.Debug.Log("Login FB, Error");
	// 		this.Logout(null);
	// 		this.LoginWithReadPermission(callback);
	// 	}
	// 	else if (callback != null)
	// 	{
	// 		UnityEngine.Debug.Log("Login FB, false");
	// 		callback(false);
	// 	}
	// }

	// private void GetUserInfoCallback(IGraphResult result, UnityAction<FbUserInfo> callback)
	// {
	// 	if (!result.Cancelled && string.IsNullOrEmpty(result.Error))
	// 	{
	// 		string text = result.ResultDictionary["id"].ToString();
	// 		string name = result.ResultDictionary["name"].ToString();
	// 		string email = string.Empty;
	// 		string profileImageURL = string.Format(this.profileImgLinkFormat, text);

	// 		Mp_playerSettings.instance.facebookName = name;
	// 		Mp_playerSettings.instance.facebookEmail = email;

	// 		if (result.ResultDictionary.ContainsKey("email"))
	// 		{
	// 			email = result.ResultDictionary["email"].ToString();
	// 		}
	// 		this.LoggedInUserInfo = new FbUserInfo(text, name, email, profileImageURL);
	// 		string value = JsonConvert.SerializeObject(this.LoggedInUserInfo);
	// 		PlayerPrefs.SetString("FbUserInfo", value);
	// 		PlayerPrefs.Save();
			
	// 		if (callback != null)
	// 		{
	// 			callback(this.LoggedInUserInfo);
	// 		}
	// 	}
	// 	else if (callback != null)
	// 	{
	// 		callback(null);
	// 	}
	// }

	/*public IEnumerator LoadImage(string url) 
	{
		WWW www = new WWW(url);
		yield return www;
		HudTournamentRanking.instance.imgPlayerAvatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

		// FireBase Remove
		//write data in firebase
		// ControllerFirebase.controllerFirebase.WriteFaceBookInfoInDatabase();
	}*/

	// private void ShareScreenshotCallback(IGraphResult result, UnityAction<bool> callback)
	// {
	// 	if (!result.Cancelled && string.IsNullOrEmpty(result.Error))
	// 	{
	// 		if (callback != null)
	// 		{
	// 			callback(true);
	// 		}
	// 	}
	// 	else if (callback != null)
	// 	{
	// 		callback(false);
	// 	}
	// 	this.isSendingDataToFacebook = false;
	// }

	// private void FeedsShareCallback(IShareResult result, UnityAction<bool> callback)
	// {
	// 	if (!result.Cancelled && string.IsNullOrEmpty(result.Error))
	// 	{
	// 		if (callback != null)
	// 		{
	// 			callback(true);
	// 		}
	// 	}
	// 	else if (callback != null)
	// 	{
	// 		callback(false);
	// 	}
	// }

	// private void GetFriendsCallback(int numberFriendInvite, IGraphResult result, UnityAction<List<FbUserInfo>> callback)
	// {
	// 	List<FbUserInfo> list = new List<FbUserInfo>();
	// 	if (!result.Cancelled && string.IsNullOrEmpty(result.Error))
	// 	{
	// 		List<object> list2 = result.ResultDictionary["data"] as List<object>;
	// 		int num = 0;
	// 		int num2;
	// 		if (list2.Count > numberFriendInvite)
	// 		{
	// 			num = UnityEngine.Random.Range(0, list2.Count - numberFriendInvite);
	// 			num2 = num + numberFriendInvite;
	// 		}
	// 		else if (list2.Count == numberFriendInvite)
	// 		{
	// 			num2 = numberFriendInvite;
	// 		}
	// 		else
	// 		{
	// 			num2 = list2.Count;
	// 		}
	// 		for (int i = num; i < num2; i++)
	// 		{
	// 			Dictionary<string, object> dictionary = list2[i] as Dictionary<string, object>;
	// 			string id = dictionary["id"].ToString();
	// 			string name = dictionary["name"].ToString();
	// 			string profileImageURL = string.Empty;
	// 			Dictionary<string, object> dictionary2 = dictionary["picture"] as Dictionary<string, object>;
	// 			Dictionary<string, object> dictionary3 = dictionary2["data"] as Dictionary<string, object>;
	// 			profileImageURL = dictionary3["url"].ToString();
	// 			list.Add(new FbUserInfo(id, name, string.Empty, profileImageURL));
	// 		}
	// 	}
	// 	if (callback != null)
	// 	{
	// 		callback(list);
	// 	}
	// }

	// private void InviteFriendsCallback(IAppRequestResult result, UnityAction<int> callback)
	// {
	// 	if (!result.Cancelled && string.IsNullOrEmpty(result.Error))
	// 	{
	// 		if (callback != null && result.ResultDictionary.ContainsKey("to"))
	// 		{
	// 			string text = result.ResultDictionary["to"] as string;
	// 			string[] array = text.Split(new char[]
	// 			{
	// 				','
	// 			});
	// 			callback(array.Length);
	// 		}
	// 	}
	// 	else if (callback != null)
	// 	{
	// 		callback(0);
	// 	}
	// }

	// private void GetDataFromAppRequestCallback(IGraphResult result, UnityAction<List<FbAppRequestData>> callback)
	// {
	// 	List<FbAppRequestData> list = new List<FbAppRequestData>();
	// 	if (!result.Cancelled && string.IsNullOrEmpty(result.Error))
	// 	{
	// 		List<object> list2 = result.ResultDictionary["data"] as List<object>;
	// 		int i = 0;
	// 		int count = list2.Count;
	// 		while (i < count)
	// 		{
	// 			Dictionary<string, object> dictionary = list2[i] as Dictionary<string, object>;
	// 			if (dictionary.ContainsKey("data"))
	// 			{
	// 				string requestId = dictionary["id"].ToString();
	// 				string data = dictionary["data"].ToString();
	// 				Dictionary<string, object> dictionary2 = dictionary["from"] as Dictionary<string, object>;
	// 				string senderId = dictionary2["id"].ToString();
	// 				string senderName = dictionary2["name"].ToString();
	// 				FbAppRequestData item = new FbAppRequestData(requestId, data, senderId, senderName);
	// 				list.Add(item);
	// 			}
	// 			i++;
	// 		}
	// 	}
	// 	if (callback != null)
	// 	{
	// 		callback(list);
	// 	}
	// }

	// private void DeleteAppRequestCallback(IGraphResult result, UnityAction<List<string>> callback)
	// {
	// 	List<string> list = new List<string>();
	// 	if (!result.Cancelled && string.IsNullOrEmpty(result.Error))
	// 	{
	// 		foreach (KeyValuePair<string, object> current in result.ResultDictionary)
	// 		{
	// 			if ((bool)current.Value)
	// 			{
	// 				list.Add(current.Key.Split(new char[]
	// 				{
	// 					':'
	// 				})[0]);
	// 			}
	// 		}
	// 	}
	// 	if (callback != null)
	// 	{
	// 		callback(list);
	// 	}
	// }
}