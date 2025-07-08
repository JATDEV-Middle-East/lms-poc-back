using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
	public static class CourseSeeder
	{
		public static async Task SeedCoursesAsync(DatabaseContext context, int numberOfCourses = 50)
		{
			if (context.Courses.Any())
			{
				Console.WriteLine("Database already contains courses. Skipping seeding.");
				return;
			}

			var random = new Random();

			var learningOutcomeFaker = new Faker<LearningOutcome>()
				.RuleFor(lo => lo.OutcomeText, f => f.Lorem.Sentence(3, 5)); // Generate a short sentence

			// Faker for Lecture Items
			var lectureItemFaker = new Faker<LectureItem>()
				.RuleFor(li => li.Title, f => f.Lorem.Sentence(4, 8))
				.RuleFor(li => li.Type, f => f.PickRandom<LectureItemType>())
				.RuleFor(li => li.Duration, f => f.Date.Timespan().ToString(@"hh\:mm")) // Format as HH:MM
				.RuleFor(li => li.VideoUrl, f => f.Internet.Url() + ".mp4") // Dummy video URL
				.RuleFor(li => li.QuestionsCount, f => f.Random.Int(1, 10))
				.RuleFor(li => li.DocumentUrl, f => f.Internet.Url() + ".pdf") // Dummy document URL
				.RuleFor(li => li.IsCompleted, f => f.Random.Bool())
				.RuleFor(li => li.IsPreviewable, f => f.Random.Bool());

			// Faker for Course Sections
			var courseSectionFaker = new Faker<CourseSection>()
				.RuleFor(cs => cs.Id, f => Guid.NewGuid().ToString("N").Substring(0, 20)) // Generate a short unique string ID
				.RuleFor(cs => cs.Title, f => f.Commerce.ProductName())
				.RuleFor(cs => cs.Lectures, (f, cs) => lectureItemFaker.Generate(f.Random.Int(3, 8)).ToList()); // 3-8 lectures per section

			// Faker for Course Details
			var courseDetailFaker = new Faker<Course>()
				.RuleFor(c => c.Image, f => f.Image.PicsumUrl(640, 360, true)) // Random image URL
				.RuleFor(c => c.IntroVideoUrl, f => f.Internet.Url() + ".webm") // Dummy intro video URL
				.RuleFor(c => c.Type, f => f.PickRandom<CourseType>())
				.RuleFor(c => c.Level, f => f.PickRandom<CourseLevel>())
				.RuleFor(c => c.IsLive, f => f.Random.Bool())
				.RuleFor(c => c.EnrollmentCount, f => f.Random.Int(10, 1000))
				.RuleFor(c => c.Rating, f => f.Random.Double(3.0, 5.0))
				.RuleFor(c => c.Title, f => f.Commerce.ProductAdjective() + " " + f.Commerce.ProductName() + " Course")
				.RuleFor(c => c.Description, f => f.Lorem.Paragraph(2))
				.RuleFor(c => c.StartDate, f => f.Date.Past(1))
				.RuleFor(c => c.EndDate, (f, c) => f.Date.Future(1, c.StartDate))
				.RuleFor(c => c.DurationHours, f => f.Random.Double(5.0, 50.0))
				.RuleFor(c => c.Amount, f => f.Random.Decimal(10.0m, 500.0m))
				.RuleFor(c => c.LearningOutcomes, (f, c) => learningOutcomeFaker.Generate(f.Random.Int(5, 15)).ToList()) // 5-15 outcomes
				.RuleFor(c => c.Sections, (f, c) => courseSectionFaker.Generate(f.Random.Int(3, 7)).ToList()); // 3-7 sections

			var courses = courseDetailFaker.Generate(numberOfCourses);

			// Manually link nested entities to their parents
			foreach (var course in courses)
			{
				foreach (var section in course.Sections)
				{
					section.Course = course; // Link section back to course
					foreach (var lecture in section.Lectures)
					{
						lecture.CourseSection = section; // Link lecture back to section
					}
				}
				foreach (var outcome in course.LearningOutcomes)
				{
					outcome.Course = course; // Link outcome back to course
				}
			}

			await context.Courses.AddRangeAsync(courses);
			await context.SaveChangesAsync();

			Console.WriteLine("Course seeding complete.");
		}
	}
}
