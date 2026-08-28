using ITBees.Interfaces.Repository;
using ITBees.Notifications.Controllers.Models;
using ITBees.Notifications.DbModels;
using ITBees.Notifications.Interfaces;
using ITBees.UserManager.Interfaces;

namespace ITBees.Notifications.Services;

public class NotificationsService : INotificationsService
{
    private readonly IAspCurrentUserService _aspCurrentUserService;
    private readonly IReadOnlyRepository<Notification> _notificationsRoRepository;
    private readonly IWriteOnlyRepository<Notification> _notificationsRwRepository;

    public NotificationsService(IAspCurrentUserService aspCurrentUserService,
        IReadOnlyRepository<Notification> notificationsRoRepository,
        IWriteOnlyRepository<Notification> notificationsRwRepository)
    {
        _aspCurrentUserService = aspCurrentUserService;
        _notificationsRoRepository = notificationsRoRepository;
        _notificationsRwRepository = notificationsRwRepository;
    }

    public NotificationsCounterVm GetMyNotificationsCounters() =>
        GetMyNotificationsCounters(null, null, null);

    public NotificationsCounterVm GetMyNotificationsCounters(string? discriminator, string? scopeKind, Guid? scopeId)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        var query = _notificationsRoRepository.GetDataQueryable(x =>
            x.UserAccountGuid == cu.Guid &&
            (discriminator == null || x.Discriminator == discriminator) &&
            (scopeKind == null || x.ScopeKind == scopeKind) &&
            (scopeId == null || x.ScopeId == scopeId));

        var counts = query
            .GroupBy(x => 1) // Group all notifications into one group
            .Select(g => new
            {
                // Total notifications count for the given user
                TotalCount = g.Count(),
                // Count of notifications where HasBeenRead is true
                ReadCount = g.Sum(x => x.HasBeenRead ? 1 : 0)
            })
            .FirstOrDefault();

        int totalCount = counts?.TotalCount ?? 0;
        int readCount = counts?.ReadCount ?? 0;
        return new NotificationsCounterVm()
        {
            UnreadMessagesCount = totalCount - readCount,
            TotalMessagesCount = totalCount,
        };
    }

    public PaginatedResult<MyNotificationVm> GetMyNotifications(bool onlyUnread, int? page, int? pageSize,
        string? sortColumn,
        SortOrder? sortOrder) =>
        GetMyNotifications(onlyUnread, page, pageSize, sortColumn, sortOrder, null, null, null);

    public PaginatedResult<MyNotificationVm> GetMyNotifications(bool onlyUnread, int? page, int? pageSize,
        string? sortColumn, SortOrder? sortOrder, string? discriminator, string? scopeKind, Guid? scopeId)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        return _notificationsRoRepository
            .GetDataPaginated(x =>
                    x.UserAccountGuid == cu.Guid &&
                    (!onlyUnread || !x.HasBeenRead) &&
                    (discriminator == null || x.Discriminator == discriminator) &&
                    (scopeKind == null || x.ScopeKind == scopeKind) &&
                    (scopeId == null || x.ScopeId == scopeId),
                new SortOptions(page, pageSize, "Received", SortOrder.Descending))
            .MapTo(x => new MyNotificationVm(x));
    }

    public MyNotificationVm MarkNotificationAsClicked(MyNotificationClickedUm notificationUm)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        var result = _notificationsRwRepository.UpdateData(x => x.UserAccountGuid == cu.Guid && x.Guid == notificationUm.Guid,
            x =>
            {
                x.HasBeenClicked = true;
                x.HasBeenClickedDate = DateTime.Now;
            }).FirstOrDefault();
        return new MyNotificationVm(result);
    }

    public MyNotificationVm SetReadStatus(MyNotificationReadStatusIm notificationIm)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        var result = _notificationsRwRepository.UpdateData(x => x.UserAccountGuid == cu.Guid && x.Guid == notificationIm.Guid,
            x =>
            {
                x.HasBeenRead = notificationIm.IsReadStatus; 
                x.HasBeenReadDate = DateTime.Now;
            }).FirstOrDefault();
        return new MyNotificationVm(result);
    }

    public void Delete(MyNotificationDeleteDm notificationDeleteDm)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        _notificationsRwRepository.DeleteData(x =>
            x.UserAccountGuid == cu.Guid && x.Guid == notificationDeleteDm.Guid);
    }

    public NotificationsCounterVm MarkAllNotyficationsAsRead(
        AllMyNotificationsMarkedAsReadUm notificationsMarkedAsReadUm)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        _notificationsRwRepository.UpdateData(x =>
                x.UserAccountGuid == cu.Guid && !x.HasBeenRead &&
                (notificationsMarkedAsReadUm.Discriminator == null ||
                 x.Discriminator == notificationsMarkedAsReadUm.Discriminator) &&
                (notificationsMarkedAsReadUm.ScopeKind == null ||
                 x.ScopeKind == notificationsMarkedAsReadUm.ScopeKind) &&
                (notificationsMarkedAsReadUm.ScopeId == null ||
                 x.ScopeId == notificationsMarkedAsReadUm.ScopeId),
            x =>
            {
                x.HasBeenRead = true;
                x.HasBeenReadDate = DateTime.Now;
            });
        
        return GetMyNotificationsCounters(notificationsMarkedAsReadUm.Discriminator,
            notificationsMarkedAsReadUm.ScopeKind, notificationsMarkedAsReadUm.ScopeId);
    }

    public void DeleteAllMyNotifications() => DeleteAllMyNotifications(null, null, null);

    public void DeleteAllMyNotifications(string? discriminator, string? scopeKind, Guid? scopeId)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        //todo mark as deleted in future if we want to track open rate for notifications
        _notificationsRwRepository.DeleteData(x =>
            x.UserAccountGuid == cu.Guid &&
            (discriminator == null || x.Discriminator == discriminator) &&
            (scopeKind == null || x.ScopeKind == scopeKind) &&
            (scopeId == null || x.ScopeId == scopeId));
    }
}
