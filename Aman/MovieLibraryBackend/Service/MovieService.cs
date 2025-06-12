using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieLibraryApi.Interface;
using MovieLibraryApi.Model;
using MovieLibraryApi.Model.Dtos;
using MovieLibraryApi.Persistence.Data;
using MovieLibraryApi.Persistence.Entities;

namespace MovieLibraryApi.Service;

public class MovieService(AppDbContext dbContext,
	IMapper mapper, ICommonService commonService) : IMovieService
{
	#region List
	public async Task<ResponseModel> GetMoviesListAsync(int pageNumber, int genreId)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetMediaListAsync("movie", pageNumber, genreId),
			"Movie fetched successfully.",
			"Failed to fetch movie.");
	}
	#endregion List

	#region Search
	public async Task<ResponseModel> GetSearchMoviesListAsync(string keyword, int pageNumber)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetSearchedMediaAsync("movie", keyword, pageNumber),
			"Searched movie fetched successfully.",
			"Failed to fetch searched movie.");
	}
	#endregion Search

	#region Detail
	public async Task<ResponseModel> GetDetailOfMovieAsync(int movieId)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetMediaDetailAsync("movie", movieId),
			"Movie detail fetched successfully.",
			"Failed to fetch movie detail.");
	}
	#endregion Detail

	#region Images
	public async Task<ResponseModel> GetImagesOfMovieAsync(int movieId)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetMediaImagesAsync("movie", movieId),
			"Images fetched successfully.",
			"Failed to fetch images.");
	}
	#endregion Images

	#region Similar
	public async Task<ResponseModel> GetSimilarMoviesAsync(int movieId, int pageNumber)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetSimilarMediaAsync("movie", movieId, pageNumber),
			"Similar tv show fetched successfully.",
			"Failed to fetch similar tv show.");
	}
	#endregion Similar

	#region Reviews
	public async Task<ResponseModel> GetMovieReviewAsync(int movieId)
	{
		var reviews = await dbContext.ReviewMovie
			.Where(x => x.MovieId == movieId)
			.OrderByDescending(x => x.CreatedDate)
			.ProjectTo<ReviewSummaryDto>(mapper.ConfigurationProvider)
			.ToListAsync();

		var istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
		foreach (var review in reviews)
		{
			review.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(review.CreatedDate, istTimeZone);
		}

		return reviews.Count > 0
			? ResponseModel.Success(string.Empty, reviews)
			: ResponseModel.Fail("Reviews are empty");
	}
	public async Task<ResponseModel> SaveMovieReviewAsync(int movieId, ReviewMovieDto request)
	{
		var reviewMovie = mapper.Map<ReviewMovie>(request);
		reviewMovie.MovieId = movieId;

		await dbContext.ReviewMovie.AddAsync(reviewMovie);
		var result = await dbContext.SaveChangesAsync();

		return result > 0
			? ResponseModel.Success("Review saved successfully.", null)
			: ResponseModel.Fail("Review are not saved");
	}
	#endregion Reviews

	#region Trailer
	public async Task<ResponseModel> GetMovieTrailerAsync(int movieId)
	{
		var response = await commonService.HandleTmdbApiCallAsync<TmdbTrailerResponse>(
			() => commonService.GetMediaTrailerAsync("movie", movieId),
			"Trailer fetched successfully.",
			"Failed to fetch Trailer.");

		if (response.data != null)
		{
			var trailerResponse = response.data as TmdbTrailerResponse;
			var trailerObject = trailerResponse?.Results?
					.FirstOrDefault(x =>
					string.Equals(x.Name, "Official Trailer", StringComparison.OrdinalIgnoreCase) &&
					string.Equals(x.Site, "Youtube", StringComparison.OrdinalIgnoreCase));

			response.data = trailerObject;

			return response;
		}
		else
		{
			return response;
		}
	}
	#endregion Trailer

	#region Genre
	public async Task<ResponseModel> GetGenreOfMovieAsync()
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetMediaGenresAsync("movie"),
			"Genres fetched successfully.",
			"Failed to fetch genres.");
	}
	#endregion Genre

	#region Sort
	public async Task<ResponseModel> GetSortMoviesAsync(string sortBy, int pageNumber)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetSortMediaAsync("movie", sortBy, pageNumber),
			"Sorted movies fetched successfully.",
			"Failed to fetch sorted movies.");
	}
	#endregion Sort
}
