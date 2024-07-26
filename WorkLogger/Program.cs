using WorkLogger.Application.Extensions;
using WorkLogger.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

const string origin = "http://localhost:4200";
app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader().WithOrigins(origin)
    .WithExposedHeaders("Content-Disposition"));


/*app.UseHttpsRedirection();*/
app.UseAuthentication();

app.UseAuthorization();



app.MapControllers();


app.Run();
