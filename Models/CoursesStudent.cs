namespace FictionalSchool.Models;

public partial class CoursesStudent
{
    public string? Grade { get; set; }

    public DateOnly? GradeDate { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public int? TeacherId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Staff? Teacher { get; set; }
}
