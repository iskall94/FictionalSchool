using FictionalSchool.Services;
using Spectre.Console;

namespace FictionalSchool.Menus
{
    internal class GeneralMenu
    {
        public static void ShowGeneralMenu()
        {
            AnsiConsole.Clear();

            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Generell hantering av systemet:")
                    .AddChoices(
                    [
                    "Visa antal i alla avdelningar",
                    "Student Information",
                    "Visa aktiva kurser",
                    "Lägg till/ändra betyg",
                    "Tillbaka till huvudmenyn"
                    ]));

            switch (menu)
            {
                case "Visa antal i alla avdelningar":
                    GeneralService.AmountTeachersInDepartments();
                    break;
                case "Student Information":
                    GeneralService.StudentInformation();
                    break;
                case "Visa aktiva kurser":
                    GeneralService.ShowActiveCourses();
                    break;
                case "Lägg till/ändra betyg":
                    GeneralService.GradeTransaction();
                    break;
                case "Tillbaka till huvudmenyn":
                    return;
            }
        }
    }
}
