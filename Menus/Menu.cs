using Spectre.Console;

namespace FictionalSchool.Menus
{
    internal static class Menu
    {
        public static void ShowMenu()
        {
            AnsiConsole.Clear();
            while (true)
            {
                var menu = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Välkommen till Fiktionella Gymnasiet!")
                        .AddChoices(
                        [
                        "Gå till studentmenyn",
                        "Gå till personalmenyn",
                        "Gå till generell hantering",
                        "Avsluta"
                        ]));

                switch (menu)
                {
                    case "Gå till studentmenyn":
                        StudentMenu.ShowStudentMenu();
                        break;
                    case "Gå till personalmenyn":
                        StaffMenu.ShowStaffMenu();
                        break;
                    case "Gå till generell hantering":
                        GeneralMenu.ShowGeneralMenu();
                        break;
                    case "Avsluta":
                        Console.CursorVisible = false;
                        AnsiConsole.MarkupLine("[green]Tack för att du använde Fiktionella Gymnasiets hanteringssystem![/]");
                        AnsiConsole.MarkupLine("[green]Programmet avslutas om 3 sekunder...[/]");
                        Thread.Sleep(3000);
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
