using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOpenTransportDoor : MonoBehaviour
{
	public Transform startDoor;

	public Transform startDoorOpenPosition;

	public Transform endDoor;

	public Transform endDoorOpenPosition;

	public Transform playerPoint;

	public GameObject wallLockLeftUpstairs;

	public GameObject[] objectsHide;

	public GameObject[] objectsShow;

	private BoxCollider2D triggerPlayerEnter;

	private Vector2 startDoorClosedPosition;

	private Vector2 endDoorClosedPosition;

	private bool isOpeningDoor;

	private bool isClosingDoor;

	private void Awake()
	{
		this.triggerPlayerEnter = base.GetComponent<BoxCollider2D>();
		this.triggerPlayerEnter.enabled = false;
		this.startDoorClosedPosition = this.startDoor.position;
		this.endDoorClosedPosition = this.endDoor.position;
		this.isOpeningDoor = true;
		SoundManager.Instance.PlaySfx("sfx_door_open", 0f);
	}

	private void Update()
	{
		if (this.isOpeningDoor)
		{
			if (Mathf.Abs(this.startDoor.position.y - this.startDoorOpenPosition.position.y) > 0.1f)
			{
				this.startDoor.position = Vector2.MoveTowards(this.startDoor.position, this.startDoorOpenPosition.position, 2f * Time.deltaTime);
			}
			else
			{
				this.startDoor.position = this.startDoorOpenPosition.position;
				this.isOpeningDoor = false;
				this.triggerPlayerEnter.enabled = true;
			}
		}
		if (this.isClosingDoor)
		{
			if (Mathf.Abs(this.endDoor.position.y - this.endDoorClosedPosition.y) > 0.1f)
			{
				this.endDoor.position = Vector2.MoveTowards(this.endDoor.position, this.endDoorClosedPosition, 2f * Time.deltaTime);
			}
			else
			{
				this.endDoor.position = this.endDoorClosedPosition;
				this.isClosingDoor = false;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.triggerPlayerEnter.enabled = false;
			this.LoadObjects();
			this.endDoor.position = this.endDoorOpenPosition.position;
			SceneFading.Instance.FadePingPongBlackAlpha(2f, new UnityAction(this.MovePlayerUpstairs), new UnityAction(this.CloseDestinationDoor));
		}
	}

	private void LoadObjects()
	{
		for (int i = 0; i < this.objectsHide.Length; i++)
		{
			this.objectsHide[i].SetActive(false);
		}
		for (int j = 0; j < this.objectsShow.Length; j++)
		{
			this.objectsShow[j].SetActive(true);
		}
	}

	private void MovePlayerUpstairs()
	{
		Singleton<GameController>.Instance.Player.transform.position = this.playerPoint.position;
		Singleton<GameController>.Instance.CampaignMap.SetDefaultMapMargin();
		Singleton<CameraFollow>.Instance.SetMarginLeft(this.wallLockLeftUpstairs.transform.position.x);
	}

	private void CloseDestinationDoor()
	{
		this.isClosingDoor = true;
		SoundManager.Instance.PlaySfx("sfx_door_close", 0f);
	}
}
