using MovieLibraryApi.Model;

namespace MovieLibraryApi.Interface;

public interface ICommonService
{
	#region Media List
	/// <summary>
	/// Retrieves a list of media items (movies or TV shows) for a specified media type, page number, and genre from the external TMDB API.
	/// </summary>
	/// <param name="mediaType">The type of media ("movie" or "tv").</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <param name="genreId">The genre ID to filter the media items.</param>
	/// <returns>A task that represents the asynchronous operation, containing the JSON string with the media list data.</returns>
	Task<string> GetMediaListAsync(string mediaType, int pageNumber, int genreId);
	#endregion Media List

	#region Search Media
	/// <summary>
	/// Retrieves a list of media items (movies or TV shows) from the external TMDB API that match the specified search keyword, media type, and page number.
	/// </summary>
	/// <param name="mediaType">The type of media ("movie" or "tv").</param>
	/// <param name="keyword">The search keyword to filter media items.</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A task that represents the asynchronous operation, containing the JSON string with the search results.</returns>
	Task<string> GetSearchedMediaAsync(string mediaType, string keyword, int pageNumber);
	#endregion Search Media

	#region Media Detail
	/// <summary>
	/// Retrieves detailed information for a specified media item (movie or TV show) by its type and TMDB ID from the external TMDB API.
	/// </summary>
	/// <param name="mediaType">The type of media ("movie" or "tv").</param>
	/// <param name="mediaId">The TMDB ID of the media item.</param>
	/// <returns>A task that represents the asynchronous operation, containing the JSON string with detailed media data.</returns>
	Task<string> GetMediaDetailAsync(string mediaType, int mediaId);
	#endregion Media Detail

	#region Media Images
	/// <summary>
	/// Retrieves images for a specified media type (movie or TV show) and media ID from the external TMDB API.
	/// </summary>
	/// <param name="mediaType">The type of media ("movie" or "tv").</param>
	/// <param name="mediaId">The TMDB ID of the media item.</param>
	/// <returns>A task that represents the asynchronous operation, containing the JSON string with image data.</returns>
	Task<string> GetMediaImagesAsync(string mediaType, int mediaId);
	#endregion Media Images

	#region Similar Media
	/// <summary>
	/// Retrieves a list of similar media items (movies or TV shows) from the external TMDB API for a specified media type and ID.
	/// </summary>
	/// <param name="mediaType">The type of media ("movie" or "tv").</param>
	/// <param name="mediaId">The TMDB ID of the media item.</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A task that represents the asynchronous operation, containing the JSON string with similar media data.</returns>
	Task<string> GetSimilarMediaAsync(string mediaType, int mediaId, int pageNumber);
	#endregion Similar Media

	#region Media Trailer
	/// <summary>
	/// Retrieves trailer information for a specified media type (movie or TV show) and media ID from the external TMDB API.
	/// </summary>
	/// <param name="mediaType">The type of media ("movie" or "tv").</param>
	/// <param name="mediaId">The TMDB ID of the media item.</param>
	/// <returns>A task that represents the asynchronous operation, containing the JSON string with trailer data.</returns>
	Task<string> GetMediaTrailerAsync(string mediaType, int mediaId);
	#endregion Media Trailer

	#region Media Genres
	/// <summary>
	/// Retrieves a list of genres for a specified media type (movie or TV show) from the external TMDB API.
	/// </summary>
	/// <param name="mediaType">The type of media ("movie" or "tv").</param>
	/// <returns>A task that represents the asynchronous operation, containing the JSON string with genre data.</returns>
	Task<string> GetMediaGenresAsync(string mediaType);
	#endregion Media Genres

	#region Media Sorting
	/// <summary>
	/// Retrieves a list of media items (movies or TV shows) sorted by the specified criteria from the external TMDB API.
	/// </summary>
	/// <param name="mediaType">The type of media ("movie" or "tv").</param>
	/// <param name="sortBy">The sorting criteria (e.g., "popularity.desc", "release_date.desc").</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A task that represents the asynchronous operation, containing the JSON string with sorted media data.</returns>
	Task<string> GetSortMediaAsync(string mediaType, string sortBy, int pageNumber);
	#endregion Media Sorting

	#region TMDB API Call
	/// <summary>
	/// Handles the execution of a TMDB API call, processes the result, and returns a standardized <see cref="ResponseModel"/>.
	/// </summary>
	/// <typeparam name="T">The type to which the API response should be deserialized.</typeparam>
	/// <param name="apiCall">A function that performs the TMDB API call and returns the raw JSON response as a string.</param>
	/// <param name="successMessage">The message to include in the response if the API call is successful.</param>
	/// <param name="failMessage">The message to include in the response if the API call fails.</param>
	/// <param name="customDeserializer">An optional function to deserialize the JSON response to the specified type <typeparamref name="T"/>.</param>
	/// <returns>A <see cref="ResponseModel"/> indicating the success or failure of the operation, including the deserialized data if successful.</returns>
	Task<ResponseModel> HandleTmdbApiCallAsync<T>(Func<Task<string>> apiCall, string successMessage, string failMessage);
	#endregion TMDB API Call
}
