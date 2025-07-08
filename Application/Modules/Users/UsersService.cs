using Application.Helpers;
using Application.Models;
using Application.Modules.Auth;
using Application.Modules.Users.Dto;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Users
{
	public class UsersService : IUsersService
	{
		private readonly DatabaseContext _context;
		private readonly IAuthService _authService;
		private readonly MyJWT _myJWT;
		private readonly IConfiguration _config;

		public string activeStatus = "active";
		public string deletedStatus = "deleted";
		public string deactivatedStatus = "deactivated";
		public UsersService(DatabaseContext context,
			IAuthService authService,
			IConfiguration config,
			MyJWT myjwt
			)
		{
			_context = context;
			_authService = authService;
			_config = config;
			_myJWT = myjwt;
		}

		public async Task<Response<UserModelDto>> GetProfile()
		{
			var id = _myJWT.GetAuthUser();

			var user = await _context.users.Where(x => x.Id == id).FirstOrDefaultAsync();
			//var user = await _context.users.Where(x => x.Email == request.Email || x.PhoneNumber == request.PhoneNumber).FirstOrDefaultAsync();

			if (user == null)
			{
				return await Task.FromResult(new Response<UserModelDto>
				{
					Success = false,
					Message = "User not found",
				});
			}
			var responseModel = await _authService.BuildUserModelAsync(user);
			if (responseModel != null)
			{
				return await Task.FromResult(new Response<UserModelDto>
				{
					Success = true,
					Message = "Request successful",
					Data = responseModel
				});
			}
			else
			{
				return await Task.FromResult(new Response<UserModelDto>
				{
					Success = false,
					Message = "Request successful",
				});
			}
		}

	}
}
