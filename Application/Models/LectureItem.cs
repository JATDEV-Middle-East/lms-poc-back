using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
	public class LectureItem
	{
		[Key]
		public int Id { get; set; } 

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

		
		public string CourseSectionId { get; set; } 

		
		public CourseSection CourseSection { get; set; }
	}

}
