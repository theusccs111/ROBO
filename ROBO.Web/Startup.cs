using EspecificacaoAnalise.Servico.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ROBO.Dominio;
using ROBO.Dominio.Entidades;
using ROBO.Servico.Validations;
using ROBO.Web.Filters;
using ROBO.Web.Middleware;
using System.Text;

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
            ConfigureSwaggerService(services, "Robo");

            ConfigureCorsService(services, Configuration);

            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<RoboContext>(options => options.UseSqlServer(connectionString));

            services.AddAutoMapper(c => c.AddProfile<AutoMapperConfiguration>(), typeof(Startup));
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build()
                );
            });
            services.AddHttpContextAccessor();
            services.AddScoped<RequestValidationFilter>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            ConfigureJWTService(services, Configuration["JwtSecurityToken:Key"]);

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
                c.SwaggerEndpoint(swaggerEndpoint, "Robo");
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
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<RoboService>();
        }

        private static void InjectRepositories(IServiceCollection services)
        {
            //services.AddScoped<IUnityOfWork, UnityOfWork>();
        }

        public static void ConfigureCorsService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(o =>
                o.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200") // Permitir apenas o dom�nio do front-end
                            .AllowAnyMethod()   // Permitir qualquer m�todo (GET, POST, etc.)
                            .AllowAnyHeader()   // Permitir qualquer cabe�alho
                            .AllowCredentials(); // Se necess�rio, se voc� precisar de credenciais como cookies, etc.
                }));
        }

        public static void ConfigureJWTService(IServiceCollection services, string key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(x =>
             {
                 var keys = Encoding.ASCII.GetBytes(key);
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(keys)
                 };
                 x.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         var accessToken = context.Request.Query["access_token"];
                         var path = context.HttpContext.Request.Path;
                         if (string.IsNullOrEmpty(accessToken) == false &&
                         (path.StartsWithSegments("/Messaging") || path.StartsWithSegments("/Comments")))
                         {
                             context.Token = accessToken;
                         }

                         return Task.CompletedTask;
                     }
                 };
             });
        }

        public void ConfigureSwaggerService(IServiceCollection services, string apiName)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1", Description = "Robo" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }


    }
}
