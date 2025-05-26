using System;
using UnityEngine;

public class CustomMapRacingBoat : BaseCustomMap
{
	public RubberBoat boatPrefab;

	public bool isBoatAutoMoveFirst = true;

	public override void Init(Map map)
	{
		RubberBoat rubberBoat = UnityEngine.Object.Instantiate<RubberBoat>(this.boatPrefab, map.playerSpawnPoint.position, map.playerSpawnPoint.rotation);
		rubberBoat.autoMove = this.isBoatAutoMoveFirst;
		Singleton<GameController>.Instance.Player = rubberBoat;
		Singleton<GameController>.Instance.AddUnit(rubberBoat.gameObject, rubberBoat);
		Rambo ramboPrefab = GameResourcesUtils.GetRamboPrefab(ProfileManager.UserProfile.ramboId);
		Rambo rambo = UnityEngine.Object.Instantiate<Rambo>(ramboPrefab);
		int id = ProfileManager.UserProfile.ramboId;
		int ramboLevel = GameData.playerRambos.GetRamboLevel(id);
		rambo.Active(id, ramboLevel, Vector2.zero);
		rubberBoat.GetIn(rambo);
		Singleton<CameraFollow>.Instance.SetTarget(rubberBoat.transform);
	}
}
