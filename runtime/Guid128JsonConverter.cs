#if USING_NEWTONSOFT_JSON

using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

using UnityEngine;

namespace UniqueIdentifiers
{
	public class Guid128JsonConverter : JsonConverter<Guid128>
	{
		public override Guid128 ReadJson(JsonReader reader, Type objectType, Guid128 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			try
			{
				if (Guid128.TryParse((string)reader.Value, out Guid128 id))
				{
					return id;
				}
			}
			catch { }

			return Guid128.Empty;
		}

		public override void WriteJson(JsonWriter writer, Guid128 value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
	}
}

#endif // USING_NEWTONSOFT_JSON
