using System;

public enum PlayerRamboState
{
	Unlock = 0,
	IAP = 1,
	Gift = 2
}

public class PlayerRamboData
{
	public int id;

	public int level;

	public PlayerRamboState state;

}
