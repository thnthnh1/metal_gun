using System;
using UnityEngine;
using UnityEngine.UI;

public class TournamentRankRewardDisplay : MonoBehaviour
{
	public Image icon;

	public Text value;

	public void SetInformation(RewardData data)
	{
		this.icon.sprite = GameResourcesUtils.GetRewardImage(data.type);
		this.value.text = string.Format("{0:n0}", data.value);
	}
}
