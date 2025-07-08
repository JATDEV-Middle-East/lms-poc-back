using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
	public class Course
	{
		[Key]
		public int Id { get; set; }
		[MaxLength(255)]
		public string? Image { get; set; }
		[Required]
		[MaxLength(2048)]
		public string IntroVideoUrl { get; set; }
		[Required]
		public CourseType Type { get; set; }
		[Required]
		public CourseLevel Level { get; set; }

		[Required]
		public bool IsLive { get; set; }
		[Required]
		public int EnrollmentCount { get; set; }
		[Required]
		public double Rating { get; set; }
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
		public ICollection<LearningOutcome> LearningOutcomes { get; set; } = new List<LearningOutcome>();
		public ICollection<CourseSection> Sections { get; set; } = new List<CourseSection>();
		public ICollection<MyLearning> Purchases { get; set; } = new List<MyLearning>();
	}

	public enum CourseType
	{
		Paid,
		Free
	}

	public enum CourseLevel
	{
		Beginner,
		Intermediate,
		Advanced
	}

	public enum LectureItemType
	{
		Video,
		Quiz,
		Document,
		Assignment,
		Other
	}
}
