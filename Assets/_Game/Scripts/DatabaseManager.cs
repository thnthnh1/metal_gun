using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
	public InputField inputPlayerName;

	private static UnityAction<bool> __f__am_cache0;

	private static UnityAction<UserInfo> __f__am_cache1;

	private static UnityAction<UserInfo> __f__am_cache2;

	private void Start()
	{
        /*
		FbController.Instance.LoginWithReadPermission(delegate(bool success)
		{
			if (success)
			{
				Singleton<FireBaseDatabase>.Instance.AuthenWithFacebook(AccessToken.CurrentAccessToken.UserId, AccessToken.CurrentAccessToken.TokenString, delegate(UserInfo callback)
				{
					UnityEngine.Debug.Log(callback.ToString());
				});
			}
		});
		*/
	}

	public void SearchPlayerByName()
	{
		if (!string.IsNullOrEmpty(this.inputPlayerName.text))
		{
            /*
			Singleton<FireBaseDatabase>.Instance.SearUserByName(this.inputPlayerName.text, delegate(UserInfo user)
			{
				UnityEngine.Debug.Log(user.ToString());
			});
			*/
		}
	}
}
