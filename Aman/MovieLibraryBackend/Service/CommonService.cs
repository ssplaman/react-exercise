using MovieLibraryApi.Interface;
using MovieLibraryApi.Model;
using RestSharp;
using System.Text.Json;

namespace MovieLibraryApi.Service;

public class CommonService(IConfiguration configuration,
	IHttpClientFactory httpClientFactory) : ICommonService
{
	#region Readonly Strings
	private readonly string _bearerToken = configuration["TMDB:Token"] ?? throw new InvalidOperationException("TMDB Token not configured.");
	private readonly string _language = configuration["TMDB:Language"] ?? throw new InvalidOperationException("TMDB Language not configured.");
	private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
	#endregion Readonly Strings

	#region Media List
	public async Task<string> GetMediaListAsync(string mediaType, int pageNumber, int genreId)
	{
		if (mediaType != "movie" && mediaType != "tv")
			throw new ArgumentException("mediaType must be 'movie' or 'tv'.");

		string endpoint = $"discover/{mediaType}?language={_language}&page={pageNumber}";
		if (genreId > 0)
		{
			endpoint = $"discover/{mediaType}?language={_language}&page={pageNumber}&with_genres={genreId}";
		}
		return await ExecuteTmdbRequestAsync(endpoint, Method.Get);
	}
	#endregion Media List

	#region Search Media
	public async Task<string> GetSearchedMediaAsync(string mediaType, string keyword, int pageNumber)
	{
		if (mediaType != "movie" && mediaType != "tv")
			throw new ArgumentException("mediaType must be 'movie' or 'tv'.");

		var endpoint = $"search/{mediaType}?query={keyword}&language={_language}&page={pageNumber}";
		return await ExecuteTmdbRequestAsync(endpoint, Method.Get);
	}
	#endregion Search Media

	#region Media Detail
	public async Task<string> GetMediaDetailAsync(string mediaType, int mediaId)
	{
		if (mediaType != "movie" && mediaType != "tv")
			throw new ArgumentException("mediaType must be 'movie' or 'tv'.");

		var endpoint = $"{mediaType}/{mediaId}?language={_language}";
		return await ExecuteTmdbRequestAsync(endpoint, Method.Get);
	}
	#endregion Media Detail

	#region Media Images
	public async Task<string> GetMediaImagesAsync(string mediaType, int mediaId)
	{
		if (mediaType != "movie" && mediaType != "tv")
			throw new ArgumentException("mediaType must be 'movie' or 'tv'.");

		var endpoint = $"{mediaType}/{mediaId}/images";
		return await ExecuteTmdbRequestAsync(endpoint, Method.Get);
	}
	#endregion Media Images

	#region Similar Media
	public async Task<string> GetSimilarMediaAsync(string mediaType, int mediaId, int pageNumber)
	{
		if (mediaType != "movie" && mediaType != "tv")
			throw new ArgumentException("mediaType must be 'movie' or 'tv'.");

		var endpoint = $"{mediaType}/{mediaId}/similar?language={_language}&page={pageNumber}";
		return await ExecuteTmdbRequestAsync(endpoint, Method.Get);
	}
	#endregion Similar Media

	#region Media Trailer
	public async Task<string> GetMediaTrailerAsync(string mediaType, int mediaId)
	{
		if (mediaType != "movie" && mediaType != "tv")
			throw new ArgumentException("mediaType must be 'movie' or 'tv'.");

		var endpoint = $"{mediaType}/{mediaId}/videos?language={_language}";
		return await ExecuteTmdbRequestAsync(endpoint, Method.Get);
	}
	#endregion Media Trailer

	#region Media Genres
	public async Task<string> GetMediaGenresAsync(string mediaType)
	{
		if (mediaType != "movie" && mediaType != "tv")
			throw new ArgumentException("mediaType must be 'movie' or 'tv'.");

		var endpoint = $"genre/{mediaType}/list?language={_language}";
		return await ExecuteTmdbRequestAsync(endpoint, Method.Get);
	}
	#endregion Media Genres

	#region Media Sorting
	public async Task<string> GetSortMediaAsync(string mediaType, string sortBy, int pageNumber)
	{
		if (mediaType != "movie" && mediaType != "tv")
			throw new ArgumentException("mediaType must be 'movie' or 'tv'.");

		var endpoint = $"{mediaType}/{sortBy}?language={_language}&page={pageNumber}";
		return await ExecuteTmdbRequestAsync(endpoint, Method.Get);
	}
	#endregion Media Sorting

	#region TMDB API Call
	public async Task<ResponseModel> HandleTmdbApiCallAsync<T>(Func<Task<string>> apiCall, string successMessage, string failMessage)
	{
		try
		{
			var json = await apiCall();

			if (!IsValidJson(json))
				return ResponseModel.Fail(failMessage, json);

			var data = JsonSerializer.Deserialize<T>(json);

			return ResponseModel.Success(successMessage, data);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Exception in TMDB API call: {ex}");
			return ResponseModel.Fail(failMessage, ex.Message);
		}
	}
	#endregion TMDB API Call

	#region Private Method
	private async Task<string> ExecuteTmdbRequestAsync(string endpoint, Method method)
	{
		try
		{

			var client = new RestClient("https://api.themoviedb.org/3/");
			var request = new RestRequest(endpoint, method);
			request.AddHeader("Authorization", $"Bearer {_bearerToken}");

			var response = await client.ExecuteAsync(request);

			return response.Content ?? response.ErrorMessage ?? string.Empty;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	private static bool IsValidJson(string input)
	{
		input = input.Trim();
		if ((input.StartsWith("{") && input.EndsWith("}")) ||
			(input.StartsWith("[") && input.EndsWith("]")))
		{
			try
			{
				JsonDocument.Parse(input);
				return true;
			}
			catch
			{
				return false;
			}
		}
		return false;
	}
	#endregion Private Method
}
