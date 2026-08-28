namespace ITBees.Notifications.Controllers.Models;

public class AllMyNotificationsMarkedAsReadUm
{
    public bool ConfirmAllMarkedAsRead { get; set; }
    public string? Discriminator { get; set; }
    public string? ScopeKind { get; set; }
    public Guid? ScopeId { get; set; }
}
