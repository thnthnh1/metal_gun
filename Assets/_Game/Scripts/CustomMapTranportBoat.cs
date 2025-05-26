using System;
using UnityEngine;

public class CustomMapTranportBoat : BaseCustomMap
{
	public RubberBoat boatPrefab;

	public Transform stopPoint;

	private RubberBoat boat;

	private void Start()
	{
		this.boat = UnityEngine.Object.Instantiate<RubberBoat>(this.boatPrefab, base.transform.position, base.transform.rotation);
		this.boat.playerStopPoint = this.stopPoint;
		this.boat.autoMove = false;
		Singleton<GameController>.Instance.AddUnit(this.boat.gameObject, this.boat);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			base.gameObject.SetActive(false);
			this.boat.GetIn((Rambo)Singleton<GameController>.Instance.Player);
			this.boat.StartMove();
		}
	}
}
