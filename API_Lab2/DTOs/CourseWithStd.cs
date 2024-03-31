namespace API_Lab2.DTOs
{
    public class CourseWithStd
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<String> StudentsNames { get; set; } = new List<String>();
    }
}
