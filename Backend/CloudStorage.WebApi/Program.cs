using CloudStorage.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var configuration = builder.Configuration;

builder.Services.AddPersistence(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();