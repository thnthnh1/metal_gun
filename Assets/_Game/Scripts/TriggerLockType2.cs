using System;
using UnityEngine;

public class TriggerLockType2 : MonoBehaviour
{
	public Collider2D col;

	public GameObject[] objShow;

	public GameObject[] objHide;

	public Transform cameraMarginPoint;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			for (int i = 0; i < this.objShow.Length; i++)
			{
				this.objShow[i].SetActive(true);
			}
			for (int j = 0; j < this.objHide.Length; j++)
			{
				this.objHide[j].SetActive(false);
			}
			Singleton<CameraFollow>.Instance.SetMarginLeft(this.cameraMarginPoint.position.x);
		}
		this.col.enabled = false;
	}
}
