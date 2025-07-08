using Application.Helpers;
using Application.Modules.Courses.Dto;
using Application.Modules.Users;
using Application.Modules.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lms_app.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersService _userService;

		public UsersController(IUsersService userService)
		{
			_userService = userService;
		}
		[HttpGet("profile")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<UserModelDto>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetUserProfile()
		{
			var user = await _userService.GetProfile();
			return await Task.FromResult(new JsonResult(user));
		}

	}
}
