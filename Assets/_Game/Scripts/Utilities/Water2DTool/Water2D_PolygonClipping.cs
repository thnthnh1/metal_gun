using System;
using System.Collections.Generic;
using UnityEngine;

namespace Water2DTool
{
	public class Water2D_PolygonClipping
	{
		private class Edge
		{
			public Vector2 From;

			public Vector2 To;

			public Edge(Vector2 from, Vector2 to)
			{
				this.From = from;
				this.To = to;
			}
		}

		private List<Vector2> outputList;

		private Water2D_PolygonClipping.Edge clipEdge;

		public Water2D_PolygonClipping()
		{
			this.outputList = new List<Vector2>();
			this.clipEdge = new Water2D_PolygonClipping.Edge(Vector2.zero, Vector2.one);
		}

		public List<Vector2> GetIntersectedPolygon(List<Vector2> subjectPoly, Vector2[] linePoints, out bool intersecting)
		{
			this.outputList.Clear();
			intersecting = true;
			this.clipEdge.From = linePoints[0];
			this.clipEdge.To = linePoints[1];
			Vector2 vector = subjectPoly[subjectPoly.Count - 1];
			int count = subjectPoly.Count;
			for (int i = 0; i < count; i++)
			{
				Vector2 vector2 = subjectPoly[i];
				if (this.IsInside(this.clipEdge, vector2))
				{
					if (!this.IsInside(this.clipEdge, vector))
					{
						Vector2? intersect = this.GetIntersect(vector, vector2, this.clipEdge.From, this.clipEdge.To);
						if (intersect.HasValue)
						{
							this.outputList.Add(intersect.Value);
						}
					}
					this.outputList.Add(vector2);
				}
				else if (this.IsInside(this.clipEdge, vector))
				{
					Vector2? intersect2 = this.GetIntersect(vector, vector2, this.clipEdge.From, this.clipEdge.To);
					if (intersect2.HasValue)
					{
						this.outputList.Add(intersect2.Value);
					}
				}
				vector = vector2;
			}
			if (this.outputList.Count == 0)
			{
				intersecting = false;
			}
			return this.outputList;
		}

		private Vector2? GetIntersect(Vector2 line1From, Vector2 line1To, Vector2 line2From, Vector2 line2To)
		{
			Vector2 a = line1To - line1From;
			Vector2 vector = line2To - line2From;
			float num = a.x * vector.y - a.y * vector.x;
			if (this.IsNearZero(num))
			{
				return null;
			}
			Vector2 vector2 = line2From - line1From;
			float d = (vector2.x * vector.y - vector2.y * vector.x) / num;
			return new Vector2?(line1From + d * a);
		}

		private bool IsInside(Water2D_PolygonClipping.Edge edge, Vector2 test)
		{
			bool? flag = this.IsLeftOf(edge, test);
			return !flag.HasValue || !flag.Value;
		}

		private bool? IsLeftOf(Water2D_PolygonClipping.Edge edge, Vector2 test)
		{
			Vector2 vector = edge.To - edge.From;
			Vector2 vector2 = test - edge.To;
			double num = (double)(vector.x * vector2.y - vector.y * vector2.x);
			if (num < 0.0)
			{
				return new bool?(false);
			}
			if (num > 0.0)
			{
				return new bool?(true);
			}
			return null;
		}

		public bool IsClockwise(List<Vector2> polygon)
		{
			for (int i = 2; i < polygon.Count; i++)
			{
				this.clipEdge.From = polygon[0];
				this.clipEdge.To = polygon[1];
				bool? flag = this.IsLeftOf(this.clipEdge, polygon[i]);
				if (flag.HasValue)
				{
					return !flag.Value;
				}
			}
			return true;
		}

		private bool IsNearZero(float testValue)
		{
			return (double)Mathf.Abs(testValue) <= 1E-09;
		}
	}
}
