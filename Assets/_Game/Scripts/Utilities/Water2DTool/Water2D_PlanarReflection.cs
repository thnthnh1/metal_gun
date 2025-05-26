using System;
using System.Collections.Generic;
using UnityEngine;

namespace Water2DTool
{
	[ExecuteInEditMode]
	public class Water2D_PlanarReflection : MonoBehaviour
	{
		[Range(1f, 10f), SerializeField]
		private int reflectionQuality = 10;

		private string reflectionSampler = "_ReflectionTex";

		private Vector3 m_Oldpos;

		private Camera m_ReflectionCamera;

		private Material m_SharedMaterial;

		private Dictionary<Camera, bool> m_HelperCameras;

		private Water2D_Tool water2D;

		private bool getComponent = true;

		private Skybox mainCameraSkybox;

		private Skybox refCameraSkybox;

		public LayerMask reflectionMask = -1;

		public Color clearColor = Color.grey;

		public bool reflectSkybox = true;

		public bool UpdateSceneView = true;

		public float clipPlaneOffset;

		private void Awake()
		{
			this.water2D = base.GetComponentInParent<Water2D_Tool>();
		}

		private void Start()
		{
			base.gameObject.layer = LayerMask.NameToLayer("Water");
			this.setMaterial();
		}

		private void OnEnable()
		{
			base.gameObject.layer = LayerMask.NameToLayer("Water");
			this.setMaterial();
		}

		private void OnDisable()
		{
			if (this.m_ReflectionCamera != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionCamera);
			}
		}

		public void setMaterial()
		{
			if (Application.isPlaying)
			{
				this.m_SharedMaterial = base.GetComponent<Renderer>().material;
			}
			else
			{
				this.m_SharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			}
		}

		private Camera CreateReflectionCameraFor(Camera cam)
		{
			string name = string.Concat(new object[]
			{
				base.gameObject.name,
				"Reflection",
				base.GetInstanceID(),
				"-for-",
				cam.name
			});
			GameObject gameObject = GameObject.Find(name);
			if (!gameObject)
			{
				gameObject = new GameObject(name, new Type[]
				{
					typeof(Camera)
				});
				gameObject.hideFlags = HideFlags.HideAndDontSave;
			}
			if (!gameObject.GetComponent(typeof(Camera)))
			{
				gameObject.AddComponent(typeof(Camera));
			}
			Camera component = gameObject.GetComponent<Camera>();
			component.backgroundColor = this.clearColor;
			component.clearFlags = ((!this.reflectSkybox) ? CameraClearFlags.Color : CameraClearFlags.Skybox);
			this.SetStandardCameraParameter(component, this.reflectionMask);
			if (!component.targetTexture)
			{
				component.targetTexture = this.CreateTextureFor(cam);
			}
			return component;
		}

		private void SetStandardCameraParameter(Camera cam, LayerMask mask)
		{
			cam.cullingMask = (mask & ~(1 << LayerMask.NameToLayer("Water")));
			cam.backgroundColor = Color.black;
			cam.enabled = false;
		}

		private RenderTexture CreateTextureFor(Camera cam)
		{
			int width = Mathf.FloorToInt((float)(cam.pixelWidth / (11 - this.reflectionQuality)));
			int height = Mathf.FloorToInt((float)(cam.pixelHeight / (11 - this.reflectionQuality)));
			return new RenderTexture(width, height, 24)
			{
				hideFlags = HideFlags.DontSave
			};
		}

		public void RenderHelpCameras(Camera currentCam)
		{
			if (this.m_HelperCameras == null)
			{
				this.m_HelperCameras = new Dictionary<Camera, bool>();
			}
			if (!this.m_HelperCameras.ContainsKey(currentCam))
			{
				this.m_HelperCameras.Add(currentCam, false);
			}
			if (this.m_HelperCameras[currentCam] && !this.UpdateSceneView)
			{
				return;
			}
			if (!this.m_ReflectionCamera)
			{
				this.m_ReflectionCamera = this.CreateReflectionCameraFor(currentCam);
			}
			this.RenderReflectionFor(currentCam, this.m_ReflectionCamera);
			this.m_HelperCameras[currentCam] = true;
		}

		public void LateUpdate()
		{
			if (this.m_HelperCameras != null)
			{
				this.m_HelperCameras.Clear();
			}
		}

		public void WaterTileBeingRendered(Transform tr, Camera currentCam)
		{
			this.RenderHelpCameras(currentCam);
			if (this.m_ReflectionCamera && this.m_SharedMaterial)
			{
				this.m_SharedMaterial.SetTexture(this.reflectionSampler, this.m_ReflectionCamera.targetTexture);
			}
		}

		public void OnWillRenderObject()
		{
			this.WaterTileBeingRendered(base.transform, Camera.current);
		}

