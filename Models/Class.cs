namespace FictionalSchool.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string? ClassName { get; set; }

    public int? TeacherId { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Staff? Teacher { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
