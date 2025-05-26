using System;
using UnityEngine;

public class EventAnimMuzzleFireBall : MonoBehaviour
{
	public BaseMuzzle mainMuzzle;

	public void Deactive()
	{
		this.mainMuzzle.Deactive();
	}
}
