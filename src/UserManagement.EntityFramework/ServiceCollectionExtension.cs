using UserManagement.EntityFramework.Helpers;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUserManagementEntityFramework(this IServiceCollection services)
    {
        return services.AddDbContext<DataContext>();
    }
}
