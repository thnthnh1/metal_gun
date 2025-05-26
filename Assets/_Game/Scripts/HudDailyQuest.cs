using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HudDailyQuest : MonoBehaviour, IEnhancedScrollerDelegate
{
	public EnhancedScroller scroller;

	public EnhancedScrollerCellView cellViewDailyQuest;

	public Text countNotification;

	private SmallList<CellViewDailyQuestData> dailyQuestData = new SmallList<CellViewDailyQuestData>();

	private static Func<PlayerDailyQuestData, bool> __f__am_cache0;

	private void Awake()
	{
		EventDispatcher.Instance.RegisterListener(EventID.NewDay, delegate(Component sender, object param)
		{
			this.RefreshDailyQuest();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ClaimDailyQuestReward, delegate(Component sender, object param)
		{
			this.OnClaimReward((CellViewDailyQuestData)param);
		});
		this.scroller.CreateContainer();
		this.scroller.Delegate = this;
	}

	private void OnEnable()
	{
		this.CreateDailyQuestData();
		this.scroller.ReloadData(0f);
	}

	public void CalculateNotification()
	{
		int numberReadyQuest = GameData.playerDailyQuests.GetNumberReadyQuest();
		if (numberReadyQuest > 0)
		{
			this.countNotification.transform.parent.gameObject.SetActive(true);
			this.countNotification.text = numberReadyQuest.ToString();
		}
		else
		{
			this.countNotification.transform.parent.gameObject.SetActive(false);
		}
	}

	private void CreateDailyQuestData()
	{
		this.dailyQuestData.Clear();
		for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
		{
			PlayerDailyQuestData playerDailyQuestData = GameData.playerDailyQuests[i];
			StaticDailyQuestData data = GameData.staticDailyQuestData.GetData(playerDailyQuestData.type);
			CellViewDailyQuestData cellViewDailyQuestData = new CellViewDailyQuestData();
			cellViewDailyQuestData.type = playerDailyQuestData.type;
			cellViewDailyQuestData.title = data.title;
			cellViewDailyQuestData.description = string.Format(data.description, data.value);
			cellViewDailyQuestData.progress = playerDailyQuestData.progress;
			cellViewDailyQuestData.target = data.value;
			cellViewDailyQuestData.isClaimed = playerDailyQuestData.isClaimed;
			cellViewDailyQuestData.rewards = data.rewards;
			this.dailyQuestData.Add(cellViewDailyQuestData);
		}
	}

	private void RefreshDailyQuest()
	{
		this.CreateDailyQuestData();
		this.scroller.ReloadData(0f);
	}

	private void OnClaimReward(CellViewDailyQuestData data)
	{
		if (data.type != DailyQuestType.COMPLETE_ALL_QUEST)
		{
			int num = GameData.playerDailyQuests.Count((PlayerDailyQuestData x) => x.isClaimed && x.type != DailyQuestType.COMPLETE_ALL_QUEST);
			if (num == GameData.playerDailyQuests.Count - 1)
			{
				EventDispatcher.Instance.PostEvent(EventID.CompleteAllDailyQuests);
				for (int i = 0; i < this.dailyQuestData.Count; i++)
				{
					CellViewDailyQuestData cellViewDailyQuestData = this.dailyQuestData[i];
					if (cellViewDailyQuestData.type == DailyQuestType.COMPLETE_ALL_QUEST)
					{
						cellViewDailyQuestData.progress++;
					}
				}
				EventLogger.LogEvent("N_CompleteAllDailyQuests", new object[0]);
			}
		}
		this.scroller.RefreshActiveCellViews();
		RewardUtils.Receive(data.rewards);
		Singleton<Popup>.Instance.ShowToastMessage("Claim reward successfully", ToastLength.Normal);
		SoundManager.Instance.PlaySfx("sfx_get_reward", 0f);
	}

	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return this.dailyQuestData.Count;
	}

	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 98f;
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		CellViewDailyQuest cellViewDailyQuest = scroller.GetCellView(this.cellViewDailyQuest) as CellViewDailyQuest;
		cellViewDailyQuest.name = this.dailyQuestData[dataIndex].type.ToString();
		cellViewDailyQuest.SetData(this.dailyQuestData[dataIndex]);
		return cellViewDailyQuest;
	}
}
