using System;
using UnityEngine;

public class Zone : MonoBehaviour
{
	public int id;

	public bool isFinalZone;

	public bool isLockWallEndWhenClear;

	public Collider2D wallStart;

	public CameraLockDirection wallStartLockDir;

	public Collider2D wallEnd;

	public CameraLockDirection wallEndLockDir = CameraLockDirection.Right;

	public GameObject[] objectAppearWhenClear;

	private void Awake()
	{
		this.wallStart.gameObject.SetActive(false);
		this.wallEnd.gameObject.SetActive(false);
		this.ShowObjects(false);
	}

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ClearZone, new Action<Component, object>(this.OnClearZone));
	}

	public void Lock()
	{
		this.wallStart.gameObject.SetActive(true);
		this.wallEnd.gameObject.SetActive(true);
		this.SetCameraMargin();
	}

	private void OnClearZone(Component sender, object param)
	{
		int num = (int)param;
		if (num == this.id && !this.isFinalZone)
		{
			this.wallEnd.gameObject.SetActive(false);
		}
	}

	public void ShowObjects(bool isShow)
	{
		for (int i = 0; i < this.objectAppearWhenClear.Length; i++)
		{
			this.objectAppearWhenClear[i].SetActive(isShow);
		}
	}

	public void SetCameraMargin()
	{
		if (this.wallStartLockDir == CameraLockDirection.Left && this.wallEndLockDir == CameraLockDirection.Right)
		{
			Singleton<CameraFollow>.Instance.SetMarginLeft(this.wallStart.transform.position.x);
			Singleton<CameraFollow>.Instance.SetMarginRight(this.wallEnd.transform.position.x);
		}
		else if (this.wallStartLockDir == CameraLockDirection.Right && this.wallEndLockDir == CameraLockDirection.Left)
		{
			Singleton<CameraFollow>.Instance.SetMarginRight(this.wallStart.transform.position.x);
			Singleton<CameraFollow>.Instance.SetMarginLeft(this.wallEnd.transform.position.x);
		}
	}
}
