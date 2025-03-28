using EspecificacaoAnalise.Persistencia.Data;
using EspecificacaoAnalise.Servico.Service;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ROBO.Dominio;
using ROBO.Persistencia;
using ROBO.Servico.Interfaces;
using ROBO.Servico.Validations;
using ROBO.Web.Filters;
using ROBO.Web.Middleware;

namespace ROBO.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCorsService(services, Configuration);

            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RoboContext>(options => options.UseSqlServer(connectionString));

            services.AddAutoMapper(c => c.AddProfile<AutoMapperConfiguration>(), typeof(Startup));

            services.AddHttpContextAccessor();
            services.AddScoped<RequestValidationFilter>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<GlobalExceptionHandlerMiddleware>();

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new CustomExceptionFilterAttribute());
                    options.EnableEndpointRouting = false;
                })
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<RoboValidator>());

            InjectServices(services);
            InjectRepositories(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseRouting();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var swaggerEndpoint = env.IsDevelopment() ? "/swagger/v1/swagger.json" : "/ea/swagger/v1/swagger.json";
                c.SwaggerEndpoint(swaggerEndpoint, "EspecificacaoAnalise");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<RoboService>();
        }

        private static void InjectRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnityOfWork, UnityOfWork>();
        }

        public static void ConfigureCorsService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(o =>
                o.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200") // Permitir apenas o domínio do front-end
                            .AllowAnyMethod()   // Permitir qualquer método (GET, POST, etc.)
                            .AllowAnyHeader()   // Permitir qualquer cabeçalho
                            .AllowCredentials(); // Se necessário, se você precisar de credenciais como cookies, etc.
                }));
        }

        
    }
}
