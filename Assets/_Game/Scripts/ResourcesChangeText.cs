using System;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesChangeText : MonoBehaviour
{
	public Text content;

	public RectTransform rectTransform;

	public void Active(bool isReceive, int value, Vector2 position, Transform parent = null)
	{
		this.content.color = ((!isReceive) ? Color.red : Color.green);
		string format = (!isReceive) ? "-{0:n0}" : "+{0:n0}";
		this.content.text = string.Format(format, value);
		base.transform.SetParent(parent);
		this.rectTransform.position = position;
		this.rectTransform.localScale = Vector3.one;
		base.gameObject.SetActive(true);
	}

	public void Deactive()
	{
		Header.poolTextChange.Store(this);
		base.gameObject.SetActive(false);
	}
}
