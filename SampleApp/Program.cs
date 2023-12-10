using Microsoft.EntityFrameworkCore;
using SampleApp.Models;

namespace SampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            #if DEBUG
            builder.Services.AddSassCompiler();
            #endif

            // Подключение базы данных SQL Server
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<SampleAppContext>(options => options.UseSqlServer(connection));
            //builder.Services.AddDbContext<SampleAppContext>(options => options.UseNpgsql(connection));

            builder.Services.AddFlashes();

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "SampleSession";
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}