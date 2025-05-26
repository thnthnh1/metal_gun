using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VantvTest : MonoBehaviour
{
	private float timer;

	private void Start()
	{
		string value = "{\"code\":2,\"data\":{\"dateTime\":\"2018-04-08T15:29:32Z\"}}";
		MasterInfoResponse masterInfoResponse = JsonConvert.DeserializeObject<MasterInfoResponse>(value);
		UnityEngine.Debug.Log(masterInfoResponse.data.dateTime);
	}

	private void GenerateFirebaseTestUsers()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		for (int i = 0; i < 50; i++)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("profile", new Dictionary<string, string>
			{
				{
					"name",
					"Name User " + i
				},
				{
					"email",
					i + "@test.com"
				},
				{
					"authId",
					i.ToString()
				}
			});
			dictionary.Add(i.ToString(), dictionary2);
		}
		UnityEngine.Debug.Log(JsonConvert.SerializeObject(dictionary));
	}

	private void GenerateFirebaseTestTournaments()
	{
		Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>();
		for (int i = 0; i < 50; i++)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("score", 49 - i);
			dictionary2.Add("primaryGunId", UnityEngine.Random.Range(0, 100));
			dictionary2.Add("received", false);
			dictionary.Add(i.ToString(), dictionary2);
		}
		UnityEngine.Debug.Log(JsonConvert.SerializeObject(dictionary));
	}
}
