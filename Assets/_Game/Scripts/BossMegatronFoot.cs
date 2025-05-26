using System;

public class BossMegatronFoot : BaseUnit
{
	private BossMegatron boss;

	protected override void Awake()
	{
		base.Awake();
		this.boss = base.transform.root.GetComponent<BossMegatron>();
	}

	private void OnEnable()
	{
		Singleton<GameController>.Instance.AddUnit(base.gameObject, this);
	}

	private void OnDisable()
	{
		Singleton<GameController>.Instance.RemoveUnit(base.gameObject);
	}

	public override void TakeDamage(AttackData attackData)
	{
		attackData.damage *= 0.8f;
		this.boss.TakeDamage(attackData);
	}
}
