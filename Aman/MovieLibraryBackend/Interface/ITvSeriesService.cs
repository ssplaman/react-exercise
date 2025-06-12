using MovieLibraryApi.Model;
using MovieLibraryApi.Model.Dtos;

namespace MovieLibraryApi.Interface;

public interface ITvSeriesService
{
	#region List
	/// <summary>
	/// Retrieves a list of tv show from the external TMDB API for the specified page and genre.
	/// </summary>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <param name="genreId">The genre ID to filter the tv show.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the list of tv show or error information.</returns>
	Task<ResponseModel> GetTvShowListAsync(int pageNumber, int genreId);
	#endregion List

	#region Search
	/// <summary>
	/// Retrieves a list of tv show from the external TMDB API that match the specified search keyword and page number.
	/// </summary>
	/// <param name="keyword">The search keyword to filter tv show.</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the search results or error information.</returns>
	Task<ResponseModel> GetSearchTvShowListAsync(string keyword, int pageNumber);
	#endregion Search

	#region Detail
	/// <summary>
	/// Retrieves detailed information for a specific tv show by its ID from the external TMDB API.
	/// </summary>
	/// <param name="tvId">The ID of the tv show for which to fetch detailed information.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the tv show details or error information.</returns>
	Task<ResponseModel> GetDetailOfTvShowAsync(int tvId);
	#endregion Detail

	#region Images
	/// <summary>
	/// Retrieves images for a specific tv show by its ID from the external TMDB API.
	/// </summary>
	/// <param name="tvId">The ID of the tv show for which to fetch images.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the image data or error information.</returns>
	Task<ResponseModel> GetImagesOfTvShowAsync(int tvId);
	#endregion Images

	#region Similar
	/// <summary>
	/// Retrieves a list of tv shows similar to the specified movie by its ID from the external TMDB API.
	/// </summary>
	/// <param name="movieId">The ID of the movie for which to fetch similar tv shows.</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the similar tv shows data or error information.</returns>
	Task<ResponseModel> GetSimilarTvShowAsync(int tvId, int pageNumber);
	#endregion Similar

	#region Reviews
	/// <summary>
	/// Retrieves all reviews for a specific tv show by its ID.
	/// </summary>
	/// <param name="tvId">The ID of the tv show for which to get reviews.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the tv show reviews.</returns>
	Task<ResponseModel> GetTvReviewAsync(int tvId);

	/// <summary>
	/// Saves a tv show review for the specified tv show.
	/// </summary>
	/// <param name="tvId">The ID of the tv show to which the review belongs.</param>
	/// <param name="request">The review details to be saved.</param>
	/// <returns>A <see cref="ResponseModel"/> indicating the success or failure of the operation.</returns>
	Task<ResponseModel> SaveTvReviewAsync(int tvId, ReviewMovieDto request);
	#endregion Reviews

	#region Trailer
	/// <summary>
	/// Retrieves trailer information for a specific tv show by its ID from the external TMDB API.
	/// </summary>
	/// <param name="tvId">The ID of the tv show for which to fetch trailer information.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the trailer data or error information.</returns>
	Task<ResponseModel> GetTvShowTrailerAsync(int tvId);
	#endregion Trailer

	#region Genre
	/// <summary>
	/// Retrieves a list of tv show genres from the external TMDB API.
	/// </summary>
	/// <returns>A <see cref="ResponseModel"/> containing the genre data or error information.</returns>
	Task<ResponseModel> GetGenreOfTvShowAsync();
	#endregion Genre

	#region Sort
	/// <summary>
	/// Retrieves a list of tv show sorted by the specified criteria from the external TMDB API.
	/// </summary>
	/// <param name="sortBy">The sorting criteria ("popular", "top rated").</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the sorted tv show data or error information.</returns>
	Task<ResponseModel> GetSortTvShowAsync(string sortBy, int pageNumber);
	#endregion Sort
}
