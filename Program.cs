using Microsoft.EntityFrameworkCore;
using Todos.WebApi.Context;
using Todos.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapGet("/getall", (ApplicationDbContext context) => Results.Ok(context.Todos.ToList()));

app.MapPost("/create", (ApplicationDbContext context, Todo todo) =>
{
    context.Add(todo);
    context.SaveChanges();
    Results.Ok(todo);
});

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var context = sp.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
