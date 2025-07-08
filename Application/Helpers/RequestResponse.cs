using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
	public class Response<T>
	{
		public bool? Success { get; set; } = false;
		public int? StatusCode { get; set; } = 200;
		public string? Message { get; set; }
		public T? Data { get; set; }
		public string? errormessage { get; set; } = "";
	}

	public class PaginatedResponse<T>
	{
		public List<T> Data { get; set; }
		public int PageSize { get; set; } = 10;
		public int TotalCount { get; set; } = 0;
		public int CurrentPage { get; set; } = 0;
		public bool? Success { get; set; } = false;
		public int? StatusCode { get; set; } = 200;
		public string? Message { get; set; }
	}
}
