using System;
using UnityEngine;

namespace Water2DTool
{
	public class ShaderParam
	{
		public int[] WaterRippleID;

		public int[] recObstVarIDs;

		public int waterWidthID;

		public int amplitude1ID;

		public int amplitude2ID;

		public int amplitude3ID;

		public int waveLength1ID;

		public int waveLength2ID;

		public int waveLength3ID;

		public int phaseOffset1ID;

		public int phaseOffset2ID;

		public int phaseOffset3ID;

		public int fallOffID;

		public int amplitudeFadeID;

		public int[] cirObstVarIDs;

		public int prevTexID;

		public int faceCullingID;

		public int oneOrZeroID;

		public int waveHeightScaleID;

		public int bottomPosID;

		public int dampingID;

		public int axisScaleID;

		public ShaderParam()
		{
			this.WaterRippleID = new int[10];
			this.recObstVarIDs = new int[5];
			this.cirObstVarIDs = new int[5];
			for (int i = 0; i < 10; i++)
			{
				int num = i + 1;
				this.WaterRippleID[i] = Shader.PropertyToID("_WaterRipple" + num);
			}
			for (int j = 0; j < 5; j++)
			{
				int num2 = j + 1;
				this.recObstVarIDs[j] = Shader.PropertyToID("_RecObst" + num2);
				this.cirObstVarIDs[j] = Shader.PropertyToID("_CircleObst" + num2);
			}
			this.prevTexID = Shader.PropertyToID("_PrevTex");
			this.waveHeightScaleID = Shader.PropertyToID("_WaveHeightScale");
			this.bottomPosID = Shader.PropertyToID("_BottomPos");
			this.faceCullingID = Shader.PropertyToID("_FaceCulling");
			this.oneOrZeroID = Shader.PropertyToID("_OneOrZero");
			this.dampingID = Shader.PropertyToID("_Damping");
			this.axisScaleID = Shader.PropertyToID("_AxisScale");
			this.waterWidthID = Shader.PropertyToID("_WaterWidth");
			this.amplitude1ID = Shader.PropertyToID("_Amplitude1");
			this.amplitude2ID = Shader.PropertyToID("_Amplitude2");
			this.amplitude3ID = Shader.PropertyToID("_Amplitude3");
			this.waveLength1ID = Shader.PropertyToID("_WaveLength1");
			this.waveLength2ID = Shader.PropertyToID("_WaveLength2");
			this.waveLength3ID = Shader.PropertyToID("_WaveLength3");
			this.phaseOffset1ID = Shader.PropertyToID("_PhaseOffset1");
			this.phaseOffset2ID = Shader.PropertyToID("_PhaseOffset2");
			this.phaseOffset3ID = Shader.PropertyToID("_PhaseOffset3");
			this.fallOffID = Shader.PropertyToID("_FallOff");
			this.amplitudeFadeID = Shader.PropertyToID("_AmplitudeFade");
		}
	}
}
