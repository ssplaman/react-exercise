using Microsoft.AspNetCore.Mvc;
using MovieLibraryApi.Interface;
using MovieLibraryApi.Model;
using MovieLibraryApi.Model.Dtos;

namespace MovieLibraryApi.Controllers;

[ApiController]
[Route("api/movie")]
public class MovieController(IMovieService movieService) : ControllerBase
{
	#region List
	[HttpGet]
	[Route("list")]
	public async Task<ActionResult<ResponseModel>> GetMoviesListAsync(int genreId, int pageNumber = 1)
	{
		var response = await movieService.GetMoviesListAsync(pageNumber, genreId);

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
	public async Task<ActionResult<ResponseModel>> GetSearchMoviesListAsync(string search, int pageNumber = 1)
	{
		var response = await movieService.GetSearchMoviesListAsync(search, pageNumber);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Search

	#region Detail
	[HttpGet]
	[Route("{movieId}/detail")]
	public async Task<ActionResult<ResponseModel>> GetDetailOfMovieAsync(int movieId)
	{
		if (movieId <= 0)
			return BadRequest(ResponseModel.Fail("Movie id is invalid"));

		var response = await movieService.GetDetailOfMovieAsync(movieId);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Detail

	#region Images
	[HttpGet]
	[Route("{movieId}/images")]
	public async Task<ActionResult<ResponseModel>> GetImagesOfMovieAsync(int movieId)
	{
		if (movieId <= 0)
			return BadRequest(ResponseModel.Fail("Movie id is invalid"));

		var response = await movieService.GetImagesOfMovieAsync(movieId);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Images

	#region Similar
	[HttpGet]
	[Route("{movieId}/similar")]
	public async Task<ActionResult<ResponseModel>> GetSimilarMoviesAsync(int movieId, int pageNumber = 1)
	{
		if (movieId <= 0)
			return BadRequest(ResponseModel.Fail("Movie id is invalid"));

		var response = await movieService.GetSimilarMoviesAsync(movieId, pageNumber);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Similar

	#region Reviews
	[HttpGet]
	[Route("{movieId}/reviews")]
	public async Task<ActionResult<ResponseModel>> GetMovieReviewAsync(int movieId)
	{
		if (movieId <= 0)
			return BadRequest(ResponseModel.Fail("Movie id is invalid"));

		var response = await movieService.GetMovieReviewAsync(movieId);

		if (response.data == null || response.data is IEnumerable<ReviewSummaryDto> reviews && !reviews.Any())
			return NoContent();

		return Ok(response);
	}

	[HttpPost]
	[Route("{movieId}/reviews")]
	public async Task<ActionResult<ResponseModel>> SaveReviewAsync(int movieId, [FromBody] ReviewMovieDto request)
	{
		if (movieId <= 0)
			return BadRequest(ResponseModel.Fail("Movie Id is required"));

		if (!ModelState.IsValid)
			return BadRequest(ResponseModel.Fail("Invalid Input"));

		var response = await movieService.SaveMovieReviewAsync(movieId, request);

		return response.IsSuccess ? Ok(response) : BadRequest(response);
	}
	#endregion Reviews

	#region Trailer
	[HttpGet]
	[Route("{movieId}/trailer")]
	public async Task<ActionResult<ResponseModel>> GetMovieTrailerAsync(int movieId)
	{
		if (movieId <= 0)
			return BadRequest(ResponseModel.Fail("Movie id is invalid"));

		var response = await movieService.GetMovieTrailerAsync(movieId);

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
	public async Task<ActionResult<ResponseModel>> GetGenreOfMovieAsync()
	{
		var response = await movieService.GetGenreOfMovieAsync();

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
	public async Task<ActionResult<ResponseModel>> GetSortMoviesAsync(string sortBy, int pageNumber = 1)
	{
		if (string.IsNullOrEmpty(sortBy))
			return BadRequest(ResponseModel.Fail("Sort option is invalid"));

		if (!string.Equals(sortBy, "popular", StringComparison.OrdinalIgnoreCase) && !string.Equals(sortBy, "top_rated", StringComparison.OrdinalIgnoreCase))
			return BadRequest(ResponseModel.Fail("Sort option must be either 'popular' or 'top_rated'."));

		var response = await movieService.GetSortMoviesAsync(sortBy, pageNumber);

		if (!response.IsSuccess)
			return BadRequest(response);

		if (response.data == null)
			return NoContent();

		return Ok(response);
	}
	#endregion Sort
}
