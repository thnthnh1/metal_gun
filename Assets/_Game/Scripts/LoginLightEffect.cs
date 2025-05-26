using System;
using UnityEngine;

public class LoginLightEffect : MonoBehaviour
{
	public Animation anim;

	private float timer;

	private void Update()
	{
		this.timer += Time.deltaTime;
		if (this.timer >= 5f)
		{
			this.timer = 0f;
			this.anim.Play();
		}
	}
}
