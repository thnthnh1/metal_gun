using System;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardElement : MonoBehaviour
{
	public Image icon;

	public Text value;

	public void SetInformation(RewardData data, bool isBonus = false)
	{
		this.icon.sprite = GameResourcesUtils.GetRewardImage(data.type);
        if(value != null)
	    this.value.text = string.Format("{0:n0}", data.value);
	}
}
