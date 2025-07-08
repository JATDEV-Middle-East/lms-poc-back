using Application.Helpers;
using Application.Migrations;
using Application.Models;
using Application.Modules.Courses.Dto;
using Application.Modules.Courses.Request;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Modules.Courses
{
	public class CourseService : ICourseService
	{
		private readonly DatabaseContext _context;
		private readonly MyJWT _myjwt;
		public CourseService(DatabaseContext context, MyJWT myJWT)
		{
			_context = context;
			_myjwt = myJWT;
		}
		public async Task<PaginatedResponse<CourseDto>> GetAllCoursesAsync(int pageNumber, int pageSize)
		{
			var userId = _myjwt.GetAuthUser();
			var myLearnings = await _context.MyLearnings.Select(l => new MyLearningDto
			{
				Id = l.Id,
				CourseId = l.CourseId,
				Progress = l.Progress,
				PurchaseDate = l.PurchaseDate
			}).ToListAsync();
			var courses = _context.Courses
							.Include(c => c.Purchases)
							.Include(c => c.LearningOutcomes)
							.Include(c => c.Sections)
								.ThenInclude(s => s.Lectures)
							.Select(CourseResource(userId));
			var offset = (pageNumber - 1) * pageSize;
			var totalPages = await courses.CountAsync();
			var response = await courses
				.Skip(offset)
				.Take(pageSize)
				.ToListAsync();

			var res = new PaginatedResponse<CourseDto>()
			{
				TotalCount = totalPages,
				CurrentPage = pageNumber,
				PageSize = pageSize,
				Success = true,
				Message = $"request successful",
				StatusCode = 200,
				Data = response

			};
			return await Task.FromResult(res);
		}

		public async Task<Response<CourseDto>> GetCourseByIdAsync(int id)
		{
			var userId = _myjwt.GetAuthUser();
			var myLearnings = await _context.MyLearnings.Select(l => new MyLearningDto
			{
				Id = l.Id,
				CourseId = l.CourseId,
				Progress = l.Progress,
				PurchaseDate = l.PurchaseDate
			}).ToListAsync();
			var course = await _context.Courses
								.Include(c => c.Purchases)
								.Include(c => c.LearningOutcomes)
								.Include(c => c.Sections)
									.ThenInclude(s => s.Lectures)
								.Select(CourseResource(userId))
								.FirstOrDefaultAsync(c => c.Id == id);
			return await Task.FromResult(new Response<CourseDto>
			{
				Success = true,
				Message = "",
				Data = course
			});
		}

		public async Task<Response<CourseDto>> PurchaseAsync(MyLearningRequest model)
		{
			var userId = _myjwt.GetAuthUser();
			var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == model.CourseId);

			if(course == null)
			{
				return await Task.FromResult(new Response<CourseDto>
				{
					Success = false,
					StatusCode = 400,
					Message = "Course not found",
				});
			}

			var isPurchased = await _context.MyLearnings.Where(l => l.CourseId == course.Id && l.UserId == userId).FirstOrDefaultAsync();
			if(isPurchased != null)
			{
				return await Task.FromResult(new Response<CourseDto>
				{
					Success = false,
					StatusCode = 400,
					Message = "User already enrolled for this course",
				});
			}

			var myLearning = new MyLearning()
			{
				CourseId = course.Id,
				UserId = userId,
				Progress = 0,
				PurchaseDate = DateTime.UtcNow
			};
			await _context.MyLearnings.AddAsync(myLearning);
			await _context.SaveChangesAsync();
			return await Task.FromResult(new Response<CourseDto>
			{
				Success = true,
				Message = "Course enrolled successful",
			});

		}

		public async Task<Response<IEnumerable<CourseDto>>> MyLearningAsync(int pageNumber, int pageSize)
		{
			var userId = _myjwt.GetAuthUser();
			var myLearningIds = await _context.MyLearnings.Select(l => l.CourseId).ToListAsync();
			var myLearnings = await _context.MyLearnings.Select(l => new MyLearningDto
			{
				Id = l.Id,
				CourseId = l.CourseId,
				Progress = l.Progress,
				PurchaseDate = l.PurchaseDate
			}).ToListAsync();

			var courses = _context.Courses
					.Include(c => c.Purchases)
					.Include(c => c.LearningOutcomes)
					.Include(c => c.LearningOutcomes)
					.Include(c => c.Sections)
						.ThenInclude(s => s.Lectures)
					.Select(CourseResource(userId))
					.Where(l => myLearningIds.Contains(l.Id));

			var offset = (pageNumber - 1) * pageSize;
			var response = await courses.Take(pageSize).Skip(offset).ToListAsync();

			return await Task.FromResult(new Response<IEnumerable<CourseDto>>
			{
				Success = true,
				Message = "",
				Data = response
			});
		}
		private static Expression<Func<Course, CourseDto>> CourseResource(int userId)
		{
			return course => new CourseDto
			{
                // Map primitive properties
                Id = course.Id,
				Image = course.Image,
				IntroVideoUrl = course.IntroVideoUrl,
				Type = course.Type,
                Level = course.Level,
				Assigned = course.Purchases.Any(l => l.UserId == userId),
				IsLive = course.IsLive,
				EnrollmentCount = course.EnrollmentCount,
				Rating = course.Rating,
				Title = course.Title,
				Description = course.Description,
				StartDate = course.StartDate,
				EndDate = course.EndDate,
				DurationHours = course.DurationHours,
				Progress = course.Purchases.Where(p => p.UserId == userId).FirstOrDefault() == null ? course.Purchases.Where(p => p.UserId == userId).First().Progress : 0,
				Amount = course.Amount,
				PurchaseDate = course.Purchases.Where(p => p.UserId == userId).FirstOrDefault() == null ? course.Purchases.Where(p => p.UserId == userId).First().PurchaseDate : null,
				LearningOutcomes = course.LearningOutcomes.Select(lo => new LearningOutcomeDto
				{
					Id = lo.Id,
					OutcomeText = lo.OutcomeText
				}).ToList(),

                Sections = course.Sections.Select(s => new CourseSectionDto
				{
					Id = s.Id,
					Title = s.Title,
					Lectures = s.Lectures.Select(l => new LectureItemDto
					{
						Id = l.Id,
						Title = l.Title,
						Type = l.Type, 
                        Duration = l.Duration,
						VideoUrl = l.VideoUrl,
						QuestionsCount = l.QuestionsCount,
						DocumentUrl = l.DocumentUrl,
						IsCompleted = l.IsCompleted,
						IsPreviewable = l.IsPreviewable
					}).ToList()
				}).ToList()
			};

		}
	}
}