		private void RenderReflectionFor(Camera cam, Camera reflectCamera)
		{
			if (!reflectCamera)
			{
				return;
			}
			if (this.m_SharedMaterial && !this.m_SharedMaterial.HasProperty(this.reflectionSampler))
			{
				return;
			}
			reflectCamera.cullingMask = (this.reflectionMask & ~(1 << LayerMask.NameToLayer("Water")));
			this.SaneCameraSettings(reflectCamera);
			reflectCamera.backgroundColor = this.clearColor;
			reflectCamera.clearFlags = ((!this.reflectSkybox) ? CameraClearFlags.Color : CameraClearFlags.Skybox);
			if (this.getComponent)
			{
				this.mainCameraSkybox = (cam.GetComponent(typeof(Skybox)) as Skybox);
				this.refCameraSkybox = (reflectCamera.GetComponent(typeof(Skybox)) as Skybox);
				if (Application.isPlaying)
				{
					this.getComponent = false;
				}
			}
			if (this.reflectSkybox && this.mainCameraSkybox != null)
			{
				if (!this.refCameraSkybox)
				{
					this.refCameraSkybox = (Skybox)reflectCamera.gameObject.AddComponent(typeof(Skybox));
				}
				this.refCameraSkybox.material = this.mainCameraSkybox.material;
			}
			GL.invertCulling = true;
			Transform transform = base.transform;
			Vector3 eulerAngles = cam.transform.eulerAngles;
			reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
			reflectCamera.transform.position = cam.transform.position;
			if (this.water2D == null)
			{
				this.water2D = base.GetComponentInParent<Water2D_Tool>();
			}
			Vector3 vector = Vector3.zero;
			if (this.water2D != null)
			{
				vector = base.transform.TransformPoint(this.water2D.handlesPosition[0]);
			}
			else
			{
				vector = base.transform.position;
			}
			Vector3 up = transform.transform.up;
			float w = -Vector3.Dot(up, vector) - this.clipPlaneOffset;
			Vector4 plane = new Vector4(up.x, up.y, up.z, w);
			Matrix4x4 matrix4x = Matrix4x4.zero;
			matrix4x = Water2D_PlanarReflection.CalculateReflectionMatrix(matrix4x, plane);
			this.m_Oldpos = cam.transform.position;
			Vector3 position = matrix4x.MultiplyPoint(this.m_Oldpos);
			reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
			Vector4 clipPlane = this.CameraSpacePlane(reflectCamera, vector, up, 1f);
			Matrix4x4 matrix4x2 = cam.projectionMatrix;
			matrix4x2 = Water2D_PlanarReflection.CalculateObliqueMatrix(matrix4x2, clipPlane);
			reflectCamera.projectionMatrix = matrix4x2;
			reflectCamera.transform.position = position;
			Vector3 eulerAngles2 = cam.transform.eulerAngles;
			reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
			reflectCamera.Render();
			GL.invertCulling = false;
		}

		private void SaneCameraSettings(Camera helperCam)
		{
			helperCam.depthTextureMode = DepthTextureMode.None;
			helperCam.backgroundColor = Color.black;
			helperCam.clearFlags = CameraClearFlags.Color;
			helperCam.renderingPath = RenderingPath.Forward;
		}

		private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
		{
			Vector4 b = projection.inverse * new Vector4(Water2D_PlanarReflection.Sgn(clipPlane.x), Water2D_PlanarReflection.Sgn(clipPlane.y), 1f, 1f);
			Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
			projection[2] = vector.x - projection[3];
			projection[6] = vector.y - projection[7];
			projection[10] = vector.z - projection[11];
			projection[14] = vector.w - projection[15];
			return projection;
		}

		private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
		{
			reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
			reflectionMat.m01 = -2f * plane[0] * plane[1];
			reflectionMat.m02 = -2f * plane[0] * plane[2];
			reflectionMat.m03 = -2f * plane[3] * plane[0];
			reflectionMat.m10 = -2f * plane[1] * plane[0];
			reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
			reflectionMat.m12 = -2f * plane[1] * plane[2];
			reflectionMat.m13 = -2f * plane[3] * plane[1];
			reflectionMat.m20 = -2f * plane[2] * plane[0];
			reflectionMat.m21 = -2f * plane[2] * plane[1];
			reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
			reflectionMat.m23 = -2f * plane[3] * plane[2];
			reflectionMat.m30 = 0f;
			reflectionMat.m31 = 0f;
			reflectionMat.m32 = 0f;
			reflectionMat.m33 = 1f;
			return reflectionMat;
		}

		private static float Sgn(float a)
		{
			if (a > 0f)
			{
				return 1f;
			}
			if (a < 0f)
			{
				return -1f;
			}
			return 0f;
		}

		private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 v = pos + normal * this.clipPlaneOffset;
			Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
			Vector3 lhs = worldToCameraMatrix.MultiplyPoint(v);
			Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
		}
	}
}
