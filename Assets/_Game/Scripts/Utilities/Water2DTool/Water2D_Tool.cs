using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Water2DTool
{
	[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
	public class Water2D_Tool : MonoBehaviour
	{
		private Renderer frontMeshRend;

		private float rtPixelSize = 1f;

		private Water2D_Ripple ripple;

		private float rRTWidthInWorldSpace;

		private float rRTHeightInWorldSpace;

		public bool boundsAndNormals = true;

		private Mesh parentMesh;

		private Mesh childMesh;

		private Water2D_Mesh parentDMesh;

		private Water2D_Mesh childDMesh;

		private float zVertDistance = 1f;

		private float xVertDistance = 1f;

		private int ySegments = 1;

		private new Transform transform;

		public Vector2 unitsPerUV = Vector2.one;

		public Vector2 unitsPerUV2 = Vector2.one;

		public Vector2 rUnitsPerUV = Vector2.one;

		public Vector2 rUnitsPerUV2 = Vector2.one;

		public int xSegments = 1;

		public int zSegments = 1;

		public float width = 1f;

		public float height = 1f;

		public float length = 1f;

		public float pixelsPerUnit = 100f;

		public float segmentsPerUnit = 3f;

		public bool showMesh = true;

		public bool createCollider = true;

		public float colliderYAxisOffset;

		public List<Vector3> handlesPosition = new List<Vector3>();

		public float curentWaterArea;

		public bool use3DCollider;

		public float overlapingSphereZOffset = 0.5f;

		public float boxColliderZSize = 1f;

		public float handleScale = 1f;

		public bool cubeWater;

		public float zSize = 10f;

		public int frontMeshVertsCount;

		public float waterHeight = 6f;

		public float waterWidth = 10f;

		public bool useHandles = true;

		public int prevSurfaceVertsCount;

		public int xVerts = 2;

		public int yVerts = 2;

		public int zVerts = 2;

		public bool squareSegments;

		public bool showMeshInfo = true;

		public int renderTextureWidth = 128;

		public int renderTextureHeight = 128;

		public bool water2DRippleScript;

		public bool quadMesh;

		public bool zSegmentsCap;

		public int zSegmentsSize = 1;

		public GameObject topMeshGameObject;

		public Vector3 boxColliderCenterOffset = Vector3.zero;

		public Vector3 boxColliderSizeOffset = Vector3.zero;

		public Water2D_Mesh ParentDMesh
		{
			get
			{
				if (this.parentDMesh == null)
				{
					this.parentDMesh = new Water2D_Mesh();
				}
				return this.parentDMesh;
			}
		}

		public Water2D_Mesh ChildDMesh
		{
			get
			{
				if (this.childDMesh == null)
				{
					this.childDMesh = new Water2D_Mesh();
				}
				return this.childDMesh;
			}
		}

		private void GetReferences()
		{
			this.frontMeshRend = base.GetComponent<Renderer>();
			this.transform = base.GetComponent<Transform>();
			this.ripple = base.GetComponent<Water2D_Ripple>();
			if (this.ripple != null)
			{
				this.water2DRippleScript = true;
			}
		}

		public void RecreateWaterMesh()
		{
			this.ParentDMesh.Clear();
			this.ChildDMesh.Clear();
			this.RecalculateMeshParameters();
			if (this.water2DRippleScript)
			{
				this.GenerateFrontMeshNoTiling();
			}
			else
			{
				this.GenerateFrontMeshWithTiling();
			}
			if (this.cubeWater)
			{
				if (this.water2DRippleScript)
				{
					this.GenerateTopMeshNoTiling();
				}
				else
				{
					this.GenerateTopMeshWithTiling();
				}
			}
			if (!this.use3DCollider)
			{
				this.UpdateCollider2D();
			}
			else
			{
				this.UpdateCollider3D();
			}
			this.ParentDMesh.Build(ref this.parentMesh, this.boundsAndNormals);
			if (this.cubeWater)
			{
				this.ChildDMesh.Build(ref this.childMesh, this.boundsAndNormals);
			}
		}

		public void OnAwakeMeshRebuild()
		{
			Mesh mesh = new Mesh();
			mesh.name = string.Format("{0}{1}-Mesh", base.gameObject.name, base.gameObject.GetInstanceID());
			base.GetComponent<MeshFilter>().sharedMesh = null;
			base.GetComponent<MeshFilter>().sharedMesh = mesh;
			this.parentMesh = base.GetComponent<MeshFilter>().sharedMesh;
			if (this.cubeWater)
			{
				mesh = new Mesh();
				mesh.name = string.Format("{0}{1}-Mesh", this.topMeshGameObject.gameObject.name, this.topMeshGameObject.gameObject.GetInstanceID());
				this.topMeshGameObject.GetComponent<MeshFilter>().sharedMesh = null;
				this.topMeshGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
				this.childMesh = this.topMeshGameObject.GetComponent<MeshFilter>().sharedMesh;
			}
			this.boundsAndNormals = false;
			this.GetReferences();
			this.RecreateWaterMesh();
		}

		private void CheckComponentReference()
		{
			Mesh mesh = this.GetMesh(true);
			base.GetComponent<MeshFilter>().sharedMesh = mesh;
			this.parentMesh = mesh;
			if (this.cubeWater)
			{
				mesh = this.GetMesh(false);
				this.topMeshGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
				this.childMesh = mesh;
			}
		}

		private void RecalculateMeshParameters()
		{
			this.width = Mathf.Abs(this.handlesPosition[3].x - this.handlesPosition[2].x);
			this.height = Mathf.Abs(this.handlesPosition[0].y - this.handlesPosition[1].y);
			float num = 1f / this.segmentsPerUnit;
			this.rUnitsPerUV = new Vector2(1f / this.unitsPerUV.x, 1f / this.unitsPerUV.y);
			this.rUnitsPerUV2 = new Vector2(1f / this.unitsPerUV2.x, 1f / this.unitsPerUV2.y);
			if (this.cubeWater)
			{
				this.length = Mathf.Abs(this.handlesPosition[0].z - this.handlesPosition[4].z);
				if (this.water2DRippleScript)
				{
					this.SetRenderTextureVariables();
					this.length = this.rtPixelSize * (float)this.renderTextureHeight;
				}
			}
			this.xSegments = (int)Mathf.Ceil(this.width * this.segmentsPerUnit);
			this.xVertDistance = num;
			if (this.squareSegments)
			{
				if (this.water2DRippleScript)
				{
					this.zSegments = (int)Mathf.Ceil(this.length * this.segmentsPerUnit);
					this.zVertDistance = num;
				}
				else
				{
					this.zSegments = (int)Mathf.Ceil(this.length * this.segmentsPerUnit);
					this.zVertDistance = num;
				}
			}
			else
			{
				this.zSegments = this.zSegmentsSize;
				this.zVertDistance = this.length / (float)this.zSegments;
			}
			this.zVerts = this.zSegments + 1;
			this.xVerts = this.xSegments + 1;
			if (this.zSegmentsCap && this.zSegments > this.zSegmentsSize)
			{
				this.zVerts = this.zSegmentsSize + 1;
				this.zSegments = this.zSegmentsSize;
				if (!this.squareSegments)
				{
					this.zVertDistance = this.length / (float)this.zSegments;
				}
			}
			if (this.quadMesh)
			{
				this.zVerts = 2;
				this.xVerts = 2;
				this.zSegments = 1;
				this.xSegments = 1;
				this.zVertDistance = this.length / (float)this.zSegments;
			}
			if (this.cubeWater)
			{
				this.VertexLimit();
			}
		}

		private void VertexLimit()
		{
			float num = (float)(this.xVerts * this.zVerts + this.xVerts * 2);
			if (num > 65535f)
			{
				while (num > 65535f)
				{
					this.zVerts--;
					this.zSegments--;
					num = (float)(this.xVerts * this.zVerts + this.xVerts * 2);
				}
			}
		}

		private void GenerateFrontMeshNoTiling()
		{
			if (base.GetComponent<Renderer>().sharedMaterial == null)
			{
				return;
			}
			this.SetFrontMeshUnitsPerUV();
			this.GenerateFrontMeshDataNoTiling();
			this.ParentDMesh.GenerateTriangles(this.xSegments, this.ySegments, this.xVerts);
		}

		private void GenerateFrontMeshDataNoTiling()
		{
			Vector3 zero = Vector3.zero;
			this.prevSurfaceVertsCount = this.frontMeshVertsCount / 2;
			this.frontMeshVertsCount = 0;
			for (int i = 0; i < this.yVerts; i++)
			{
				for (int j = 0; j < this.xVerts; j++)
				{
					float x;
					if (j < this.xVerts - 1)
					{
						x = (float)j * this.xVertDistance + this.handlesPosition[2].x;
					}
					else
					{
						x = this.handlesPosition[2].x + (float)this.renderTextureWidth * this.rtPixelSize;
					}
					float y = this.handlesPosition[1].y + (float)i * this.height;
					zero.x = x;
					zero.y = y;
					Vector2 aUV;
					this.GetFrontMeshUVNoTiling(j, i, out aUV);
					this.ParentDMesh.AddVertex(zero, aUV, true);
					this.frontMeshVertsCount++;
				}
			}
			if (this.water2DRippleScript)
			{
				this.frontMeshRend.sharedMaterial.SetFloat("_WaterHeight", this.height);
				this.frontMeshRend.sharedMaterial.SetFloat("_WaterWidth", this.width);
			}
		}

		private void GetFrontMeshUVNoTiling(int x, int y, out Vector2 UV)
		{
			if (x < this.xVerts - 1)
			{
				UV.x = this.xVertDistance * (float)x * this.rRTWidthInWorldSpace;
			}
			else
			{
				UV.x = 1f;
			}
			if (y == 0)
			{
				UV.y = 0f;
			}
			else
			{
				UV.y = 1f;
			}
		}

		private void GenerateTopMeshNoTiling()
		{
			this.SetTopMeshUnitsPerUV();
			this.GenerateTopMeshDataNoTiling();
			this.ChildDMesh.GenerateTriangles(this.xSegments, this.zSegments, this.xVerts);
		}

		private void GenerateTopMeshDataNoTiling()
		{
			Vector2 aUV = Vector2.zero;
			for (int i = 0; i < this.zVerts; i++)
			{
				for (int j = 0; j < this.xVerts; j++)
				{
					float x;
					if (j < this.xVerts - 1)
					{
						x = (float)j * this.xVertDistance + this.handlesPosition[2].x;
					}
					else
					{
						x = this.handlesPosition[2].x + (float)this.renderTextureWidth * this.rtPixelSize;
					}
					float z;
					if (i < this.zVerts - 1)
					{
						z = (float)i * this.zVertDistance;
					}
					else
					{
						z = (float)this.renderTextureHeight * this.rtPixelSize;
					}
					Vector3 vertexPoss = new Vector3(x, this.handlesPosition[0].y, z);
					aUV = this.GetTopMeshUVWithoutTiling(j, i);
					this.ChildDMesh.AddVertex(vertexPoss, aUV, false);
				}
			}
		}

		private Vector2 GetTopMeshUVWithoutTiling(int x, int z)
		{
			float x2;
			if (x < this.xVerts - 1)
			{
				x2 = this.xVertDistance * (float)x * this.rRTWidthInWorldSpace;
			}
			else
			{
				x2 = 1f;
			}
			float y;
			if (z < this.zVerts - 1)
			{
				y = 1f - this.zVertDistance * (float)z * this.rRTHeightInWorldSpace;
			}
			else
			{
				y = 0f;
			}
			return new Vector2(x2, y);
		}

		private void GenerateFrontMeshWithTiling()
		{
			if (this.frontMeshRend.sharedMaterial == null)
			{
				return;
			}
			this.SetFrontMeshUnitsPerUV();
			this.GenerateFrontMeshDataWithTiling();
			this.ParentDMesh.GenerateTriangles(this.xSegments, this.ySegments, this.xVerts);
		}

		private void GenerateFrontMeshDataWithTiling()
		{
			Vector3 zero = Vector3.zero;
			Vector2 aUV = Vector2.zero;
			this.prevSurfaceVertsCount = this.frontMeshVertsCount / 2;
			this.frontMeshVertsCount = 0;
			float x = this.transform.position.x;
			float x2 = this.handlesPosition[3].x;
			float x3 = this.handlesPosition[2].x;
			float y = this.handlesPosition[1].y;
			for (int i = 0; i < this.yVerts; i++)
			{
				for (int j = 0; j < this.xVerts; j++)
				{
					if (j == this.xVerts - 1)
					{
						zero = new Vector3(x2, y + (float)i * this.height, 0f);
					}
					else
					{
						zero = new Vector3((float)j * this.xVertDistance + x3, y + (float)i * this.height, 0f);
					}
					aUV = this.GetFrontMeshUVWithTiling(zero, i, x);
					this.ParentDMesh.AddVertex(zero, aUV, true);
					this.frontMeshVertsCount++;
				}
			}
		}

		private Vector2 GetFrontMeshUVWithTiling(Vector3 vertPos, int y, float posX)
		{
			float x = (vertPos.x + posX) * this.rUnitsPerUV.x;
			float y2;
			if (y == 0)
			{
				y2 = 1f - this.height * this.rUnitsPerUV.y;
			}
			else
			{
				y2 = 1f;
			}
			return new Vector2(x, y2);
		}

		private void GenerateTopMeshWithTiling()
		{
			this.SetTopMeshUnitsPerUV();
			this.GenerateTopMeshDataWithTiling();
			this.ChildDMesh.GenerateTriangles(this.xSegments, this.zSegments, this.xVerts);
		}

		private void GenerateTopMeshDataWithTiling()
		{
			float y = this.handlesPosition[0].y;
			float x = this.handlesPosition[2].x;
			float x2 = this.handlesPosition[3].x;
			float x3 = this.transform.position.x;
			for (int i = 0; i < this.zVerts; i++)
			{
				for (int j = 0; j < this.xVerts; j++)
				{
					Vector3 vector;
					if (j < this.xVerts - 1)
					{
						vector = new Vector3((float)j * this.xVertDistance + x, y, (float)i * this.zVertDistance);
					}
					else
					{
						vector = new Vector3(x2, y, (float)i * this.zVertDistance);
					}
					Vector2 topMeshUVWithTiling = this.GetTopMeshUVWithTiling(vector, i, x3);
					this.ChildDMesh.AddVertex(vector, topMeshUVWithTiling, true);
				}
			}
		}

		private Vector2 GetTopMeshUVWithTiling(Vector3 vertPos, int z, float posX)
		{
			float x = (vertPos.x + posX) * this.rUnitsPerUV2.x;
			float y = 1f - this.zVertDistance * (float)z * this.rUnitsPerUV2.y;
			return new Vector2(x, y);
		}

		private void SetRenderTextureVariables()
		{
			Water2D_Ripple component = base.GetComponent<Water2D_Ripple>();
			int rtPixelsToUnits = component.rtPixelsToUnits;
			this.renderTextureWidth = (int)Mathf.Ceil(this.width * (float)rtPixelsToUnits);
			this.renderTextureHeight = (int)Mathf.Ceil(this.length * (float)rtPixelsToUnits);
			this.rtPixelSize = 1f / (float)rtPixelsToUnits;
			this.rRTWidthInWorldSpace = 1f / ((float)this.renderTextureWidth * this.rtPixelSize);
			this.rRTHeightInWorldSpace = 1f / ((float)this.renderTextureHeight * this.rtPixelSize);
		}

		private void SetTopMeshUnitsPerUV()
		{
			Material sharedMaterial = this.topMeshGameObject.GetComponent<Renderer>().sharedMaterial;
			if (sharedMaterial.HasProperty("_MainTex") && sharedMaterial.mainTexture != null)
			{
				this.unitsPerUV2.x = (float)sharedMaterial.mainTexture.width / this.pixelsPerUnit;
				this.unitsPerUV2.y = (float)sharedMaterial.mainTexture.height / this.pixelsPerUnit;
			}
			else
			{
				this.unitsPerUV2.x = 512f / this.pixelsPerUnit;
				this.unitsPerUV2.y = 512f / this.pixelsPerUnit;
			}
		}

		public void AddNewWaterMeshInstance()
		{
			this.parentMesh = null;
			this.childMesh = null;
			base.GetComponent<MeshFilter>().sharedMesh = null;
			if (this.cubeWater)
			{
				this.topMeshGameObject.GetComponent<MeshFilter>().sharedMesh = null;
			}
			Mesh mesh = this.GetMesh(true);
			base.GetComponent<MeshFilter>().sharedMesh = mesh;
			this.parentMesh = mesh;
			if (this.cubeWater)
			{
				mesh = this.GetMesh(false);
				this.topMeshGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
				this.childMesh = mesh;
			}
			this.RecreateWaterMesh();
		}

		public void UpdateCollider2D()
		{
			if (!this.createCollider)
			{
				if (base.GetComponent<BoxCollider2D>() != null)
				{
					BoxCollider2D component = base.GetComponent<BoxCollider2D>();
					component.enabled = false;
				}
				return;
			}
			if (base.GetComponent<BoxCollider2D>() == null)
			{
				base.gameObject.AddComponent<BoxCollider2D>();
				BoxCollider2D component2 = base.GetComponent<BoxCollider2D>();
				component2.isTrigger = true;
			}
			BoxCollider2D component3 = base.GetComponent<BoxCollider2D>();
			if (!component3.enabled)
			{
				component3.enabled = true;
			}
			component3.size = new Vector2(this.width, this.height + this.colliderYAxisOffset);
			Vector2 offset = this.handlesPosition[1];
			offset.y += this.height / 2f + this.colliderYAxisOffset / 2f;
			component3.offset = offset;
		}

		private void SetFrontMeshUnitsPerUV()
		{
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (sharedMaterial.HasProperty("_MainTex") && sharedMaterial.mainTexture != null)
			{
				this.unitsPerUV.x = (float)sharedMaterial.mainTexture.width / this.pixelsPerUnit;
				this.unitsPerUV.y = (float)sharedMaterial.mainTexture.height / this.pixelsPerUnit;
			}
			else
			{
				this.unitsPerUV.x = 512f / this.pixelsPerUnit;
				this.unitsPerUV.y = 512f / this.pixelsPerUnit;
			}
		}

		public void UpdateCollider3D()
		{
			if (!this.createCollider)
			{
				if (base.GetComponent<BoxCollider>() != null)
				{
					BoxCollider component = base.GetComponent<BoxCollider>();
					component.enabled = false;
				}
				return;
			}
			if (base.GetComponent<BoxCollider>() == null)
			{
				base.gameObject.AddComponent<BoxCollider>();
				BoxCollider component2 = base.GetComponent<BoxCollider>();
				component2.isTrigger = true;
			}
			BoxCollider component3 = base.GetComponent<BoxCollider>();
			if (!component3.enabled)
			{
				component3.enabled = true;
			}
			if (this.water2DRippleScript)
			{
				component3.size = new Vector3(this.width, this.height + this.colliderYAxisOffset, this.length);
				Vector3 center = this.handlesPosition[1];
				center.y += this.height * 0.5f + this.colliderYAxisOffset * 0.5f;
				center.z += this.length * 0.5f;
				component3.center = center;
			}
			else
			{
				if (this.cubeWater)
				{
					component3.size = new Vector3(this.width, this.height + this.colliderYAxisOffset, this.length);
				}
				else
				{
					component3.size = new Vector3(this.width, this.height + this.colliderYAxisOffset, this.boxColliderZSize);
				}
				component3.size += this.boxColliderSizeOffset;
				Vector3 center2 = this.handlesPosition[1];
				center2.y += this.height * 0.5f + this.colliderYAxisOffset * 0.5f;
				if (this.cubeWater)
				{
					center2.z = this.length * 0.5f;
				}
				component3.center = center2;
				component3.center += this.boxColliderCenterOffset;
			}
		}

		private Mesh GetMesh(bool meshForThisObject)
		{
			MeshFilter component;
			if (meshForThisObject)
			{
				component = base.GetComponent<MeshFilter>();
			}
			else
			{
				component = this.topMeshGameObject.GetComponent<MeshFilter>();
			}
			string meshName = this.GetMeshName(meshForThisObject);
			Mesh mesh = component.sharedMesh;
			if (!this.IsPrefab())
			{
				if (component.sharedMesh == null || component.sharedMesh.name != meshName)
				{
					mesh = new Mesh();
				}
			}
			mesh.name = meshName;
			return mesh;
		}

		private bool IsPrefab()
		{
			return false;
		}

		public string GetMeshName(bool nameForThisObject)
		{
			if (this.IsPrefab())
			{
				string text;
				Transform parent;
				if (nameForThisObject)
				{
					text = base.gameObject.name;
					parent = base.gameObject.transform.parent;
				}
				else
				{
					text = this.topMeshGameObject.gameObject.name;
					parent = this.topMeshGameObject.gameObject.transform.parent;
				}
				while (parent != null)
				{
					text = parent.name + "." + text;
					parent = parent.transform.parent;
				}
				return text + "-Mesh";
			}
			if (nameForThisObject)
			{
				return string.Format("{0}{1}-Mesh", base.gameObject.name, base.gameObject.GetInstanceID());
			}
			return string.Format("{0}{1}-Mesh", this.topMeshGameObject.gameObject.name, this.topMeshGameObject.gameObject.GetInstanceID());
		}

		public void UpdateWaterMesh(float wWidth, float wHeight, bool centerPos)
		{
			this.waterWidth = wWidth;
			this.waterHeight = wHeight;
			float num = this.waterHeight / 2f;
			this.handlesPosition[0] = new Vector2(0f, num);
			this.handlesPosition[1] = new Vector2(0f, -num);
			num = this.waterWidth / 2f;
			this.handlesPosition[2] = new Vector2(-num, 0f);
			this.handlesPosition[3] = new Vector2(num, 0f);
			if (this.cubeWater)
			{
				this.handlesPosition[4] = new Vector3(0f, this.handlesPosition[0].y, this.zSize);
			}
			if (centerPos)
			{
				this.ReCenterPivotPoint();
			}
		}

		public void ReCenterPivotPoint()
		{
			Vector3 a = Vector3.zero;
			this.transform = base.GetComponent<Transform>();
			for (int i = 0; i < 4; i++)
			{
				a += this.handlesPosition[i];
			}
			if (this.cubeWater)
			{
				a = a / (float)(this.handlesPosition.Count - 1) + new Vector3(this.transform.position.x, this.transform.position.y, 0f);
			}
			else
			{
				a = a / (float)this.handlesPosition.Count + new Vector3(this.transform.position.x, this.transform.position.y, 0f);
			}
			Vector3 b = a - new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, 0f);
			for (int j = 0; j < 4; j++)
			{
				List<Vector3> list;
				int index;
				(list = this.handlesPosition)[index = j] = list[index] - b;
			}
			if (this.cubeWater)
			{
				this.handlesPosition[4] = new Vector3(this.handlesPosition[0].x, this.handlesPosition[0].y, this.handlesPosition[4].z);
			}
			base.gameObject.transform.position = new Vector3(a.x, a.y, base.gameObject.transform.position.z);
			this.RecreateWaterMesh();
		}

		public void Add(Vector3 pos)
		{
			this.handlesPosition.Add(pos);
		}

		public void SetTopMeshMaterial()
		{
			if (this.cubeWater && this.topMeshGameObject.GetComponent<Renderer>().sharedMaterial == null)
			{
				this.topMeshGameObject.GetComponent<Renderer>().sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			}
		}

		public void SetDefaultMaterial()
		{
			Renderer component = base.GetComponent<Renderer>();
			Material material = Resources.Load("Default_Material", typeof(Material)) as Material;
			if (material != null)
			{
				component.material = material;
				this.unitsPerUV.x = (float)base.GetComponent<Renderer>().sharedMaterial.mainTexture.width / this.pixelsPerUnit;
				this.unitsPerUV.y = (float)base.GetComponent<Renderer>().sharedMaterial.mainTexture.height / this.pixelsPerUnit;
			}
			else
			{
				UnityEngine.Debug.LogWarning("The default material was not found. This happened most likely because you moved Water2D_Tool from the  Assets folder to a subfolder, deleted or renamed the Default_Material from the Resources folder. Click on this message to set the name of the default material if you renamed it.");
			}
		}

		public void SetGPUWaterDefaultMaterial()
		{
			Renderer component = base.GetComponent<Renderer>();
			Renderer component2 = this.topMeshGameObject.GetComponent<Renderer>();
			Material material = Resources.Load("Default_LitWaterRippleAdvanced_FM", typeof(Material)) as Material;
			Material material2 = Resources.Load("Default_LitWaterRippleAdvanced_TM", typeof(Material)) as Material;
			if (material != null && material2 != null)
			{
				component.material = material;
				component2.material = material2;
				component.shadowCastingMode = ShadowCastingMode.Off;
				component2.shadowCastingMode = ShadowCastingMode.Off;
			}
			else
			{
				UnityEngine.Debug.LogWarning("The default material was not found. This happened most likely because you moved Water2D_Tool from the  Assets folder to a subfolder, deleted or renamed the Default_Material from the Resources folder. Click on this message to set the name of the default material if you renamed it.");
			}
		}
	}
}
