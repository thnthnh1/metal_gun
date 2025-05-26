using System;
using System.Collections.Generic;
using UnityEngine;

namespace Water2DTool
{
	public class ObstructionPolygon : MonoBehaviour
	{
		public List<Vector3> handlesPosition = new List<Vector3>();

		public float handleScale = 1f;

		public void AddShapePoint(Vector3 hP)
		{
			this.handlesPosition.Add(hP);
		}

		public List<Vector2> GetShapePointsWorldPos()
		{
			int count = this.handlesPosition.Count;
			List<Vector2> list = new List<Vector2>();
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = base.transform.TransformPoint(this.handlesPosition[i]);
				list.Add(new Vector2(vector.x, vector.z));
			}
			return list;
		}
	}
}
