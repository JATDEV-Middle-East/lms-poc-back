using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
	public class CourseSection
	{
		[Key]
		[MaxLength(50)] 
		public string Id { get; set; } 

		[Required]
		[MaxLength(255)]
		public string Title { get; set; }
		public int CourseId { get; set; }

		public Course? Course { get; set; }
		
		public ICollection<LectureItem> Lectures { get; set; } = new List<LectureItem>();
	}

}
