using System.Text.Json.Serialization;

namespace MovieLibraryApi.Model.Dtos;
public class TmdbTrailerResponse
{
	[JsonPropertyName("id")]
	public int Id { get; set; }
	[JsonPropertyName("results")]
	public List<TmdbTrailerResult>? Results { get; set; }
}
public class TmdbTrailerResult
{
	//[JsonPropertyName("iso_639_1")]
	//public string? ISO6391 { get; set; }
	//[JsonPropertyName("iso_3166_1")]
	//public string? ISO31661 { get; set; }
	[JsonPropertyName("name")]
	public string? Name { get; set; }
	[JsonPropertyName("key")]
	public string? Key { get; set; }
	[JsonPropertyName("site")]
	public string? Site { get; set; }
	//[JsonPropertyName("size")]
	//public int Size { get; set; }
	//[JsonPropertyName("type")]
	//public string? Type { get; set; }
	//[JsonPropertyName("official")]
	//public bool Official { get; set; }
	//[JsonPropertyName("published_at")]
	//public DateTime Published_at { get; set; }
	//[JsonPropertyName("id")]
	//public string? Id { get; set; }
}
