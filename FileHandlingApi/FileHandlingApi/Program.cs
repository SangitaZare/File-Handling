var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("ng", policy => policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// 2. Use CORS (before UseAuthorization)
app.UseCors("ng");

app.UseAuthorization();
app.MapControllers();

app.Run();