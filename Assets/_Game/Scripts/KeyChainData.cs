using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class KeyChainData
{
	private string _userId_k__BackingField;

	private string _uuid_k__BackingField;

	public string userId
	{
		get;
		set;
	}

	public string uuid
	{
		get;
		set;
	}

	public override string ToString()
	{
		return string.Format("[KeyChainData: userId={0}, uuid={1}]", this.userId, this.uuid);
	}
}
