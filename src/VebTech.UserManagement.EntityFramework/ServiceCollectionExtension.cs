using VebTech.UserManagement.EntityFramework.Helpers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUserManagementEntityFramework(this IServiceCollection services)
    {
        return services.AddDbContext<DataContext>();
    }
}
