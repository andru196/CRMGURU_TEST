using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CRMGURU_TEST
{
	class CityConverter : JsonConverter<City>
	{
        public override City Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {

            // fallback to default handling
            return new City { Name = reader.GetString() };
        }

        public override void Write(Utf8JsonWriter writer, City value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.Name);
		}
	}

	class RegionConverter : JsonConverter<Region>
	{
		public override Region Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
		{

			// fallback to default handling
			return new Region { Name = reader.GetString() };
		}

		public override void Write(Utf8JsonWriter writer, Region value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.Name);
		}
	}
}
