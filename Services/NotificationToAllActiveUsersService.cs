using ITBees.Interfaces.Repository;
using ITBees.Models.Users;
using ITBees.Notifications.Controllers.Models;
using ITBees.Notifications.Interfaces;
using ITBees.UserManager.Interfaces;

namespace ITBees.Notifications.Services;

public class NotificationToAllActiveUsersService : INotificationToAllActiveUsersService
{
    private readonly IReadOnlyRepository<UserAccount> _userAccountRoRepo;
    private readonly IAspCurrentUserService _currentUserService;
    private readonly IWriteOnlyRepository<ITBees.Notifications.DbModels.Notification> _notificationRwRepo;

    public NotificationToAllActiveUsersService(IReadOnlyRepository<UserAccount> userAccountRoRepo,
        IAspCurrentUserService currentUserService,
        IWriteOnlyRepository<ITBees.Notifications.DbModels.Notification> notificationRwRepo)
    {
        _userAccountRoRepo = userAccountRoRepo;
        _currentUserService = currentUserService;
        _notificationRwRepo = notificationRwRepo;
    }
    public NotificationToAllActiveUsersResultVm Send(NotificationToAllActiveUsersIm notification)
    {
        try
        {
            var notifications = new List<ITBees.Notifications.DbModels.Notification>();
            var userAccount = _userAccountRoRepo.GetData(x => x.Email.Contains("DELETED_") == false).ToList();
            foreach (var account in userAccount)
            {
                notifications.Add(new ITBees.Notifications.DbModels.Notification()
                {
                    UserAccountGuid = account.Guid,
                    Link = notification.Link,
                    Message = notification.Message,
                    Received = DateTime.UtcNow,
                    Title = notification.Title,
                    LinkOpenInNewWindow = notification.LinkOpenInNewWindow,
                    Discriminator = notification.Discriminator,
                    ScopeKind = notification.ScopeKind,
                    ScopeId = notification.ScopeId
                });
            }

            _notificationRwRepo.InsertData(notifications);

            return new NotificationToAllActiveUsersResultVm()
            {
                Success = true,
                Messaage = string.Empty
            };
        }
        catch (Exception e)
        {
            return new NotificationToAllActiveUsersResultVm()
            {
                Success = false,
                Messaage = $"Problem sending notifications: {e.Message}"
            };
        }
    }

    public NotificationToAllActiveUsersResultVm SendToSelected(NotificationToSelectedUsersIm notification)
    {
        try
        {
            var notifications = new List<ITBees.Notifications.DbModels.Notification>();
            foreach (var account in notification.RecipientUserAccounts)
            {
                notifications.Add(new ITBees.Notifications.DbModels.Notification()
                {
                    UserAccountGuid = account.UserAccountGuid,
                    Link = notification.Link,
                    Message = notification.Message,
                    Received = DateTime.UtcNow,
                    Title = notification.Title,
                    LinkOpenInNewWindow = notification.LinkOpenInNewWindow,
                    Discriminator = notification.Discriminator,
                    ScopeKind = notification.ScopeKind,
                    ScopeId = notification.ScopeId
                });
            }

            _notificationRwRepo.InsertData(notifications);

            return new NotificationToAllActiveUsersResultVm()
            {
                Success = true,
                Messaage = string.Empty
            };
        }
        catch (Exception e)
        {
            return new NotificationToAllActiveUsersResultVm()
            {
                Success = false,
                Messaage = $"Problem sending notifications: {e.Message}"
            };
        }
    }
}
