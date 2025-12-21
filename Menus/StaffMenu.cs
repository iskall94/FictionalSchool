using FictionalSchool.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace FictionalSchool.Menus
{
    internal class StaffMenu
    {
        public static void ShowStaffMenu()
        {
            AnsiConsole.Clear();

            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hantering av personal:")
                    .AddChoices(
                    [
                    "Hämta alla anställda",
                    "Hämta anställda baserat på kategori",
                    "Lägg till mer personal",
                    "Tillbaka till huvudmenyn"
                    ]));

            switch (menu)
            {
                case "Hämta alla anställda":
                    StaffService.ShowAllStaff();
                    break;
                case "Hämta anställda baserat på kategori":
                    StaffCategoryMenu();
                    break;
                case "Lägg till mer personal":
                    StaffService.AddNewStaff();
                    break;
                case "Tillbaka till huvudmenyn":
                    return;
            }
        }

        public static void StaffCategoryMenu()
        {
            AnsiConsole.Clear();

            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Vänligen välj kategorin av anställda")
                    .AddChoices(
                    [
                    "Rektor",
                    "Administratör",
                    "Lärare",
                    "Tillbaka till föregående meny"
                    ]));

            switch (menu)
            {
                case "Rektor":
                    StaffService.StaffCategory("Rektor");
                    break;
                case "Administratör":
                    StaffService.StaffCategory("Administratör");
                    break;
                case "Lärare":
                    StaffService.StaffCategory("Lärare");
                    break;
                case "Tillbaka till föregående meny":
                    ShowStaffMenu();
                    break;
            }
        }
    }
}
