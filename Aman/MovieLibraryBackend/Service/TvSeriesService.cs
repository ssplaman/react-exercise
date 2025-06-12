using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieLibraryApi.Interface;
using MovieLibraryApi.Model;
using MovieLibraryApi.Model.Dtos;
using MovieLibraryApi.Persistence.Data;
using MovieLibraryApi.Persistence.Entities;

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
		var response = await commonService.HandleTmdbApiCallAsync<TmdbTrailerResponse>(
			() => commonService.GetMediaTrailerAsync("tv", tvId),
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
	public async Task<ResponseModel> GetGenreOfTvShowAsync()
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetMediaGenresAsync("tv"),
			"Genres fetched successfully.",
			"Failed to fetch genres.");
	}
	#endregion Genre

	#region Sort
	public async Task<ResponseModel> GetSortTvShowAsync(string sortBy, int pageNumber)
	{
		return await commonService.HandleTmdbApiCallAsync<object>(
			() => commonService.GetSortMediaAsync("tv", sortBy, pageNumber),
			"Sorted tv show fetched successfully.",
			"Failed to fetch sorted tv show.");
	}
	#endregion Sort
}
