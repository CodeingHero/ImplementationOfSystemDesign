using SystemDesign.ShortenUrl.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<GenerateKeyService>(x => {
//  return new GenerateKeyService(new ConfigService());
//});
builder.Services.AddScoped<ShortenUrlService>();
builder.Services.AddCors(options => {
  string[] trustUrls = new[] { "http://127.0.0.1:5174" };
  options.AddDefaultPolicy(b => {
    b.WithOrigins(trustUrls)
    .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
