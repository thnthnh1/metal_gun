using System;
using UnityEngine;

public class ItemDropCoin : BaseItemDrop
{
	private float timerDisappear;

	private float timerFlash;

	private bool flagFlash;

	private bool isAutoMoveToPlayer;

	private string methodNameAutoMove = "ActiveAutoMove";

	private void Update()
	{
		if (this.isAutoMoveToPlayer)
		{
			base.transform.position = Vector2.MoveTowards(base.transform.position, Singleton<GameController>.Instance.Player.BodyCenterPoint.position, Time.deltaTime * 25f);
		}
		else
		{
			this.timerDisappear += Time.deltaTime;
			if (this.timerDisappear >= 7f)
			{
				this.timerDisappear = 0f;
				this.Deactive();
			}
			else if (this.timerDisappear >= 4f)
			{
				this.flagFlash = true;
			}
			if (this.flagFlash)
			{
				this.timerFlash += Time.deltaTime;
				if (this.timerFlash > 0.2f)
				{
					this.timerFlash = 0f;
					Color color = this.spr.color;
					color.a = ((color.a != 1f) ? 1f : 0f);
					this.spr.color = color;
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			EventDispatcher.Instance.PostEvent(EventID.GetItemDrop, this.data);
			this.Deactive();
		}
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolItemDropCoin.Store(this);
	}

	public override void Active(ItemDropData data, Vector2 position)
	{
		base.Active(data, position);
		this.flagFlash = false;
		this.isAutoMoveToPlayer = false;
		this.timerDisappear = 0f;
		this.timerFlash = 0f;
		this.spr.color = Color.white;
		this.rigid.bodyType = RigidbodyType2D.Dynamic;
		this.col.isTrigger = false;
		this.AddRandomForce();
		if (GameData.isAutoCollectCoin)
		{
			base.Invoke(this.methodNameAutoMove, 2f);
		}
	}

	private void AddRandomForce()
	{
		float x = UnityEngine.Random.Range(-80f, 80f);
		float y = UnityEngine.Random.Range(200f, 250f);
		Vector2 force;
		force.x = x;
		force.y = y;
		this.rigid.AddForce(force, ForceMode2D.Force);
	}

	private void ActiveAutoMove()
	{
		this.rigid.bodyType = RigidbodyType2D.Kinematic;
		this.col.isTrigger = true;
		this.isAutoMoveToPlayer = true;
	}
}
