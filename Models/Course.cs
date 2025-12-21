using System;
using System.Collections.Generic;

namespace FictionalSchool.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? CourseCode { get; set; }

    public int? Points { get; set; }

    public virtual ICollection<CoursesStudent> CoursesStudents { get; set; } = new List<CoursesStudent>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
