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

    public NotificationsCounterVm GetMyNotificationsCounters()
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        _notificationsRoRepository.GetDataQueryable(x => x.UserAccountGuid == cu.Guid).Select(x => x);
        var query = _notificationsRoRepository.GetDataQueryable(x => x.UserAccountGuid == cu.Guid);

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
        SortOrder? sortOrder)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        if (onlyUnread)
        {
            var result = _notificationsRoRepository
                .GetDataPaginated(x => x.UserAccountGuid == cu.Guid && x.HasBeenRead == false , new SortOptions(page, pageSize, "Received", SortOrder.Descending))
                .MapTo(x => new MyNotificationVm(x));
            return result;    
        }
        else
        {
            var result = _notificationsRoRepository
                .GetDataPaginated(x => x.UserAccountGuid == cu.Guid , new SortOptions(page, pageSize, "Received", SortOrder.Descending))
                .MapTo(x => new MyNotificationVm(x));
            return result;
        }
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
        _notificationsRwRepository.DeleteData(x=>x.Guid == notificationDeleteDm.Guid);
    }

    public NotificationsCounterVm MarkAllNotyficationsAsRead(
        AllMyNotificationsMarkedAsReadUm notificationsMarkedAsReadUm)
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        var result = _notificationsRwRepository.UpdateData(x => x.UserAccountGuid == cu.Guid && x.HasBeenRead ==false,
            x =>
            {
                x.HasBeenRead = true;
                x.HasBeenReadDate = DateTime.Now;
            });
        
        return GetMyNotificationsCounters();
    }

    public void DeleteAllMyNotifications()
    {
        var cu = _aspCurrentUserService.GetCurrentUser();
        //todo mark as deleted in future if we want to track open rate for notifications
        _notificationsRwRepository.DeleteData(x => x.UserAccountGuid == cu.Guid);
    }
}