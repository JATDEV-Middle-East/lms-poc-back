using Application.Helpers;
using Application.Modules.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Users
{
	public interface IUsersService
	{
		Task<Response<UserModelDto>> GetProfile();
	}
}
