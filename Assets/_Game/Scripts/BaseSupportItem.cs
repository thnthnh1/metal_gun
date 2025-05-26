using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseSupportItem : MonoBehaviour
{
	public Button icon;

	public GameObject groupFree;

	public GameObject groupPrice;

	public Text textPrice;

	protected int countUsed;

	protected int priceUse;

	public virtual void Init()
	{
		EventDispatcher.Instance.RegisterListener(EventID.CompleteWave, delegate(Component sender, object param)
		{
			this.OnCompleteWave();
		});
		this.icon.onClick.AddListener(new UnityAction(this.Consume));
		this.Active(true);
	}

	protected virtual void OnCompleteWave()
	{
	}

	protected virtual void Consume()
	{
		this.countUsed++;
	}

	protected virtual void Active(bool isActive)
	{
		this.icon.interactable = isActive;
		if (!isActive)
		{
			this.groupFree.SetActive(false);
			this.groupPrice.SetActive(false);
		}
	}
}
