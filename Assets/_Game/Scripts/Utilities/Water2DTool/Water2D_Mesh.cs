using System;
using System.Collections.Generic;
using UnityEngine;

namespace Water2DTool
{
	public class Water2D_Mesh
	{
		public List<Vector3> meshVerts;

		private List<int> meshIndices;

		public List<Vector2> meshUVs;

		private List<Vector4> mTans;

		private List<Vector3> mNorms;

		public Water2D_Mesh()
		{
			this.meshVerts = new List<Vector3>();
			this.meshUVs = new List<Vector2>();
			this.meshIndices = new List<int>();
			this.mTans = new List<Vector4>();
			this.mNorms = new List<Vector3>();
		}

		public void Clear()
		{
			this.meshVerts.Clear();
			this.meshIndices.Clear();
			this.meshUVs.Clear();
			this.mTans.Clear();
			this.mNorms.Clear();
		}

		public void Build(ref Mesh mesh, bool boundsAndNormals)
		{
			mesh.Clear();
			mesh.SetVertices(this.meshVerts);
			mesh.SetUVs(0, this.meshUVs);
			mesh.SetTriangles(this.meshIndices, 0);
			mesh.SetNormals(this.mNorms);
			mesh.SetTangents(this.mTans);
			if (boundsAndNormals)
			{
				mesh.RecalculateBounds();
				mesh.RecalculateNormals();
				mesh.RecalculateTangents();
			}
		}

		public void GenerateTriangles(int xSegments, int ySegments, int xVertices)
		{
			for (int i = 0; i < ySegments; i++)
			{
				for (int j = 0; j < xSegments; j++)
				{
					this.meshIndices.Add(i * xVertices + j);
					this.meshIndices.Add((i + 1) * xVertices + j);
					this.meshIndices.Add(i * xVertices + j + 1);
					this.meshIndices.Add((i + 1) * xVertices + j);
					this.meshIndices.Add((i + 1) * xVertices + j + 1);
					this.meshIndices.Add(i * xVertices + j + 1);
				}
			}
		}

		public void AddVertex(Vector3 vertexPoss, Vector2 aUV, bool frontMesh)
		{
			this.meshVerts.Add(vertexPoss);
			this.meshUVs.Add(aUV);
			if (frontMesh)
			{
				this.mNorms.Add(new Vector3(0f, 1f, 0f));
				this.mTans.Add(new Vector4(1f, 0f, 0f, -1f));
			}
			else
			{
				this.mNorms.Add(new Vector3(0f, 1f, 0f));
				this.mTans.Add(new Vector4(1f, 0f, 0f, -1f));
			}
		}

		public int[] GetCurrentTriangleList(int startIndex = 0)
		{
			int[] array = new int[this.meshIndices.Count - startIndex];
			int num = 0;
			for (int i = startIndex; i < this.meshIndices.Count; i++)
			{
				array[num] = this.meshIndices[i];
				num++;
			}
			return array;
		}
	}
}
