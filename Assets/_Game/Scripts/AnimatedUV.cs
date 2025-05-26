using System;
using UnityEngine;

public class AnimatedUV : MonoBehaviour
{
	public Vector2 offsetSpeed;

	private Vector2 defaultUVOffset;

	private Material mat;

	private float limitOffsetX;

	private float limitOffsetY;

	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().sharedMaterial;
		if (this.mat == null)
		{
			base.enabled = false;
			return;
		}
		this.defaultUVOffset = this.mat.mainTextureOffset;
		this.limitOffsetX = this.defaultUVOffset.x + 10f;
		this.limitOffsetY = this.defaultUVOffset.y + 10f;
	}

	private void Update()
	{
		Vector2 vector = this.mat.mainTextureOffset;
		vector += this.offsetSpeed * Time.deltaTime;
		if (vector.x >= this.limitOffsetX || vector.x <= -this.limitOffsetX)
		{
			vector.x = this.defaultUVOffset.x;
		}
		if (vector.y >= this.limitOffsetY || vector.y <= -this.limitOffsetY)
		{
			vector.y = this.defaultUVOffset.y;
		}
		this.mat.mainTextureOffset = vector;
	}

	private void OnDisable()
	{
		if (this.mat != null)
		{
			this.mat.mainTextureOffset = this.defaultUVOffset;
		}
	}
}
