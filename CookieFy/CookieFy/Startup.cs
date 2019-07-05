using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CookieFy.Data;
using CookieFy.Includes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CookieFy {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services) {

            // passa o dbContext a todas as páginas
            services.AddDbContext<CookieDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddCors();

            services.AddMemoryCache();

            services.AddDistributedMemoryCache();

            services.AddSession(options => {

                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            // adiciona a classe de serviç para utilizarmos nas páginas
            services.AddScoped<ILoginService, LoginService>();

            // configura autenticação por cookie
            services.AddAuthentication(options => {

                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(o => { o.LoginPath = new PathString("/Login"); o.Cookie.Name = "CookieFyLogin"; });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

            if (env.IsDevelopment()) {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());

            //adiciona autenticação ao projeto
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc();

            app.UseSession();
        }
    }


}
