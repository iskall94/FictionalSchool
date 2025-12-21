using FictionalSchool.Data;
using FictionalSchool.Menus;
using FictionalSchool.Models;
using Spectre.Console;

namespace FictionalSchool.Services
{
    internal static class StaffService
    {
        // Show staff based on category
        public static void StaffCategory(string category)
        {
            AnsiConsole.Clear();

            using var context = new FictionalSchoolDBContext();

            var showCategory = context.Staff.Where(s => s.Role.Equals(category)).ToList();

            var table = new Table();

            table.AddColumn("ID");
            table.AddColumn("First Name");
            table.AddColumn("Last Name");
            table.AddColumn("Personal Number");
            table.AddColumn("Role");
            foreach (var staff in showCategory)
            {
                table.AddRow(staff.StaffId.ToString(), staff.FirstName ?? "", staff.LastName ?? "", staff.PersonalNumber ?? "",staff.Role ?? "");
            }
            AnsiConsole.Write(table);

            AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");

            Console.ReadKey();

            StaffMenu.StaffCategoryMenu();
        }

        // Show all staff
        public static void ShowAllStaff()
        {
            AnsiConsole.Clear();
            
            using var context = new FictionalSchoolDBContext();
            
            var allStaff = context.Staff.ToList();
            
            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("First Name");
            table.AddColumn("Last Name");
            table.AddColumn("Personal Number");
            table.AddColumn("Role");
            
            foreach (var staff in allStaff)
            {
                table.AddRow(staff.StaffId.ToString(), staff.FirstName ?? "", staff.LastName ?? "", staff.PersonalNumber ?? "",staff.Role ?? "");
            }
            
            AnsiConsole.Write(table);
            
            AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
            
            Console.ReadKey();
            StaffMenu.ShowStaffMenu();
        }

        // Add new staff (primary key somehow starts at 1002 instead of 1-x, not sure why)
        public static void AddNewStaff()
        {
            AnsiConsole.Clear();

            var newStaffConfirmed = AnsiConsole.Confirm("Vill du lägga till en ny anställd?");

            if (newStaffConfirmed)
            {
                using var context = new FictionalSchoolDBContext();

                var firstName = AnsiConsole.Ask<string>("Ange personalens förnamn:");
                var lastName = AnsiConsole.Ask<string>("Ange personalens efternamn:");
                var personalNumber = AnsiConsole.Ask<string>("Ange personalens personnummer (xxxxxxxx-xxxx):");
                var role = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Vänligen välj kategorin av anställda")
                        .AddChoices(["Rektor", "Administratör","Lärare"]));

                var newStaff = new Staff
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PersonalNumber = personalNumber,
                    Role = role
                };

                context.Staff.Add(newStaff);
                context.SaveChanges();

                AnsiConsole.MarkupLine($"[green]Personal {firstName} {lastName} har lagts till![/]");
                AnsiConsole.Write("\nKlicka någon knapp för att gå tillbaka till föregående meny...");
                Console.ReadKey();
            }
            StaffMenu.ShowStaffMenu();
        }
    }
}
