using System;

public class FbAppRequestData
{
	public string requestId;

	public string requestIdPrefix;

	public string data;

	public string senderName;

	public string senderId;

	public FbAppRequestData(string requestId, string data, string senderId, string senderName)
	{
		this.requestId = requestId;
		this.requestIdPrefix = requestId.Split(new char[]
		{
			'_'
		})[0];
		this.data = data;
		this.senderId = senderId;
		this.senderName = senderName;
	}

	public override string ToString()
	{
		return string.Format("Request ID: {0}, Sender ID: {1}, Sender Name: {2}, Data: {3}", new object[]
		{
			this.requestId,
			this.senderId,
			this.senderName,
			this.data
		});
	}
}
