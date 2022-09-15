using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SistemaPracticasAcademicas.Services;
using Rotativa.AspNetCore;
using System;

namespace SistemaPracticasAcademicas
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(typeof(PeriodoService));
#if RELEASE
            //Usa la base de datos que esta en la nube
            services.AddDbContext<Models.sistema_academicoContext>(optionsBuilder => {
                optionsBuilder.UseMySql("server=sistemapracticasitesrc.mysql.database.azure.com;user=itesrcPractica@sistemapracticasitesrc;password=GestionAcademica1;database=sistema_academico", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.32-mysql"));

            });
#else
            //Usa la base de datos que se encuentra local
            services.AddDbContext<Models.sistema_academicoContext>(optionsBuilder =>
            {
                optionsBuilder.UseMySql("server=MYSQL5046.site4now.net;user=a88803_academi;password=tedeadmau5;database=db_a88803_academi;", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.32-mysql"));
            });
#endif
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(25);
                options.SlidingExpiration = true;
                options.LoginPath = "/Home/Login";
                options.LogoutPath = "/Home/Index";
                options.AccessDeniedPath = "/Acceso-Denegado";
                options.Cookie.Name = "practicaSesion";
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseFileServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{_id?}");
                endpoints.MapDefaultControllerRoute();
            });
            RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa");
        }
    }
}
