using System.Text;
using application.Controllers;
using domain.Entities;
using infra.Data.Context;
using infra.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using service.Services;


namespace application;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddDbContext<NpSqlContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );
        services.AddCors();
        services.AddControllers();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme  = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE=")),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

        });
        services.AddAuthorization();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tcc API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        services.AddScoped<DbContext, NpSqlContext>();

        services.AddScoped<BaseRepository<User>>();
        services.AddScoped<BaseRepository<Authentication>>();
        services.AddScoped<BaseRepository<Project>>();
        services.AddScoped<BaseRepository<Request>>();
        services.AddScoped<BaseRepository<Teacher>>();
        services.AddScoped<BaseRepository<Student>>();
        services.AddScoped<BaseRepository<Student>>();
        services.AddScoped<BaseRepository<ProjectTask>>();
        services.AddScoped<BaseRepository<TaskAttachment>>();
        services.AddScoped<BaseRepository<TaskComment>>();

        services.AddScoped<AuthenticationRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<ProjectRepository>();
        services.AddScoped<TeacherRepository>();
        services.AddScoped<RequestRepository>();
        services.AddScoped<StudentRepository>();
        services.AddScoped<StudentRepository>();
        services.AddScoped<ProjectTaskRepository>();
        services.AddScoped<TaskAttachmentRepository>();

        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();
        services.AddScoped<ProjectService>();
        services.AddScoped<RequestService>();
        services.AddScoped<TeacherService>();
        services.AddScoped<ProjectTaskService>();

        services.AddScoped<AuthController>();
        services.AddScoped<UserController>();
        services.AddScoped<ProjectController>();
        services.AddScoped<RequestController>();
        services.AddScoped<ProjectTaskController>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
