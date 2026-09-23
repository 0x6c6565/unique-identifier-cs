#if USING_NEWTONSOFT_JSON

using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace UniqueIdentifiers
{
	public class Guid64JsonConverter : JsonConverter<Guid64>
	{
		public override Guid64 ReadJson(JsonReader reader, Type objectType, Guid64 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			try
			{
				if (Guid64.TryParse((string)reader.Value, out Guid64 id))
				{
					return id;
				}
			}
			catch { }

			return Guid64.Empty;
		}

		public override void WriteJson(JsonWriter writer, Guid64 value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
	}
}

#endif // USING_NEWTONSOFT_JSON
