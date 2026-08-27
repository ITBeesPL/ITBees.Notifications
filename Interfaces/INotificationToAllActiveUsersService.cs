using ITBees.Notifications.Controllers.Models;

namespace ITBees.Notifications.Interfaces;

public interface INotificationToAllActiveUsersService
{
    NotificationToAllActiveUsersResultVm Send(NotificationToAllActiveUsersIm notification);
    NotificationToAllActiveUsersResultVm SendToSelected(NotificationToSelectedUsersIm notification);
}