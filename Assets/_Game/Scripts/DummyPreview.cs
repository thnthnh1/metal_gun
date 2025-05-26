using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DummyPreview : MonoBehaviour
{
	public BaseEffect textDamageTMP;

	public Color32 colorText;

	private SpriteRenderer sprite;

	private List<string> showTexts = new List<string>
	{
		"Ratata",
		"Oh nooo!",
		"Oops!!"
	};

	private bool isFlashing;

	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		EventDispatcher.Instance.RegisterListener(EventID.PreviewDummyTakeDamage, delegate(Component sender, object param)
		{
			this.TakeDamge();
		});
	}

	public void TakeDamge()
	{
		this.EffectTakeDamage();
		Vector2 vector = base.transform.position;
		vector.x += UnityEngine.Random.Range(-0.5f, 0.5f);
		vector.y += UnityEngine.Random.Range(0f, 0.5f);
		TextDamage textDamage = Singleton<PoolingPreviewController>.Instance.textDamage.New();
		if (textDamage == null)
		{
			textDamage = (UnityEngine.Object.Instantiate<BaseEffect>(this.textDamageTMP) as TextDamage);
		}
		string text = this.showTexts[UnityEngine.Random.Range(0, this.showTexts.Count)];
		TextDamage arg_C2_0 = textDamage;
		Vector2 position = vector;
		string content = text;
		Color32 color = this.colorText;
		Transform group = Singleton<PoolingPreviewController>.Instance.group;
		arg_C2_0.Active(position, content, color, 3.5f, group, false);
	}

	protected void EffectTakeDamage()
	{
		if (!this.isFlashing)
		{
			this.isFlashing = true;
			DOTween.To(new DOSetter<float>(this.ColorSetter), 1f, 0f, 0.1f).OnComplete(new TweenCallback(this.ChangeColorToDefault));
		}
	}

	private void ColorSetter(float pNewValue)
	{
		Color color = this.sprite.color;
		color.g = pNewValue;
		color.b = pNewValue;
		this.sprite.color = color;
	}

	private void ChangeColorToDefault()
	{
		this.sprite.color = Color.white;
		this.isFlashing = false;
	}

	private void FocusOnThis(BulletPreviewRocketChaser rocket)
	{
		rocket.Focus(base.transform);
	}
}
