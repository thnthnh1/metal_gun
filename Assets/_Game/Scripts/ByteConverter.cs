using System;
using System.IO;
using System.Xml.Serialization;

public static class ByteConverter
{
	private static byte[] emptyData = new byte[0];

	public static byte[] SerializeObject<T>(T serializableObject)
	{
		if (serializableObject == null)
		{
			return ByteConverter.emptyData;
		}
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			xmlSerializer.Serialize(memoryStream, serializableObject);
			result = memoryStream.ToArray();
		}
		return result;
	}

	public static T DeserializeObject<T>(byte[] serilizedBytes)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		T result;
		using (MemoryStream memoryStream = new MemoryStream(serilizedBytes))
		{
			try
			{
				result = (T)((object)xmlSerializer.Deserialize(memoryStream));
			}
			catch (Exception var_3_29)
			{
				result = default(T);
			}
		}
		return result;
	}
}
