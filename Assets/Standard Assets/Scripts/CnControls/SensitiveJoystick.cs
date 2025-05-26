using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CnControls
{
	public class SensitiveJoystick : SimpleJoystick
	{
		public AnimationCurve SensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 1f, 1f),
			new Keyframe(1f, 1f, 1f, 1f)
		});

		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);
			float value = this.HorizintalAxis.Value;
			float value2 = this.VerticalAxis.Value;
			float num = Mathf.Sign(value);
			float num2 = Mathf.Sign(value2);
			this.HorizintalAxis.Value = num * this.SensitivityCurve.Evaluate(num * value);
			this.VerticalAxis.Value = num2 * this.SensitivityCurve.Evaluate(num2 * value2);
		}
	}
}
