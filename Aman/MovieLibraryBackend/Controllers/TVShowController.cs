using Microsoft.AspNetCore.Mvc;
using MovieLibraryApi.Interface;
using MovieLibraryApi.Model;
using MovieLibraryApi.Model.Dtos;

namespace MovieLibraryApi.Controllers;

[ApiController]
[Route("api/tv")]
public class TVShowController(ITvSeriesService tvService) : ControllerBase
{
	#region List
	[HttpGet]
	[Route("list")]
	public async Task<ActionResult<ResponseModel>> GetTvShowListAsync(int genreId, int pageNumber = 1)
	{
		var response = await tvService.GetTvShowListAsync(pageNumber, genreId);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion List

	#region Search
	[HttpGet]
	[Route("{search}/search")]
	public async Task<ActionResult<ResponseModel>> GetSearchTvShowListAsync(string search, int pageNumber = 1)
	{
		var response = await tvService.GetSearchTvShowListAsync(search, pageNumber);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Search

	#region Detail
	[HttpGet]
	[Route("{tvId}/detail")]
	public async Task<ActionResult<ResponseModel>> GetDetailOfTvShowAsync(int tvId)
	{
		if (tvId <= 0)
			return BadRequest(ResponseModel.Fail("TV show id is invalid"));

		var response = await tvService.GetDetailOfTvShowAsync(tvId);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Detail

	#region Images
	[HttpGet]
	[Route("{tvId}/images")]
	public async Task<ActionResult<ResponseModel>> GetImagesOfTvShowAsync(int tvId)
	{
		if (tvId <= 0)
			return BadRequest(ResponseModel.Fail("TV show id is invalid"));

		var response = await tvService.GetImagesOfTvShowAsync(tvId);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Images

	#region Similar
	[HttpGet]
	[Route("{tvId}/similar")]
	public async Task<ActionResult<ResponseModel>> GetSimilarTvShowAsync(int tvId, int pageNumber = 1)
	{
		if (tvId <= 0)
			return BadRequest(ResponseModel.Fail("TV show id is invalid"));

		var response = await tvService.GetSimilarTvShowAsync(tvId, pageNumber);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Similar

	#region Reviews
	[HttpGet]
	[Route("{tvId}/reviews")]
	public async Task<ActionResult<ResponseModel>> GetTvReviewAsync(int tvId)
	{
		if (tvId <= 0)
			return BadRequest(ResponseModel.Fail("TV show id is invalid"));

		var response = await tvService.GetTvReviewAsync(tvId);

		if (response.data == null || response.data is IEnumerable<ReviewSummaryDto> reviews && !reviews.Any())
			return NoContent();

		return Ok(response);
	}

	[HttpPost]
	[Route("{tvId}/reviews")]
	public async Task<ActionResult<ResponseModel>> SaveReviewAsync(int tvId, [FromBody] ReviewMovieDto request)
	{
		if (tvId <= 0)
			return BadRequest(ResponseModel.Fail("TV show is required"));

		if (!ModelState.IsValid)
			return BadRequest(ResponseModel.Fail("Invalid Input"));

		var response = await tvService.SaveTvReviewAsync(tvId, request);

		return response.IsSuccess ? Ok(response) : BadRequest(response);
	}
	#endregion Reviews

	#region Trailer
	[HttpGet]
	[Route("{tvId}/trailer")]
	public async Task<ActionResult<ResponseModel>> GetTvShowTrailerAsync(int tvId)
	{
		if (tvId <= 0)
			return BadRequest(ResponseModel.Fail("TV show id is invalid"));

		var response = await tvService.GetTvShowTrailerAsync(tvId);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Trailer

	#region Genre
	[HttpGet]
	[Route("genre")]
	public async Task<ActionResult<ResponseModel>> GetGenreOfTvShowAsync()
	{
		var response = await tvService.GetGenreOfTvShowAsync();

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Genre

	#region Sort
	[HttpGet]
	[Route("{sortBy}")]
	public async Task<ActionResult<ResponseModel>> GetSortTvShowAsync(string sortBy, int pageNumber = 1)
	{
		if (string.IsNullOrEmpty(sortBy))
			return BadRequest(ResponseModel.Fail("Sort option is invalid"));

		if (!string.Equals(sortBy, "popular", StringComparison.OrdinalIgnoreCase) && !string.Equals(sortBy, "top_rated", StringComparison.OrdinalIgnoreCase))
			return BadRequest(ResponseModel.Fail("Sort option must be either 'popular' or 'top_rated'."));

		var response = await tvService.GetSortTvShowAsync(sortBy, pageNumber);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Sort
}
