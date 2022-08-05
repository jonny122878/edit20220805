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

            //從組態讀取登入逾時設定
            double LoginExpireMinute = this.Configuration.GetValue<double>("LoginExpireMinute");
            //註冊 CookieAuthentication，Scheme必填
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //或許要從組態檔讀取，自己斟酌決定
                option.LoginPath = new PathString("/Hmoe/Index");//登入頁
                option.LogoutPath = new PathString("/Home/Logout");//登出Action
                //用戶頁面停留太久，登入逾期，或Controller中用戶登入時機點也可以設定↓
                option.ExpireTimeSpan = TimeSpan.FromMinutes(LoginExpireMinute);//沒給預設14天
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

            //第三方
            
            app.UseRouting();
            app.UseCors();


            app.UseAuthorization();
            app.UseSession();

            //留意先執行驗證...
            app.UseAuthentication();
            app.UseAuthorization();//Controller、Action才能加上 [Authorize] 屬性

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
