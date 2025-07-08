using Application.Helpers;
using Application.Models;
using Application.Modules.Auth.Dto;
using Application.Modules.Auth.Request;
using Application.Modules.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Auth
{
	public interface IAuthService
	{
		Task<Response<SignupDto>> SignUpWithEmailAsync(EmailSignUpRequest dto);
		Task<UserModelDto> BuildUserModelAsync(User user);

		Task<Response<UserModelDto>> LoginAsync(LoginRequest request);
	}
}
