using Microsoft.Extensions.FileProviders;
using WorkLogger.Application.Extensions;
using WorkLogger.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("https://localhost:7178");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

const string backend = "https://localhost:7178";
const string frontend = "https://localhost:4200";
app.UseCors(options => options
    .AllowAnyMethod()
    .AllowCredentials()
    .AllowAnyHeader()
    .WithOrigins(backend, frontend)
    .WithExposedHeaders("Content-Disposition"));


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

/*app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot/browser")
    ),
    RequestPath = ""
});*/

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot/browser")
    ),
    RequestPath = ""
});

app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();



app.MapFallbackToController("Index", "FallBack");


app.Run();
