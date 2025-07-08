using Application.Helpers;
using Application.Modules.Auth;
using Application.Modules.Auth.Dto;
using Application.Modules.Auth.Request;
using Application.Modules.Users.Dto;
using Microsoft.AspNetCore.Mvc;

namespace lms_app.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("signup/email")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<SignupDto>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> SignUpWithEmail([FromBody] EmailSignUpRequest model)
		{
			var user = await _authService.SignUpWithEmailAsync(model);
			return await Task.FromResult(new JsonResult(user));
		}

		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<UserModelDto>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> LoginAsync([FromBody] LoginRequest model)
		{
			var user = await _authService.LoginAsync(model);
			return await Task.FromResult(new JsonResult(user));
		}
	}

}
