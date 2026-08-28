using ITBees.Interfaces.Repository;
using ITBees.Notifications.Controllers.Models;

namespace ITBees.Notifications.Interfaces;

public interface INotificationsService
{
    NotificationsCounterVm GetMyNotificationsCounters();
    NotificationsCounterVm GetMyNotificationsCounters(string? discriminator, string? scopeKind, Guid? scopeId);
    PaginatedResult<MyNotificationVm> GetMyNotifications(bool onlyUnread, int? page, int? pageSize, string? sortColumn,
        SortOrder? sortOrder);
    PaginatedResult<MyNotificationVm> GetMyNotifications(bool onlyUnread, int? page, int? pageSize,
        string? sortColumn, SortOrder? sortOrder, string? discriminator, string? scopeKind, Guid? scopeId);
    MyNotificationVm MarkNotificationAsClicked(MyNotificationClickedUm notificationUm);
    MyNotificationVm SetReadStatus(MyNotificationReadStatusIm notificationIm);
    void Delete(MyNotificationDeleteDm notificationDeleteDm);
    NotificationsCounterVm MarkAllNotyficationsAsRead(AllMyNotificationsMarkedAsReadUm notificationsMarkedAsReadUm);
    void DeleteAllMyNotifications();
    void DeleteAllMyNotifications(string? discriminator, string? scopeKind, Guid? scopeId);
}
