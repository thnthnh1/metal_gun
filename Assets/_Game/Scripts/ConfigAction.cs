using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[JsonObject]
[Serializable]
public class ConfigAction
{
	[JsonProperty("m")]
	public int actionDelayInMs;

	[JsonProperty("n")]
	public List<Vector2> actionConstraints;
}
