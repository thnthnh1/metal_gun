using System;
using UnityEngine;

public class AutoMoveBar : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			other.transform.root.parent = base.transform;
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			other.transform.parent = null;
		}
	}
}
