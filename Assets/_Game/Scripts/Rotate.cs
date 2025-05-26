using System;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	private Vector3 angle;

	private void Start()
	{
		this.angle = base.transform.eulerAngles;
	}

	private void Update()
	{
		this.angle.y = this.angle.y + Time.deltaTime * 100f;
		base.transform.eulerAngles = this.angle;
	}
}
