using System;
using System.Collections.Generic;
using UnityEngine;

namespace Water2DTool
{
	public class Water2D_Ripple : MonoBehaviour
	{
		private float prevWaterLineWorldPos;

		private ShaderParam shaderParam;

		public Vector3 playerOnExitRipplePos = Vector3.zero;

		private RenderTexture tempRT;

		private RenderTexture bufferCurrent;

		private RenderTexture bufferPrev;

		private RenderTexture _finalHeightMap;

		private RenderTextureFormat rtFormat = RenderTextureFormat.RFloat;

		private int renderTextureWidth;

		private int renderTextureHeight;

		private int waterSimCount;

		private int waterInteractions;

		public float rainTimeCounter;

		public float heightMapTimeCounter;

		public float interactionsTimeCounter;

		private bool addInteraction;

		private List<BoxCollider> boxColObstList;

		private List<SphereCollider> sphereColObstList;

		private List<RippleSource> kinematicRippleSourcesList;

		private Material heightMapMaterial;

		private Material heightMapMaterialTexObst;

		private Material heightMapMaterialDynObst;

		private Material bCubicResamplingMat;

		private Material ambientWavesMat;

		private Water2D_Tool water2DTool;

		private Water2D_Simulation water2DSim;

		private Vector4 xyAxisScale = new Vector4(1f, 1f, 0f, 0f);

		private Vector3 leftHandleWorldPos;

		private float texelSize = 0.1f;

		private float waterWidth;

		private float waterLength;

		private BoxCollider waterBoxCollider;

		private Vector3 prevPos;

		private new Transform transform;

		private Transform camTransform;

		private Renderer frontMeshRend;

		private Renderer topMeshRend;

		private float rWaterWidth;

		private float rWaterLength;

		private int prevCamPosRelativeToWaterLine = 1;

		private float interactionsTimeStep = 0.02f;

		public bool playerOnExitRipple;

		public float waveHeightScale = 1f;

		public int rippleSourcesSize = 1;

		public float amplitude1 = 0.02f;

		public float amplitude2 = 0.03f;

		public float amplitude3 = 0.015f;

		public float waveLength1 = 2f;

		public float waveLength2 = 1.5f;

		public float waveLength3 = 1f;

		public float phaseOffset1 = 1f;

		public float phaseOffset2 = 1.5f;

		public float phaseOffset3 = 2f;

		public float amplitudeFallOff = 0.7f;

		public bool ambientWaves;

		public bool amplitudeZAxisFade;

		public float heightUpdateMapTimeStep = 0.015f;

		public bool bicubicResampling;

		public List<RippleSource> rippleSourcesList = new List<RippleSource>();

		public bool rippleSources;

		public float rippleWaterFPSCap = 60f;

		public bool rainDrops;

		public float rainDropRadius = 0.25f;

		public float rainDropStrength = -0.2f;

		public float rainDropFrequency = 20f;

		public float waterDamping = 0.05f;

		public float mouseRadius = 0.3f;

		public float mouseStregth = -0.1f;

		public int rtPixelsToUnits = 12;

		public Texture2D obstructionTexture;

		public bool mouseInteraction = true;

		public float playerRippleRadius = 0.5f;

		public float playerRippleStrength = -0.08f;

		public float playerVelocityFilter = 0.5f;

		public float playerRippleXOffset;

		public float objectRippleRadius = 0.5f;

		public float objectRippleStrength = -0.04f;

		public float objectRadiusScale = 1f;

		public float objectVelocityFilter = 0.5f;

		public float objectXAxisRippleOffset = 0.5f;

		public LayerMask obstructionLayers;

		public Water2D_ObstructionType obstructionType;

		public Water2D_RippleSimulationUpdate rippleSimulationUpdate = Water2D_RippleSimulationUpdate.FixedUpdateMethod;

		public bool drawBothSides;

		public float strengthScale = 6f;

		public bool fixedRadius;

		private void Awake()
		{
			this.kinematicRippleSourcesList = new List<RippleSource>();
			this.heightMapMaterial = new Material(Shader.Find("Hidden/Water_CircularRippleWaves"));
			this.heightMapMaterialDynObst = new Material(Shader.Find("Hidden/Water_CircularRippleWaves_DynamicObs"));
			this.heightMapMaterialTexObst = new Material(Shader.Find("Hidden/Water_CircularRippleWaves_TextureObs"));
			if (this.obstructionTexture != null)
			{
				this.heightMapMaterialTexObst.SetTexture("_ObstructionTex", this.obstructionTexture);
			}
			this.ambientWavesMat = new Material(Shader.Find("Hidden/AmbientWaves"));
			this.bCubicResamplingMat = new Material(Shader.Find("Hidden/BSplineResampling"));
			this.transform = base.GetComponent<Transform>();
		}

