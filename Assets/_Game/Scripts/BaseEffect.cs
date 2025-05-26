using System;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
	public virtual void Active(Vector3 position, Transform parent = null)
	{
		base.transform.position = position;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
	}

	public virtual void Deactive()
	{
		base.gameObject.SetActive(false);
	}
}
