using FictionalSchool.Menus;
// scaffold-DbContext "Data Source=DESKTOP-2AI4QFB;Database=FictionalSchoolDB;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir Data -Context FictionalSchoolDBContext
namespace FictionalSchool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu.ShowMenu();
        }
    }
}
