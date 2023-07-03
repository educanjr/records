using BallastLane.App.Configuration;
using BallastLane.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstallServices(builder.Configuration, typeof(IServiceInstaller).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ensure database and tables exist, this should be done only for Development
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
    await context.Init();
}

// Disable the following line to be able to run the application without certificate.
// app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
