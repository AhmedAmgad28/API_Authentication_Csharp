using API_Lab2.Entity;
using API_Lab2.Model;
using System.ComponentModel.DataAnnotations;

namespace API_Lab2.Validators
{
    public class NameUniqnessAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Student student = (Student)validationContext.ObjectInstance;

            //using (var dbContext = new UniContext())
            //{
            //    bool isNameUnique = !dbContext.Students.Any(s => s.Name == student.Name && s.Id != student.Id);

            //    if (!isNameUnique)
            //    {
            //        return new ValidationResult("This name has been used before");
            //    }
            //}

            return ValidationResult.Success;
        }
    }
}
