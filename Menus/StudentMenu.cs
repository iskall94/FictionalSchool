using FictionalSchool.Services;
using Spectre.Console;

namespace FictionalSchool.Menus
{
    internal static class StudentMenu
    {
        public static void ShowStudentMenu()
        {
            AnsiConsole.Clear();
            {
                var menu = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Hantering av studenter:")
                        .AddChoices(
                        [
                        "Hämta alla studenter",
                        "Hämta alla studenter från en viss klass",
                        "Lägg till nya studenter",
                        "Tillbaka till huvudmenyn"
                        ]));

                switch (menu)
                {
                    case "Hämta alla studenter":
                        StudentOrderMenu();
                        break;
                    case "Hämta alla studenter från en viss klass":
                        StudentService.ShowStudentsSpecificClass();
                        break;
                    case "Lägg till nya studenter":
                        StudentService.AddNewStudents();
                        break;
                    case "Tillbaka till huvudmenyn":
                        return;
                }
            }
        }

        public static void StudentOrderMenu()
        {
            AnsiConsole.Clear();
            
            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Vänligen välj sorteringen av studenter:")
                    .AddChoices(
                    [
                    "Sortera A-Ö",
                    "Sortera Ö-A",
                    "Exit back to Main Menu"
                    ]));

            switch (menu)
            {
                case "Sortera A-Ö":
                    StudentService.ShowStudents(false);
                    break;
                case "Sortera Ö-A":
                    StudentService.ShowStudents(true);
                    break;
                case "Tillbaka till föregående meny":
                    return;
            }
        }
    }
}
