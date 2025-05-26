using DG.Tweening;
using DG.Tweening.Core;
using System;
using UnityEngine;

public class ExplosiveBridge : MonoBehaviour
{
	public float delayExplode = 0.5f;

	public Animator[] c4;

	private SpriteRenderer sprite;

	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			DOTween.To(new DOSetter<float>(this.ColorSetter), 1f, 0f, 0.5f);
			this.StartDelayAction(new Action(this.Explode), this.delayExplode);
			for (int i = 0; i < this.c4.Length; i++)
			{
				this.c4[i].Play("C4");
			}
		}
	}

	private void Explode()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, base.transform.position);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
		base.gameObject.SetActive(false);
	}

	private void ColorSetter(float pNewValue)
	{
		Color color = this.sprite.color;
		color.g = pNewValue;
		color.b = pNewValue;
		this.sprite.color = color;
	}
}