		public void InstantiateRenderTextures()
		{
			this.bufferCurrent = null;
			this.bufferPrev = null;
			this._finalHeightMap = null;
			this.water2DSim = base.GetComponent<Water2D_Simulation>();
			this.water2DTool = base.GetComponent<Water2D_Tool>();
			this.renderTextureWidth = this.water2DTool.renderTextureWidth;
			this.renderTextureHeight = this.water2DTool.renderTextureHeight;
			this.bufferCurrent = new RenderTexture(this.renderTextureWidth, this.renderTextureHeight, 0, this.rtFormat, RenderTextureReadWrite.Linear);
			this.bufferCurrent.filterMode = FilterMode.Trilinear;
			this.bufferPrev = new RenderTexture(this.renderTextureWidth, this.renderTextureHeight, 0, this.rtFormat, RenderTextureReadWrite.Linear);
			this.bufferPrev.filterMode = this.bufferCurrent.filterMode;
			this._finalHeightMap = new RenderTexture(this.renderTextureWidth, this.renderTextureHeight, 0, this.rtFormat, RenderTextureReadWrite.Linear);
			this._finalHeightMap.filterMode = this.bufferCurrent.filterMode;
			RenderTexture.active = this.bufferPrev;
			GL.Clear(true, true, Color.grey);
			RenderTexture.active = null;
			RenderTexture.active = this.bufferCurrent;
			GL.Clear(true, true, Color.grey);
			RenderTexture.active = null;
			RenderTexture.active = this._finalHeightMap;
			GL.Clear(true, true, Color.grey);
			RenderTexture.active = null;
			base.GetComponent<Renderer>().material.SetTexture("_MainTex", this._finalHeightMap);
			this.water2DTool.topMeshGameObject.GetComponent<Renderer>().material.mainTexture = this._finalHeightMap;
			this.camTransform = Camera.main.GetComponent<Transform>();
			if (this.water2DTool.cubeWater)
			{
				this.waterBoxCollider = base.GetComponent<BoxCollider>();
			}
			if (this.water2DTool.use3DCollider)
			{
				this.fixedRadius = true;
			}
			this.shaderParam = new ShaderParam();
		}

		private void Start()
		{
			this.xyAxisScale = new Vector4(1f, 1f, 0f, 0f);
			if (this.renderTextureWidth > this.renderTextureHeight)
			{
				this.xyAxisScale = new Vector4((float)this.renderTextureWidth / (float)this.renderTextureHeight, 1f, 0f, 0f);
			}
			if (this.renderTextureWidth < this.renderTextureHeight)
			{
				this.xyAxisScale = new Vector4(1f, (float)this.renderTextureHeight / (float)this.renderTextureWidth, 0f, 0f);
			}
			this.prevPos = this.transform.position;
			this.leftHandleWorldPos = this.transform.TransformPoint(this.water2DTool.handlesPosition[2]);
			if (this.water2DTool.width > this.water2DTool.length)
			{
				this.texelSize = 1f / this.water2DTool.length;
			}
			else
			{
				this.texelSize = 1f / this.water2DTool.width;
			}
			this.boxColObstList = new List<BoxCollider>();
			this.sphereColObstList = new List<SphereCollider>();
			this.waterWidth = this.water2DTool.width;
			this.waterLength = this.water2DTool.length;
			this.rWaterWidth = 1f / this.waterWidth;
			this.rWaterLength = 1f / this.waterLength;
			this.interactionsTimeStep = 0.0166666675f;
			this.frontMeshRend = base.GetComponent<Renderer>();
			this.topMeshRend = this.water2DTool.topMeshGameObject.GetComponent<Renderer>();
			this.SetAmbientWavesShaderParameters();
			this.UpdateRippleShaderParameters();
			if (this.camTransform.position.y < this.water2DSim.waterLineCurrentWorldPos.y)
			{
				this.prevCamPosRelativeToWaterLine = -1;
			}
			else
			{
				this.prevCamPosRelativeToWaterLine = 1;
			}
			this.SetAmbientWavesShaderParameters();
			this.frontMeshRend.material.SetFloat(this.shaderParam.bottomPosID, this.water2DTool.handlesPosition[1].y);
			this.topMeshRend.material.SetFloat(this.shaderParam.bottomPosID, this.water2DTool.handlesPosition[1].y);
			this.SetWaveHeightScale();
			this.prevWaterLineWorldPos = this.transform.TransformPoint(this.water2DTool.handlesPosition[0]).y;
			Camera.main.depthTextureMode = DepthTextureMode.Depth;
			this.SetWaterLinePosition();
		}

		public void SetWaveHeightScale()
		{
			this.frontMeshRend.material.SetFloat(this.shaderParam.waveHeightScaleID, this.waveHeightScale);
			this.topMeshRend.material.SetFloat(this.shaderParam.waveHeightScaleID, this.waveHeightScale);
		}

		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(this.bufferCurrent);
			UnityEngine.Object.Destroy(this.bufferPrev);
			UnityEngine.Object.Destroy(this._finalHeightMap);
			this.bufferCurrent = null;
			this.bufferPrev = null;
			this._finalHeightMap = null;
		}

		private void Update()
		{
			this.WaterObjectMoved();
			this.TopMeshCulling();
			if (this.rippleSimulationUpdate == Water2D_RippleSimulationUpdate.UpdateMethod)
			{
				this.RippleWaterSimulationUpdate();
			}
		}

		private void FixedUpdate()
		{
			if (this.rippleSimulationUpdate == Water2D_RippleSimulationUpdate.FixedUpdateMethod)
			{
				this.RippleWaterSimulationFixedUpdate();
			}
		}

