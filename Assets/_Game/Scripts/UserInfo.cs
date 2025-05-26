using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class UserInfo
{
	public string id;

	public string authId;

	public string name;

	public string email;

	public UserInfo()
	{
	}

	public UserInfo(string id, string authId, string name, string email)
	{
		this.id = id;
		this.authId = authId;
		this.name = name;
		this.email = email;
	}

	public override string ToString()
	{
		return string.Format("ID: {0} | AuthId: {1} | Name: {2} | Email: {3}", new object[]
		{
			this.id,
			this.authId,
			this.name,
			this.email
		});
	}

	public string GetJsonString()
	{
		return JsonConvert.SerializeObject(new Dictionary<string, string>
		{
			{
				"authId",
				this.authId
			},
			{
				"name",
				this.name
			},
			{
				"email",
				this.email
			}
		});
	}
}
