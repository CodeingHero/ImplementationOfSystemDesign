using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SystemDesign.EF;
using SystemDesign.EF.Models;
using SystemDesign.MyMiddleWare;
using SystemDesign.Services;
using SystemDesign.ShortenUrl.Services;
using SystemDesign.UserManager.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:7111");

// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(options => {
  options.AddPolicy("MyPolicy",
      builder => {
        builder.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
      });
});

//DB Config
var connStr = builder.Configuration.GetConnectionString("SqliteDatabase");
services.AddDbContext<MyDBContext>(opt => opt.UseSqlite(connStr));
services.AddDataProtection();
//Configure Index
services.AddSingleton<IndexService>();

//Configure User
UserConfig.ConfigureUserOptions(builder.Services);

var idBuilder = new IdentityBuilder(typeof(SDUser), typeof(UserRole), builder.Services);
idBuilder.AddEntityFrameworkStores<MyDBContext>()
  .AddDefaultTokenProviders()
  .AddRoleManager<RoleManager<UserRole>>()
  .AddUserManager<UserManager<SDUser>>()
  .AddSignInManager<SignInManager<SDUser>>();

services.AddSingleton<UserService>();

//Jwt
services.AddSingleton<JwtBuilder>();
services.Configure<JwtOption>(builder.Configuration.GetSection("Jwt"));
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opt => {
    opt.TokenValidationParameters = new() {
      ValidateIssuer = false,
      ValidateAudience = false,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      //ValidIssuer = builder.Configuration["Jwt:Issuer"],
      //ValidAudience = builder.Configuration["Jwt:Issuer"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]))
    };
  });

//Inject ShortenUrl Services
services.AddSingleton<ShortenUrlService>();
var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<MyDBContext>();
db.Database.EnsureCreated();

app.UseCors("MyPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseWhen(context => {
  var res = !(context.Request.Path.StartsWithSegments("/user")
        || context.Request.Path.StartsWithSegments("/sd")
        || context.Request.Path.StartsWithSegments("/s")
        || context.Request.Path.StartsWithSegments("/assets"));
  return res;
}, branch => {
  branch.Use((context,next) => {
    context.Request.Path = new PathString("/");
    Console.WriteLine("Path changed to:" + context.Request.Path.Value);
    return next();
  });
});
app.UseDefaultFiles();
app.UseStaticFiles();
//Use JWT
app.UseAuthentication();
//Use Identity
app.UseAuthorization();


app.MapControllers();

app.Run();
