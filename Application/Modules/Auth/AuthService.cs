using Application.Helpers;
using Application.Models;
using Application.Modules.Auth.Dto;
using Application.Modules.Auth.Request;
using Application.Modules.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Auth
{
	public class AuthService: IAuthService
	{
		private readonly DatabaseContext _context;
		private readonly IConfiguration _config;

		public string activeStatus = "active";
		public string deletedStatus = "deleted";
		public string deactivatedStatus = "deactivated";
		public AuthService(DatabaseContext context,
			IConfiguration config
		)
		{
			_context = context;
			_config = config;
		}


		public async Task<Response<UserModelDto>> LoginAsync(LoginRequest request)
		{
			User? user = user = await _context.users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
			

			if (user == null)
			{
				return await Task.FromResult(new Response<UserModelDto>
				{
					Success = false,
					Message = "Invalid login combination",
					StatusCode = 404
				});
			}

			if (!VerifyPassword(request.Password, user.Password))
			{
				return await Task.FromResult(new Response<UserModelDto>
				{
					Message = "Invalid login combination"
				});
			}

			if (user.Status != activeStatus)
			{
				user.Status = activeStatus;
				user.DeletedOn = null;
				user.DeactivateReason = "";
				user.DeleteReason = "";
				await _context.SaveChangesAsync();
			}
			var responseModel = await BuildUserModelAsync(user);
			var token = GenerateJSONWebToken(user);


			responseModel.Token = new TokenDataDto
			{
				Token = token.Token,
				ExpireFrom = DateTime.Now,
				ExpireTo = token.validTo,
				ExpireTimeTo = $"{token.validTo.ToString("HH:mm:ss")}"
			};


			return await Task.FromResult(new Response<UserModelDto>
			{
				Success = true,
				Message = "login sucessfully",
				Data = responseModel

			});

		}


		public async Task<Response<SignupDto>> SignUpWithEmailAsync(EmailSignUpRequest dto)
		{
			var isUser = await _context.users.FirstOrDefaultAsync(x => x.Email.ToLower() == dto.Email.ToLower());

			if (isUser != null && isUser.IsEmailVerified == true)
			{
				SignupDto resp2 = new SignupDto();
				resp2.Email = isUser.Email;
				resp2.Id = isUser.Id;

				return await Task.FromResult(new Response<SignupDto>
				{
					Success = false,
					Message = "User already exit and Account Verified",
					StatusCode = 409,
					Data = resp2
				});
			}

			if (dto.Password != dto.ConfirmPassword)
			{
				return await Task.FromResult(new Response<SignupDto>
				{
					Success = false,
					Message = "Password and confirm password must be same",
					StatusCode = 400
				});
			}



			Random rx = new Random();
			int rand = rx.Next(100000, 999999);
			var user = new User();
			if (isUser == null)
			{
				user = new User
				{
					Email = dto.Email.ToLower(),
					CreatedAt = DateTime.UtcNow,
					AuthCode = rand,
					FirstName = dto.FirstName,
					LastName = dto.LastName,
				};
				_context.users.Add(user);
			}
			else
			{
				user = isUser;
				user.AuthCode = rand;
			}

			
			user.Password = EncryptionHelper.Encrypt(dto.Password);
			user.Status = activeStatus;
			await _context.SaveChangesAsync();

			user = await _context.users.Where(u => u.Email == dto.Email).FirstOrDefaultAsync();
			SignupDto resp1 = new SignupDto();
			resp1.Email = user.Email;
			resp1.Id = user.Id;
			return await Task.FromResult(new Response<SignupDto>
			{
				Success = true,
				Message = "Please check your email for otp ",
				Data = resp1
			});
		}
		public async Task<UserModelDto> BuildUserModelAsync(User user)
		{
			try
			{
				var u = await _context.users
					.Where(us => us.Id == user.Id)
					.FirstOrDefaultAsync();

			
				var userModel = new UserModelDto
				{
					Id = user.Id,
					Email = user.Email,
					FirstName = user.FirstName,
					LastName = user.LastName,
					UserName = user.UserName,
				};
				return userModel;
			}
			catch (Exception ex)
			{

				return new UserModelDto();
			}
		}
		public bool VerifyPassword(string password, string hashPassword)
		{

			string PlainPassword = EncryptionHelper.Decrypt(hashPassword);
			return PlainPassword == password;
		}
		private JsonTokenData GenerateJSONWebToken(User user)
		{
			// Create claims to include in the JWT token
			var claims = new[]
			{
					new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
					new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var expires = DateTime.UtcNow.AddMinutes((int.Parse(_config["Jwt:ExpireIn"]) + 1000000));
			// Create the token
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = expires,
				Issuer = _config["Jwt:Issuer"],
				Audience = _config["Jwt:ValidAudience"],
				SigningCredentials = credentials
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			var finalToken = tokenHandler.WriteToken(token);
			var JsonTokenData = new JsonTokenData()
			{
				Token = finalToken,
				validTo = expires
			};
			return JsonTokenData;
		}

	}

}
