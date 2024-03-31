using System.ComponentModel.DataAnnotations;
namespace API_Lab2.DTOs
{
    public class DepartmentWithStds
    {
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public string DeptAddress { get; set; }
        public List<String> StudentsNames { get; set; } = new List<String>();
    }
}
