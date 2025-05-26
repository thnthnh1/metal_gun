using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraFollow : Singleton<CameraFollow>
{
	[Header("PADDING")]
	public float paddingHorizontal;

	public float paddingVertical;

	[Header("MARGINS")]
	public Transform left;

	public Transform right;

	public Transform top;

	public Transform bottom;

	[Header("TARGET"), SerializeField]
	private Transform target;

	public float followSpeed = 20f;

	[Header("SLOW MOTION")]
	public TiltShift lensBlur;

	public SlowMotion slowMotion;

	[Space(20f)]
	public Transform pointAirSpawnLeft;

	public Transform pointAirSpawnRight;

	public Transform[] patrolPoints;

	public LayerMask layerCheckSpawnGroundEnemy;

	[HideInInspector]
	public Vector2 pointGroundSpawnLeft;

	[HideInInspector]
	public Vector2 pointGroundSpawnRight;

	public Camera _camera;

	private Grayscale grayScale;

	private float marginLeft = -1000f;

	private float marginRight = 1000f;

	private float marginTop = 1000f;

	private float marginBottom = -1000f;

	private float horzExtent;

	private float vertExtent;

	private float shakeAmount;

	private float shakeDelta;

	private bool flagMoveToNewZone;

	private float nextZoneX;

	private int tmpZoneId;

	private bool flagSlowMotion;

	private void Awake()
	{
		this.grayScale = base.GetComponent<Grayscale>();
		this.SetGrayScaleEffect(false);
		EventDispatcher.Instance.RegisterListener(EventID.MoveCameraToNewZone, new Action<Component, object>(this.MoveCameraToNewZone));
	}

	private void LateUpdate()
	{
		if (this.flagMoveToNewZone)
		{
			Vector3 position = base.transform.position;
			position.x = Mathf.MoveTowards(position.x, this.nextZoneX, 10f * Time.deltaTime);
			base.transform.position = position;
			if (position.x == this.nextZoneX)
			{
				this.flagMoveToNewZone = false;
				EventDispatcher.Instance.PostEvent(EventID.ClearZone, this.tmpZoneId);
			}
		}
		else
		{
			if (this.target != null)
			{
				this.FollowTarget();
			}
			if (this.shakeAmount != 0f)
			{
				this.Shake();
			}
		}
	}

	public void SetCameraSize(float size)
	{
		this._camera.orthographicSize = size;
		this.horzExtent = this._camera.orthographicSize * (float)Screen.width / (float)Screen.height;
		this.vertExtent = this._camera.orthographicSize;
		this.InitMargin();
	}

	public float GetCameraSize()
	{
		return this._camera.orthographicSize;
	}

	public void SetInitialPoint(Transform point)
	{
		if (point)
		{
			Vector3 position = point.position;
			position.z = -10f;
			base.transform.position = position;
		}
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void SetSlowMotion()
	{
		this.flagSlowMotion = true;
	}

	public void ResetCameraToPlayer()
	{
		this.flagSlowMotion = false;
		this.SetTarget(Singleton<GameController>.Instance.Player.transform);
		this._camera.orthographicSize = 4f;
	}

	public void AddShake(float shakeAmount, float duration)
	{
		this.shakeDelta = shakeAmount / duration;
		this.shakeAmount = shakeAmount;
	}

	public void SetMarginLeft(float margin)
	{
		this.marginLeft = margin;
	}

	public void SetMarginRight(float margin)
	{
		this.marginRight = margin;
	}

	public void SetMarginTop(float margin)
	{
		this.marginTop = margin;
	}

	public void SetMarginBottom(float margin)
	{
		this.marginBottom = margin;
	}

	public void SetGrayScaleEffect(bool isOn)
	{
	}

	private void InitMargin()
	{
		Vector2 zero = Vector2.zero;
		zero.y = this._camera.orthographicSize;
		this.top.transform.localPosition = zero;
		this.bottom.transform.localPosition = -this.top.transform.localPosition;
		zero = Vector2.zero;
		zero.x = this.horzExtent;
		this.right.transform.localPosition = zero;
		this.left.transform.localPosition = -this.right.transform.localPosition;
		Vector2 v = this.left.transform.position;
		v.x -= 2f;
		v.y = this.top.transform.position.y;
		v.y -= 2f;
		this.pointAirSpawnLeft.position = v;
		v.x *= -1f;
		this.pointAirSpawnRight.position = v;
	}

	private void MoveCameraToNewZone(Component sender, object param)
	{
		this.flagMoveToNewZone = true;
		this.tmpZoneId = (int)param;
		this.nextZoneX = Singleton<GameController>.Instance.Player.transform.position.x;
	}

	private void FollowTarget()
	{
		Vector3 position = this.target.transform.position;
		position.z = -10f;
		position.x += this.paddingHorizontal;
		position.y += this.paddingVertical;
		if (position.x - this.horzExtent < this.marginLeft)
		{
			position.x = this.marginLeft + this.horzExtent;
		}
		if (position.x + this.horzExtent > this.marginRight)
		{
			position.x = this.marginRight - this.horzExtent;
		}
		if (position.y + this.vertExtent > this.marginTop)
		{
			position.y = this.marginTop - this.vertExtent;
		}
		if (position.y - this.vertExtent < this.marginBottom)
		{
			position.y = this.marginBottom + this.vertExtent;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, position, this.followSpeed * Time.deltaTime);
	}

	private void Shake()
	{
		Vector3 position = this.target.transform.position;
		position.z = -10f;
		position.x += this.paddingHorizontal;
		position.y += this.paddingVertical;
		this.shakeAmount = Mathf.MoveTowards(this.shakeAmount, 0f, this.shakeDelta * Time.deltaTime);
		position.x += UnityEngine.Random.Range(-this.shakeAmount, this.shakeAmount);
		position.y += UnityEngine.Random.Range(-this.shakeAmount, this.shakeAmount);
		if (position.x - this.horzExtent < this.marginLeft)
		{
			position.x = this.marginLeft + this.horzExtent;
		}
		if (position.x + this.horzExtent > this.marginRight)
		{
			position.x = this.marginRight - this.horzExtent;
		}
		if (position.y + this.vertExtent > this.marginTop)
		{
			position.y = this.marginTop - this.vertExtent;
		}
		if (position.y - this.vertExtent < this.marginBottom)
		{
			position.y = this.marginBottom + this.vertExtent;
		}
		base.transform.position = position;
	}

	public bool IsCanSpawnGroundEnemyFromLeft()
	{
		this.GetPointGroundSpawnLeft();
		bool flag = !Physics2D.Linecast(this.pointGroundSpawnLeft, this.left.position, this.layerCheckSpawnGroundEnemy);
		Vector2 end = this.pointGroundSpawnLeft;
		end.y -= 3f;
		bool flag2 = Physics2D.Linecast(this.pointGroundSpawnLeft, end, this.layerCheckSpawnGroundEnemy);
		return flag && flag2;
	}

	public bool IsCanSpawnGroundEnemyFromRight()
	{
		this.GetPointGroundSpawnRight();
		bool flag = !Physics2D.Linecast(this.pointGroundSpawnRight, this.right.position, this.layerCheckSpawnGroundEnemy);
		Vector2 end = this.pointGroundSpawnRight;
		end.y -= 3f;
		bool flag2 = Physics2D.Linecast(this.pointGroundSpawnRight, end, this.layerCheckSpawnGroundEnemy);
		return flag && flag2;
	}

	private void GetPointGroundSpawnLeft()
	{
		Vector2 vector = this.left.position;
		vector.x -= 2f;
		this.pointGroundSpawnLeft = vector;
	}

	private void GetPointGroundSpawnRight()
	{
		Vector2 vector = this.right.position;
		vector.x += 2f;
		this.pointGroundSpawnRight = vector;
	}

	public Vector2 GetNextDestination(EnemyHelicopter helicopter)
	{
		helicopter.indexMove++;
		if (helicopter.indexMove > this.patrolPoints.Length - 1)
		{
			helicopter.indexMove = 0;
		}
		return this.patrolPoints[helicopter.indexMove].position;
	}
}
