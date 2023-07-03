using BallastLane.Persistence;
using Scrutor;

namespace BallastLane.App.Configuration;

public class InfrasctructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(selector => selector
                .FromAssemblies(
                    BallastLane.Infrastructure.AssemblyReference.Assembly,
                    BallastLane.Persistence.AssemblyReference.Assembly)
                .AddClasses(false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime());

        services.AddScoped<IDBSettings, DBSettings>((services) => new DBSettings(configuration.GetConnectionString("Database")));

        services.AddAutoMapper(BallastLane.Persistence.AssemblyReference.Assembly);
    }
}
