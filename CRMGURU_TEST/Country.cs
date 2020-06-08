using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CRMGURU_TEST
{
	public class Country
	{
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("alpha2Code")]
		public string CountryCode { get; set; }
		[JsonPropertyName("capital")]
		[JsonConverter(typeof(CityConverter))]
		public City Capital { get; set; }
		[JsonPropertyName("area")]
		public double Square { get; set; }
		[JsonPropertyName("population")]
		public long Population { get; set; }
		[JsonPropertyName("region")]
		[JsonConverter(typeof(RegionConverter))]
		public Region Region { get; set; }
		public override string ToString()
		{
			var strBuilder = new StringBuilder();
			var tmp = this.Id.ToString();
			strBuilder.Append(tmp).Append('\t');
			strBuilder.Append(this.Name).Append(new string('\t', 4 - Name.Length / 8 ));
			strBuilder.Append(this.CountryCode).Append("\t");
			tmp = this.Capital?.Name;
			strBuilder.Append(tmp).Append(new String('\t', 3 - tmp.Length / 8));
			tmp = Square.ToString();
			strBuilder.Append(tmp).Append(new String('\t', 2 - tmp.Length / 8));
			tmp = this.Population.ToString();
			strBuilder.Append(tmp).Append(new String('\t', 2 - tmp.Length / 8));
			strBuilder.Append(Region?.Name);
			return strBuilder.ToString();
		}
		public static string GetHeader()
		=> "Id\tName\t\t\t\tCode\tCapitalt\t\tSquare\t\tPopulation\tRegion\n";
	}
	
}
