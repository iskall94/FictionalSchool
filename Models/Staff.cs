using System;
using System.Collections.Generic;

namespace FictionalSchool.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string? Role { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PersonalNumber { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<CoursesStudent> CoursesStudents { get; set; } = new List<CoursesStudent>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