		private void RippleWaterSimulationUpdate()
		{
			if (this.obstructionType == Water2D_ObstructionType.DynamicObstruction)
			{
				this.UpdateDynamicObstructions();
			}
			this.AddRain();
			this.interactionsTimeCounter += Time.deltaTime;
			if (this.interactionsTimeCounter > this.interactionsTimeStep)
			{
				this.interactionsTimeCounter -= this.interactionsTimeStep;
				this.UpdateWaterInteractions();
				this.UpdateHeightMapWithWaterInteractions();
			}
			this.heightMapTimeCounter += Time.deltaTime;
			if (this.heightMapTimeCounter > this.heightUpdateMapTimeStep)
			{
				int num = 0;
				while (this.heightMapTimeCounter > this.heightUpdateMapTimeStep)
				{
					this.heightMapTimeCounter -= this.heightUpdateMapTimeStep;
					this.UpdateHeightMap();
					num++;
					if (num > 5)
					{
						break;
					}
				}
			}
		}

		private void RippleWaterSimulationFixedUpdate()
		{
			if (this.obstructionType == Water2D_ObstructionType.DynamicObstruction)
			{
				this.UpdateDynamicObstructions();
			}
			this.AddRain();
			this.UpdateWaterInteractions();
			this.UpdateHeightMapWithWaterInteractions();
			this.UpdateHeightMap();
		}

