using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class TournamentData
{
	public string id;

	public int score;

	public int primaryGunId;

	public bool received;

	public TournamentData()
	{
	}

	public TournamentData(string id, int score, int primaryGunId, bool received)
	{
		this.id = id;
		this.score = score;
		this.primaryGunId = primaryGunId;
		this.received = received;
	}

	public override string ToString()
	{
		return string.Format("ID: {0} | Primary Gun: {1} | Score: {2} | Received reward: {3}", new object[]
		{
			this.id,
			this.primaryGunId,
			this.score,
			this.received
		});
	}

	public string GetJsonString()
	{
		return JsonConvert.SerializeObject(new Dictionary<string, object>
		{
			{
				"score",
				this.score
			},
			{
				"primaryGunId",
				this.primaryGunId
			},
			{
				"received",
				this.received
			}
		});
	}
}
