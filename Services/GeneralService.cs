using FictionalSchool.Data;
using FictionalSchool.Menus;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace FictionalSchool.Services
{
    internal class GeneralService
    {
        // Show number of staff (lärare, administratörer etc) in each department
        public static void AmountTeachersInDepartments()
        {
            AnsiConsole.Clear();

            using var context = new FictionalSchoolDBContext();

            var departmentStaffCounts = context.Staff
                .GroupBy(s => s.Role)
                .Select(c => new
                {
                    Role = c.Key,
                    Count = c.Count()
                })
                .ToList();

            var table = new Table();
            table.AddColumn("Avdelning/Roll");
            table.AddColumn("Antal Personal");

            foreach (var dsc in departmentStaffCounts)
            {
                table.AddRow(dsc.Role, dsc.Count.ToString());
            }

            AnsiConsole.Write(table);

            Console.ReadKey(true);
            AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
            GeneralMenu.ShowGeneralMenu();
        }

        // Show student information including full name, personal number, class, courses and grades
        public static void StudentInformation()
        {
            AnsiConsole.Clear();

            using var context = new FictionalSchoolDBContext();

            var studentInfo = context.Students
                .Select(s => new
                {
                    FullName = $"{s.FirstName} {s.LastName}",
                    s.PersonalNumber,
                    s.Class.ClassName,

                    gradesInfo = s.CoursesStudents.Select(cs => new
                        {
                            cs.Course.CourseName,
                            cs.Grade
                        }).ToList()
                });

            var table = new Table();
            table.AddColumn("Fullständigt Namn");
            table.AddColumn("Personnummer");
            table.AddColumn("Klass");
            table.AddColumn("Kurs");
            table.AddColumn("Betyg");

            foreach (var info in studentInfo)
            {
                var coursesText = string.Join("\n", info.gradesInfo.Select(g => g.CourseName));
                var gradesText = string.Join("\n", info.gradesInfo.Select(g => g.Grade ?? ""));

                table.AddRow(
                    info.FullName,
                    info.PersonalNumber,
                    info.ClassName,
                    coursesText,
                    gradesText
                );
            }
            AnsiConsole.Write(table);
            Console.ReadKey(true);
            AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
            GeneralMenu.ShowGeneralMenu();
        }
        
        // Show all active and inactive courses
        public static void ShowActiveCourses()
        {
            AnsiConsole.Clear();

            using var context = new FictionalSchoolDBContext();
            var activeCourses = context.Courses
                .Where(c => c.IsActive == true)
                .ToList();

            var table = new Table();

            table.AddColumn("Kurs ID");
            table.AddColumn("Kurs Namn");
            table.AddColumn("Kurs Kod");
            table.AddColumn("Poäng");

            foreach (var course in activeCourses)
            {
                table.AddRow(
                    course.CourseId.ToString(),
                    course.CourseName ?? "",
                    course.CourseCode ?? "",
                    course.Points?.ToString() ?? ""
                );
            }
            AnsiConsole.WriteLine("Aktiva Kurser:");
            AnsiConsole.Write(table);

            AnsiConsole.WriteLine($"Totalt antal aktiva kurser: {activeCourses.Count}");

            var inactiveCourses = context.Courses
                .Where(c => c.IsActive == false)
                .ToList();

            var inactiveTable = new Table();

            inactiveTable.AddColumn("Kurs ID");
            inactiveTable.AddColumn("Kurs Namn");
            inactiveTable.AddColumn("Kurs Kod");
            inactiveTable.AddColumn("Poäng");
            foreach (var course in inactiveCourses)
            {
                inactiveTable.AddRow(
                    course.CourseId.ToString(),
                    course.CourseName ?? "",
                    course.CourseCode ?? "",
                    course.Points?.ToString() ?? ""
                );
            }
            AnsiConsole.WriteLine("\nInaktiva kurser:");

            AnsiConsole.Write(inactiveTable);
            AnsiConsole.WriteLine($"Totalt antal inaktiva kurser: {inactiveCourses.Count}");

            Console.ReadKey(true);
            AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
            GeneralMenu.ShowGeneralMenu();
        }

        // Grade transaction method, pick teacher > pick student > pick course > set grade
        public static void GradeTransaction()
        {
            AnsiConsole.Clear();
            using var context = new FictionalSchoolDBContext();
            
            var staff = context.Staff
                .Where(s => s.Role == "Lärare")
                .Select(s => new
                {
                    s.StaffId,
                    FullName = $"{s.FirstName} {s.LastName} ({s.PersonalNumber})",
                })
                .ToList();

            var staffNames = staff.Select(s => s.FullName).ToList();

            var selectedTeacherName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj en lärare som sätter betyg:")
                    .PageSize(10)
                    .AddChoices(staffNames)
            );

            var selectedTeacher = staff.First(s => s.FullName == selectedTeacherName);

            var students = context.Students
                .Select(s => new
                {
                    s.StudentId,
                    s.PersonalNumber,
                    NameAndPersonalNumber = $"{s.FirstName} {s.LastName} ({s.PersonalNumber})"
                })
                .ToList();

            var studentNames = students.Select(s => s.NameAndPersonalNumber).ToList();

            var selectedStudentName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj en student att uppdatera betyg för:")
                    .PageSize(10)
                    .AddChoices(studentNames)
            );

            var selectedStudent = students.First(s => s.NameAndPersonalNumber == selectedStudentName);

            var coursesForStudent = context.CoursesStudents
                .Where(cs => cs.StudentId == selectedStudent.StudentId)
                .Include(cs => cs.Course)
                .ToList();

            if (!coursesForStudent.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Denna student har inga kurser att uppdatera betyg för.[/]");
                
                AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
                Console.ReadKey(true);
                GeneralMenu.ShowGeneralMenu();
                return;
            }

            var courseNames = coursesForStudent
                .Select(cs => cs.Course.CourseName ?? "")
                .ToList();

            var selectedCourseName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj en kurs att uppdatera betyg för:")
                    .PageSize(10)
                    .AddChoices(courseNames)
            );

            var selectedCourse = coursesForStudent.First(cs => cs.Course.CourseName == selectedCourseName);

            var newGrade = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj det nya betyget:")
                    .AddChoices(["A", "B", "C", "D", "E", "F"]));

            using var transaction = context.Database.BeginTransaction();

            try
            {
                selectedCourse.Grade = newGrade.ToUpper();
                selectedCourse.GradeDate = DateOnly.FromDateTime(DateTime.Now);
                selectedCourse.TeacherId = selectedTeacher.StaffId;
                context.SaveChanges();

                transaction.Commit();
                AnsiConsole.MarkupLine("[green]Transaktionen lyckades och betyg uppdaterades![/]");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                AnsiConsole.MarkupLine($"[red]Transaktionen misslyckades: {ex.Message}[/]");
            }
            
            AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
            Console.ReadKey(true);
            GeneralMenu.ShowGeneralMenu();
        }
    }
}
