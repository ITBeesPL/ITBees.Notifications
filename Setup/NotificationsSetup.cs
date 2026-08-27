using ITBees.FAS.Setup;
using ITBees.Notifications.DbModels;
using ITBees.Notifications.Interfaces;
using ITBees.Notifications.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITBees.Notifications.Setup;

public class NotificationsSetup : IFasDependencyRegistration
{
    public void Register(IServiceCollection services, IConfigurationRoot configurationRoot)
    {
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<INotificationToAllActiveUsersService, NotificationToAllActiveUsersService>();
    }
}
public class DbModelBuilder
{
    public static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>().HasKey(x => x.Guid);
    }
}