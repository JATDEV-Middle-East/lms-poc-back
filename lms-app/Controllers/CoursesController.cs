using Application.Helpers;
using Application.Models;
using Application.Modules.Courses;
using Application.Modules.Courses.Dto;
using Application.Modules.Courses.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lms_app.Controllers
{
	[Route("api/courses")]
	[ApiController]
	[Authorize]
	public class CoursesController : ControllerBase
	{
		private readonly ICourseService _courseService;

		public CoursesController(ICourseService courseService)
		{
			_courseService = courseService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<CourseDto>))]
		public async Task<IActionResult> GetCourses(int pageNumber = 1, int pageSize = 10)
		{
			var response = await _courseService.GetAllCoursesAsync(pageNumber, pageSize);
			return await Task.FromResult(new JsonResult(response));
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CourseDto>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCourse(int id)
		{
			var response = await _courseService.GetCourseByIdAsync(id);
			return await Task.FromResult(new JsonResult(response));
		}

		[HttpPost("purchase")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CourseDto>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Purchase([FromBody] MyLearningRequest model)
		{
			var response = await _courseService.PurchaseAsync(model);
			return await Task.FromResult(new JsonResult(response));
		}

		[HttpGet("my-learning")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<IEnumerable<CourseDto>>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> MyLearning(int pageNumber = 1, int pageSize = 10)
		{
			var response = await _courseService.MyLearningAsync(pageNumber, pageSize);
			return await Task.FromResult(new JsonResult(response));
		}
	}
}
