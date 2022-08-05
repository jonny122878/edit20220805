using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edit20210325.Function;
using edit20210325.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace edit20210325
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(option => 
            {
                option.AddDefaultPolicy(builder => 
                {
                    builder.WithOrigins("https://accounts.google.com/");
                });
            });

            services.AddDbContext<CASE20210405Context>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("caseDatabase")));

            services.AddControllersWithViews().AddSessionStateTempDataProvider(); 
            services.AddSession();
            services.AddControllersWithViews();

            //�q�պAŪ���n�J�O�ɳ]�w
            double LoginExpireMinute = this.Configuration.GetValue<double>("LoginExpireMinute");
            //���U CookieAuthentication�AScheme����
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //�γ\�n�q�պA��Ū���A�ۤv�r�u�M�w
                option.LoginPath = new PathString("/Hmoe/Index");//�n�J��
                option.LogoutPath = new PathString("/Home/Logout");//�n�XAction
                //�Τ᭶�����d�Ӥ[�A�n�J�O���A��Controller���Τ�n�J�ɾ��I�]�i�H�]�w��
                option.ExpireTimeSpan = TimeSpan.FromMinutes(LoginExpireMinute);//�S���w�]14��
            });

            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
         
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //�ĤT��
            
            app.UseRouting();
            app.UseCors();


            app.UseAuthorization();
            app.UseSession();

            //�d�N����������...
            app.UseAuthentication();
            app.UseAuthorization();//Controller�BAction�~��[�W [Authorize] �ݩ�

            app.UseEndpoints(endpoints =>
            {             
                endpoints.MapAreaControllerRoute(
                name: ProjectSet.AdminName,
                areaName: ProjectSet.AdminName,
                pattern: ProjectSet.AdminName+"/{controller=Login}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=JudgeIndex}/{id?}");

            });
        }
    }
}
