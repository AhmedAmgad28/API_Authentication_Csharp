using API_Lab2.DTOs;
using API_Lab2.Entity;
using API_Lab2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        public CourseController(UniContext _db)
        {
            db = _db;
        }
        UniContext db;
        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = db.Course.Include(d => d.Students).ToList();
            if (courses is null)
            {
                return NotFound();
            }
            var courseDetails = new List<CourseWithStd>();
            foreach (var crs in courses)
            {
                var crsdetails = new CourseWithStd();
                crsdetails.Name = crs.Name;
                crsdetails.Description = crs.Description;
                foreach (var stdCourses in crs.Students)
                {
                    crsdetails.StudentsNames.Add(stdCourses.Name);
                }
                courseDetails.Add(crsdetails);
            }
            return Ok(courseDetails);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetByID(int id)
        {
            var courses = db.Course.Include(d => d.Students).FirstOrDefault(s => s.Id == id);
            if (courses is null)
            {
                return NotFound();
            }
               var crsdetails = new CourseWithStd();
                crsdetails.Name = courses.Name;
                crsdetails.Description = courses.Description;
                foreach (var stdCourses in courses.Students)
                {
                    crsdetails.StudentsNames.Add(stdCourses.Name);
                }
            return Ok(new { message = $"Course with id:{id} exists.", Course = crsdetails });
        }

        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            var courses = db.Course.Include(d => d.Students).FirstOrDefault(x => x.Name == name);
            if (courses is null)
            {
                return NotFound();
            }
            var crsdetails = new CourseWithStd();
            crsdetails.Name = courses.Name;
            crsdetails.Description = courses.Description;
            foreach (var stdCourses in courses.Students)
            {
                crsdetails.StudentsNames.Add(stdCourses.Name);
            }
            return Ok(new { message = $"Course with name:{name} exists.", Course = crsdetails });
        }

        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            if (course is null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                db.Course.Add(course);
                db.SaveChanges();
                return CreatedAtAction("GetByID", new { id = course.Id }, "Added Successfully");
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        public IActionResult RemoveCourse(int id)
        {
            var course = db.Course.Find(id);
            if (course is null)
            {
                return NotFound();
            }
            db.Course.Remove(course);
            db.SaveChanges();
            return Ok(new { message = $"Course with id:{id} Deleted Successfully.", Course = course });
        }
    }
}
