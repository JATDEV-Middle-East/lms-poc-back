using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
	public class MyLearning
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int CourseId { get; set; }
		[Required]
		public int UserId { get; set; }
		public double Progress { get; set; }
		[Required]
		public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
		public Course Course { get; set; }
		public User User { get; set; }
	}
}
