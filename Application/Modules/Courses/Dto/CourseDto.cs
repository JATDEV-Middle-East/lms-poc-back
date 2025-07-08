using Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Courses.Dto
{
	public class CourseDto
	{
		public int Id { get; set; }
		public string? Image { get; set; }
		public string IntroVideoUrl { get; set; }
		public CourseType Type { get; set; }
		public CourseLevel Level { get; set; }
		public bool Assigned { get; set; }
		public bool IsLive { get; set; }
		public int EnrollmentCount { get; set; }
		public double Rating { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime EndDate { get; set; }
		public double DurationHours { get; set; }
		public double? Progress { get; set; }
		public decimal? Amount { get; set; }
		public ICollection<LearningOutcomeDto> LearningOutcomes { get; set; } = new List<LearningOutcomeDto>();
		public ICollection<CourseSectionDto> Sections { get; set; } = new List<CourseSectionDto>();
	}

	public class LearningOutcomeDto
	{
		public int Id { get; set; }
		public string OutcomeText { get; set; }
	}

	public class CourseSectionDto
	{
		public string Id { get; set; } // Matches the string ID from your model
		public string Title { get; set; }
		public ICollection<LectureItemDto> Lectures { get; set; } = new List<LectureItemDto>();
	}

	public class LectureItemDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public LectureItemType Type { get; set; }
		public string? Duration { get; set; }
		public string? VideoUrl { get; set; }
		public int? QuestionsCount { get; set; }
		public string? DocumentUrl { get; set; }
		public bool? IsCompleted { get; set; }
		public bool? IsPreviewable { get; set; }
	}
	public class MyLearningDto
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public int UserId { get; set; }
		public double Progress { get; set; }
		public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
	}
}
