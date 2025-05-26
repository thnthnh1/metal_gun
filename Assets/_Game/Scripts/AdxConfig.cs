using Newtonsoft.Json;
using System;

[Serializable]
public class AdxConfig
{
	[JsonProperty("a")]
	public string interstitialAd;

	[JsonProperty("r")]
	public string rewardAd;

	[JsonProperty("b")]
	public ConfigMode backgroundMode;

	[JsonProperty("k")]
	public ConfigMode foregroundMode;
}
