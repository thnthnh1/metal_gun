using DG.Tweening;
using Spine.Unity;
using System;
using UnityEngine;

public class TransportJet : MonoBehaviour
{
	public Transform playerStandPoint;

	public SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	public string move;

	[SpineBone("", "", true, false)]
	public string boneStand;

	private AudioSource audioMove;

	private AudioClip soundMove;

	private void Awake()
	{
		this.audioMove = base.GetComponent<AudioSource>();
		this.soundMove = SoundManager.Instance.GetAudioClip("sfx_plane_move");
	}

	public void Active(Vector2 destination)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.move, true);
		this.EnableAudioMove(true);
		base.transform.DOMove(destination, 4f, false).OnComplete(delegate
		{
			EventDispatcher.Instance.PostEvent(EventID.TransportJetDone);
			base.Invoke("Escape", 1f);
		});
	}

	private void Escape()
	{
		Vector2 v = base.transform.position;
		v.y += 5f;
		base.transform.DOMove(v, 4f, false).OnComplete(delegate
		{
			this.EnableAudioMove(false);
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	private void EnableAudioMove(bool isEnable)
	{
		if (isEnable)
		{
			if (this.audioMove.clip == null)
			{
				this.audioMove.clip = this.soundMove;
				this.audioMove.loop = true;
			}
			this.audioMove.Play();
		}
		else
		{
			AudioClip audioClip = this.soundMove;
			this.audioMove.clip = audioClip;
			if (audioClip)
			{
				this.audioMove.clip = null;
			}
			this.audioMove.Stop();
		}
	}
}
