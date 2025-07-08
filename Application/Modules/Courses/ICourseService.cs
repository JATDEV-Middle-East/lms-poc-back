using Application.Helpers;
using Application.Modules.Courses.Dto;
using Application.Modules.Courses.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Courses
{
	public interface ICourseService
	{
		Task<PaginatedResponse<CourseDto>> GetAllCoursesAsync(int pageNumber, int pageSize);
		Task<Response<CourseDto>> GetCourseByIdAsync(int id);
		Task<Response<CourseDto>> PurchaseAsync(MyLearningRequest model);
		Task<Response<IEnumerable<CourseDto>>> MyLearningAsync(int pageNumber, int pageSize);


	}
}
