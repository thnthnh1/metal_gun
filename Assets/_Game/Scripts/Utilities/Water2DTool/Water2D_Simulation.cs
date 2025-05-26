using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Water2D_ClipperLib;

namespace Water2DTool
{
	[RequireComponent(typeof(Water2D_Tool)), RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
	public class Water2D_Simulation : MonoBehaviour
	{
		private sealed class _RandomWave_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _randomVert___0;

			internal float _randomVelocity___0;

			internal Water2D_Simulation _this;

			internal object _current;

			internal bool _disposing;

			internal int _PC;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public _RandomWave_c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
				{
					this._this.makeSplash = true;
					this._randomVert___0 = UnityEngine.Random.Range(0, this._this.surfaceVertsCount - 1);
					this._randomVelocity___0 = UnityEngine.Random.Range(this._this.minVelocity, this._this.maxVelocity);
					List<float> velocities;
					int index;
					(velocities = this._this.velocities)[index = this._randomVert___0] = velocities[index] + this._randomVelocity___0;
					if (this._randomVert___0 > 0)
					{
						int index2;
						(velocities = this._this.velocities)[index2 = this._randomVert___0 - 1] = velocities[index2] + this._randomVelocity___0 * this._this.neighborVertVelocityScale;
					}
					if (this._randomVert___0 < this._this.surfaceVertsCount - 1)
					{
						int index3;
						(velocities = this._this.velocities)[index3 = this._randomVert___0 + 1] = velocities[index3] + this._randomVelocity___0 * this._this.neighborVertVelocityScale;
					}
					this._current = new WaitForSeconds(this._this.timeStep);
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				case 1u:
					this._this.makeSplash = false;
					this._PC = -1;
					break;
				}
				return false;
			}

			public void Dispose()
			{
				this._disposing = true;
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private sealed class _OnTriggerEnter2D_c__AnonStorey1
		{
			internal Collider2D other;

			internal bool __m__0(FloatingObject o)
			{
				return o.Equals(this.other);
			}
		}

		private sealed class _OnTriggerExit2D_c__AnonStorey2
		{
			internal Collider2D other;

			internal bool __m__0(FloatingObject o)
			{
				return o.Equals(this.other);
			}

			internal bool __m__1(FloatingObject o)
			{
				return o.Equals(this.other);
			}
		}

		private sealed class _OnTriggerEnter_c__AnonStorey3
		{
			internal Collider other;

			internal bool __m__0(FloatingObject o)
			{
				return o.Equals(this.other);
			}
		}

		private sealed class _OnTriggerExit_c__AnonStorey4
		{
			internal Collider other;

			internal bool __m__0(FloatingObject o)
			{
				return o.Equals(this.other);
			}

			internal bool __m__1(FloatingObject o)
			{
				return o.Equals(this.other);
			}
		}

		private float originalWaterHeight;

		private List<float> velocities;

		private List<float> vertYOffsets;

		private List<float> accelerations;

		private List<float> leftDeltas;

		private List<float> rightDeltas;

		private List<float> sineY;

		private List<Vector3> frontMeshVertices;

		private List<Vector3> topMeshVertices;

		private List<Vector2> frontMeshUVs;

		private List<Vector2> topMeshUVs;

		private float phase;

		private int surfaceVertsCount;

		private float waterLineCurrentLocalPos;

		private float waterLinePreviousLocalPos;

		private float forceFactor;

		private Vector3 forcePosition;

		private Vector3 upLift;

		private float defaultWaterHeight;

		private float defaultWaterArea;

		private float prevTopEdgeYPos;

		private float prevBottomEdgeYPos;

		private float prevLeftEdgeXPos;

		private float prevRightEdgeXPos;

		private bool recreateWaterMesh;

		private bool updateWaterHeight;

		private float waterLineYPosOffset;

		private float prevDefaultWaterAreaOffset;

		private Mesh frontMeshFilter;

		private Mesh topMeshFilter;

		private Water2D_Tool water2D;

		private bool makeSplash;

		private bool onTriggerPlayerDetected;

		private float area;

		private float displacedMass;

		private float precisionFactor = 0.001f;

		private Vector3 leftHandleGlobalPos;

		private List<Vector2> waterLinePoints;

		private float scaleFactor = 100000f;

		private float rScaleFactor;

		private bool updateMeshDate;

		private Water2D_PolygonClipping polygonClipping;

		private List<Vector2> boxVertices;

		private List<Vector2> submergedPolygon;

		private List<Vector2> clipPolygon;

		private Clipper clipper;

		private List<List<IntPoint>> solutionPath;

		private List<IntPoint> subjPath;

		private List<IntPoint> clipPath;

		private List<Vector2> intersectionPolygon;

		private List<List<Vector2>> intersectionPolygons;

		private Vector2[] linePoints;

		private RaycastHit2D[] hit2D;

		private Collider[] hit3D;

		private bool tiling = true;

		private bool hasRippleScript;

		private float yAxisOffset;

		private Renderer frontMeshRend;

		private Renderer topMeshRend;

		private new Transform transform;

		public Water2D_BuoyantForceMode buoyantForceMode = Water2D_BuoyantForceMode.PhysicsBased;

		public Water2D_SurfaceWaves surfaceWaves;

		public Water2D_Type waterType;

		public Water2D_AnimationMethod animationMethod;

		public Water2D_ClippingMethod clippingMethod;

		public Water2D_FlowDirection flowDirection = Water2D_FlowDirection.Right;

		public Water2D_SineWaves sineWavesType;

		public Water2D_CharacterControllerType characterControllerType;

		public Water2D_CollisionDetectionMode collisionDetectionMode;

		public List<FloatingObject> floatingObjects;

		public float playerOnExitVelocity = 0.2f;

		public bool playerOnExitRipple;

		public bool playerOnExitPSAndSound;

		public List<float> sineAmplitudes = new List<float>();

		public List<float> sineStretches = new List<float>();

		public List<float> phaseOffset = new List<float>();

		public float defaultWaterAreaOffset;

		public int sineWaves = 4;

		public float springConstant = 0.02f;

		public float damping = 0.04f;

		public float spread = 0.03f;

		public float floatHeight = 3f;

		public float bounceDamping = 0.15f;

		public Vector3 forcePositionOffset;

		public float collisionVelocityScale = 0.0125f;

		public Transform topEdge;

		public Transform bottomEdge;

		public Transform leftEdge;

		public Transform rightEdge;

		public GameObject particleS;

		public bool constantWaterArea;

		public bool waterDisplacement;

		public bool animateWaterArea;

		public float waveSpeed = 8f;

		public float velocityFilter = -2f;

		public float waterDensity = 1f;

		public int polygonCorners = 8;

		public float maxDrag = 500f;

		public float maxLift = 200f;

		public float dragCoefficient = 0.4f;

		public float liftCoefficient = 0.25f;

		public float timeStep = 0.5f;

		public float maxAmplitude = 0.1f;

		public float minAmplitude = 0.01f;

		public float maxStretch = 2f;

		public float minStretch = 0.8f;

		public float maxPhaseOffset = 0.1f;

		public float minPhaseOffset = 0.02f;

		public float maxVelocity = 0.1f;

		public float minVelocity = -0.1f;

		public float forceScale = 4f;

		public float liniarBFDragCoefficient = 0.1f;

		public float liniarBFAbgularDragCoefficient = 0.01f;

		public float interactionRegion = 1f;

		public Vector3 waterLineCurrentWorldPos;

		public AudioClip splashSound;

		public Vector2 playerBoundingBoxSize = new Vector2(1f, 1f);

		public Vector2 playerBoundingBoxCenter = Vector2.zero;

		public float playerBuoyantForceScale = 3f;

		public bool showPolygon;

		public bool showForces;

		public float neighborVertVelocityScale = 0.5f;

		public float sineWaveVelocityScale = 0.05f;

		public float overlapSphereRadius = 0.05f;

		public bool springSimulation = true;

		public float topEdgeYOffset;

		public float bottomEdgeYOffset;

		public float leftEdgeXOffset;

		public float rightEdgeXOffset;

		public Vector3 particleSystemPosOffset = Vector3.zero;

		public string particleSystemSortingLayerName = "Default";

		public int particleSystemOrderInLayer;

		public int meshSegmentsPerWaterLineSegment = 4;

		public bool showClippingPlolygon;

		public bool waterFlow;

		public float flowAngle;

		public float waterFlowForce = 5f;

		public bool useAngles;

		public float waveAmplitude = 0.1f;

		public float waveStretch = 1f;

		public float wavePhaseOffset = 0.2f;

		public bool randomValues = true;

		public bool particleSystemSorting;

		public float rippleWidth = 1f;

		public float playerOnEnterVelocity = -0.2f;

		public LayerMask collisionLayers = -1;

		public float raycastDistance = 0.05f;

		public float interactionTime = 5f;

		private float interactionTimeCount = 6f;

		private Water2D_Ripple water2DRipple;

		private void Awake()
		{
			this.transform = base.GetComponent<Transform>();
			this.water2D = base.GetComponent<Water2D_Tool>();
			this.water2DRipple = base.GetComponent<Water2D_Ripple>();
			this.water2D.OnAwakeMeshRebuild();
			if (this.water2DRipple)
			{
				this.water2DRipple.InstantiateRenderTextures();
				this.springSimulation = false;
				this.tiling = false;
				this.hasRippleScript = true;
			}
			this.frontMeshFilter = base.GetComponent<MeshFilter>().sharedMesh;
			this.intersectionPolygon = new List<Vector2>();
			this.intersectionPolygons = new List<List<Vector2>>();
			this.submergedPolygon = new List<Vector2>();
			this.polygonClipping = new Water2D_PolygonClipping();
			this.clipPolygon = new List<Vector2>();
			this.clipper = new Clipper(0);
			this.solutionPath = new List<List<IntPoint>>();
			this.subjPath = new List<IntPoint>();
			this.clipPath = new List<IntPoint>();
			this.rScaleFactor = 1f / this.scaleFactor;
			this.frontMeshVertices = this.frontMeshFilter.vertices.ToList<Vector3>();
			this.surfaceVertsCount = this.water2D.frontMeshVertsCount / 2;
			this.frontMeshUVs = this.frontMeshFilter.uv.ToList<Vector2>();
			if (this.water2D.cubeWater)
			{
				this.topMeshFilter = this.water2D.topMeshGameObject.GetComponent<MeshFilter>().sharedMesh;
				this.topMeshVertices = this.topMeshFilter.vertices.ToList<Vector3>();
				this.topMeshUVs = this.topMeshFilter.uv.ToList<Vector2>();
			}
			this.waterLineCurrentWorldPos = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + 1]);
			this.waterLineCurrentLocalPos = this.frontMeshVertices[this.surfaceVertsCount + 1].y;
			this.waterLinePreviousLocalPos = this.frontMeshVertices[this.surfaceVertsCount + 1].y;
			this.defaultWaterHeight = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
			this.originalWaterHeight = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
			this.defaultWaterArea = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y) * Mathf.Abs(this.water2D.handlesPosition[2].x - this.water2D.handlesPosition[3].x);
			this.sineY = new List<float>();
			this.floatingObjects = new List<FloatingObject>();
			this.waterLinePoints = new List<Vector2>();
			this.velocities = new List<float>();
			this.accelerations = new List<float>();
			this.leftDeltas = new List<float>();
			this.rightDeltas = new List<float>();
			this.boxVertices = new List<Vector2>();
			this.linePoints = new Vector2[2];
			this.vertYOffsets = new List<float>();
			if (!this.water2D.use3DCollider)
			{
				this.hit2D = new RaycastHit2D[5];
			}
			else
			{
				this.hit3D = new Collider[5];
			}
			for (int i = 0; i < 4; i++)
			{
				this.boxVertices.Add(Vector2.zero);
			}
			for (int j = 0; j < this.surfaceVertsCount; j++)
			{
				this.velocities.Add(0f);
				this.accelerations.Add(0f);
				this.leftDeltas.Add(0f);
				this.rightDeltas.Add(0f);
				this.sineY.Add(0f);
			}
			if (this.topEdge != null)
			{
				this.prevTopEdgeYPos = this.topEdge.transform.position.y;
			}
			if (this.bottomEdge != null)
			{
				this.prevBottomEdgeYPos = this.bottomEdge.transform.position.y;
			}
			if (this.leftEdge != null)
			{
				this.prevLeftEdgeXPos = this.leftEdge.transform.position.x;
			}
			if (this.rightEdge != null)
			{
				this.prevRightEdgeXPos = this.rightEdge.transform.position.x;
			}
			if (this.sineWavesType == Water2D_SineWaves.MultipleSineWaves && this.randomValues)
			{
				this.GenerateSineVariables();
			}
			if (this.surfaceVertsCount > this.meshSegmentsPerWaterLineSegment)
			{
				for (int k = 0; k < this.surfaceVertsCount; k += this.meshSegmentsPerWaterLineSegment)
				{
					this.waterLinePoints.Add(this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + k]));
				}
				if (this.meshSegmentsPerWaterLineSegment != 1)
				{
					Vector2 vector = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + this.surfaceVertsCount - 1]);
					this.waterLinePoints.Add(new Vector2(vector.x, vector.y));
				}
			}
			else
			{
				this.waterLinePoints.Add(this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount]));
				Vector2 vector2 = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + this.surfaceVertsCount - 1]);
				this.waterLinePoints.Add(new Vector2(vector2.x, vector2.y));
			}
			this.frontMeshFilter.bounds = new Bounds(Vector3.zero, Vector3.one * 2000f);
			if (this.water2D.cubeWater)
			{
				this.topMeshFilter.bounds = new Bounds(Vector3.zero, Vector3.one * 2000f);
			}
			this.frontMeshRend = base.GetComponent<Renderer>();
			if (this.water2D.cubeWater)
			{
				this.topMeshRend = this.water2D.topMeshGameObject.GetComponent<Renderer>();
			}
			if (this.surfaceWaves != Water2D_SurfaceWaves.None)
			{
				this.interactionTimeCount = 0f;
			}
		}

		private void FixedUpdate()
		{
			this.waterLineCurrentWorldPos = this.transform.TransformPoint(this.water2D.handlesPosition[0]);
			this.waterLineCurrentLocalPos = this.water2D.handlesPosition[0].y;
			if (this.waterDisplacement)
			{
				this.WaterDisplacement();
			}
			if (this.surfaceWaves != Water2D_SurfaceWaves.None)
			{
				if (this.surfaceWaves == Water2D_SurfaceWaves.SineWaves)
				{
					this.SineWaves();
				}
				if (this.surfaceWaves == Water2D_SurfaceWaves.RandomSplashes && !this.makeSplash)
				{
					base.StartCoroutine(this.RandomWave());
				}
			}
			if (this.animationMethod != Water2D_AnimationMethod.None)
			{
				if (this.hasRippleScript)
				{
					this.ShaderAnimation();
				}
				else
				{
					this.WaterAnimation();
				}
			}
			if (this.springSimulation)
			{
				this.WaterWaves();
			}
			this.WaterMesh();
			if (!this.springSimulation && this.surfaceWaves != Water2D_SurfaceWaves.None)
			{
				this.UpdateWaterLinePoints();
			}
			if (this.buoyantForceMode != Water2D_BuoyantForceMode.None)
			{
				this.Buoyancy();
			}
			if (this.springSimulation || this.updateMeshDate || this.surfaceWaves != Water2D_SurfaceWaves.None)
			{
				this.frontMeshFilter.SetVertices(this.frontMeshVertices);
				this.frontMeshFilter.SetUVs(0, this.frontMeshUVs);
				if (!this.water2D.cubeWater)
				{
					this.updateMeshDate = false;
				}
				if (this.water2D.cubeWater)
				{
					this.topMeshFilter.SetVertices(this.topMeshVertices);
					this.topMeshFilter.SetUVs(0, this.topMeshUVs);
					this.updateMeshDate = false;
				}
			}
			this.waterLineYPosOffset = 0f;
		}

		private bool ColliderExists(int cIndex)
		{
			if (cIndex >= this.floatingObjects.Count)
			{
				return false;
			}
			if (!this.floatingObjects[cIndex].HasCollider())
			{
				this.floatingObjects.RemoveAt(cIndex);
				return false;
			}
			return true;
		}

		private void GenerateSineVariables()
		{
			this.sineAmplitudes.Clear();
			this.sineStretches.Clear();
			this.phaseOffset.Clear();
			for (int i = 0; i < this.sineWaves; i++)
			{
				this.sineAmplitudes.Add(UnityEngine.Random.Range(this.minAmplitude, this.maxAmplitude));
				this.sineStretches.Add(UnityEngine.Random.Range(this.minStretch, this.maxStretch));
				this.phaseOffset.Add(UnityEngine.Random.Range(this.minPhaseOffset, this.maxPhaseOffset));
			}
		}

		private void Buoyancy()
		{
			if (this.buoyantForceMode == Water2D_BuoyantForceMode.PhysicsBased && this.waterType == Water2D_Type.Dynamic)
			{
				this.PhysicsBasedBuoyantForce();
			}
			if (this.buoyantForceMode == Water2D_BuoyantForceMode.Linear && this.waterType == Water2D_Type.Dynamic)
			{
				this.LinearBuoyantForce();
			}
		}

		private void WaterDisplacement()
		{
			this.waterLineCurrentLocalPos = this.water2D.handlesPosition[1].y + this.defaultWaterHeight;
			this.waterLineCurrentWorldPos = this.transform.TransformPoint(new Vector3(0f, this.waterLineCurrentLocalPos, 0f));
			if (!this.water2D.use3DCollider)
			{
				int count = this.floatingObjects.Count;
				for (int i = 0; i < count; i++)
				{
					this.ApplyWaterDisplacement(i);
				}
			}
		}

		private void ApplyWaterDisplacement(int oIndex)
		{
			if (!this.ColliderExists(oIndex))
			{
				return;
			}
			this.submergedPolygon.Clear();
			if (this.floatingObjects[oIndex].bounds.min.y < this.waterLineCurrentWorldPos.y)
			{
				bool flag = true;
				List<Vector2> polygon = this.floatingObjects[oIndex].GetPolygon();
				this.linePoints[0] = new Vector2(this.water2D.handlesPosition[2].x, this.waterLineCurrentWorldPos.y);
				this.linePoints[1] = new Vector2(this.water2D.handlesPosition[3].x, this.waterLineCurrentWorldPos.y);
				this.submergedPolygon = this.polygonClipping.GetIntersectedPolygon(polygon, this.linePoints, out flag);
				float polygonArea = this.GetPolygonArea(this.submergedPolygon);
				float num = polygonArea / Mathf.Abs(this.water2D.handlesPosition[2].x - this.water2D.handlesPosition[3].x);
				this.waterLineYPosOffset += num;
				this.waterLineCurrentLocalPos += num;
				this.waterLineCurrentWorldPos.y = this.waterLineCurrentWorldPos.y + num;
			}
		}

		private IEnumerator RandomWave()
		{
			Water2D_Simulation._RandomWave_c__Iterator0 _RandomWave_c__Iterator = new Water2D_Simulation._RandomWave_c__Iterator0();
			_RandomWave_c__Iterator._this = this;
			return _RandomWave_c__Iterator;
		}

		private void SineWaves()
		{
			this.phase += 1f;
			int num = 0;
			if (this.springSimulation)
			{
				int num2 = this.surfaceVertsCount;
				if (this.sineWavesType == Water2D_SineWaves.SingleSineWave)
				{
					for (int i = num; i < num2; i++)
					{
						List<float> list;
						int index;
						(list = this.velocities)[index = i] = list[index] + this.waveAmplitude * Mathf.Sin(this.frontMeshVertices[i].x * this.waveStretch + this.phase * this.wavePhaseOffset) * this.sineWaveVelocityScale;
					}
				}
				else
				{
					for (int j = num; j < num2; j++)
					{
						this.sineY[j] = this.OverlapSineWaves(this.frontMeshVertices[j].x);
						List<float> list;
						int index2;
						(list = this.velocities)[index2 = j] = list[index2] + this.sineY[j] * this.sineWaveVelocityScale;
					}
				}
			}
			else
			{
				int num2 = this.surfaceVertsCount;
				if (this.sineWavesType == Water2D_SineWaves.SingleSineWave)
				{
					for (int k = 0; k < num2; k++)
					{
						float num3 = this.waveAmplitude * Mathf.Sin(this.frontMeshVertices[k].x * this.waveStretch + this.phase * this.wavePhaseOffset);
						Vector3 value = this.frontMeshVertices[k + num2];
						value.y = this.waterLineCurrentLocalPos + num3;
						this.frontMeshVertices[k + num2] = value;
					}
				}
				else
				{
					for (int l = 0; l < num2; l++)
					{
						this.sineY[l] = this.OverlapSineWaves(this.frontMeshVertices[l].x);
						Vector3 value = this.frontMeshVertices[l + num2];
						value.y = this.waterLineCurrentLocalPos + this.sineY[l];
						this.frontMeshVertices[l + num2] = value;
					}
				}
			}
		}

		private float OverlapSineWaves(float x)
		{
			float num = 0f;
			for (int i = 0; i < this.sineWaves; i++)
			{
				num += this.sineAmplitudes[i] * Mathf.Sin(x * this.sineStretches[i] + this.phase * this.phaseOffset[i]);
			}
			return num;
		}

		private void ShaderAnimation()
		{
			if (this.topEdge != null && Mathf.Abs(this.topEdge.position.y - this.prevTopEdgeYPos) > this.precisionFactor)
			{
				if (this.animationMethod == Water2D_AnimationMethod.Follow)
				{
					this.defaultWaterHeight += this.topEdge.position.y - this.prevTopEdgeYPos;
				}
				else
				{
					this.defaultWaterHeight = Mathf.Abs(this.transform.InverseTransformPoint(this.topEdge.position).y - this.water2D.handlesPosition[1].y + this.topEdgeYOffset);
				}
				this.defaultWaterArea = this.defaultWaterHeight * Mathf.Abs(this.water2D.handlesPosition[2].x - this.water2D.handlesPosition[3].x);
				this.yAxisOffset = this.defaultWaterHeight - this.originalWaterHeight;
				this.prevTopEdgeYPos = this.topEdge.transform.position.y;
				this.UpdateTopLeftRightHandles();
				this.UpdateCollider();
				this.frontMeshRend.material.SetFloat("_HeightOffset", this.yAxisOffset);
				this.topMeshRend.material.SetFloat("_HeightOffset", this.yAxisOffset);
				float value = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
				this.frontMeshRend.material.SetFloat("_WaterHeight", value);
			}
		}

		private void WaterAnimation()
		{
			this.recreateWaterMesh = false;
			this.updateWaterHeight = false;
			if (this.waterDisplacement && this.waterLineYPosOffset != 0f)
			{
				this.updateWaterHeight = true;
				this.UpdateTopLeftRightHandles();
			}
			if (this.animateWaterArea && Mathf.Abs(this.defaultWaterAreaOffset - this.prevDefaultWaterAreaOffset) > this.precisionFactor)
			{
				this.defaultWaterArea += this.defaultWaterAreaOffset - this.prevDefaultWaterAreaOffset;
				this.prevDefaultWaterAreaOffset = this.defaultWaterAreaOffset;
				float num = Mathf.Abs(this.water2D.handlesPosition[2].x - this.water2D.handlesPosition[3].x);
				this.defaultWaterHeight = this.defaultWaterArea / num;
				this.UpdateTopLeftRightHandles();
				this.recreateWaterMesh = true;
			}
			if (this.topEdge != null && Mathf.Abs(this.topEdge.position.y - this.prevTopEdgeYPos) > this.precisionFactor)
			{
				if (this.animationMethod == Water2D_AnimationMethod.Follow)
				{
					this.defaultWaterHeight += this.topEdge.position.y - this.prevTopEdgeYPos;
				}
				else
				{
					this.defaultWaterHeight = Mathf.Abs(this.transform.InverseTransformPoint(this.topEdge.position).y - this.water2D.handlesPosition[1].y + this.topEdgeYOffset);
				}
				this.defaultWaterArea = this.defaultWaterHeight * Mathf.Abs(this.water2D.handlesPosition[2].x - this.water2D.handlesPosition[3].x);
				this.prevTopEdgeYPos = this.topEdge.transform.position.y;
				this.UpdateTopLeftRightHandles();
				this.updateWaterHeight = true;
			}
			if (this.bottomEdge != null && Mathf.Abs(this.bottomEdge.position.y - this.prevBottomEdgeYPos) > this.precisionFactor)
			{
				if (this.animationMethod == Water2D_AnimationMethod.Follow)
				{
					this.water2D.handlesPosition[1] = new Vector2(this.water2D.handlesPosition[1].x, this.water2D.handlesPosition[1].y + this.bottomEdge.position.y - this.prevBottomEdgeYPos);
				}
				else
				{
					this.water2D.handlesPosition[1] = new Vector2(this.water2D.handlesPosition[1].x, this.transform.InverseTransformPoint(this.bottomEdge.position).y + this.bottomEdgeYOffset);
				}
				this.UpdateTopLeftRightHandles();
				this.prevBottomEdgeYPos = this.bottomEdge.position.y;
				this.updateWaterHeight = true;
			}
			if (this.leftEdge != null && Mathf.Abs(this.leftEdge.position.x - this.prevLeftEdgeXPos) > this.precisionFactor)
			{
				if (this.animationMethod == Water2D_AnimationMethod.Follow)
				{
					this.water2D.handlesPosition[2] = new Vector2(this.water2D.handlesPosition[2].x + this.leftEdge.position.x - this.prevLeftEdgeXPos, this.water2D.handlesPosition[2].y);
				}
				else
				{
					this.water2D.handlesPosition[2] = new Vector2(this.transform.InverseTransformPoint(this.leftEdge.position).x + this.leftEdgeXOffset, this.water2D.handlesPosition[2].y);
				}
				this.prevLeftEdgeXPos = this.leftEdge.transform.position.x;
				float num2 = Mathf.Abs(this.water2D.handlesPosition[2].x - this.water2D.handlesPosition[3].x);
				if (this.constantWaterArea)
				{
					this.defaultWaterHeight = this.defaultWaterArea / num2;
					this.water2D.handlesPosition[0] = new Vector2(this.water2D.handlesPosition[0].x, this.water2D.handlesPosition[1].y + this.defaultWaterHeight + this.waterLineYPosOffset);
				}
				this.water2D.handlesPosition[0] = new Vector2(this.water2D.handlesPosition[2].x + num2 * 0.5f, this.water2D.handlesPosition[0].y);
				this.water2D.handlesPosition[1] = new Vector2(this.water2D.handlesPosition[2].x + num2 * 0.5f, this.water2D.handlesPosition[1].y);
				this.recreateWaterMesh = true;
			}
			if (this.rightEdge != null && Mathf.Abs(this.rightEdge.position.x - this.prevRightEdgeXPos) > this.precisionFactor)
			{
				if (this.animationMethod == Water2D_AnimationMethod.Follow)
				{
					this.water2D.handlesPosition[3] = new Vector2(this.water2D.handlesPosition[3].x + this.rightEdge.position.x - this.prevRightEdgeXPos, this.water2D.handlesPosition[3].y);
				}
				else
				{
					this.water2D.handlesPosition[3] = new Vector2(this.transform.InverseTransformPoint(this.rightEdge.position).x + this.rightEdgeXOffset, this.water2D.handlesPosition[3].y);
				}
				this.prevRightEdgeXPos = this.rightEdge.position.x;
				float num3 = Mathf.Abs(this.water2D.handlesPosition[2].x - this.water2D.handlesPosition[3].x);
				if (this.constantWaterArea)
				{
					this.defaultWaterHeight = this.defaultWaterArea / num3;
					this.water2D.handlesPosition[0] = new Vector2(this.water2D.handlesPosition[0].x, this.water2D.handlesPosition[1].y + this.defaultWaterHeight + this.waterLineYPosOffset);
				}
				this.water2D.handlesPosition[0] = new Vector2(this.water2D.handlesPosition[2].x + num3 * 0.5f, this.water2D.handlesPosition[0].y);
				this.water2D.handlesPosition[1] = new Vector2(this.water2D.handlesPosition[2].x + num3 * 0.5f, this.water2D.handlesPosition[1].y);
				this.recreateWaterMesh = true;
			}
		}

		private void WaterMesh()
		{
			if (this.recreateWaterMesh)
			{
				this.RecreateWaterMesh();
			}
			if (this.updateWaterHeight && !this.recreateWaterMesh)
			{
				this.UpdateWaterHeight();
				this.updateMeshDate = true;
			}
		}

		private void UpdateWaterHeight()
		{
			this.vertYOffsets.Clear();
			float num = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
			Vector3 value = Vector2.zero;
			for (int i = 0; i < this.surfaceVertsCount; i++)
			{
				this.vertYOffsets.Add(this.frontMeshVertices[this.surfaceVertsCount + i].y - this.waterLinePreviousLocalPos);
				float num2 = num / this.water2D.unitsPerUV.y;
				this.frontMeshUVs[i] = new Vector2(this.frontMeshUVs[i].x, (!this.tiling || num2 <= 1f) ? (1f - num2) : 0f);
				value.x = this.frontMeshVertices[i].x;
				value.y = this.water2D.handlesPosition[1].y;
				value.z = 0f;
				this.frontMeshVertices[i] = value;
				value = this.frontMeshVertices[this.surfaceVertsCount + i];
				value.y = this.water2D.handlesPosition[0].y + this.vertYOffsets[i];
				this.frontMeshVertices[this.surfaceVertsCount + i] = value;
				if (this.water2D.cubeWater)
				{
					for (int j = 0; j < this.water2D.zSegments + 1; j++)
					{
						value = this.topMeshVertices[j * this.surfaceVertsCount + i];
						value.y = this.frontMeshVertices[this.surfaceVertsCount + i].y;
						this.topMeshVertices[j * this.surfaceVertsCount + i] = value;
					}
				}
			}
			if (this.frontMeshRend.material.HasProperty("_WaterHeight"))
			{
				float value2 = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
				this.frontMeshRend.material.SetFloat("_WaterHeight", value2);
			}
			this.UpdateCollider();
		}

		private void UpdateCollider()
		{
			float num = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
			if (!this.water2D.use3DCollider)
			{
				BoxCollider2D component = base.GetComponent<BoxCollider2D>();
				component.size = new Vector2(this.water2D.width, num + this.water2D.colliderYAxisOffset);
				Vector2 offset = this.water2D.handlesPosition[1];
				offset.y += num / 2f + this.water2D.colliderYAxisOffset / 2f;
				component.offset = offset;
			}
			else
			{
				BoxCollider component2 = base.GetComponent<BoxCollider>();
				component2.size = new Vector3(this.water2D.width, num + this.water2D.colliderYAxisOffset, this.water2D.length);
				Vector3 center = this.water2D.handlesPosition[1];
				center.y += num * 0.5f + this.water2D.colliderYAxisOffset * 0.5f;
				component2.center = center;
				if (this.water2D.cubeWater)
				{
					center.z = this.water2D.length * 0.5f;
				}
				component2.center = center;
				component2.center += this.water2D.boxColliderCenterOffset;
			}
			this.waterLinePreviousLocalPos = this.water2D.handlesPosition[1].y + this.defaultWaterHeight + this.waterLineYPosOffset;
			this.waterLineCurrentWorldPos = this.transform.TransformPoint(this.water2D.handlesPosition[0]);
			this.waterLineCurrentLocalPos = this.water2D.handlesPosition[1].y + this.defaultWaterHeight + this.waterLineYPosOffset;
		}

		private void RecreateWaterMesh()
		{
			this.vertYOffsets.Clear();
			Vector3 value = Vector3.zero;
			for (int i = 0; i < this.surfaceVertsCount; i++)
			{
				this.vertYOffsets.Add(this.frontMeshVertices[this.surfaceVertsCount + i].y - this.waterLinePreviousLocalPos);
			}
			this.water2D.RecreateWaterMesh();
			this.UpdateVariables();
			int count = this.vertYOffsets.Count;
			if (count < this.surfaceVertsCount || count == this.surfaceVertsCount)
			{
				for (int j = 0; j < count; j++)
				{
					value = this.frontMeshVertices[this.surfaceVertsCount + j];
					value.y = this.waterLineCurrentLocalPos + this.vertYOffsets[j];
					this.frontMeshVertices[this.surfaceVertsCount + j] = value;
					if (this.water2D.cubeWater)
					{
						for (int k = 0; k < this.water2D.zSegments + 1; k++)
						{
							value = this.topMeshVertices[this.surfaceVertsCount * k + j];
							value.y = this.frontMeshVertices[this.surfaceVertsCount + j].y;
							this.topMeshVertices[this.surfaceVertsCount * k + j] = value;
						}
					}
				}
				int num = this.surfaceVertsCount - this.water2D.prevSurfaceVertsCount;
				for (int l = 0; l < num; l++)
				{
					value = this.frontMeshVertices[this.surfaceVertsCount * 2 - l - 1];
					value.y = this.frontMeshVertices[this.surfaceVertsCount * 2 - num - 1].y;
					this.frontMeshVertices[this.surfaceVertsCount * 2 - l - 1] = value;
					if (this.water2D.cubeWater)
					{
						for (int m = 0; m < this.water2D.zSegments + 1; m++)
						{
							value = this.topMeshVertices[this.surfaceVertsCount * m + this.surfaceVertsCount - l - 1];
							value.y = this.frontMeshVertices[this.surfaceVertsCount * 2 - num - 1].y;
							this.topMeshVertices[this.surfaceVertsCount * m + this.surfaceVertsCount - l - 1] = value;
						}
					}
				}
			}
			if (count > this.surfaceVertsCount)
			{
				for (int n = 0; n < this.surfaceVertsCount; n++)
				{
					value = this.frontMeshVertices[this.surfaceVertsCount + n];
					value.y = this.waterLineCurrentLocalPos + this.vertYOffsets[n];
					this.frontMeshVertices[this.surfaceVertsCount + n] = value;
					if (this.water2D.cubeWater)
					{
						for (int num2 = 0; num2 < this.water2D.zSegments + 1; num2++)
						{
							value = this.topMeshVertices[this.surfaceVertsCount * num2 + n];
							value.y = this.frontMeshVertices[this.surfaceVertsCount + n].y;
							this.topMeshVertices[this.surfaceVertsCount * num2 + n] = value;
						}
					}
				}
			}
			if (this.frontMeshRend.material.HasProperty("_WaterHeight"))
			{
				float value2 = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
				float value3 = Mathf.Abs(this.water2D.handlesPosition[2].y - this.water2D.handlesPosition[3].y);
				this.frontMeshRend.material.SetFloat("_WaterHeight", value2);
				this.frontMeshRend.material.SetFloat("_WaterWidth", value3);
			}
			this.CreateWaterLinePoints();
		}

		private void CreateWaterLinePoints()
		{
			this.waterLinePoints.Clear();
			if (this.surfaceVertsCount > this.meshSegmentsPerWaterLineSegment)
			{
				for (int i = 0; i < this.surfaceVertsCount; i += this.meshSegmentsPerWaterLineSegment)
				{
					this.waterLinePoints.Add(this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + i]));
				}
				if (this.meshSegmentsPerWaterLineSegment != 1)
				{
					Vector2 vector = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + this.surfaceVertsCount - 1]);
					this.waterLinePoints.Add(new Vector2(vector.x, vector.y));
				}
			}
			else
			{
				this.waterLinePoints.Add(this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount]));
				Vector2 vector2 = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + this.surfaceVertsCount - 1]);
				this.waterLinePoints.Add(new Vector2(vector2.x, vector2.y));
			}
		}

		private void UpdateTopLeftRightHandles()
		{
			this.water2D.handlesPosition[0] = new Vector2(this.water2D.handlesPosition[0].x, this.water2D.handlesPosition[1].y + this.defaultWaterHeight + this.waterLineYPosOffset);
			this.water2D.handlesPosition[2] = new Vector2(this.water2D.handlesPosition[2].x, this.water2D.handlesPosition[1].y + this.defaultWaterHeight * 0.5f + this.waterLineYPosOffset * 0.5f);
			this.water2D.handlesPosition[3] = new Vector2(this.water2D.handlesPosition[3].x, this.water2D.handlesPosition[1].y + this.defaultWaterHeight * 0.5f + this.waterLineYPosOffset * 0.5f);
		}

		private void WaterWaves()
		{
			this.leftHandleGlobalPos = this.transform.TransformPoint(this.water2D.handlesPosition[2]);
			int count = this.floatingObjects.Count;
			if (this.waterType == Water2D_Type.Dynamic && count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					this.GenerateWaterWaves(i);
				}
			}
			if (this.surfaceWaves == Water2D_SurfaceWaves.None)
			{
				this.interactionTimeCount += Time.deltaTime;
			}
			if (this.interactionTimeCount < this.interactionTime)
			{
				this.UpdateVertsPosition();
			}
			if (this.clippingMethod == Water2D_ClippingMethod.Complex)
			{
				this.UpdateWaterLinePoints();
			}
		}

		private void UpdateWaterLinePoints()
		{
			int num = 0;
			Vector2 value = Vector2.zero;
			int count = this.waterLinePoints.Count;
			for (int i = 0; i < count - 1; i++)
			{
				value = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + num]);
				this.waterLinePoints[i] = value;
				num += this.meshSegmentsPerWaterLineSegment;
			}
			value = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + this.surfaceVertsCount - 1]);
			this.waterLinePoints[count - 1] = value;
		}

		private void GetVertMinMaxIndex(Vector3 worldPos, float width, out int minIndex, out int maxIndex)
		{
			float num = Mathf.Abs(this.leftHandleGlobalPos.x - worldPos.x);
			float num2 = Mathf.Abs(this.water2D.handlesPosition[3].x - this.water2D.handlesPosition[2].x);
			float num3 = num - width * 0.5f;
			float num4 = num + width * 0.5f;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			if (num4 > num2)
			{
				num4 = num2;
			}
			minIndex = (int)Mathf.Floor(num3 * this.water2D.segmentsPerUnit);
			maxIndex = (int)Mathf.Floor(num4 * this.water2D.segmentsPerUnit);
			if (maxIndex > this.surfaceVertsCount - 1)
			{
				maxIndex = this.surfaceVertsCount - 1;
			}
			if (minIndex < 0)
			{
				minIndex = 0;
			}
		}

		public void GenerateRippleAtPosition(Vector3 rippleWorldPos, float rippleWidth, float vertexVelocity, bool psAndSound)
		{
			int num;
			int num2;
			this.GetVertMinMaxIndex(rippleWorldPos, rippleWidth, out num, out num2);
			for (int i = num; i <= num2; i++)
			{
				this.velocities[i] = vertexVelocity;
			}
			if (psAndSound)
			{
				if (this.particleS != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.particleS, new Vector3(rippleWorldPos.x + this.particleSystemPosOffset.x, this.waterLineCurrentWorldPos.y + this.particleSystemPosOffset.y, rippleWorldPos.z + this.particleSystemPosOffset.z), Quaternion.Euler(new Vector3(270f, 0f, 0f)));
					if (this.particleSystemSorting)
					{
						gameObject.GetComponent<Renderer>().sortingLayerName = this.particleSystemSortingLayerName;
						gameObject.GetComponent<Renderer>().sortingOrder = this.particleSystemOrderInLayer;
					}
					gameObject.transform.parent = this.transform;
					UnityEngine.Object.Destroy(gameObject, 1f);
				}
				if (this.splashSound != null)
				{
					AudioSource.PlayClipAtPoint(this.splashSound, rippleWorldPos);
				}
			}
		}

		private void GenerateWaterWaves(int oIndex)
		{
			if (!this.CanGenerateRipples(oIndex))
			{
				return;
			}
			int minIndex;
			int maxIndex;
			this.GetVertMinMaxIndex(this.floatingObjects[oIndex].bounds.center, this.floatingObjects[oIndex].bounds.extents.x * 2f, out minIndex, out maxIndex);
			if (this.collisionDetectionMode == Water2D_CollisionDetectionMode.RaycastBased)
			{
				this.GenerateRippleBasedOnRaycast(oIndex, minIndex, maxIndex);
			}
			else
			{
				this.GenerateRippleBasedOnPosition(oIndex, minIndex, maxIndex);
			}
		}

		private void GenerateRippleBasedOnRaycast(int oIndex, int minIndex, int maxIndex)
		{
			for (int i = minIndex + 1; i <= maxIndex; i++)
			{
				Vector2 vector = this.frontMeshVertices[i + this.surfaceVertsCount];
				vector = this.transform.TransformPoint(vector);
				int num;
				if (!this.water2D.use3DCollider)
				{
					vector.x -= this.raycastDistance;
					num = Physics2D.RaycastNonAlloc(vector, new Vector2(1f, 0f), this.hit2D, 0.1f, this.collisionLayers.value);
				}
				else
				{
					num = Physics.OverlapSphereNonAlloc(new Vector3(vector.x, vector.y, this.transform.position.z + this.water2D.overlapingSphereZOffset), this.overlapSphereRadius, this.hit3D, this.collisionLayers.value);
				}
				for (int j = 0; j < num; j++)
				{
					if (this.water2D.use3DCollider || !(this.floatingObjects[oIndex].transform != this.hit2D[j].transform))
					{
						if (!this.water2D.use3DCollider || !(this.floatingObjects[oIndex].transform != this.hit3D[j].transform))
						{
							Vector3 velocity = new Vector3(0f, 0f, 0f);
							velocity = this.floatingObjects[oIndex].GetVelocity();
							this.velocities[i] = velocity.y * this.collisionVelocityScale;
							this.interactionTimeCount = 0f;
							if (this.particleS != null && velocity.y < this.velocityFilter - 2f && !this.floatingObjects[oIndex].HasInstantiatedParticleSystem())
							{
								Vector3 position;
								if (!this.water2D.use3DCollider)
								{
									position = this.hit2D[j].transform.position;
								}
								else
								{
									position = this.hit3D[j].transform.position;
								}
								this.InstantiateParticleSystem(oIndex, vector.y, position);
							}
							if (this.splashSound != null && !this.floatingObjects[oIndex].HasPlayedSoundEffect())
							{
								this.PlaySoundEffect(vector, oIndex);
							}
							break;
						}
					}
				}
			}
		}

		private void GenerateRippleBasedOnPosition(int oIndex, int minIndex, int maxIndex)
		{
			Vector3 velocity = this.floatingObjects[oIndex].GetVelocity();
			this.interactionTimeCount = 0f;
			for (int i = minIndex + 1; i <= maxIndex; i++)
			{
				this.velocities[i] = velocity.y * this.collisionVelocityScale;
			}
			if (this.splashSound != null && !this.floatingObjects[oIndex].HasPlayedSoundEffect())
			{
				this.PlaySoundEffect(this.floatingObjects[oIndex].transform.position, oIndex);
			}
			if (this.particleS != null && velocity.y < this.velocityFilter - 2f && !this.floatingObjects[oIndex].HasInstantiatedParticleSystem())
			{
				this.InstantiateParticleSystem(oIndex, this.waterLineCurrentWorldPos.y, this.floatingObjects[oIndex].transform.position);
			}
		}

		private void InstantiateParticleSystem(int oIndex, float yAxisPos, Vector3 pos)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.particleS, new Vector3(pos.x + this.particleSystemPosOffset.x, yAxisPos + this.particleSystemPosOffset.y, pos.z + this.particleSystemPosOffset.z), Quaternion.Euler(new Vector3(270f, 0f, 0f)));
			if (this.particleSystemSorting)
			{
				gameObject.GetComponent<Renderer>().sortingLayerName = this.particleSystemSortingLayerName;
				gameObject.GetComponent<Renderer>().sortingOrder = this.particleSystemOrderInLayer;
			}
			gameObject.transform.parent = this.transform;
			UnityEngine.Object.Destroy(gameObject, 1f);
			this.floatingObjects[oIndex].SetParticleSystemInstantiated(true);
		}

		private void PlaySoundEffect(Vector3 pos, int oIndex)
		{
			AudioSource.PlayClipAtPoint(this.splashSound, pos);
			this.floatingObjects[oIndex].SetSoundPlayed(true);
		}

		private bool CanGenerateRipples(int oIndex)
		{
			if (!this.ColliderExists(oIndex))
			{
				return false;
			}
			Vector2 vector = this.floatingObjects[oIndex].bounds.size;
			float num = vector.y * 0.5f - Mathf.Abs(this.waterLineCurrentWorldPos.y - this.floatingObjects[oIndex].bounds.center.y);
			return this.floatingObjects[oIndex].bounds.center.y >= this.waterLineCurrentWorldPos.y && num <= ((this.interactionRegion <= vector.y) ? this.interactionRegion : vector.y) && this.floatingObjects[oIndex].GetVelocity().y < this.velocityFilter;
		}

		private void UpdateVertsPosition()
		{
			Vector3 value = Vector3.zero;
			for (int i = 0; i < this.surfaceVertsCount; i++)
			{
				float num = this.frontMeshVertices[this.surfaceVertsCount + i].y - this.waterLineCurrentLocalPos;
				this.accelerations[i] = -this.springConstant * num - this.velocities[i] * this.damping;
				List<float> list;
				int index;
				(list = this.velocities)[index = i] = list[index] + this.accelerations[i];
				num += this.velocities[i];
				value = this.frontMeshVertices[this.surfaceVertsCount + i];
				value.y = num + this.waterLineCurrentLocalPos;
				this.frontMeshVertices[this.surfaceVertsCount + i] = value;
			}
			for (int j = 0; j < this.surfaceVertsCount; j++)
			{
				float y = this.frontMeshVertices[this.surfaceVertsCount + j].y;
				if (j > 0)
				{
					float y2 = this.frontMeshVertices[this.surfaceVertsCount + j - 1].y;
					this.leftDeltas[j] = this.spread * (y - y2);
					List<float> list;
					int index2;
					(list = this.velocities)[index2 = j - 1] = list[index2] + this.leftDeltas[j] * this.waveSpeed;
				}
				if (j < this.surfaceVertsCount - 1)
				{
					float y3 = this.frontMeshVertices[this.surfaceVertsCount + j + 1].y;
					this.rightDeltas[j] = this.spread * (y - y3);
					List<float> list;
					int index3;
					(list = this.velocities)[index3 = j + 1] = list[index3] + this.rightDeltas[j] * this.waveSpeed;
				}
			}
			for (int k = 0; k < this.surfaceVertsCount; k++)
			{
				if (k > 0)
				{
					value = this.frontMeshVertices[this.surfaceVertsCount + k - 1];
					value.y += this.leftDeltas[k];
					this.frontMeshVertices[this.surfaceVertsCount + k - 1] = value;
				}
				if (k < this.surfaceVertsCount - 1)
				{
					value = this.frontMeshVertices[this.surfaceVertsCount + k + 1];
					value.y += this.rightDeltas[k];
					this.frontMeshVertices[this.surfaceVertsCount + k + 1] = value;
				}
			}
			if (this.water2D.cubeWater)
			{
				for (int l = 0; l < this.water2D.zSegments + 1; l++)
				{
					for (int m = 0; m < this.surfaceVertsCount; m++)
					{
						value = this.topMeshVertices[l * this.surfaceVertsCount + m];
						value.y = this.frontMeshVertices[this.surfaceVertsCount + m].y;
						this.topMeshVertices[l * this.surfaceVertsCount + m] = value;
					}
				}
			}
		}

		private void PhysicsBasedBuoyantForce()
		{
			this.leftHandleGlobalPos = this.transform.TransformPoint(this.water2D.handlesPosition[2]);
			int count = this.floatingObjects.Count;
			for (int i = 0; i < count; i++)
			{
				this.ApplyPhysicsBasedBuoyantForce(i);
			}
		}

		private void ApplyPhysicsBasedBuoyantForce(int oIndex)
		{
			if (!this.ColliderExists(oIndex))
			{
				return;
			}
			float num = Mathf.Abs(this.leftHandleGlobalPos.x - this.floatingObjects[oIndex].bounds.center.x);
			bool flag = true;
			int num2 = (int)Mathf.Floor(num * this.water2D.segmentsPerUnit);
			if (num2 > this.surfaceVertsCount - 1)
			{
				num2 = this.surfaceVertsCount - 1;
			}
			Vector3 vector;
			if (this.water2DRipple != null)
			{
				vector = this.waterLineCurrentWorldPos;
			}
			else
			{
				vector = this.transform.TransformPoint(this.frontMeshVertices[num2 + this.surfaceVertsCount]);
			}
			this.area = 0f;
			this.displacedMass = 0f;
			if (this.floatingObjects[oIndex].bounds.min.y < vector.y)
			{
				if (this.clippingMethod == Water2D_ClippingMethod.Simple)
				{
					this.linePoints[0] = new Vector2(this.water2D.handlesPosition[2].x, vector.y);
					this.linePoints[1] = new Vector2(this.water2D.handlesPosition[3].x, vector.y);
				}
				if (this.floatingObjects[oIndex].IsPlayer())
				{
					this.boxVertices = this.GetPlayerBoudingBoxVerticesGlobalPos(oIndex);
					if (this.boxVertices[1].y > vector.y)
					{
						return;
					}
					List<Vector2> intersectedPolygon = this.polygonClipping.GetIntersectedPolygon(this.boxVertices, this.linePoints, out flag);
					this.ApplyPhysicsForces(oIndex, intersectedPolygon);
					return;
				}
				else
				{
					List<Vector2> polygon = this.floatingObjects[oIndex].GetPolygon();
					this.ApplyForcesToObject(polygon, this.linePoints, oIndex);
				}
			}
		}

		private void ApplyForcesToObject(List<Vector2> subjPoly, Vector2[] clipPoly, int oIndex)
		{
			this.intersectionPolygon.Clear();
			this.intersectionPolygons.Clear();
			bool flag = true;
			if (this.clippingMethod == Water2D_ClippingMethod.Simple)
			{
				this.intersectionPolygon = this.polygonClipping.GetIntersectedPolygon(subjPoly, clipPoly, out flag);
				if (flag)
				{
					this.ApplyPhysicsForces(oIndex, this.intersectionPolygon);
				}
			}
			else
			{
				int minIndex = 0;
				int maxIndex = 0;
				this.GetVertMinMaxIndex(this.floatingObjects[oIndex].bounds.center, this.floatingObjects[oIndex].bounds.extents.x * 2f, out minIndex, out maxIndex);
				this.CalculateIntersectionPolygons(subjPoly, minIndex, maxIndex, out flag);
				if (flag)
				{
					int count = this.intersectionPolygons.Count;
					for (int i = 0; i < count; i++)
					{
						this.intersectionPolygon = this.intersectionPolygons[i];
						this.ApplyPhysicsForces(oIndex, this.intersectionPolygon);
					}
				}
			}
		}

		private void CalculateIntersectionPolygons(List<Vector2> subjectPoly, int minIndex, int maxIndex, out bool isIntersecting)
		{
			Vector2 vector = this.transform.TransformPoint(this.water2D.handlesPosition[1]);
			this.clipPolygon.Clear();
			this.subjPath.Clear();
			this.clipPath.Clear();
			this.clipper.Clear();
			isIntersecting = true;
			if (this.surfaceVertsCount > this.meshSegmentsPerWaterLineSegment)
			{
				int num = (int)Mathf.Floor((float)(minIndex / this.meshSegmentsPerWaterLineSegment));
				int num2 = (int)Mathf.Floor((float)(maxIndex / this.meshSegmentsPerWaterLineSegment)) + 1;
				if (num2 > this.waterLinePoints.Count - 2)
				{
					num2 = this.waterLinePoints.Count - 2;
				}
				for (int i = num; i <= num2; i++)
				{
					this.clipPolygon.Add(this.waterLinePoints[i]);
				}
				int index = this.clipPolygon.Count - 1;
				this.clipPolygon.Add(new Vector2(this.clipPolygon[index].x, vector.y));
				this.clipPolygon.Add(new Vector2(this.clipPolygon[0].x, vector.y));
			}
			else
			{
				Vector2 item = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount]);
				this.clipPolygon.Add(item);
				item = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + this.surfaceVertsCount - 1]);
				this.clipPolygon.Add(new Vector2(item.x, item.y));
				int index2 = this.clipPolygon.Count - 1;
				this.clipPolygon.Add(new Vector2(this.clipPolygon[index2].x, vector.y));
				this.clipPolygon.Add(new Vector2(this.clipPolygon[0].x, vector.y));
			}
			if (this.showClippingPlolygon)
			{
				for (int j = 0; j < this.clipPolygon.Count; j++)
				{
					if (j < this.clipPolygon.Count - 1)
					{
						UnityEngine.Debug.DrawLine(this.clipPolygon[j], this.clipPolygon[j + 1], Color.green);
					}
					else
					{
						UnityEngine.Debug.DrawLine(this.clipPolygon[j], this.clipPolygon[0], Color.green);
					}
				}
			}
			int count = subjectPoly.Count;
			for (int k = 0; k < count; k++)
			{
				this.subjPath.Add(new IntPoint((double)(subjectPoly[k].x * this.scaleFactor), (double)(subjectPoly[k].y * this.scaleFactor)));
			}
			count = this.clipPolygon.Count;
			for (int l = 0; l < count; l++)
			{
				this.clipPath.Add(new IntPoint((double)(this.clipPolygon[l].x * this.scaleFactor), (double)(this.clipPolygon[l].y * this.scaleFactor)));
			}
			this.clipper.AddPath(this.subjPath, PolyType.ptSubject, true);
			this.clipper.AddPath(this.clipPath, PolyType.ptClip, true);
			this.clipper.Execute(ClipType.ctIntersection, this.solutionPath, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
			if (this.solutionPath.Count != 0)
			{
				count = this.solutionPath.Count;
				for (int m = 0; m < count; m++)
				{
					int count2 = this.solutionPath[m].Count;
					List<Vector2> list = new List<Vector2>();
					for (int n = 0; n < count2; n++)
					{
						list.Add(new Vector2((float)this.solutionPath[m][n].X * this.rScaleFactor, (float)this.solutionPath[m][n].Y * this.rScaleFactor));
					}
					this.intersectionPolygons.Add(list);
				}
			}
			else
			{
				isIntersecting = false;
			}
		}

		public List<Vector2> GetPlayerBoudingBoxVerticesGlobalPos(int oIndex)
		{
			Vector3 position = this.floatingObjects[oIndex].transform.position;
			this.boxVertices[0] = new Vector2(position.x - this.playerBoundingBoxSize.x * 0.5f + this.playerBoundingBoxCenter.x, position.y + this.playerBoundingBoxSize.y * 0.5f + this.playerBoundingBoxCenter.y);
			this.boxVertices[1] = new Vector2(position.x - this.playerBoundingBoxSize.x * 0.5f + this.playerBoundingBoxCenter.x, position.y - this.playerBoundingBoxSize.y * 0.5f + this.playerBoundingBoxCenter.y);
			this.boxVertices[2] = new Vector2(position.x + this.playerBoundingBoxSize.x * 0.5f + this.playerBoundingBoxCenter.x, position.y - this.playerBoundingBoxSize.y * 0.5f + this.playerBoundingBoxCenter.y);
			this.boxVertices[3] = new Vector2(position.x + this.playerBoundingBoxSize.x * 0.5f + this.playerBoundingBoxCenter.x, position.y + this.playerBoundingBoxSize.y * 0.5f + this.playerBoundingBoxCenter.y);
			return this.boxVertices;
		}

		private Vector2 Cross(float a, Vector2 b)
		{
			return new Vector2(a * b.y, -a * b.x);
		}

		private void ApplyPhysicsForces(int oIndex, List<Vector2> intersectionPolygon)
		{
			if (!this.ColliderExists(oIndex))
			{
				return;
			}
			int count = intersectionPolygon.Count;
			this.forcePosition = this.GetPolygonAreaAndCentroid(intersectionPolygon, out this.area);
			this.displacedMass = this.area * this.waterDensity;
			this.upLift = -Physics2D.gravity * this.displacedMass;
			if (this.floatingObjects[oIndex].IsPlayer())
			{
				this.upLift *= this.playerBuoyantForceScale;
			}
			this.floatingObjects[oIndex].AddForceAtPosition(this.upLift, this.forcePosition);
			for (int i = 0; i < count; i++)
			{
				Vector2 vector = intersectionPolygon[i];
				Vector2 vector2 = intersectionPolygon[(i + 1) % count];
				Vector2 vector3 = 0.5f * (vector + vector2);
				Vector3 vector4 = this.floatingObjects[oIndex].GetPointVelocity(vector3);
				float magnitude = vector4.magnitude;
				vector4.Normalize();
				Vector2 vector5 = vector2 - vector;
				float magnitude2 = vector5.magnitude;
				vector5.Normalize();
				Vector2 lhs = this.Cross(-1f, vector5);
				float num = Vector2.Dot(lhs, vector4);
				if (this.waterFlow)
				{
					float f = 0f;
					if (this.useAngles)
					{
						f = this.flowAngle * 0.0174532924f;
					}
					else
					{
						switch (this.flowDirection)
						{
						case Water2D_FlowDirection.Up:
							f = 4.712389f;
							break;
						case Water2D_FlowDirection.Down:
							f = 1.57079637f;
							break;
						case Water2D_FlowDirection.Left:
							f = 0f;
							break;
						case Water2D_FlowDirection.Right:
							f = 3.14159274f;
							break;
						}
					}
					Vector2 vector6 = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
					float num2 = Vector2.Dot(lhs, vector6);
					if (num2 < 0f)
					{
						float d = num2 * magnitude2 * this.waterFlowForce;
						Vector2 v = d * vector6;
						this.floatingObjects[oIndex].AddForceAtPosition(v, vector3);
					}
				}
				if (num <= 0f)
				{
					float num3 = num * this.dragCoefficient * magnitude2 * this.waterDensity * magnitude * magnitude;
					num3 = Mathf.Min(num3, this.maxDrag);
					Vector2 v2 = num3 * vector4;
					this.floatingObjects[oIndex].AddForceAtPosition(v2, vector3);
					float num4 = Vector2.Dot(vector5, vector4);
					float num5 = num * num4 * this.liftCoefficient * magnitude2 * this.waterDensity * magnitude * magnitude;
					num5 = Mathf.Min(num5, this.maxLift);
					Vector2 a = this.Cross(1f, vector4);
					Vector2 v3 = num5 * a;
					this.floatingObjects[oIndex].AddForceAtPosition(v3, vector3);
				}
			}
		}

		public Vector2 GetPolygonAreaAndCentroid(List<Vector2> polygonVertices, out float polygonArea)
		{
			int count = polygonVertices.Count;
			Vector2 vector = new Vector2(0f, 0f);
			polygonArea = 0f;
			for (int i = 0; i < count; i++)
			{
				Vector2 vector2 = polygonVertices[i];
				Vector2 vector3 = (i + 1 >= count) ? polygonVertices[0] : polygonVertices[i + 1];
				float num = vector2.x * vector3.y - vector2.y * vector3.x;
				float num2 = 0.5f * num;
				polygonArea += num2;
				float x = (vector2.x + vector3.x) * num;
				float y = (vector2.y + vector3.y) * num;
				vector += new Vector2(x, y);
			}
			vector *= 1f / (6f * polygonArea);
			if (polygonArea < 0f)
			{
				polygonArea = 0f;
			}
			return vector;
		}

		public float GetPolygonArea(List<Vector2> polygonVertices)
		{
			int count = polygonVertices.Count;
			float num = 0f;
			for (int i = 0; i < count; i++)
			{
				Vector2 vector = polygonVertices[i];
				Vector2 vector2 = (i + 1 >= count) ? polygonVertices[0] : polygonVertices[i + 1];
				float num2 = vector.x * vector2.y - vector.y * vector2.x;
				float num3 = 0.5f * num2;
				num += num3;
			}
			if (num < 0f)
			{
				num = 0f;
			}
			return num;
		}

		private void LinearBuoyantForce()
		{
			this.leftHandleGlobalPos = this.transform.TransformPoint(this.water2D.handlesPosition[2]);
			int count = this.floatingObjects.Count;
			for (int i = 0; i < count; i++)
			{
				this.ApplyLinearBuoyantForce(i);
			}
		}

		private void ApplyLinearBuoyantForce(int oIndex)
		{
			if (!this.ColliderExists(oIndex))
			{
				return;
			}
			this.forcePosition = this.floatingObjects[oIndex].bounds.center + this.floatingObjects[oIndex].transform.TransformDirection(this.forcePositionOffset);
			float num = Mathf.Abs(this.leftHandleGlobalPos.x - this.floatingObjects[oIndex].bounds.center.x);
			int num2 = (int)Mathf.Floor(num * this.water2D.segmentsPerUnit);
			if (num2 > this.surfaceVertsCount - 1)
			{
				num2 = this.surfaceVertsCount - 1;
			}
			Vector3 vector = this.transform.TransformPoint(this.frontMeshVertices[num2 + this.surfaceVertsCount]);
			this.forceFactor = 1f - (this.forcePosition.y - vector.y) / this.floatHeight;
			if (this.forceFactor > 0f && this.floatingObjects[oIndex].bounds.min.y < vector.y)
			{
				this.upLift = -Physics2D.gravity * (this.forceFactor - this.floatingObjects[oIndex].GetVelocity().y * this.bounceDamping) * this.forceScale;
				if (this.floatingObjects[oIndex].IsPlayer())
				{
					this.upLift *= this.playerBuoyantForceScale;
				}
				this.floatingObjects[oIndex].AddForceAtPosition(this.upLift, this.forcePosition);
				if (this.waterFlow)
				{
					float f = 0f;
					if (this.useAngles)
					{
						f = this.flowAngle * 0.0174532924f;
					}
					else
					{
						switch (this.flowDirection)
						{
						case Water2D_FlowDirection.Up:
							f = 1.57079637f;
							break;
						case Water2D_FlowDirection.Down:
							f = 4.712389f;
							break;
						case Water2D_FlowDirection.Left:
							f = 3.14159274f;
							break;
						case Water2D_FlowDirection.Right:
							f = 0f;
							break;
						}
					}
					Vector2 a = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
					Vector2 v = this.waterFlowForce * a;
					this.floatingObjects[oIndex].AddForceAtPosition(v, this.forcePosition);
				}
				Vector3 a2 = this.floatingObjects[oIndex].GetPointVelocity(this.floatingObjects[oIndex].bounds.center);
				float magnitude = a2.magnitude;
				a2.Normalize();
				float d = this.liniarBFDragCoefficient * magnitude * magnitude;
				Vector2 v2 = d * -a2;
				this.floatingObjects[oIndex].AddForceAtPosition(v2, this.floatingObjects[oIndex].bounds.center);
				float torque = this.liniarBFAbgularDragCoefficient * -this.floatingObjects[oIndex].GetAngularVelocity();
				this.floatingObjects[oIndex].AddTorque(torque);
			}
		}

		private void UpdateVariables()
		{
			this.frontMeshVertices = this.water2D.ParentDMesh.meshVerts;
			this.frontMeshUVs = this.water2D.ParentDMesh.meshUVs;
			this.surfaceVertsCount = this.water2D.frontMeshVertsCount / 2;
			int prevSurfaceVertsCount = this.water2D.prevSurfaceVertsCount;
			if (this.water2D.cubeWater)
			{
				this.topMeshVertices = this.water2D.ChildDMesh.meshVerts;
				this.topMeshUVs = this.water2D.ChildDMesh.meshUVs;
			}
			this.waterLinePreviousLocalPos = this.water2D.handlesPosition[1].y + this.defaultWaterHeight + this.waterLineYPosOffset;
			this.waterLineCurrentWorldPos = this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount + 1]);
			this.waterLineCurrentLocalPos = this.water2D.handlesPosition[1].y + this.defaultWaterHeight + this.waterLineYPosOffset;
			if (prevSurfaceVertsCount != this.surfaceVertsCount)
			{
				if (prevSurfaceVertsCount < this.surfaceVertsCount)
				{
					int num = this.surfaceVertsCount - prevSurfaceVertsCount;
					int index = this.velocities.Count - 1;
					for (int i = 0; i < num; i++)
					{
						this.velocities.Add(this.velocities[index]);
						this.accelerations.Add(this.accelerations[index]);
						this.leftDeltas.Add(this.leftDeltas[index]);
						this.rightDeltas.Add(this.rightDeltas[index]);
						this.sineY.Add(this.sineY[index]);
					}
				}
				if (prevSurfaceVertsCount > this.surfaceVertsCount)
				{
					int num2 = prevSurfaceVertsCount - this.surfaceVertsCount;
					for (int j = 0; j < num2; j++)
					{
						int index2 = this.velocities.Count - 1;
						this.velocities.RemoveAt(index2);
						this.accelerations.RemoveAt(index2);
						this.leftDeltas.RemoveAt(index2);
						this.rightDeltas.RemoveAt(index2);
						this.sineY.RemoveAt(index2);
					}
				}
			}
		}

		private void GenerateParticleSystemAndSoundEffect(Vector3 pos)
		{
			if (this.particleS != null)
			{
				this.InstantiateParticleSystem(this.floatingObjects.Count - 1, this.waterLineCurrentWorldPos.y, pos);
			}
			if (this.splashSound != null)
			{
				this.PlaySoundEffect(pos, this.floatingObjects.Count - 1);
			}
		}

		public void ResetVariables()
		{
			this.frontMeshVertices = this.water2D.ParentDMesh.meshVerts;
			this.surfaceVertsCount = this.water2D.frontMeshVertsCount / 2;
			this.frontMeshUVs = this.water2D.ParentDMesh.meshUVs;
			this.frontMeshFilter.bounds = new Bounds(Vector3.zero, Vector3.one * 2000f);
			this.topMeshFilter.bounds = new Bounds(Vector3.zero, Vector3.one * 2000f);
			if (this.water2D.cubeWater)
			{
				this.topMeshVertices = this.water2D.ChildDMesh.meshVerts;
				this.topMeshUVs = this.water2D.ChildDMesh.meshUVs;
				this.defaultWaterHeight = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
				this.originalWaterHeight = Mathf.Abs(this.water2D.handlesPosition[0].y - this.water2D.handlesPosition[1].y);
				this.frontMeshRend.material.SetFloat("_HeightOffset", 0f);
				this.topMeshRend.material.SetFloat("_HeightOffset", 0f);
			}
		}

		public Vector3 GetTopLeftVertexWorldPosition()
		{
			return this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount]);
		}

		public Vector3 GetTopRightVertexWorldPosition()
		{
			return this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount * 2 - 1]);
		}

		public Vector3 GetBottomLeftVertexWorldPosition()
		{
			return this.transform.TransformPoint(this.frontMeshVertices[0]);
		}

		public Vector3 GetBottomRightVertexWorldPosition()
		{
			return this.transform.TransformPoint(this.frontMeshVertices[this.surfaceVertsCount - 1]);
		}

		public Vector3 GetTopLeftVertexLocalPosition()
		{
			return this.frontMeshVertices[this.surfaceVertsCount];
		}

		public Vector3 GetTopRightVertexLocalPosition()
		{
			return this.frontMeshVertices[this.surfaceVertsCount * 2 - 1];
		}

		public Vector3 GetBottomLeftVertexLocalPosition()
		{
			return this.frontMeshVertices[0];
		}

		public Vector3 GetBottomRightVertexLocalPosition()
		{
			return this.frontMeshVertices[this.surfaceVertsCount - 1];
		}

		private void OnTriggerExitPlayer(Vector3 pos)
		{
			if (this.hasRippleScript)
			{
				if (this.playerOnExitPSAndSound)
				{
					this.GenerateParticleSystemAndSoundEffect(pos);
				}
				if (this.playerOnExitRipple)
				{
					this.water2DRipple.playerOnExitRipple = true;
					this.water2DRipple.playerOnExitRipplePos = pos;
				}
			}
			else
			{
				if (!this.playerOnExitRipple && this.playerOnExitPSAndSound)
				{
					this.GenerateParticleSystemAndSoundEffect(pos);
				}
				if (this.playerOnExitRipple)
				{
					float x;
					float vertexVelocity;
					if (this.characterControllerType == Water2D_CharacterControllerType.PhysicsBased)
					{
						x = this.playerBoundingBoxSize.x;
						vertexVelocity = this.playerOnExitVelocity;
					}
					else
					{
						x = this.rippleWidth;
						vertexVelocity = -this.playerOnEnterVelocity;
					}
					this.interactionTimeCount = 0f;
					this.GenerateRippleAtPosition(pos, x, vertexVelocity, this.playerOnExitPSAndSound);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (Water2D_Simulation.IsInLayerMask(this.collisionLayers.value, other.gameObject.layer) && !this.floatingObjects.Exists((FloatingObject o) => o.Equals(other)))
			{
				if (other.tag == "Player" && this.onTriggerPlayerDetected)
				{
					return;
				}
				if (other.tag == "Player" && !this.onTriggerPlayerDetected)
				{
					this.onTriggerPlayerDetected = true;
					FloatingObject2D item = new FloatingObject2D(other, this.polygonCorners, true);
					this.floatingObjects.Add(item);
					if (this.characterControllerType == Water2D_CharacterControllerType.PhysicsBased)
					{
						if (this.hasRippleScript)
						{
							this.GenerateParticleSystemAndSoundEffect(other.transform.position);
						}
					}
					else if (this.hasRippleScript)
					{
						this.GenerateParticleSystemAndSoundEffect(other.transform.position);
					}
					else
					{
						this.interactionTimeCount = 0f;
						this.GenerateRippleAtPosition(other.transform.position, this.rippleWidth, this.playerOnEnterVelocity, true);
					}
				}
				else
				{
					FloatingObject2D item2 = new FloatingObject2D(other, this.polygonCorners, false);
					this.floatingObjects.Add(item2);
					if ((this.hasRippleScript && Mathf.Abs(this.floatingObjects[this.floatingObjects.Count - 1].GetVelocity().y) > 0f) || (!this.springSimulation && this.surfaceWaves == Water2D_SurfaceWaves.SineWaves))
					{
						this.GenerateParticleSystemAndSoundEffect(other.transform.position);
					}
				}
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.tag == "Player" && this.floatingObjects.Exists((FloatingObject o) => o.Equals(other)))
			{
				this.onTriggerPlayerDetected = false;
				this.OnTriggerExitPlayer(other.transform.position);
			}
			this.floatingObjects.RemoveAll((FloatingObject o) => o.Equals(other));
		}

		private void OnTriggerEnter(Collider other)
		{
			if (Water2D_Simulation.IsInLayerMask(this.collisionLayers.value, other.gameObject.layer) && !this.floatingObjects.Exists((FloatingObject o) => o.Equals(other)))
			{
				if (other.tag == "Player" && this.onTriggerPlayerDetected)
				{
					return;
				}
				if (other.tag == "Player" && !this.onTriggerPlayerDetected)
				{
					this.onTriggerPlayerDetected = true;
					FloatingObject3D item = new FloatingObject3D(other, this.polygonCorners, true);
					this.floatingObjects.Add(item);
					if (this.characterControllerType == Water2D_CharacterControllerType.PhysicsBased)
					{
						if (this.hasRippleScript)
						{
							this.GenerateParticleSystemAndSoundEffect(other.transform.position);
						}
					}
					else if (this.hasRippleScript)
					{
						this.GenerateParticleSystemAndSoundEffect(other.transform.position);
					}
					else
					{
						this.interactionTimeCount = 0f;
						this.GenerateRippleAtPosition(other.transform.position, this.rippleWidth, this.playerOnEnterVelocity, true);
					}
				}
				else
				{
					FloatingObject3D item2 = new FloatingObject3D(other, this.polygonCorners, false);
					this.floatingObjects.Add(item2);
					if ((this.hasRippleScript && Mathf.Abs(this.floatingObjects[this.floatingObjects.Count - 1].GetVelocity().y) > 0f) || (!this.springSimulation && this.surfaceWaves == Water2D_SurfaceWaves.SineWaves))
					{
						this.GenerateParticleSystemAndSoundEffect(other.transform.position);
					}
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.tag == "Player" && this.floatingObjects.Exists((FloatingObject o) => o.Equals(other)))
			{
				this.onTriggerPlayerDetected = false;
				this.OnTriggerExitPlayer(other.transform.position);
			}
			this.floatingObjects.RemoveAll((FloatingObject o) => o.Equals(other));
		}

		public static bool IsInLayerMask(int layerMask, int objectLayerIndex)
		{
			if (objectLayerIndex == 4 || objectLayerIndex == 5)
			{
				objectLayerIndex--;
			}
			if (objectLayerIndex > 5)
			{
				objectLayerIndex -= 3;
			}
			int num = 1 << objectLayerIndex;
			return (layerMask & num) != 0;
		}
	}
}
