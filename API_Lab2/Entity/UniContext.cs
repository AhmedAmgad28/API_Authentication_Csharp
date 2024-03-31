using API_Lab2.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API_Lab2.Entity
{
    public class UniContext : IdentityDbContext<ApplicationUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("Server=AHMED-IDEAPADL3\\SQLEXPRESS;Database=DB_Api_Course;Trusted_Connection=True;Encrypt=False");
        //}
        public UniContext(DbContextOptions option) :base( option)
        {
            
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
