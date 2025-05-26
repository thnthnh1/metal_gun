using System;
using UnityEngine;

public class SniperLaser : MonoBehaviour
{
	public LineRenderer laserRender;

	public LayerMask collisionMask;

	public float laserWidth = 0.3f;

	public float laserRange = 10f;

	private RaycastHit2D hit;

	private void Start()
	{
		this.laserRender.startWidth = this.laserWidth;
		this.laserRender.endWidth = this.laserWidth;
	}

	private void LateUpdate()
	{
		if (this.laserRender != null)
		{
			this.hit = Physics2D.Linecast(base.transform.position, base.transform.position + base.transform.right * this.laserRange, this.collisionMask);
			if (this.hit)
			{
				this.laserRender.SetPosition(0, base.transform.position);
				Vector3 position = this.hit.point;
				this.laserRender.SetPosition(1, position);
			}
			else
			{
				this.laserRender.SetPosition(0, base.transform.position);
				Vector3 position = base.transform.position + base.transform.right * this.laserRange;
				this.laserRender.SetPosition(1, position);
			}
		}
	}
}
