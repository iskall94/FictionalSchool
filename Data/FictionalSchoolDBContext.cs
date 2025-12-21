using System;
using System.Collections.Generic;
using FictionalSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace FictionalSchool.Data;

public partial class FictionalSchoolDBContext : DbContext
{
    public FictionalSchoolDBContext()
    {
    }

    public FictionalSchoolDBContext(DbContextOptions<FictionalSchoolDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CoursesStudent> CoursesStudents { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-2AI4QFB;Database=FictionalSchoolDB;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927C05B7E4E39");

            entity.Property(e => e.ClassName).HasMaxLength(5);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Class_Teacher");

            entity.HasMany(d => d.Courses).WithMany(p => p.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "CoursesClass",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Course_CoursesClass"),
                    l => l.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Class_CoursesClass"),
                    j =>
                    {
                        j.HasKey("ClassId", "CourseId").HasName("PK__CoursesC__A78BF0DA96459860");
                        j.ToTable("CoursesClass");
                    });
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A709E8C0BC");

            entity.Property(e => e.CourseCode).HasMaxLength(6);
            entity.Property(e => e.CourseName).HasMaxLength(50);
        });

        modelBuilder.Entity<CoursesStudent>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId }).HasName("PK__CoursesS__5E57FC83ABE48ACD");

            entity.Property(e => e.Grade)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Course).WithMany(p => p.CoursesStudents)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Courses_CoursesStudents");

            entity.HasOne(d => d.Student).WithMany(p => p.CoursesStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_CoursesStudents");

            entity.HasOne(d => d.Teacher).WithMany(p => p.CoursesStudents)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Grades_Teacher");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB1773304635");

            entity.Property(e => e.FirstName).HasMaxLength(25);
            entity.Property(e => e.LastName).HasMaxLength(25);
            entity.Property(e => e.PersonalNumber).HasMaxLength(13);
            entity.Property(e => e.Role).HasMaxLength(50);

            entity.HasMany(d => d.Courses).WithMany(p => p.Staff)
                .UsingEntity<Dictionary<string, object>>(
                    "CoursesStaff",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Courses_CoursesStaff"),
                    l => l.HasOne<Staff>().WithMany()
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Staff_CoursesStaff"),
                    j =>
                    {
                        j.HasKey("StaffId", "CourseId").HasName("PK__CoursesS__FA467C0DAC190F4E");
                        j.ToTable("CoursesStaff");
                    });
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B99D1D66ABF");

            entity.Property(e => e.FirstName).HasMaxLength(25);
            entity.Property(e => e.LastName).HasMaxLength(25);
            entity.Property(e => e.PersonalNumber).HasMaxLength(13);

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Classes_Students");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
