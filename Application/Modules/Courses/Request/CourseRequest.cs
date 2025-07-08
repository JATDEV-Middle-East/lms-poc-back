using Application.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Modules.Courses.Request
{
	public class CreateCourseDto
	{
		[MaxLength(255)]
		public string? Image { get; set; }

		[Required]
		[MaxLength(2048)]
		public string IntroVideoUrl { get; set; }

		[Required]
		public CourseType Type { get; set; }

		[Required]
		public CourseLevel Level { get; set; }

		public bool Assigned { get; set; } = false; // Default values for creation
		public bool IsLive { get; set; } = false;
		public int EnrollmentCount { get; set; } = 0;
		public double Rating { get; set; } = 0.0;

		[Required]
		[MaxLength(255)]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime EndDate { get; set; }

		[Required]
		public double DurationHours { get; set; }

		public decimal? Amount { get; set; }

		public ICollection<CreateLearningOutcomeDto> LearningOutcomes { get; set; } = new List<CreateLearningOutcomeDto>();
		public ICollection<CreateCourseSectionDto> Sections { get; set; } = new List<CreateCourseSectionDto>();
	}

	public class CreateLearningOutcomeDto
	{
		[Required]
		public string OutcomeText { get; set; }
	}

	public class CreateCourseSectionDto
	{
		[Required]
		[MaxLength(50)]
		public string Id { get; set; }

		[Required]
		[MaxLength(255)]
		public string Title { get; set; }
		public ICollection<CreateLectureItemDto> Lectures { get; set; } = new List<CreateLectureItemDto>();
	}

	public class CreateLectureItemDto
	{
		[Required]
		[MaxLength(255)]
		public string Title { get; set; }

		[Required]
		public LectureItemType Type { get; set; }

		[MaxLength(50)]
		public string? Duration { get; set; }

		[MaxLength(2048)]
		public string? VideoUrl { get; set; }

		public int? QuestionsCount { get; set; }

		[MaxLength(2048)]
		public string? DocumentUrl { get; set; }

		public bool? IsCompleted { get; set; }
		public bool? IsPreviewable { get; set; }
	}
}
