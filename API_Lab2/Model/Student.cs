using API_Lab2.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Lab2.Model
{
    public class Student
    {
        public int Id { get; set; }

        //[NameUniqnessAttribute]
        public string Name { get; set; }

        [Range(18, 22, ErrorMessage = "Age must be between 18 and 22 years old")]
        public int Age { get; set; }

        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Address must be in Characters only")]
        public string Address { get; set; }

        [RegularExpression("^.+\\.(png|jpg)$", ErrorMessage = "Image must have .jpg or .png extention")]
        public string Image { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
    }
}
