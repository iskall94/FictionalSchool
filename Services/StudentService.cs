using FictionalSchool.Data;
using Spectre.Console;
using FictionalSchool.Menus;
using Microsoft.EntityFrameworkCore;
using FictionalSchool.Models;

namespace FictionalSchool.Services
{
    internal static class StudentService
    {
        // Show all students with ascending or descending order based on bool parameter
        public static void ShowStudents(bool ascendOrDescend)
        {
            AnsiConsole.Clear();
            
            using var context = new FictionalSchoolDBContext();

            List<Student> student;
            
            if (!ascendOrDescend)
            {
                student = context.Students.Include(s => s.Class).OrderBy(s => s.FirstName).ToList();
            }
            else
            {
                student = context.Students.Include(s => s.Class).OrderByDescending(s => s.FirstName).ToList();
            }
            
            var table = new Table();

            table.AddColumn("Student ID");
            table.AddColumn("First Name");
            table.AddColumn("Last Name");
            table.AddColumn("Personal Number");
            table.AddColumn("Class");

            foreach (var students in student)
            {
                table.AddRow(students.StudentId.ToString(), students.FirstName, students.LastName, students.PersonalNumber, students.Class.ClassName);
            }

            AnsiConsole.Write(table);

            AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
            
            Console.ReadKey(true);
            StudentMenu.ShowStudentMenu();
        }
        // Show students in a specific class
        public static void ShowStudentsSpecificClass()
        {
            AnsiConsole.Clear();
            
            using var context = new FictionalSchoolDBContext();

            // Ensure className is a List<string> (not List<string?>)
            var classNames = context.Classes
                .Select(c => c.ClassName)
                .Cast<string>()
                .ToList();

            var classMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj vilken klass...")
                    .AddChoices(classNames));

            AnsiConsole.Clear();

            var studentClassName = context.Students.Include(s => s.Class).Where(s => s.Class.ClassName == classMenu).ToList();
            
            var table = new Table();
            table.AddColumn("Student ID");
            table.AddColumn("First Name");
            table.AddColumn("Last Name");
            table.AddColumn("Personal Number");
            table.AddColumn("Class");

            foreach (var s in studentClassName)
            {
                table.AddRow(s.StudentId.ToString(), s.FirstName, s.LastName, s.PersonalNumber, s.Class.ClassName);
            }

            AnsiConsole.Write(table);

            AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
            
            Console.ReadKey(true);
            StudentMenu.ShowStudentMenu();
        }

        // Adds a new student to the database (primary key bugged and starts with 100x of 1-x)
        public static void AddNewStudents()
        {
            AnsiConsole.Clear();

            var newStudentConfirmed = AnsiConsole.Confirm("Vill du lägga till en ny student?");

            if (newStudentConfirmed)
            {
                using var context = new FictionalSchoolDBContext();

                var classNames = context.Classes
                    .Select(c => c.ClassName)
                    .Cast<string>()
                    .ToList();

                var selectedClassName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj studentens klass:")
                    .AddChoices(classNames));

                var selectedClass = context.Classes.FirstOrDefault(c => c.ClassName == selectedClassName);

                var firstName = AnsiConsole.Ask<string>("Ange studentens förnamn:");
                var lastName = AnsiConsole.Ask<string>("Ange studentens efternamn:");
                var personalNumber = AnsiConsole.Ask<string>("Ange studentens personnummer:");

                var newStudent = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PersonalNumber = personalNumber,
                    ClassId = selectedClass?.ClassId
                };

                context.Students.Add(newStudent);
                context.SaveChanges();

                AnsiConsole.MarkupLine($"[green]Student {firstName} {lastName} har lagts till i klass {selectedClassName}![/]");
                AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
                Console.ReadKey(true);
            }
            StudentMenu.ShowStudentMenu();
        }
    }
}