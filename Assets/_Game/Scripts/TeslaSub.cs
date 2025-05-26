using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TeslaSub : MonoBehaviour
{
	public LineRenderer lineRenderer;

	public Transform startPoint;

	public Transform endPoint;

	[Range(0f, 8f)]
	public int generations = 6;

	[Range(0.01f, 1f)]
	public float duration = 0.05f;

	[Range(0f, 1f)]
	public float chaosFactor = 0.15f;

	public bool manualMode;

	private float timer;

	private System.Random randomGenerator = new System.Random();

	private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();

	private int startIndex;

	private Vector2 size;

	private Vector2[] offsets;

	private int animationOffsetIndex;

	private int animationPingPongDirection = 1;

	private bool orthographic;

	[Header("")]
	public BaseUnit victim;

	public Transform hitEffect;

	private RaycastHit2D hit;

	private void Start()
	{
		if (this.lineRenderer == null)
		{
			base.enabled = false;
			return;
		}
		this.orthographic = (Camera.main != null && Camera.main.orthographic);
		this.lineRenderer.positionCount = 0;
	}

	private void OnDisable()
	{
		this.lineRenderer.positionCount = 0;
	}

	private void Update()
	{
		this.orthographic = (Camera.main != null && Camera.main.orthographic);
		if (this.timer <= 0f)
		{
			if (this.manualMode)
			{
				this.timer = this.duration;
				this.lineRenderer.positionCount = 0;
			}
			else
			{
				this.Trigger();
			}
		}
		this.timer -= Time.deltaTime;
	}

	private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
	{
		if (directionNormalized == Vector3.zero)
		{
			side = Vector3.right;
		}
		else
		{
			float x = directionNormalized.x;
			float y = directionNormalized.y;
			float z = directionNormalized.z;
			float num = Mathf.Abs(x);
			float num2 = Mathf.Abs(y);
			float num3 = Mathf.Abs(z);
			float num4;
			float num5;
			float num6;
			if (num >= num2 && num2 >= num3)
			{
				num4 = 1f;
				num5 = 1f;
				num6 = -(y * num4 + z * num5) / x;
			}
			else if (num2 >= num3)
			{
				num6 = 1f;
				num5 = 1f;
				num4 = -(x * num6 + z * num5) / y;
			}
			else
			{
				num6 = 1f;
				num4 = 1f;
				num5 = -(x * num6 + y * num4) / z;
			}
			Vector3 vector = new Vector3(num6, num4, num5);
			side = vector.normalized;
		}
	}

	private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount)
	{
		if (generation < 0 || generation > 8)
		{
			return;
		}
		if (this.orthographic)
		{
			start.z = (end.z = Mathf.Min(start.z, end.z));
		}
		this.segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));
		if (generation == 0)
		{
			return;
		}
		if (offsetAmount <= 0f)
		{
			offsetAmount = (end - start).magnitude * this.chaosFactor;
		}
		while (generation-- > 0)
		{
			int num = this.startIndex;
			this.startIndex = this.segments.Count;
			for (int i = num; i < this.startIndex; i++)
			{
				start = this.segments[i].Key;
				end = this.segments[i].Value;
				Vector3 vector = (start + end) * 0.5f;
				Vector3 b;
				this.RandomVector(ref start, ref end, offsetAmount, out b);
				vector += b;
				this.segments.Add(new KeyValuePair<Vector3, Vector3>(start, vector));
				this.segments.Add(new KeyValuePair<Vector3, Vector3>(vector, end));
			}
			offsetAmount *= 0.5f;
		}
	}

	public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
	{
		if (this.orthographic)
		{
			Vector3 normalized = (end - start).normalized;
			Vector3 a = new Vector3(-normalized.y, normalized.x, normalized.z);
			float d = (float)this.randomGenerator.NextDouble() * offsetAmount * 2f - offsetAmount;
			result = a * d;
		}
		else
		{
			Vector3 normalized2 = (end - start).normalized;
			Vector3 point;
			this.GetPerpendicularVector(ref normalized2, out point);
			float d2 = ((float)this.randomGenerator.NextDouble() + 0.1f) * offsetAmount;
			float angle = (float)this.randomGenerator.NextDouble() * 360f;
			result = Quaternion.AngleAxis(angle, normalized2) * point * d2;
		}
	}

	private void UpdateLineRenderer()
	{
		int num = this.segments.Count - this.startIndex + 1;
		this.lineRenderer.positionCount = num;
		if (num < 1)
		{
			return;
		}
		int num2 = 0;
		this.lineRenderer.SetPosition(num2++, this.segments[this.startIndex].Key);
		for (int i = this.startIndex; i < this.segments.Count; i++)
		{
			this.lineRenderer.SetPosition(num2++, this.segments[i].Value);
		}
		this.segments.Clear();
	}

	public void Trigger()
	{
		if (this.victim)
		{
			if (this.victim.isDead)
			{
				this.Deactive();
				return;
			}
			this.startPoint.position = base.transform.position;
			this.endPoint.position = this.victim.BodyCenterPoint.position;
			this.hitEffect.transform.position = this.endPoint.position;
			this.timer = this.duration + Mathf.Min(0f, this.timer);
			this.startIndex = 0;
			this.GenerateLightningBolt(this.startPoint.position, this.endPoint.position, this.generations, this.generations, 0f);
			this.UpdateLineRenderer();
		}
		else
		{
			this.Deactive();
		}
	}

	public void Active(Vector3 startPoint, BaseUnit victim)
	{
		this.victim = victim;
		this.startPoint.position = startPoint;
		this.endPoint.position = victim.BodyCenterPoint.position;
		base.gameObject.SetActive(true);
	}

	public void Deactive()
	{
		this.victim = null;
		base.gameObject.SetActive(false);
	}

	public void ApplyDamage(AttackData atkData)
	{
		if (this.victim && !this.victim.isDead && !this.victim.isImmortal)
		{
			this.victim.TakeDamage(atkData);
		}
	}
}
