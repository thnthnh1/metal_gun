using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Core.Extension
{
    public static class UnixDateTimeExtension
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private const string InvalidUnixEpochErrorMessage = "Unix epoc starts January 1st, 1970";

        /// <summary>
        ///   Convert a long into a DateTime
        /// </summary>
        public static DateTime FromUnixTime(this Int64 self)
        {
            return UnixEpoch.AddSeconds(self);
        }

        public static DateTime FromUnixTime(this uint self)
        {
            return UnixEpoch.AddSeconds(self);
        }

        public static DateTime FromUnixTime(this int self)
        {
            return UnixEpoch.AddSeconds(self);
        }

        /// <summary>
        ///   Convert a long of Milliseconds into a DateTime
        /// </summary>
        public static DateTime FromUnixTimeMs(this Int64 self)
        {
            return UnixEpoch.AddMilliseconds(self);
        }

        /// <summary>
        ///   Convert a DateTime into a long
        /// </summary>
        public static long ToUnixTime(this DateTime self)
        {
            if (self < UnixEpoch)
            {
                if (self == DateTime.MinValue)
                {
                    return 0;
                }

                throw new ArgumentOutOfRangeException(InvalidUnixEpochErrorMessage);
            }

            TimeSpan delta = self.Subtract(UnixEpoch);
            var result = (long)delta.TotalSeconds;
            return result;
        }

        /// <summary>
        ///   Convert a DateTime into a long of milliseconds
        /// </summary>
        public static long ToUnixTimeMs(this DateTime self)
        {
            if (self < UnixEpoch)
            {
                if (self == DateTime.MinValue)
                {
                    return 0;
                }

                throw new ArgumentOutOfRangeException(InvalidUnixEpochErrorMessage);
            }

            TimeSpan delta = self.Subtract(UnixEpoch);
            var result = (long)delta.TotalMilliseconds;
            return result;
        }

        /// <summary>
        ///   Converts a long of milliseconds to an ISO 8061 Datetime string, used for Appboy
        /// </summary>
        public static string ToIso8601Date(this DateTime self)
        {
            return self.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        //public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
        //{
        //    if (reader.TokenType != JsonToken.Integer)
        //        throw new Exception("Wrong Token Type");

        //    long ticks = (long)reader.Value;
        //    return ticks.FromUnixTime();
        //}

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long val;
            if (value is DateTime)
            {
                val = ((DateTime)value).ToUnixTime();
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
            writer.WriteValue(val);
        }

        /// <summary>
        ///   Reads the JSON representation of the object.
        /// </summary>
        /// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
        /// <param name = "objectType">Type of the object.</param>
        /// <param name = "existingValue">The existing value of object being read.</param>
        /// <param name = "serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return null;
                }
                else if (reader.TokenType == JsonToken.String)
                {
                    long ticksstr = long.Parse((string)reader.Value);
                    return ticksstr.FromUnixTime();
                }
                else
                {
                    throw new Exception("Wrong Token Type. Expecting Integer; found " + reader.TokenType);
                }
            }

            long ticks = (long)reader.Value;
            return ticks.FromUnixTime();
        }
    }
}