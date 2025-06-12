using Microsoft.EntityFrameworkCore;
using MovieLibraryApi.Interface;
using MovieLibraryApi.Mapping;
using MovieLibraryApi.Middlewares;
using MovieLibraryApi.Persistence.Data;
using MovieLibraryApi.Service;

const string corsPolicyName = "AllowLocalFrontend";
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("Connection string not found.", nameof(connectionString));


builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});

// Register Servie & Interface
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ITvSeriesService, TvSeriesService>();
builder.Services.AddScoped<ICommonService, CommonService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpClient();

// Add CORS
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
	options.AddPolicy(corsPolicyName,
		policy =>
		{
			policy.WithOrigins(allowedOrigins!)
				  .AllowAnyHeader()
				  .AllowAnyMethod();
		});
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable console logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(corsPolicyName);

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/404.html");

app.Run();
