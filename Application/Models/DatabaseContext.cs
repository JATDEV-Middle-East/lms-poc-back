using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Application.Models
{
	public class DatabaseContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options)
			: base(options)
		{
		}

		public DbSet<User> users { get; set; }

		public DbSet<Course> Courses { get; set; }
		public DbSet<LearningOutcome> LearningOutcomes { get; set; }
		public DbSet<CourseSection> CourseSections { get; set; }
		public DbSet<LectureItem> LectureItems { get; set; }
		public DbSet<MyLearning> MyLearnings { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			
			builder.Entity<LectureItem>()
			  .HasOne(di => di.CourseSection)
			  .WithMany(u => u.Lectures)
			  .HasForeignKey(di => di.CourseSectionId);

			builder.Entity<CourseSection>()
			  .HasOne(di => di.Course)
			  .WithMany(u => u.Sections)
			  .HasForeignKey(di => di.CourseId);
			
			builder.Entity<LearningOutcome>()
			  .HasOne(di => di.Course)
			  .WithMany(u => u.LearningOutcomes)
			  .HasForeignKey(di => di.CourseId);

			builder.Entity<MyLearning>()
			  .HasOne(di => di.Course)
			  .WithMany(u => u.Purchases)
			  .HasForeignKey(di => di.CourseId);

			builder.Entity<MyLearning>()
			  .HasOne(di => di.User)
			  .WithMany(u => u.MyLearning)
			  .HasForeignKey(di => di.UserId);

			builder.Entity<Course>()
				.Property(c => c.IntroVideoUrl)
				.HasMaxLength(2048); 


		}
	}
}
