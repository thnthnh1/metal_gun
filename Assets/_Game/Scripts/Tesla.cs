using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Tesla : MonoBehaviour
{
	private sealed class _GetNearestEnemies_c__AnonStorey0
	{
		internal Vector2 posChain;

		internal float __m__0(BaseUnit x)
		{
			return Vector2.Distance(x.BodyCenterPoint.position, this.posChain);
		}
	}

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
	public TeslaSub teslaSubPrefab;

	public Transform hitEffect;

	public LayerMask stopLayerMask;

	[HideInInspector]
	public GunTesla gun;

	public AudioClip soundActive;

	public List<TeslaSub> subs = new List<TeslaSub>();

	private float range = 10f;

	private float timerApplyDamage;

	private BaseUnit victim;

	private RaycastHit2D hit;

	private AudioSource audio;

	private void Awake()
	{
		this.audio = base.GetComponent<AudioSource>();
		this.audio.loop = true;
		this.audio.clip = this.soundActive;
	}

	private void Start()
	{
		if (this.lineRenderer == null)
		{
			base.enabled = false;
			return;
		}
		this.orthographic = (Camera.main != null && Camera.main.orthographic);
		this.lineRenderer.positionCount = 0;
		for (int i = 0; i < 3; i++)
		{
			TeslaSub item = UnityEngine.Object.Instantiate<TeslaSub>(this.teslaSubPrefab, base.transform);
			this.subs.Add(item);
		}
	}

	private void OnDisable()
	{
		this.lineRenderer.positionCount = 0;
		this.victim = null;
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
		Rambo rambo = (Rambo)this.gun.shooter;
		if (rambo.isUsingVerticalUp)
		{
			this.range = Mathf.Abs(Singleton<CameraFollow>.Instance.top.position.y - base.transform.position.y);
		}
		else if (rambo.isUsingVerticalDown)
		{
			if (rambo.isGrounded)
			{
				this.range = ((!rambo.IsFacingRight) ? Mathf.Abs(Singleton<CameraFollow>.Instance.left.position.x - base.transform.position.x) : Mathf.Abs(Singleton<CameraFollow>.Instance.right.position.x - base.transform.position.x));
			}
			else
			{
				this.range = Mathf.Abs(Singleton<CameraFollow>.Instance.bottom.position.y - base.transform.position.y);
			}
		}
		else
		{
			this.range = ((!rambo.IsFacingRight) ? Mathf.Abs(Singleton<CameraFollow>.Instance.left.position.x - base.transform.position.x) : Mathf.Abs(Singleton<CameraFollow>.Instance.right.position.x - base.transform.position.x));
		}
		this.hit = Physics2D.Linecast(base.transform.position, base.transform.position + base.transform.right * this.range, this.stopLayerMask);
		Vector2 v;
		if (this.hit)
		{
			v = this.hit.point;
			if (this.hit.transform.root.CompareTag("Enemy") || this.hit.transform.CompareTag("Enemy Body Part"))
			{
				if (this.hit.transform.CompareTag("Enemy Body Part"))
				{
					this.victim = Singleton<GameController>.Instance.GetUnit(this.hit.transform.gameObject);
				}
				else if (this.hit.transform.root.CompareTag("Enemy"))
				{
					this.victim = Singleton<GameController>.Instance.GetUnit(this.hit.transform.root.gameObject);
				}
				if (this.victim)
				{
					List<BaseUnit> nearestEnemies = this.GetNearestEnemies();
					if (nearestEnemies.Count > 0)
					{
						for (int i = 0; i < this.subs.Count; i++)
						{
							this.subs[i].gameObject.SetActive(i < nearestEnemies.Count);
							if (i < nearestEnemies.Count)
							{
								this.subs[i].Active(this.victim.BodyCenterPoint.position, nearestEnemies[i]);
							}
						}
					}
				}
				else
				{
					this.DeactiveSubTesla();
				}
			}
			else
			{
				this.DeactiveSubTesla();
			}
		}
		else
		{
			this.victim = null;
			v = base.transform.position + base.transform.right * this.range;
			this.DeactiveSubTesla();
		}
		this.startPoint.position = base.transform.position;
		this.endPoint.position = v;
		this.hitEffect.transform.position = this.endPoint.position;
		this.timer = this.duration + Mathf.Min(0f, this.timer);
		this.startIndex = 0;
		this.GenerateLightningBolt(this.startPoint.position, this.endPoint.position, this.generations, this.generations, 0f);
		this.UpdateLineRenderer();
	}

	private void LateUpdate()
	{
		this.ApplyDamage();
	}

	public void Active(bool isActive)
	{
		this.timerApplyDamage = 0f;
		base.gameObject.SetActive(isActive);
		if (isActive)
		{
			this.audio.Play();
		}
		else
		{
			this.audio.Stop();
		}
	}

	private void DeactiveSubTesla()
	{
		for (int i = 0; i < this.subs.Count; i++)
		{
			this.subs[i].Deactive();
		}
	}

	private List<BaseUnit> GetNearestEnemies()
	{
		List<BaseUnit> list = new List<BaseUnit>();
		foreach (BaseUnit current in Singleton<GameController>.Instance.activeUnits.Values)
		{
			float num = base.transform.position.x - current.transform.position.x;
			bool flag = (!((Rambo)this.gun.shooter).IsFacingRight) ? (num >= 0f) : (num < 0f);
			if ((current.CompareTag("Enemy") || current.CompareTag("Enemy Body Part")) && !current.isDead && flag && !object.ReferenceEquals(current.gameObject, this.victim.gameObject))
			{
				bool flag2 = current.transform.position.x < Singleton<CameraFollow>.Instance.left.position.x - 0.5f || (double)current.transform.position.x > (double)Singleton<CameraFollow>.Instance.right.position.x + 0.5;
				bool flag3 = current.transform.position.y < Singleton<CameraFollow>.Instance.bottom.position.y - 0.5f || current.transform.position.y > Singleton<CameraFollow>.Instance.top.position.y + 0.5f;
				if (!flag2 && !flag3)
				{
					list.Add(current);
				}
			}
		}
		int numberEnemyChain = ((SO_GunTeslaStats)this.gun.baseStats).NumberEnemyChain;
		Vector2 posChain = this.victim.BodyCenterPoint.position;
		return (from x in list
		orderby Vector2.Distance(x.BodyCenterPoint.position, posChain)
		select x).Take(numberEnemyChain).ToList<BaseUnit>();
	}

	private void ApplyDamage()
	{
		if (this.victim)
		{
			this.timerApplyDamage += Time.deltaTime;
			if (this.timerApplyDamage >= ((SO_GunTeslaStats)this.gun.baseStats).TimeApplyDamage)
			{
				this.timerApplyDamage = 0f;
				AttackData gunAttackData = ((Rambo)this.gun.shooter).GetGunAttackData();
				this.victim.TakeDamage(gunAttackData);
				for (int i = 0; i < this.subs.Count; i++)
				{
					this.subs[i].ApplyDamage(gunAttackData);
				}
				this.gun.ConsumeAmmo(1);
			}
		}
	}
}
