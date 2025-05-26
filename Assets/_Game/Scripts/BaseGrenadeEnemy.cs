using System;
using UnityEngine;

public class BaseGrenadeEnemy : MonoBehaviour
{
	public Transform groundCheck;

	public LayerMask layerVictim;

	protected bool isExploding;

	protected AttackData attackData;

	protected Collider2D[] victims = new Collider2D[10];

	private Rigidbody2D rigid;

	private void Awake()
	{
		this.rigid = base.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		this.TrackOutOfScreen();
	}

	protected virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.transform.root.CompareTag("Enemy") && !this.isExploding)
		{
			this.isExploding = true;
			this.Explode();
		}
	}

	public virtual void Active(AttackData attackData, Vector3 startPoint, Vector3 endPoint, Vector2 throwDirection, Transform parent = null)
	{
		this.attackData = attackData;
		this.isExploding = false;
		base.transform.position = startPoint;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
		Vector3 vector = endPoint - startPoint;
		vector = MathUtils.ProjectVectorOnPlane(Vector3.up, vector);
		float num = Vector2.Angle((throwDirection.x <= 0f) ? Vector3.left : Vector3.right, throwDirection);
		float yOffset = -vector.y;
		float d = MathUtils.CalculateLaunchSpeed(vector.magnitude, yOffset, Physics2D.gravity.magnitude, num * 0.0174532924f);
		this.rigid.velocity = throwDirection * d;
	}

	public virtual void Deactive()
	{
		base.CancelInvoke();
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
		Singleton<PoolingController>.Instance.poolBaseGrenadeEnemy.Store(this);
	}

	public virtual void Explode()
	{
		int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.attackData.radiusDealDamage, this.victims, this.layerVictim);
		for (int i = 0; i < num; i++)
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(this.victims[i].transform.root.gameObject);
			if (unit != null && unit.CompareTag("Player"))
			{
				float num2 = Vector3.Distance(base.transform.position, unit.BodyCenterPoint.position);
				float num3 = Mathf.Clamp01((num2 - 0.5f) / (this.attackData.radiusDealDamage - 0.5f));
				float num4 = 1f - num3 * 0.4f;
				this.attackData.damage *= num4;
				unit.TakeDamage(this.attackData);
			}
		}
		this.Deactive();
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, base.transform.position);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.35f);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	private void TrackOutOfScreen()
	{
		bool flag = base.transform.position.x < Singleton<CameraFollow>.Instance.left.position.x || base.transform.position.x > Singleton<CameraFollow>.Instance.right.position.x;
		bool flag2 = base.transform.position.y < Singleton<CameraFollow>.Instance.bottom.position.y || base.transform.position.y > Singleton<CameraFollow>.Instance.top.position.y;
		if (flag || flag2)
		{
			this.Deactive();
		}
	}
}
