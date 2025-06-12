using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieLibraryApi.Interface;
using MovieLibraryApi.Model;
using MovieLibraryApi.Model.Dtos;
using MovieLibraryApi.Persistence.Data;
using MovieLibraryApi.Persistence.Entities;
using System.Text.Json;

namespace MovieLibraryApi.Service;

public class TvSeriesService(AppDbContext dbContext,
	IMapper mapper, ICommonService commonService) : ITvSeriesService
{
	#region List
	public async Task<ResponseModel> GetTvShowListAsync(int pageNumber, int genreId)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetMediaListAsync("tv", pageNumber, genreId),
			"Tv shows fetched successfully.",
			"Failed to fetch Tv shows.");
	}
	#endregion List

	#region Search
	public async Task<ResponseModel> GetSearchTvShowListAsync(string keyword, int pageNumber)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetSearchedMediaAsync("tv", keyword, pageNumber),
			"Searched tv show fetched successfully.",
			"Failed to fetch searched tv show.");
	}
	#endregion Search

	#region Detail
	public async Task<ResponseModel> GetDetailOfTvShowAsync(int tvId)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetMediaDetailAsync("tv", tvId),
			"Movie detail fetched successfully.",
			"Failed to fetch movie detail.");
	}
	#endregion Detail

	#region Images
	public async Task<ResponseModel> GetImagesOfTvShowAsync(int tvId)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetMediaImagesAsync("tv", tvId),
			"Images fetched successfully.",
			"Failed to fetch images.");
	}
	#endregion Images

	#region Similar
	public async Task<ResponseModel> GetSimilarTvShowAsync(int tvId, int pageNumber)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetSimilarMediaAsync("tv", tvId, pageNumber),
			"Similar tv show fetched successfully.",
			"Failed to fetch similar tv show.");
	}
	#endregion Similar

	#region Reviews
	public async Task<ResponseModel> GetTvReviewAsync(int tvId)
	{
		var reviews = await dbContext.ReviewTvSeries
		.Where(x => x.TvSeriesId == tvId)
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
	public async Task<ResponseModel> SaveTvReviewAsync(int tvId, ReviewMovieDto request)
	{
		var reviewTvSeries = mapper.Map<ReviewTvSeries>(request);
		reviewTvSeries.TvSeriesId = tvId;

		await dbContext.ReviewTvSeries.AddAsync(reviewTvSeries);
		var result = await dbContext.SaveChangesAsync();

		return result > 0 ? ResponseModel.Success("Review saved successfully.", null) : ResponseModel.Fail("Review are not saved");
	}
	#endregion Reviews

	#region Trailer
	public async Task<ResponseModel> GetTvShowTrailerAsync(int tvId)
	{
		try
		{
			var json = await commonService.GetMediaTrailerAsync("tv", tvId);
			var trailerResponse = JsonSerializer.Deserialize<TmdbTrailerResponse>(json);

			var trailerObject = trailerResponse?.Results?
				.FirstOrDefault(x =>
				string.Equals(x.Name, "Official Trailer", StringComparison.OrdinalIgnoreCase) &&
				string.Equals(x.Site, "Youtube", StringComparison.OrdinalIgnoreCase));

			return ResponseModel.Success("Trailer fetched successfully.", trailerObject);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Exception in GetMovieTrailer: {ex}");
			return ResponseModel.Fail("Failed to fetch trailer.", ex.Message);
		}
	}
	#endregion Trailer

	#region Genre
	public async Task<ResponseModel> GetGenreOfTvShowAsync()
	{
		try
		{
			var json = await commonService.GetMediaGenresAsync("tv");
			var imagesObject = JsonSerializer.Deserialize<object>(json);
			return ResponseModel.Success("Genres fetched successfully.", imagesObject);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Exception in GetImagesOfMovie: {ex}");
			return ResponseModel.Fail("Failed to fetch genres.", ex.Message);
		}
	}
	#endregion Genre

	#region Sort
	public async Task<ResponseModel> GetSortTvShowAsync(string sortBy, int pageNumber)
	{
		try
		{
			var json = await commonService.GetSortMediaAsync("movie", sortBy, pageNumber);
			var sotMovieObject = JsonSerializer.Deserialize<object>(json);
			return ResponseModel.Success("Sorted movies fetched successfully.", sotMovieObject);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Exception in GetSortMovies: {ex}");
			return ResponseModel.Fail("Failed to fetch sorted movies.", ex.Message);
		}
	}
	#endregion Sort
}
