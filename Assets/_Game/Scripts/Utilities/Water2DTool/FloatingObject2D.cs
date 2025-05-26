using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Water2DTool
{
	public class FloatingObject2D : FloatingObject
	{
		private enum ColliderType
		{
			Unknown,
			BoxCollider2D,
			PolygonCollider2D,
			CircleCollider2D,
			CapsuleCollider2D
		}

		private int pCorners;

		private int prevYAxisDirection = 1;

		private bool playerCollider;

		private bool soundEffectPlayed;

		private bool particleSystemInstantiated;

		private Vector3 previousPosition;

		private Collider2D collider;

		private Rigidbody2D rigidbody;

		private FloatingObject2D.ColliderType colliderType;

		private List<Vector2> polygonPoints;

		private Water2D_PolygonClipping polygonCliping;

		public override Transform transform
		{
			get
			{
				return this.collider.transform;
			}
		}

		public override Bounds bounds
		{
			get
			{
				return this.collider.bounds;
			}
		}

		public FloatingObject2D(Collider2D col, int polygonCorners, bool player)
		{
			this.collider = col;
			this.pCorners = polygonCorners;
			this.playerCollider = player;
			this.rigidbody = col.GetComponent<Rigidbody2D>();
			this.polygonCliping = new Water2D_PolygonClipping();
			if (col is BoxCollider2D)
			{
				this.colliderType = FloatingObject2D.ColliderType.BoxCollider2D;
			}
			if (col is PolygonCollider2D)
			{
				this.colliderType = FloatingObject2D.ColliderType.PolygonCollider2D;
			}
			if (col is CircleCollider2D)
			{
				this.colliderType = FloatingObject2D.ColliderType.CircleCollider2D;
			}
			if (col is CapsuleCollider2D)
			{
				this.colliderType = FloatingObject2D.ColliderType.CapsuleCollider2D;
			}
			if (this.colliderType == FloatingObject2D.ColliderType.BoxCollider2D)
			{
				this.polygonPoints = new List<Vector2>();
				for (int i = 0; i < 4; i++)
				{
					this.polygonPoints.Add(Vector2.zero);
				}
			}
			else if (this.colliderType == FloatingObject2D.ColliderType.CircleCollider2D)
			{
				this.polygonPoints = new List<Vector2>(polygonCorners);
				for (int j = 0; j < polygonCorners; j++)
				{
					this.polygonPoints.Add(Vector2.zero);
				}
			}
			else
			{
				this.polygonPoints = new List<Vector2>();
			}
			if (col is CapsuleCollider2D)
			{
				this.colliderType = FloatingObject2D.ColliderType.CapsuleCollider2D;
				if (!this.IsCircle(col as CapsuleCollider2D))
				{
					this.pCorners += 2;
					if (this.pCorners % 2 != 0)
					{
						this.pCorners++;
					}
				}
				for (int k = 0; k < this.pCorners; k++)
				{
					this.polygonPoints.Add(Vector2.zero);
				}
			}
		}

		private bool IsCircle(CapsuleCollider2D capsule2D)
		{
			bool result = false;
			Vector2 vector = new Vector2(capsule2D.size.x * capsule2D.transform.localScale.x, capsule2D.size.y * capsule2D.transform.localScale.y);
			if (capsule2D.direction == CapsuleDirection2D.Horizontal)
			{
				if (vector.y > vector.x)
				{
					result = true;
				}
			}
			else if (vector.x > vector.y)
			{
				result = true;
			}
			return result;
		}

		public override List<Vector2> GetPolygon()
		{
			switch (this.colliderType)
			{
			case FloatingObject2D.ColliderType.BoxCollider2D:
				return this.GetBoxVerticesWorldPosition();
			case FloatingObject2D.ColliderType.PolygonCollider2D:
				return this.GetPolygonCollider2DPoints();
			case FloatingObject2D.ColliderType.CircleCollider2D:
				return this.GetPolygonVerticesWorldPosition();
			case FloatingObject2D.ColliderType.CapsuleCollider2D:
				return this.GetCapsuleVerticesWorldPosition();
			default:
				return null;
			}
		}

		public override float GetRadius()
		{
			switch (this.colliderType)
			{
			case FloatingObject2D.ColliderType.BoxCollider2D:
				return this.GetRadiusFromBox();
			case FloatingObject2D.ColliderType.PolygonCollider2D:
				return this.GetRadiusFromPolygon();
			case FloatingObject2D.ColliderType.CircleCollider2D:
				return this.GetRadiusFromCircle();
			default:
				return 0f;
			}
		}

		private float GetRadiusFromBox()
		{
			return this.collider.bounds.extents.x;
		}

		private float GetRadiusFromCircle()
		{
			CircleCollider2D circleCollider2D = this.collider as CircleCollider2D;
			return circleCollider2D.radius;
		}

		private float GetRadiusFromPolygon()
		{
			return this.collider.bounds.extents.x;
		}

		private List<Vector2> GetBoxVerticesWorldPosition()
		{
			float f = this.collider.transform.eulerAngles.z * 0.0174532924f;
			float z = this.collider.transform.eulerAngles.z;
			Vector2 vector = this.collider.bounds.min;
			Vector2 vector2 = this.collider.bounds.max;
			Vector3 center = this.collider.bounds.center;
			Vector2 vector3 = this.collider.bounds.extents;
			if (z == 0f || z == 90f || z == 180f || z == 270f || z == 360f)
			{
				this.polygonPoints[0] = new Vector2(vector.x, center.y + vector3.y);
				this.polygonPoints[1] = new Vector2(vector.x, center.y - vector3.y);
				this.polygonPoints[2] = new Vector2(vector2.x, center.y - vector3.y);
				this.polygonPoints[3] = new Vector2(vector2.x, center.y + vector3.y);
			}
			else
			{
				float num = (this.collider as BoxCollider2D).size.x * this.collider.transform.localScale.x * 0.5f;
				float num2 = (this.collider as BoxCollider2D).size.y * this.collider.transform.localScale.y * 0.5f;
				this.polygonPoints[0] = new Vector2(center.x - num, center.y + num2);
				this.polygonPoints[1] = new Vector2(center.x - num, center.y - num2);
				this.polygonPoints[2] = new Vector2(center.x + num, center.y - num2);
				this.polygonPoints[3] = new Vector2(center.x + num, center.y + num2);
				for (int i = 0; i < 4; i++)
				{
					this.polygonPoints[i] = new Vector2((this.polygonPoints[i].x - center.x) * Mathf.Cos(f) - (this.polygonPoints[i].y - center.y) * Mathf.Sin(f) + center.x, (this.polygonPoints[i].x - center.x) * Mathf.Sin(f) + (this.polygonPoints[i].y - center.y) * Mathf.Cos(f) + center.y);
				}
			}
			return this.polygonPoints;
		}

		private List<Vector2> GetPolygonVerticesWorldPosition()
		{
			Vector3 center = this.collider.bounds.center;
			CircleCollider2D circleCollider2D = this.collider as CircleCollider2D;
			float num = 0.125f + circleCollider2D.radius * ((circleCollider2D.transform.localScale.x <= circleCollider2D.transform.localScale.y) ? circleCollider2D.transform.localScale.y : circleCollider2D.transform.localScale.x);
			float f = circleCollider2D.transform.eulerAngles.z * 0.0174532924f;
			float num2 = 360f / (float)this.pCorners;
			for (int i = 0; i < this.pCorners; i++)
			{
				f = num2 * (float)i * 0.0174532924f;
				this.polygonPoints[i] = new Vector2(center.x + num * Mathf.Cos(f), center.y + num * Mathf.Sin(f));
				f = circleCollider2D.transform.eulerAngles.z * 0.0174532924f;
				this.polygonPoints[i] = new Vector2((this.polygonPoints[i].x - center.x) * Mathf.Cos(f) - (this.polygonPoints[i].y - center.y) * Mathf.Sin(f) + center.x, (this.polygonPoints[i].x - center.x) * Mathf.Sin(f) + (this.polygonPoints[i].y - center.y) * Mathf.Cos(f) + center.y);
			}
			return this.polygonPoints;
		}

		private List<Vector2> GetPolygonCollider2DPoints()
		{
			this.polygonPoints.Clear();
			this.polygonPoints = (this.collider as PolygonCollider2D).points.ToList<Vector2>();
			for (int i = 0; i < this.polygonPoints.Count; i++)
			{
				this.polygonPoints[i] = this.collider.transform.TransformPoint(this.polygonPoints[i]);
			}
			if (this.polygonCliping.IsClockwise(this.polygonPoints))
			{
				this.ReverseArray(this.polygonPoints);
			}
			return this.polygonPoints;
		}

		private List<Vector2> ReverseArray(List<Vector2> points)
		{
			int num = points.Count / 2;
			int num2 = num * 2;
			for (int i = 0; i < num; i++)
			{
				Vector2 value = points[i];
				points[i] = points[num2 - i - 1];
				points[num2 - i - 1] = value;
			}
			return points;
		}

		private List<Vector2> GetCapsuleVerticesWorldPosition()
		{
			CapsuleCollider2D capsuleCollider2D = this.collider as CapsuleCollider2D;
			Vector3 localScale = capsuleCollider2D.transform.localScale;
			Vector3 center = capsuleCollider2D.bounds.center;
			bool flag = this.IsCircle(capsuleCollider2D);
			Vector2 vector = new Vector2(capsuleCollider2D.size.x * capsuleCollider2D.transform.localScale.x, capsuleCollider2D.size.y * capsuleCollider2D.transform.localScale.y);
			float num;
			float num2;
			int num3;
			if (capsuleCollider2D.direction == CapsuleDirection2D.Horizontal)
			{
				if (vector.y > vector.x)
				{
					num = vector.y * 0.5f;
					num2 = num;
				}
				else
				{
					num = vector.y * 0.5f;
					num2 = vector.x;
				}
				num3 = 0;
			}
			else
			{
				if (vector.x > vector.y)
				{
					num = vector.x * 0.5f;
					num2 = num;
				}
				else
				{
					num = vector.x * 0.5f;
					num2 = vector.y;
				}
				num3 = 1;
			}
			float f = capsuleCollider2D.transform.eulerAngles.z * 0.0174532924f;
			float num4 = (float)this.pCorners;
			int num5 = this.pCorners / 2;
			float num6 = 360f / num4;
			float num7;
			if (flag)
			{
				num7 = 0f;
			}
			else
			{
				num7 = (num2 * capsuleCollider2D.transform.localScale.y - 2f * num) * 0.5f + 0.1f;
			}
			int num8 = 0;
			while ((float)num8 < num4)
			{
				f = (num6 * (float)num8 + num6 * 0.5f) * 0.0174532924f;
				this.polygonPoints[num8] = new Vector2(center.x + num * Mathf.Cos(f), center.y + ((num8 >= num5) ? (-num7) : num7) + num * Mathf.Sin(f));
				if (num3 == 1)
				{
					f = capsuleCollider2D.transform.eulerAngles.z * 0.0174532924f;
				}
				else
				{
					f = (capsuleCollider2D.transform.eulerAngles.z + 90f) * 0.0174532924f;
				}
				this.polygonPoints[num8] = new Vector2((this.polygonPoints[num8].x - center.x) * Mathf.Cos(f) - (this.polygonPoints[num8].y - center.y) * Mathf.Sin(f) + center.x, (this.polygonPoints[num8].x - center.x) * Mathf.Sin(f) + (this.polygonPoints[num8].y - center.y) * Mathf.Cos(f) + center.y);
				num8++;
			}
			return this.polygonPoints;
		}

		public override bool HasCollider()
		{
			return this.collider != null;
		}

		public override bool HasRigidbody()
		{
			return this.rigidbody != null;
		}

		public override bool IsPlayer()
		{
			return this.playerCollider;
		}

		public override bool HasPlayedSoundEffect()
		{
			return this.soundEffectPlayed;
		}

		public override bool HasInstantiatedParticleSystem()
		{
			return this.particleSystemInstantiated;
		}

		public override void SetParticleSystemInstantiated(bool instantiated)
		{
			this.particleSystemInstantiated = instantiated;
		}

		public override void SetSoundPlayed(bool soundPlayed)
		{
			this.soundEffectPlayed = soundPlayed;
		}

		public override int GetPreviousDirectionOnYAxis()
		{
			return this.prevYAxisDirection;
		}

		public override void SetDirectionOnYAxis(int dir)
		{
			this.prevYAxisDirection = dir;
		}

		public override Vector3 GetPreviousPosition()
		{
			return this.previousPosition;
		}

		public override void SetPreviousPosition()
		{
			this.previousPosition = this.transform.position;
		}

		public override void AddForce(Vector3 force)
		{
			if (!this.HasRigidbody())
			{
				return;
			}
			this.rigidbody.AddForceAtPosition(force, this.collider.bounds.center);
		}

		public override void AddForceAtPosition(Vector3 force, Vector3 position)
		{
			if (!this.HasRigidbody())
			{
				return;
			}
			this.rigidbody.AddForceAtPosition(force, position);
		}

		public override void AddTorque(float torque)
		{
			if (!this.HasRigidbody())
			{
				return;
			}
			this.rigidbody.AddTorque(torque);
		}

		public override Vector3 GetVelocity()
		{
			if (!this.HasRigidbody())
			{
				return Vector3.zero;
			}
			return this.rigidbody.velocity;
		}

		public override Vector2 GetPointVelocity(Vector2 point)
		{
			if (!this.HasRigidbody())
			{
				return Vector2.zero;
			}
			return this.rigidbody.GetPointVelocity(point);
		}

		public override float GetAngularVelocity()
		{
			if (!this.HasRigidbody())
			{
				return 0f;
			}
			return this.rigidbody.angularVelocity;
		}

		public override bool Equals(object other)
		{
			return this.collider.Equals(other);
		}

		public override int GetHashCode()
		{
			return this.collider.GetHashCode();
		}
	}
}
