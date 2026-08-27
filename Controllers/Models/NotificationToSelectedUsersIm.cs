namespace ITBees.Notifications.Controllers.Models;

public class NotificationToSelectedUsersIm : NotificationToAllActiveUsersIm
{
    public List<RecipientUserAccountIm> RecipientUserAccounts { get; set; }
}