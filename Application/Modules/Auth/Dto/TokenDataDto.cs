using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Auth.Dto
{
	public class TokenDataDto
	{
		public string Token { get; set; } = string.Empty;
		public DateTime ExpireFrom { get; set; }
		public DateTime ExpireTo { get; set; }
		public string ExpireTimeTo { get; set; } = string.Empty;

	}
	public class JsonTokenData
	{
		public string Token { get; set; } = string.Empty;
		public DateTime validTo { get; set; }
	}
}