		private void UpdateHeightMapWithWaterInteractions()
		{
			if (this.addInteraction)
			{
				if (this.waterSimCount % 2 == 0)
				{
					RenderTexture temporary = RenderTexture.GetTemporary(this.bufferCurrent.width, this.bufferCurrent.height, 0, this.rtFormat, RenderTextureReadWrite.Linear);
					temporary.filterMode = this.bufferCurrent.filterMode;
					Water2D_ObstructionType water2D_ObstructionType = this.obstructionType;
					if (water2D_ObstructionType != Water2D_ObstructionType.None)
					{
						if (water2D_ObstructionType != Water2D_ObstructionType.TextureObstruction)
						{
							if (water2D_ObstructionType == Water2D_ObstructionType.DynamicObstruction)
							{
								Graphics.Blit(this.bufferCurrent, temporary, this.heightMapMaterialDynObst, 0);
							}
						}
						else
						{
							Graphics.Blit(this.bufferCurrent, temporary, this.heightMapMaterialTexObst, 0);
						}
					}
					else
					{
						Graphics.Blit(this.bufferCurrent, temporary, this.heightMapMaterial, 0);
					}
					Graphics.Blit(temporary, this.bufferCurrent);
					RenderTexture.ReleaseTemporary(temporary);
				}
				else
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary(this.bufferPrev.width, this.bufferPrev.height, 0, this.rtFormat, RenderTextureReadWrite.Linear);
					temporary2.filterMode = this.bufferPrev.filterMode;
					Water2D_ObstructionType water2D_ObstructionType2 = this.obstructionType;
					if (water2D_ObstructionType2 != Water2D_ObstructionType.None)
					{
						if (water2D_ObstructionType2 != Water2D_ObstructionType.TextureObstruction)
						{
							if (water2D_ObstructionType2 == Water2D_ObstructionType.DynamicObstruction)
							{
								Graphics.Blit(this.bufferPrev, temporary2, this.heightMapMaterialDynObst, 0);
							}
						}
						else
						{
							Graphics.Blit(this.bufferPrev, temporary2, this.heightMapMaterialTexObst, 0);
						}
					}
					else
					{
						Graphics.Blit(this.bufferPrev, temporary2, this.heightMapMaterial, 0);
					}
					Graphics.Blit(temporary2, this.bufferPrev);
					RenderTexture.ReleaseTemporary(temporary2);
				}
			}
			this.ResetWaterInteractionParameters();
		}

		private void UpdateHeightMap()
		{
			RenderTexture source = (this.waterSimCount % 2 != 0) ? this.bufferPrev : this.bufferCurrent;
			RenderTexture renderTexture = (this.waterSimCount % 2 != 0) ? this.bufferCurrent : this.bufferPrev;
			this.waterSimCount++;
			if (this.bicubicResampling || this.ambientWaves)
			{
				this.tempRT = RenderTexture.GetTemporary(this.bufferCurrent.width, this.bufferCurrent.height, 0, this.rtFormat, RenderTextureReadWrite.Linear);
				this.tempRT.filterMode = this.bufferCurrent.filterMode;
			}
			else
			{
				this.tempRT = null;
			}
			Water2D_ObstructionType water2D_ObstructionType = this.obstructionType;
			if (water2D_ObstructionType != Water2D_ObstructionType.None)
			{
				if (water2D_ObstructionType != Water2D_ObstructionType.TextureObstruction)
				{
					if (water2D_ObstructionType == Water2D_ObstructionType.DynamicObstruction)
					{
						this.heightMapMaterialDynObst.SetTexture(this.shaderParam.prevTexID, renderTexture);
						Graphics.Blit(source, this._finalHeightMap, this.heightMapMaterialDynObst, 1);
					}
				}
				else
				{
					this.heightMapMaterialTexObst.SetTexture(this.shaderParam.prevTexID, renderTexture);
					Graphics.Blit(source, this._finalHeightMap, this.heightMapMaterialTexObst, 1);
				}
			}
			else
			{
				this.heightMapMaterial.SetTexture(this.shaderParam.prevTexID, renderTexture);
				Graphics.Blit(source, this._finalHeightMap, this.heightMapMaterial, 1);
			}
			if (this.bicubicResampling)
			{
				Graphics.Blit(this._finalHeightMap, this.tempRT, this.bCubicResamplingMat);
				Graphics.Blit(this.tempRT, this._finalHeightMap);
			}
			Graphics.Blit(this._finalHeightMap, renderTexture);
			if (this.bicubicResampling || this.ambientWaves)
			{
				RenderTexture.active = this.tempRT;
				GL.Clear(true, true, Color.grey);
				RenderTexture.active = null;
			}
			if (this.ambientWaves)
			{
				Graphics.Blit(this._finalHeightMap, this.tempRT, this.ambientWavesMat);
				Graphics.Blit(this.tempRT, this._finalHeightMap);
			}
			if (this.bicubicResampling || this.ambientWaves)
			{
				RenderTexture.ReleaseTemporary(this.tempRT);
			}
		}

		private void TopMeshCulling()
		{
			bool flag = false;
			int num;
			if (this.camTransform.position.y < this.water2DSim.waterLineCurrentWorldPos.y)
			{
				num = -1;
			}
			else
			{
				num = 1;
			}
			if (num != this.prevCamPosRelativeToWaterLine)
			{
				this.prevCamPosRelativeToWaterLine = num;
				flag = true;
			}
			if (flag)
			{
				this.SetTopMeshCulling();
			}
		}

		public void SetTopMeshCulling()
		{
			if (this.camTransform.position.y < this.water2DSim.waterLineCurrentWorldPos.y)
			{
				int value = (!this.drawBothSides) ? 1 : 0;
				this.topMeshRend.material.SetInt(this.shaderParam.faceCullingID, value);
				this.topMeshRend.material.SetInt(this.shaderParam.oneOrZeroID, 1);
			}
			else
			{
				this.topMeshRend.material.SetInt(this.shaderParam.faceCullingID, 2);
				this.topMeshRend.material.SetInt(this.shaderParam.oneOrZeroID, 0);
			}
		}

		private void SetWaterLinePosition()
		{
			int count = this.rippleSourcesList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.rippleSourcesList[i] != null)
				{
					this.rippleSourcesList[i].waterLineYAxisWorldPosition = this.water2DSim.waterLineCurrentWorldPos.y;
				}
			}
			count = this.kinematicRippleSourcesList.Count;
			for (int j = 0; j < count; j++)
			{
				if (this.kinematicRippleSourcesList[j] != null)
				{
					this.kinematicRippleSourcesList[j].waterLineYAxisWorldPosition = this.water2DSim.waterLineCurrentWorldPos.y;
				}
			}
			this.prevWaterLineWorldPos = this.water2DSim.waterLineCurrentWorldPos.y;
		}

		private void AddRain()
		{
			if (this.rainDrops)
			{
				if (this.rippleSimulationUpdate == Water2D_RippleSimulationUpdate.UpdateMethod)
				{
					this.rainTimeCounter += Time.deltaTime;
				}
				else
				{
					this.rainTimeCounter += Time.fixedDeltaTime;
				}
				int num = 0;
				float num2 = 1f / this.rainDropFrequency;
				if (this.rainTimeCounter > num2)
				{
					this.addInteraction = true;
					while (this.rainTimeCounter > num2)
					{
						this.rainTimeCounter -= num2;
						float uvRadius = this.WorldSpaceValueToUV(this.rainDropRadius);
						Vector2 uvPos = new Vector2(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
						this.AddRippleToShader(this.waterInteractions, uvPos, uvRadius, this.rainDropStrength);
						this.waterInteractions++;
						num++;
						if (num > 6)
						{
							break;
						}
					}
				}
			}
		}

		public void UpdateRippleShaderParameters()
		{
			Water2D_ObstructionType water2D_ObstructionType = this.obstructionType;
			if (water2D_ObstructionType != Water2D_ObstructionType.None)
			{
				if (water2D_ObstructionType != Water2D_ObstructionType.TextureObstruction)
				{
					if (water2D_ObstructionType == Water2D_ObstructionType.DynamicObstruction)
					{
						this.heightMapMaterialDynObst.SetFloat(this.shaderParam.dampingID, this.waterDamping);
						this.heightMapMaterialDynObst.SetVector(this.shaderParam.axisScaleID, this.xyAxisScale);
					}
				}
				else
				{
					this.heightMapMaterialTexObst.SetFloat(this.shaderParam.dampingID, this.waterDamping);
					this.heightMapMaterialTexObst.SetVector(this.shaderParam.axisScaleID, this.xyAxisScale);
				}
			}
			else
			{
				this.heightMapMaterial.SetFloat(this.shaderParam.dampingID, this.waterDamping);
				this.heightMapMaterial.SetVector(this.shaderParam.axisScaleID, this.xyAxisScale);
			}
		}

		private void UpdateWaterInteractions()
		{
			if (Mathf.Abs(this.prevWaterLineWorldPos - this.water2DSim.waterLineCurrentWorldPos.y) > 0.0001f)
			{
				this.SetWaterLinePosition();
			}
			this.DynamicObjectsInteraction();
			if (this.water2DTool.use3DCollider)
			{
				this.MouseOnWater();
			}
			this.RippleSourcesInteraction();
			if (this.playerOnExitRipple)
			{
				this.AddRippleAtPosition(this.playerOnExitRipplePos, this.playerRippleRadius, this.playerRippleStrength * this.strengthScale);
				this.playerOnExitRipple = false;
			}
		}

		private void DynamicObjectsInteraction()
		{
			if (this.water2DSim.floatingObjects.Count > 0)
			{
				int count = this.water2DSim.floatingObjects.Count;
				for (int i = 0; i < count; i++)
				{
					float colliderTopPoint = this.GetColliderTopPoint(this.water2DSim.floatingObjects[i]);
					if (colliderTopPoint > this.water2DSim.waterLineCurrentWorldPos.y)
					{
						if (this.water2DSim.floatingObjects[i].IsPlayer())
						{
							this.PlayerInteraction(i);
						}
						else
						{
							this.ObjectInteraction(i);
						}
					}
				}
			}
		}

		private void PlayerInteraction(int oIndex)
		{
			if (this.water2DSim.characterControllerType == Water2D_CharacterControllerType.PhysicsBased)
			{
				Vector3 velocity = this.water2DSim.floatingObjects[oIndex].GetVelocity();
				float scale;
				if (this.CanGenerateRipple(velocity, oIndex, out scale))
				{
					this.PhysicsBasedCharacterController(oIndex, velocity, scale);
				}
			}
			else
			{
				this.RaycastBasedCharacterController(oIndex);
			}
		}

		public void PhysicsBasedCharacterController(int oIndex, Vector3 vel, float scale)
		{
			Vector3 a = Vector3.zero;
			Vector3 zero = Vector3.zero;
			a = this.water2DSim.floatingObjects[oIndex].transform.position;
			this.addInteraction = true;
			if (Mathf.Abs(vel.x) > this.playerVelocityFilter)
			{
				zero.x = this.playerRippleXOffset;
			}
			float uvRadius = this.WorldSpaceValueToUV(this.playerRippleRadius);
			Vector2 uVFromPosition;
			if (vel.x > 0f)
			{
				uVFromPosition = this.GetUVFromPosition(a + new Vector3(zero.x, 0f, 0f));
			}
			else
			{
				uVFromPosition = this.GetUVFromPosition(a - new Vector3(zero.x, 0f, 0f));
			}
			this.AddRippleToShader(this.waterInteractions, uVFromPosition, uvRadius, this.playerRippleStrength * scale);
			this.waterInteractions++;
		}

		private void RaycastBasedCharacterController(int oIndex)
		{
			Vector3 zero = Vector3.zero;
			Vector3 position = this.water2DSim.floatingObjects[oIndex].transform.position;
			Vector3 previousPosition = this.water2DSim.floatingObjects[oIndex].GetPreviousPosition();
			float num = Mathf.Abs(position.x - previousPosition.x);
			float num2 = Mathf.Abs(position.y - previousPosition.y);
			if (num < 0.0001f && num2 < 0.0001f)
			{
				this.water2DSim.floatingObjects[oIndex].SetPreviousPosition();
				return;
			}
			if (position.x > previousPosition.x)
			{
				zero = new Vector3(this.playerRippleXOffset, 0f, 0f);
			}
			else
			{
				zero = new Vector3(-this.playerRippleXOffset, 0f, 0f);
			}
			if (num > num2)
			{
				this.AddRippleAtPosition(position + zero, this.playerRippleRadius, this.playerRippleStrength);
			}
			else
			{
				int num3;
				if (position.y > previousPosition.y)
				{
					num3 = 1;
				}
				else
				{
					num3 = -1;
				}
				if (this.water2DSim.floatingObjects[oIndex].GetPreviousDirectionOnYAxis() != num3)
				{
					this.water2DSim.floatingObjects[oIndex].SetDirectionOnYAxis(num3);
					this.AddRippleAtPosition(position + zero, this.playerRippleRadius, this.strengthScale * this.playerRippleStrength);
				}
			}
			this.water2DSim.floatingObjects[oIndex].SetPreviousPosition();
		}

		private bool CanGenerateRipple(Vector3 vel, int oIndex, out float scale)
		{
			float num = Mathf.Abs(vel.x);
			float num2 = Mathf.Abs(vel.y);
			scale = 1f;
			float num3;
			if (this.water2DSim.floatingObjects[oIndex].IsPlayer())
			{
				num3 = this.playerVelocityFilter;
			}
			else
			{
				num3 = this.objectVelocityFilter;
			}
			if (num > num3)
			{
				return true;
			}
			if (num2 <= num3)
			{
				return false;
			}
			int num4;
			if (vel.y > 0f)
			{
				num4 = 1;
			}
			else
			{
				num4 = -1;
			}
			if (this.water2DSim.floatingObjects[oIndex].GetPreviousDirectionOnYAxis() != num4)
			{
				this.water2DSim.floatingObjects[oIndex].SetDirectionOnYAxis(num4);
				scale = this.strengthScale;
				return true;
			}
			return false;
		}

		public void AddRippleAtPosition(Vector3 pos, float radius, float strength)
		{
			if (this.IsInside(pos))
			{
				this.addInteraction = true;
				float uvRadius = this.WorldSpaceValueToUV(radius);
				Vector2 uVFromPosition = this.GetUVFromPosition(pos);
				this.AddRippleToShader(this.waterInteractions, uVFromPosition, uvRadius, strength);
				this.waterInteractions++;
			}
		}

		private void ObjectInteraction(int oIndex)
		{
			Vector3 velocity = this.water2DSim.floatingObjects[oIndex].GetVelocity();
			float num;
			if (!this.CanGenerateRipple(velocity, oIndex, out num))
			{
				return;
			}
			Vector3 a = Vector3.zero;
			Vector3 zero = Vector3.zero;
			a = this.water2DSim.floatingObjects[oIndex].bounds.center;
			this.addInteraction = true;
			float radius;
			if (this.fixedRadius)
			{
				radius = this.objectRippleRadius;
			}
			else
			{
				radius = this.water2DSim.floatingObjects[oIndex].GetRadius() * this.objectRadiusScale;
			}
			float uvRadius = this.WorldSpaceValueToUV(radius);
			if (Mathf.Abs(velocity.x) > this.objectVelocityFilter)
			{
				zero.x = this.water2DSim.floatingObjects[oIndex].bounds.extents.x * this.objectXAxisRippleOffset * 2f;
			}
			Vector2 uVFromPosition;
			if (velocity.x > 0f)
			{
				uVFromPosition = this.GetUVFromPosition(a + new Vector3(zero.x, 0f, 0f));
			}
			else
			{
				uVFromPosition = this.GetUVFromPosition(a - new Vector3(zero.x, 0f, 0f));
			}
			this.AddRippleToShader(this.waterInteractions, uVFromPosition, uvRadius, this.objectRippleStrength * num);
			this.waterInteractions++;
		}

		private void RippleSourcesInteraction()
		{
			int count;
			if (this.rippleSources)
			{
				count = this.rippleSourcesList.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.rippleSourcesList[i] != null && this.rippleSourcesList[i].newRipple && this.IsInside(this.rippleSourcesList[i].transform.position))
					{
						float uvRadius = this.WorldSpaceValueToUV(this.rippleSourcesList[i].radius);
						Vector2 uVFromPosition = this.GetUVFromPosition(this.rippleSourcesList[i].transform.position);
						this.AddRippleToShader(this.waterInteractions, uVFromPosition, uvRadius, this.rippleSourcesList[i].strength);
						this.rippleSourcesList[i].newRipple = false;
						this.addInteraction = true;
						this.waterInteractions++;
					}
				}
			}
			count = this.kinematicRippleSourcesList.Count;
			for (int j = 0; j < count; j++)
			{
				if (this.kinematicRippleSourcesList[j] != null && this.kinematicRippleSourcesList[j].newRipple && this.IsInside(this.kinematicRippleSourcesList[j].transform.position))
				{
					float uvRadius = this.WorldSpaceValueToUV(this.kinematicRippleSourcesList[j].radius);
					Vector2 uVFromPosition = this.GetUVFromPosition(this.kinematicRippleSourcesList[j].transform.position);
					this.AddRippleToShader(this.waterInteractions, uVFromPosition, uvRadius, this.kinematicRippleSourcesList[j].strength);
					this.kinematicRippleSourcesList[j].newRipple = false;
					this.addInteraction = true;
					this.waterInteractions++;
				}
			}
		}

		private bool IsInside(Vector3 point)
		{
			return point.x > this.leftHandleWorldPos.x && point.x < this.leftHandleWorldPos.x + this.waterWidth && point.z > this.leftHandleWorldPos.z && point.z < this.leftHandleWorldPos.z + this.waterLength;
		}

		private void MouseOnWater()
		{
			if (this.mouseInteraction && Input.GetMouseButton(0))
			{
				Camera main = Camera.main;
				RaycastHit raycastHit;
				if (this.waterBoxCollider.Raycast(main.ScreenPointToRay(UnityEngine.Input.mousePosition), out raycastHit, 3.40282347E+38f))
				{
					this.addInteraction = true;
					float uvRadius = this.WorldSpaceValueToUV(this.mouseRadius);
					Vector2 uVFromPosition = this.GetUVFromPosition(raycastHit.point);
					this.AddRippleToShader(this.waterInteractions, uVFromPosition, uvRadius, this.mouseStregth);
					this.waterInteractions++;
				}
			}
		}

		private void ResetWaterInteractionParameters()
		{
			if (this.addInteraction)
			{
				int num = (this.waterInteractions > 10) ? 10 : this.waterInteractions;
				for (int i = 1; i <= num; i++)
				{
					this.AddRippleToShader(i, Vector2.zero, 0f, 0f);
				}
				this.addInteraction = false;
				this.waterInteractions = 0;
			}
		}

		private float GetColliderTopPoint(FloatingObject floatingObj)
		{
			return floatingObj.bounds.extents.y + floatingObj.bounds.center.y;
		}

		private Vector2 GetUVFromPosition(Vector3 worldPos)
		{
			float x = Mathf.Abs(worldPos.x - this.leftHandleWorldPos.x) * this.rWaterWidth;
			float num = Mathf.Abs(worldPos.z - this.transform.position.z) * this.rWaterLength;
			Vector2 result = new Vector2(x, 1f - num);
			return result;
		}

		private float WorldSpaceValueToUV(float radius)
		{
			return this.texelSize * radius;
		}

		private void AddRippleToShader(int index, Vector2 uvPos, float uvRadius, float strength)
		{
			if (index < 10)
			{
				Water2D_ObstructionType water2D_ObstructionType = this.obstructionType;
				if (water2D_ObstructionType != Water2D_ObstructionType.None)
				{
					if (water2D_ObstructionType != Water2D_ObstructionType.TextureObstruction)
					{
						if (water2D_ObstructionType == Water2D_ObstructionType.DynamicObstruction)
						{
							this.heightMapMaterialDynObst.SetVector(this.shaderParam.WaterRippleID[index], new Vector4(uvPos.x, uvPos.y, uvRadius, strength));
						}
					}
					else
					{
						this.heightMapMaterialTexObst.SetVector(this.shaderParam.WaterRippleID[index], new Vector4(uvPos.x, uvPos.y, uvRadius, strength));
					}
				}
				else
				{
					this.heightMapMaterial.SetVector(this.shaderParam.WaterRippleID[index], new Vector4(uvPos.x, uvPos.y, uvRadius, strength));
				}
			}
		}

		private void UpdateDynamicObstructions()
		{
			int num = this.boxColObstList.Count;
			int num2 = this.sphereColObstList.Count;
			if (num > 5)
			{
				num = 5;
			}
			if (num2 > 5)
			{
				num2 = 5;
			}
			for (int i = 0; i < 5; i++)
			{
				Vector4 value = new Vector4(0f, 0f, 0f, 0f);
				this.heightMapMaterialDynObst.SetVector(this.shaderParam.recObstVarIDs[i], value);
			}
			for (int j = 0; j < 5; j++)
			{
				Vector3 v = new Vector3(0f, 0f, 0f);
				this.heightMapMaterialDynObst.SetVector(this.shaderParam.cirObstVarIDs[j], v);
			}
			for (int k = 0; k < num; k++)
			{
				Vector4 rectangleObstruction = this.GetRectangleObstruction(this.boxColObstList[k]);
				this.heightMapMaterialDynObst.SetVector(this.shaderParam.recObstVarIDs[k], rectangleObstruction);
			}
			for (int l = 0; l < num2; l++)
			{
				Vector4 value2 = this.GetCicleObstruction(this.sphereColObstList[l]);
				this.heightMapMaterialDynObst.SetVector(this.shaderParam.cirObstVarIDs[l], value2);
			}
		}

		private Vector3 GetCicleObstruction(SphereCollider sphere)
		{
			Vector3 vector = this.transform.TransformPoint(this.water2DTool.handlesPosition[0]);
			Vector3 position = sphere.transform.position;
			float num = sphere.transform.localScale.x;
			if (sphere.transform.localScale.y > sphere.transform.localScale.x)
			{
				num = sphere.transform.localScale.y;
			}
			if (sphere.transform.localScale.z > sphere.transform.localScale.y && sphere.transform.localScale.z > sphere.transform.localScale.x)
			{
				num = sphere.transform.localScale.y;
			}
			float num2 = sphere.radius * num;
			float num3 = Mathf.Abs(position.y - vector.y);
			float num4 = Mathf.Sqrt(num2 * num2 - num3 * num3);
			float x;
			if (this.leftHandleWorldPos.x < position.x && this.leftHandleWorldPos.x + this.waterWidth > position.x)
			{
				x = Mathf.Abs(this.leftHandleWorldPos.x - position.x) * this.rWaterWidth;
			}
			else if (this.leftHandleWorldPos.x >= position.x)
			{
				x = 0f;
			}
			else
			{
				x = 1f;
			}
			float y;
			if (this.leftHandleWorldPos.z < position.z && this.leftHandleWorldPos.z + this.waterLength > position.z)
			{
				y = 1f - Mathf.Abs(this.leftHandleWorldPos.z - position.z) * this.rWaterLength;
			}
			else if (this.leftHandleWorldPos.z >= position.z)
			{
				y = 1f;
			}
			else
			{
				y = 0f;
			}
			num4 *= this.rWaterLength;
			return new Vector3(x, y, num4);
		}

		private Vector4 GetRectangleObstruction(Collider other)
		{
			Vector3 position = other.transform.position;
			Vector3 extents = other.bounds.extents;
			Vector3 vector = this.transform.TransformPoint(this.water2DTool.handlesPosition[0]);
			if (other.bounds.center.y + other.bounds.extents.y < vector.y)
			{
				return new Vector4(0f, 0f, 0f, 0f);
			}
			float num;
			float num2;
			if (this.leftHandleWorldPos.x < position.x && this.leftHandleWorldPos.x + this.waterWidth > position.x)
			{
				num = (Mathf.Abs(this.leftHandleWorldPos.x - position.x) - extents.x) * this.rWaterWidth;
				num2 = (Mathf.Abs(this.leftHandleWorldPos.x - position.x) + extents.x) * this.rWaterWidth;
			}
			else if (this.leftHandleWorldPos.x >= position.x)
			{
				num = 0f;
				num2 = Mathf.Abs(extents.x - (this.leftHandleWorldPos.x - position.x)) * this.rWaterWidth;
			}
			else
			{
				num = Mathf.Abs(extents.x - (position.x - this.leftHandleWorldPos.x + this.waterLength)) * this.rWaterWidth;
				num2 = 1f;
			}
			float num3;
			float num4;
			if (this.leftHandleWorldPos.z < position.z && this.leftHandleWorldPos.z + this.waterLength > position.z)
			{
				num3 = 1f - (Mathf.Abs(this.leftHandleWorldPos.z - position.z) - extents.z) * this.rWaterLength;
				num4 = 1f - (Mathf.Abs(this.leftHandleWorldPos.z - position.z) + extents.z) * this.rWaterLength;
			}
			else if (this.leftHandleWorldPos.z >= position.z)
			{
				num4 = 1f - Mathf.Abs(extents.z - (this.leftHandleWorldPos.z - position.z)) * this.rWaterLength;
				num3 = 1f;
			}
			else
			{
				num3 = 1f - Mathf.Abs(extents.z - (position.z - this.leftHandleWorldPos.z)) * this.rWaterLength;
				num4 = 0f;
			}
			if (num < 0f)
			{
				num = 0f;
			}
			if (num4 < 0f)
			{
				num4 = 0f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			if (num3 > 1f)
			{
				num3 = 1f;
			}
			return new Vector4(num, num4, num2, num3);
		}

		public void SetAmbientWavesShaderParameters()
		{
			this.ambientWavesMat.SetFloat(this.shaderParam.waterWidthID, this.water2DTool.width);
			this.ambientWavesMat.SetFloat(this.shaderParam.amplitude1ID, this.amplitude1);
			this.ambientWavesMat.SetFloat(this.shaderParam.amplitude2ID, this.amplitude2);
			this.ambientWavesMat.SetFloat(this.shaderParam.waveLength1ID, this.waveLength1);
			this.ambientWavesMat.SetFloat(this.shaderParam.waveLength2ID, this.waveLength2);
			this.ambientWavesMat.SetFloat(this.shaderParam.phaseOffset1ID, this.phaseOffset1);
			this.ambientWavesMat.SetFloat(this.shaderParam.phaseOffset2ID, this.phaseOffset2);
			this.ambientWavesMat.SetFloat(this.shaderParam.fallOffID, this.amplitudeFallOff);
			if (this.amplitudeZAxisFade)
			{
				this.ambientWavesMat.SetFloat(this.shaderParam.amplitudeFadeID, 1f);
			}
			else
			{
				this.ambientWavesMat.SetFloat(this.shaderParam.amplitudeFadeID, 0f);
			}
		}

		private void WaterObjectMoved()
		{
			if (this.prevPos != this.transform.position)
			{
				this.leftHandleWorldPos = this.transform.TransformPoint(this.water2DTool.handlesPosition[2]);
				this.prevPos = this.transform.position;
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			RippleSource component = other.GetComponent<RippleSource>();
			if (component)
			{
				this.kinematicRippleSourcesList.Add(component);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			RippleSource component = other.GetComponent<RippleSource>();
			if (component && this.kinematicRippleSourcesList.Contains(component))
			{
				this.kinematicRippleSourcesList.Remove(component);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (Water2D_Ripple.IsInLayerMask(this.obstructionLayers.value, other.gameObject.layer))
			{
				BoxCollider component = other.GetComponent<BoxCollider>();
				if (component != null && !this.boxColObstList.Contains(component))
				{
					this.boxColObstList.Add(component);
				}
				SphereCollider component2 = other.GetComponent<SphereCollider>();
				if (component2 != null && !this.sphereColObstList.Contains(component2))
				{
					this.sphereColObstList.Add(component2);
				}
			}
			RippleSource component3 = other.GetComponent<RippleSource>();
			if (component3)
			{
				this.kinematicRippleSourcesList.Add(component3);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			BoxCollider component = other.GetComponent<BoxCollider>();
			if (component != null && this.boxColObstList.Contains(component))
			{
				this.boxColObstList.Remove(component);
			}
			SphereCollider component2 = other.GetComponent<SphereCollider>();
			if (component2 != null && this.sphereColObstList.Contains(component2) && other.GetComponent<SphereCollider>())
			{
				this.sphereColObstList.Remove(component2);
			}
			RippleSource component3 = other.GetComponent<RippleSource>();
			if (component3 && this.kinematicRippleSourcesList.Contains(component3))
			{
				this.kinematicRippleSourcesList.Remove(component3);
			}
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
