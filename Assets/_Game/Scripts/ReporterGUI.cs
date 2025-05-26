using System;
using UnityEngine;

public class ReporterGUI : MonoBehaviour
{
	private Reporter reporter;

	private void Awake()
	{
		this.reporter = base.gameObject.GetComponent<Reporter>();
	}

	private void OnGUI()
	{
		this.reporter.OnGUIDraw();
	}
}
