using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Lab2.Model
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Department Name must be in Characters only")]
        public string DeptName { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Address must be in Characters only")]
        public string DeptAddress { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
