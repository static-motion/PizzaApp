namespace PizzaApp.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    using static PizzaApp.Web.Infrastructure.Common.ErrorMessages;
    public static class ServiceCollectionExtensions
    {
        private const string ServiceSuffix = "Service";

        public static IServiceCollection AddCustomServices(this IServiceCollection services, Assembly assembly)
        {
            Type[] serviceClasses = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(ServiceSuffix))
                .ToArray();

            foreach (Type serviceClass in serviceClasses)
            {
                Type? interfaceType = serviceClass.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{serviceClass.Name}");

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, serviceClass);
                    Console.WriteLine($"Registered service: {interfaceType.Name} -> {serviceClass.Name}");
                }
                else
                {
                    throw new InvalidOperationException(string.Format(NoSuitableInterfaceFound, serviceClass.Name));
                }
            }

            return services;
            // Register services in the container
            // builder.Services.AddScoped<IMovieService, MovieService>();
            // builder.Services.AddScoped<IWatchlistService, WatchlistService>();
        }

        public static void AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            Type[] repositoryClasses = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .ToArray();

            foreach (Type repositoryClass in repositoryClasses)
            {
                Type? interfaceType = repositoryClass.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{repositoryClass.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repositoryClass);
                }
                else
                {
                    throw new InvalidOperationException(string.Format(NoSuitableInterfaceFound, repositoryClass.Name));
                }
            }
        }

    }
}
