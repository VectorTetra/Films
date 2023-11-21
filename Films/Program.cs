using Microsoft.EntityFrameworkCore;
using Films.Models;

namespace Films
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �������� ������ ����������� �� ����� ������������
            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

            // ��������� �������� ApplicationContext � �������� ������� � ����������
            builder.Services.AddDbContext<FilmContext>(options => options.UseSqlServer(connection));

            // ��������� ������� MVC
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseStaticFiles(); // ������������ ������� � ������ � ����� wwwroot

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}