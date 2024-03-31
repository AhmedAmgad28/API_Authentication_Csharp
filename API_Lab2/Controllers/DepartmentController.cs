using API_Lab2.DTOs;
using API_Lab2.Entity;
using API_Lab2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : ControllerBase
    {
        public DepartmentController(UniContext _db)
        {
            db = _db;
        }
        UniContext db;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var dept = db.Departments.Include(d => d.Students).ToList();
            if (dept is null)
            {
                return NotFound();
            }
            var departmentsWithStudents = new List<DepartmentWithStds>();
            foreach (var deptn in dept)
            {
                var deptWstd = new DepartmentWithStds();
                deptWstd.DeptID = deptn.DeptId;
                deptWstd.DeptName = deptn.DeptName;
                deptWstd.DeptAddress = deptn.DeptAddress;

                foreach (var student in deptn.Students)
                {
                    if (student != null)
                    {
                        deptWstd.StudentsNames.Add(student.Name);
                    }
                }
                departmentsWithStudents.Add(deptWstd);
            }
            return Ok(departmentsWithStudents);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetByID(int id)
        {
            var dept = db.Departments.Include(s=>s.Students).FirstOrDefault(d => d.DeptId == id);
            if (dept is null)
            {
                return NotFound();
            }
            DepartmentWithStds deptWstd = new DepartmentWithStds();
            deptWstd.DeptID = dept.DeptId;
            deptWstd.DeptName = dept.DeptName;
            deptWstd.DeptAddress = dept.DeptAddress;
            foreach(var s in dept.Students)
            {
                deptWstd.StudentsNames.Add(s.Name);
            }
            return Ok(new { message = $"Department with id:{id} exists.", Department = deptWstd });
        }

        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            var dept = db.Departments.FirstOrDefault(x => x.DeptName == name);
            if (dept is null)
            {
                return NotFound();
            }
            DepartmentWithStds deptWstd = new DepartmentWithStds();
            deptWstd.DeptID = dept.DeptId;
            deptWstd.DeptName = dept.DeptName;
            deptWstd.DeptAddress = dept.DeptAddress;
            foreach (var s in dept.Students)
            {
                deptWstd.StudentsNames.Add(s.Name);
            }
            return Ok(new { message = $"Department with name:{name} exists.", Department = deptWstd });
        }

            [HttpPost]
            public IActionResult AddDepartment(Department departmentInput)
            {
                if (departmentInput is null)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    Department department = new Department
                    {
                        DeptName = departmentInput.DeptName,
                        DeptAddress = departmentInput.DeptAddress
                    };
                    db.Departments.Add(department);
                    db.SaveChanges();
                return Ok(new { message = "Added Successfully" });
            }
                return BadRequest(ModelState);
            }

        [HttpDelete("{id}")]
        public IActionResult RemoveDepartment(int id)
        {
            var dept = db.Departments.Find(id);
            if (dept is null)
            {
                return NotFound();
            }
            db.Departments.Remove(dept);
            db.SaveChanges();
            return Ok(new { message = $"Department with id:{id} Deleted Successfully.", Department = dept });
        }

    }
}
