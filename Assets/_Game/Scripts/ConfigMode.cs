using Newtonsoft.Json;
using System;

[JsonObject]
[Serializable]
public class ConfigMode
{
	[JsonProperty("c")]
	public bool active;

	[JsonProperty("d")]
	public int coolDownMin;

	[JsonProperty("e")]
	public int coolDownMax;

	[JsonProperty("f")]
	public int showInMs;

	[JsonProperty("g")]
	public int hourStart;

	[JsonProperty("h")]
	public int hourEnd;

	[JsonProperty("j")]
	public int activeAfterUnlockCount;

	[JsonProperty("o")]
	public int showAfterHiddenCount;
}
