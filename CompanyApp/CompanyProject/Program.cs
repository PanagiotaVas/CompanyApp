
using AutoMapper;
using CompanyProject.Configuration;
using CompanyProject.Data;
using CompanyProject.Repositories;
using CompanyProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace CompanyProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<CompanyProjectDbContext>(options => options.UseSqlServer(connString), ServiceLifetime.Scoped);

            // Add services to the container.

            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddRepositories();

            builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperConfig());
            }).CreateMapper());


            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
                {
                    x.IncludeErrorDetails = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = false,
                        SignatureValidator = (token, validator) => { return new JwtSecurityToken(token); }
                    };
                }
            );



            /*
             *  Add for production!!! 
             */

            /*            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "http://localhost:5187",

                    ValidateAudience = true,
                    ValidAudience = "http://localhost:5187",

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                            .GetBytes(builder.Configuration.GetConnectionString("SecretKey")!))

                };
            });*/

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/User/Login";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyMethod().AllowAnyHeader()
                    .AllowAnyOrigin()
                );
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularClient",
                    b => b.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod().AllowAnyHeader());
            });

             builder.Services.AddControllers().AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                 options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                 // Adding a converter for string enums
                 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
             });

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());

            });

            builder.Services.AddEndpointsApiExplorer();
            // Config for swagger

            builder.Services.AddSwaggerGen(options =>
            {
                options.SupportNonNullableReferenceTypes();
                // options.OperationFilter<AuthorizeOperationFilter>(); 
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme",
                        Name = "Authorization",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT"
                    });
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
