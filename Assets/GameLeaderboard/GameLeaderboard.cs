using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLeaderboard : MonoBehaviour
{
	public static GameLeaderboard instance;

	[Header("Player Prefab")]
	public GameObject playerPrefab;

	[Header("Player Content")]
	public Transform playersContent;

	[Header("Player list")]
	public List<GameObject> playerList;
	public List<int> playerPointsList;
	public List<int> playerDeathsList;
	public List<int> playerTotalList;
	public List<int> playerTrophyList;


	// Start is called before the first frame update
	void Awake()
	{
		instance = this;

	}

	public void RegisterPlayer(string playerName)
	{
		GameObject newPlayer = Instantiate(playerPrefab, playersContent.position, Quaternion.identity);

		newPlayer.transform.SetParent(playersContent);

		newPlayer.transform.localScale = new Vector3(1, 1, 1);

		newPlayer.name = playerName;

		playerList.Add(newPlayer);
		playerPointsList.Add(0);
		playerDeathsList.Add(0);
		playerTotalList.Add(0);
		playerTrophyList.Add(0);

		playerList[playerList.Count - 1].transform.Find("NameText").GetComponent<Text>().text = playerName;
		Debug.Log("Dev Log is the name set Here ! " + playerName);
	}

	public void UnRegisterPlayer(string playerName)
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].name == playerName)
			{
				Destroy(playerList[i]);
				playerList.RemoveAt(i);
				playerPointsList.RemoveAt(i);
				playerDeathsList.RemoveAt(i);
				playerTotalList.RemoveAt(i);
				playerTrophyList.RemoveAt(i);

				break;
			}
		}
	}

	public void AddScrore(string playerName)
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].name == playerName)
			{
				playerPointsList[i]++;

				playerList[i].transform.Find("KillText").GetComponent<Text>().text = "" + playerPointsList[i];

				//Get total points
				int Total = playerPointsList[i] - playerDeathsList[i];
				playerList[i].transform.Find("TotalText").GetComponent<Text>().text ="" + Total;

				if (Total < 0)
				{
					Total = 0;

					playerTrophyList[i] = 0;
				}
				else
				{
					playerTrophyList[i] = Total * 2;
				}

				playerTotalList[i] = Total;
				playerList[i].transform.Find("TrophyText").GetComponent<Text>().text = "" + playerTrophyList[i];
				break;
			}
		}
	}

	public void AddDeath(string playerName)
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].name == playerName)
			{
				playerDeathsList[i]++;

				playerList[i].transform.Find("DeathText").GetComponent<Text>().text ="" + playerDeathsList[i];

				//Get total points
				int Total = playerPointsList[i] - playerDeathsList[i];
				playerList[i].transform.Find("TotalText").GetComponent<Text>().text = "" + Total;

				if (Total < 0)
				{
					Total = 0;

					playerTrophyList[i] = 0;
				}
				else
				{
					playerTrophyList[i] = Total * 2;
				}

				playerTotalList[i] = Total;
				playerList[i].transform.Find("TrophyText").GetComponent<Text>().text = "" + playerTrophyList[i];

				break;
			}
		}
	}
	public int GetPlayerPoints(string playerName)
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].name == playerName)
			{
				return i;
			}
		}
		return 0;
	}

	public bool ComprobatePlayer(string playerName)
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].name == playerName)
			{
				return true;
			}
		}
		return false;
	}
}
