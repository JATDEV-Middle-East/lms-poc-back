using Application.Models;
using Application.Modules.Auth;
using Application.Modules.Courses;
using Application.Modules.Users;
using Application.Seeder;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lms App", Version = "v1" });

	// Add JWT token authentication to Swagger
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Description = "Bearer Authentication with JWT Token",
		Type = SecuritySchemeType.Http
	});

	c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter your username and password",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Basic"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
			  {
				  {
					  new OpenApiSecurityScheme
					  {
						  Reference = new OpenApiReference
						  {
							  Id = "Bearer",
							  Type = ReferenceType.SecurityScheme
						  }
					  },
					  new List<string>()
				  },

				  {
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Basic"
							}
						},
						new string[] { }
				  }
	});
});

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<DatabaseContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
				sqlServerOptions => sqlServerOptions.MigrationsAssembly("Application")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped(typeof(MyJWT));
var myAllowSpecificORigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "_myAllowSpecificOrigins",
					  builder =>
					  {
						  builder.WithOrigins("http://localhost:4200",
											  "https://localhost:4200",
											  "https://weave-splus.cloudjat.com",
											  "https://localhost:443",
											  "http://localhost",
											  "https://localhost")
								 .AllowAnyHeader() 
								 .AllowAnyMethod();
					  });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
//builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = configuration["Jwt:Issuer"],
		ValidAudience = configuration["Jwt:ValidAudience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
	};
});

var app = builder.Build();
app.UseCors(myAllowSpecificORigins);
await AppSeeder.SeedDatabaseAsync(app.Services);

	app.UseSwagger();
	app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
