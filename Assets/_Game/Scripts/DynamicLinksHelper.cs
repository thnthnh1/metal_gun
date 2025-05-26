//using Firebase.DynamicLinks;
using System;
using UnityEngine;

public class DynamicLinksHelper : Singleton<DynamicLinksHelper>
{
	private void Awake()
	{
		//DynamicLinks.DynamicLinkReceived += new EventHandler<ReceivedDynamicLinkEventArgs>(this.OnDynamicLinkReceived);
		UnityEngine.Object.DontDestroyOnLoad(this);
	}
    /*
	private void OnDynamicLinkReceived(object sender, ReceivedDynamicLinkEventArgs e)
	{
	}
	*/  
}
