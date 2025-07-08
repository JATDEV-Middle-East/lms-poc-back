using Application.Modules.Auth.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Users.Dto
{
	public class UserModelDto
	{
		public int Id { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? PhoneCode { get; set; }
		public bool? IsEmailVerified { get; set; } = false;

		// Profile info
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Gender { get; set; } // male/female/other
		public DateTime? DateOfBirth { get; set; }

		public string? UserName { get; set; }
		public string? AvatarUrl { get; set; }
		public string? Password { get; set; }
		public string? Status { get; set; }
		public string? DeactivateReason { get; set; }
		public string? DeleteReason { get; set; }
		public DateTime? DeletedOn { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public TokenDataDto? Token { get; set; }

	}
}
