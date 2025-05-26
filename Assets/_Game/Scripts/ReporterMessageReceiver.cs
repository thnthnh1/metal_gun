using System;
using UnityEngine;

public class ReporterMessageReceiver : MonoBehaviour
{
	private Reporter reporter;

	private void Start()
	{
		this.reporter = base.gameObject.GetComponent<Reporter>();
	}

	private void OnPreStart()
	{
		if (this.reporter == null)
		{
			this.reporter = base.gameObject.GetComponent<Reporter>();
		}
		if (Screen.width < 1000)
		{
			this.reporter.size = new Vector2(32f, 32f);
		}
		else
		{
			this.reporter.size = new Vector2(48f, 48f);
		}
		this.reporter.UserData = "Put user date here like his account to know which user is playing on this device";
	}

	private void OnHideReporter()
	{
	}

	private void OnShowReporter()
	{
	}

	private void OnLog(Reporter.Log log)
	{
	}
}
