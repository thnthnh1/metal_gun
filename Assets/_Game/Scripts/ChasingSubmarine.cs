using System;
using UnityEngine;

public class ChasingSubmarine : MonoBehaviour
{
	public float defaultOffset = 7.5f;

	public float offset = 7.5f;

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.BoatTriggerObstacle, new Action<Component, object>(this.GetCloser));
		EventDispatcher.Instance.RegisterListener(EventID.BoatStop, new Action<Component, object>(this.Stop));
	}

	private void Update()
	{
		if (Singleton<GameController>.Instance.Player)
		{
			Vector3 position = Singleton<GameController>.Instance.Player.transform.position;
			position.x -= this.offset;
			position.y = base.transform.position.y;
			base.transform.position = Vector3.Lerp(base.transform.position, position, 1.5f * Time.deltaTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		this.offset = this.defaultOffset;
	}

	private void GetCloser(Component arg1, object arg2)
	{
		this.offset -= 1f;
	}

	private void Stop(Component arg1, object arg2)
	{
		this.offset = this.defaultOffset;
	}
}
