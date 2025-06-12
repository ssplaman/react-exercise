using MovieLibraryApi.Model;
using MovieLibraryApi.Model.Dtos;

namespace MovieLibraryApi.Interface;

public interface IMovieService
{
	#region List
	/// <summary>
	/// Retrieves a list of movies from the external TMDB API for the specified page and genre.
	/// </summary>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <param name="genreId">The genre ID to filter the movies.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the list of movies or error information.</returns>
	Task<ResponseModel> GetMoviesListAsync(int pageNumber, int genreId);
	#endregion List

	#region Search
	/// <summary>
	/// Retrieves a list of movies from the external TMDB API that match the specified search keyword and page number.
	/// </summary>
	/// <param name="keyword">The search keyword to filter movies.</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the search results or error information.</returns>
	Task<ResponseModel> GetSearchMoviesListAsync(string keyword, int pageNumber);
	#endregion Search

	#region Detail
	/// <summary>
	/// Retrieves detailed information for a specific movie by its ID from the external TMDB API.
	/// </summary>
	/// <param name="movieId">The ID of the movie for which to fetch detailed information.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the movie details or error information.</returns>
	Task<ResponseModel> GetDetailOfMovieAsync(int movieId);
	#endregion Detail

	#region Images
	/// <summary>
	/// Retrieves images for a specific movie by its ID from the external TMDB API.
	/// </summary>
	/// <param name="movieId">The ID of the movie for which to fetch images.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the image data or error information.</returns>
	Task<ResponseModel> GetImagesOfMovieAsync(int movieId);
	#endregion Images

	#region Similar
	/// <summary>
	/// Retrieves a list of movies similar to the specified movie by its ID from the external TMDB API.
	/// </summary>
	/// <param name="movieId">The ID of the movie for which to fetch similar movies.</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the similar movies data or error information.</returns>
	Task<ResponseModel> GetSimilarMoviesAsync(int movieId, int pageNumber);
	#endregion Similar

	#region Reviews
	/// <summary>
	/// Retrieves all reviews for a specific movie by its ID.
	/// </summary>
	/// <param name="movieId">The ID of the movie for which to get reviews.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the movie reviews.</returns>
	Task<ResponseModel> GetMovieReviewAsync(int movieId);

	/// <summary>
	/// Saves a movie review for the specified movie.
	/// </summary>
	/// <param name="movieId">The ID of the movie to which the review belongs.</param>
	/// <param name="request">The review details to be saved.</param>
	/// <returns>A <see cref="ResponseModel"/> indicating the success or failure of the operation.</returns>
	Task<ResponseModel> SaveMovieReviewAsync(int movieId, ReviewMovieDto request);
	#endregion Reviews

	#region Trailer
	/// <summary>
	/// Retrieves trailer information for a specific movie by its ID from the external TMDB API.
	/// </summary>
	/// <param name="movieId">The ID of the movie for which to fetch trailer information.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the trailer data or error information.</returns>
	Task<ResponseModel> GetMovieTrailerAsync(int movieId);
	#endregion Trailer

	#region Genre
	/// <summary>
	/// Retrieves a list of movie genres from the external TMDB API.
	/// </summary>
	/// <returns>A <see cref="ResponseModel"/> containing the genre data or error information.</returns>
	Task<ResponseModel> GetGenreOfMovieAsync();
	#endregion Genre

	#region Sort
	/// <summary>
	/// Retrieves a list of movies sorted by the specified criteria from the external TMDB API.
	/// </summary>
	/// <param name="sortBy">The sorting criteria ("popular", "top rated").</param>
	/// <param name="pageNumber">The page number for paginated results.</param>
	/// <returns>A <see cref="ResponseModel"/> containing the sorted movies data or error information.</returns>
	Task<ResponseModel> GetSortMoviesAsync(string sortBy, int pageNumber);
	#endregion Sort
}
