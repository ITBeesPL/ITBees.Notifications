using ITBees.Interfaces.Repository;
using ITBees.Notifications.Controllers.Models;

namespace ITBees.Notifications.Interfaces;

public interface INotificationsService
{
    NotificationsCounterVm GetMyNotificationsCounters();
    PaginatedResult<MyNotificationVm> GetMyNotifications(bool onlyUnread, int? page, int? pageSize, string? sortColumn,
        SortOrder? sortOrder);
    MyNotificationVm MarkNotificationAsClicked(MyNotificationClickedUm notificationUm);
    MyNotificationVm SetReadStatus(MyNotificationReadStatusIm notificationIm);
    void Delete(MyNotificationDeleteDm notificationDeleteDm);
    NotificationsCounterVm MarkAllNotyficationsAsRead(AllMyNotificationsMarkedAsReadUm notificationsMarkedAsReadUm);
    void DeleteAllMyNotifications();
}