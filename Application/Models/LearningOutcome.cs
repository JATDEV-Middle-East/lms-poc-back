using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
	public class LearningOutcome
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string OutcomeText { get; set; } 
		public int CourseId { get; set; }
		public Course Course { get; set; }
	}

}
