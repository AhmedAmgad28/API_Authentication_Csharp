using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace API_Lab2.DTOs
{
    public class StudentWithDeptName
    {
        public string Name { get; set; }
        public int Age { get; set;}
        public string Address { get; set;}
        public string DeptName { get; set;}
        public List<String> CoursesNames { get; set; } = new List<String>();
    }
}
