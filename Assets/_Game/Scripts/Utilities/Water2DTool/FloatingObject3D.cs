using System;
using System.Collections.Generic;
using UnityEngine;

namespace Water2DTool
{
	public class FloatingObject3D : FloatingObject
	{
		private enum ColliderType
		{
			Unknown,
			BoxCollider,
			SphereCollider,
			CapsuleCollider
		}

		private int prevYAxisDirection = 1;

		private Collider collider;

		private int pCorners;

		private Rigidbody rigidbody;

		private FloatingObject3D.ColliderType colliderType;

		private bool playerCollider;

		private List<Vector2> polygonPoints;

		private Vector3 previousPosition;

		private bool soundEffectPlayed;

		private bool particleSystemInstantiated;

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

		public FloatingObject3D(Collider col, int polygonCorners, bool player)
		{
			this.collider = col;
			this.pCorners = polygonCorners;
			this.playerCollider = player;
			this.rigidbody = col.GetComponent<Rigidbody>();
			if (col is BoxCollider)
			{
				this.colliderType = FloatingObject3D.ColliderType.BoxCollider;
			}
			if (col is SphereCollider)
			{
				this.colliderType = FloatingObject3D.ColliderType.SphereCollider;
			}
			if (col is CapsuleCollider)
			{
				this.colliderType = FloatingObject3D.ColliderType.CapsuleCollider;
				this.pCorners += 2;
				if (this.pCorners % 2 != 0)
				{
					this.pCorners++;
				}
			}
			if (this.colliderType == FloatingObject3D.ColliderType.BoxCollider)
			{
				this.polygonPoints = new List<Vector2>();
				for (int i = 0; i < 4; i++)
				{
					this.polygonPoints.Add(Vector2.zero);
				}
			}
			else
			{
				this.polygonPoints = new List<Vector2>(this.pCorners);
				for (int j = 0; j < this.pCorners; j++)
				{
					this.polygonPoints.Add(Vector2.zero);
				}
			}
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

		public override bool HasCollider()
		{
			return this.collider != null;
		}

		public override bool IsPlayer()
		{
			return this.playerCollider;
		}

		public override bool HasRigidbody()
		{
			return this.rigidbody != null;
		}

		public override List<Vector2> GetPolygon()
		{
			switch (this.colliderType)
			{
			case FloatingObject3D.ColliderType.BoxCollider:
				return this.GetBoxVerticesWorldPosition();
			case FloatingObject3D.ColliderType.SphereCollider:
				return this.GetPolygonVerticesWorldPosition();
			case FloatingObject3D.ColliderType.CapsuleCollider:
				return this.GetCapsuleVerticesWorldPosition();
			default:
				return null;
			}
		}

		public override float GetRadius()
		{
			switch (this.colliderType)
			{
			case FloatingObject3D.ColliderType.BoxCollider:
				return this.GetBoxRadius();
			case FloatingObject3D.ColliderType.SphereCollider:
				return this.GetSphereRadius();
			case FloatingObject3D.ColliderType.CapsuleCollider:
				return this.GetCapsuleRadius();
			default:
				return 0f;
			}
		}

		private float GetBoxRadius()
		{
			BoxCollider boxCollider = this.collider as BoxCollider;
			return boxCollider.size.z * Mathf.Abs(this.collider.transform.localScale.z);
		}

		private float GetSphereRadius()
		{
			SphereCollider sphereCollider = this.collider as SphereCollider;
			return sphereCollider.radius * Mathf.Abs(this.collider.transform.localScale.x);
		}

		private float GetCapsuleRadius()
		{
			CapsuleCollider capsuleCollider = this.collider as CapsuleCollider;
			return capsuleCollider.radius * Mathf.Abs(this.collider.transform.localScale.x);
		}

		private List<Vector2> GetBoxVerticesWorldPosition()
		{
			float f = this.collider.transform.eulerAngles.z * 0.0174532924f;
			Vector3 center = this.collider.bounds.center;
			float z = this.collider.transform.eulerAngles.z;
			Vector2 vector = this.collider.bounds.min;
			Vector2 vector2 = this.collider.bounds.max;
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
				float num = (this.collider as BoxCollider).size.x * this.collider.transform.localScale.x * 0.5f;
				float num2 = (this.collider as BoxCollider).size.y * this.collider.transform.localScale.y * 0.5f;
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
			SphereCollider sphereCollider = this.collider as SphereCollider;
			Vector3 localScale = sphereCollider.transform.localScale;
			float num = 0.125f + sphereCollider.radius * localScale.x;
			if (localScale.y > localScale.x && localScale.y > localScale.z)
			{
				num = 0.125f + sphereCollider.radius * localScale.y;
			}
			if (localScale.z > localScale.x && localScale.z > localScale.y)
			{
				num = 0.125f + sphereCollider.radius * localScale.z;
			}
			float f = sphereCollider.transform.eulerAngles.z * 0.0174532924f;
			float num2 = 360f / (float)this.pCorners;
			for (int i = 0; i < this.pCorners; i++)
			{
				f = num2 * (float)i * 0.0174532924f;
				this.polygonPoints[i] = new Vector2(center.x + num * Mathf.Cos(f), center.y + num * Mathf.Sin(f));
				f = sphereCollider.transform.eulerAngles.z * 0.0174532924f;
				this.polygonPoints[i] = new Vector2((this.polygonPoints[i].x - center.x) * Mathf.Cos(f) - (this.polygonPoints[i].y - center.y) * Mathf.Sin(f) + center.x, (this.polygonPoints[i].x - center.x) * Mathf.Sin(f) + (this.polygonPoints[i].y - center.y) * Mathf.Cos(f) + center.y);
			}
			return this.polygonPoints;
		}

		private List<Vector2> GetCapsuleVerticesWorldPosition()
		{
			CapsuleCollider capsuleCollider = this.collider as CapsuleCollider;
			Vector3 localScale = capsuleCollider.transform.localScale;
			Vector3 center = capsuleCollider.bounds.center;
			float num = 0.125f + capsuleCollider.radius * capsuleCollider.transform.localScale.x;
			if (localScale.y > localScale.x && localScale.y > localScale.z)
			{
				num = 0.125f + capsuleCollider.radius * localScale.y;
			}
			if (localScale.z > localScale.x && localScale.z > localScale.y)
			{
				num = 0.125f + capsuleCollider.radius * localScale.z;
			}
			float f = capsuleCollider.transform.eulerAngles.z * 0.0174532924f;
			int direction = capsuleCollider.direction;
			int num2 = this.pCorners / 2;
			float num3 = 360f / (float)this.pCorners;
			float num4 = (capsuleCollider.height * capsuleCollider.transform.localScale.y - 2f * num) * 0.5f + 0.1f;
			for (int i = 0; i < this.pCorners; i++)
			{
				f = (num3 * (float)i + num3 * 0.5f) * 0.0174532924f;
				this.polygonPoints[i] = new Vector2(center.x + num * Mathf.Cos(f), center.y + ((i >= num2) ? (-num4) : num4) + num * Mathf.Sin(f));
				if (direction == 1)
				{
					f = capsuleCollider.transform.eulerAngles.z * 0.0174532924f;
				}
				else
				{
					f = (capsuleCollider.transform.eulerAngles.z + 90f) * 0.0174532924f;
				}
				this.polygonPoints[i] = new Vector2((this.polygonPoints[i].x - center.x) * Mathf.Cos(f) - (this.polygonPoints[i].y - center.y) * Mathf.Sin(f) + center.x, (this.polygonPoints[i].x - center.x) * Mathf.Sin(f) + (this.polygonPoints[i].y - center.y) * Mathf.Cos(f) + center.y);
			}
			return this.polygonPoints;
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
			this.rigidbody.AddTorque(0f, torque, 0f, ForceMode.Force);
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
			return this.rigidbody.angularVelocity.z;
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
