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
    public class StudentController : ControllerBase
    {
        public StudentController(UniContext _db)
        {
            db = _db;
        }
        UniContext db;
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = db.Students.Include(d => d.Department).Include(db => db.Courses).ToList();  //.Include(db => db.Courses)
            if (students is null)
            {
                return NotFound();
            }
            var StudentswithDepartment = new List<StudentWithDeptName>();
            foreach (var std in students)
            {
                var stdDept = new StudentWithDeptName();
                stdDept.Name = std.Name;
                stdDept.Age = std.Age;
                stdDept.Address = std.Address;
                stdDept.DeptName = std.Department.DeptName;
                foreach (var stdCourses in std.Courses)
                {
                    stdDept.CoursesNames.Add(stdCourses.Name);
                }
                StudentswithDepartment.Add(stdDept);
            }
            return Ok(StudentswithDepartment);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetByID(int id)
        {
            var student = db.Students.Include(db=>db.Department).Include(db => db.Courses).FirstOrDefault(s=>s.Id==id);  //.Include(db => db.Courses)
            if (student is null)
            {
                return NotFound();
            }
            StudentWithDeptName std = new StudentWithDeptName();
            std.Name = student.Name;
            std.Age = student.Age;
            std.Address = student.Address;
            std.DeptName = student.Department.DeptName;
            foreach (var stdCourses in student.Courses)
            {
                std.CoursesNames.Add(stdCourses.Name);
            }
            return Ok(new { message = $"Student with id:{id} exists.", Student = std });
        }

        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            var student = db.Students.Include(db => db.Department).Include(db => db.Courses).FirstOrDefault(x => x.Name == name);  //.Include(db => db.Courses)
            if (student is null)
            {
                return NotFound();
            }
            StudentWithDeptName std = new StudentWithDeptName();
            std.Name = student.Name;
            std.Age = student.Age;
            std.Address = student.Address;
            std.DeptName = student.Department.DeptName;
            foreach (var stdCourses in student.Courses)
            {
                std.CoursesNames.Add(stdCourses.Name);
            }
            return Ok(new { message = $"Student with name:{name} exists.", Student = std });
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            if (student is null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return CreatedAtAction("GetByID", new { id = student.Id }, "Added Successfully");
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateStudentByID(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Update(student);
                db.SaveChanges();
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveStudent(int id)
        {
            var student = db.Students.Find(id);
            if (student is null)
            {
                return NotFound();
            }
            db.Students.Remove(student);
            db.SaveChanges();
            return Ok(new { message = $"Student with id:{id} Deleted Successfully.", Student = student });
        }
    }
}
