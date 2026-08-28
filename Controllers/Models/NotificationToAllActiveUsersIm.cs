namespace ITBees.Notifications.Controllers.Models;

public class NotificationToAllActiveUsersIm
{
    public string? Link { get; set; }
    public string? Message { get; set; }
    public string Title { get; set; }
    public string? EmailBody { get; set; }
    public bool SendAlsoEmail { get; set; }
    public bool LinkOpenInNewWindow { get; set; }
    public string? Discriminator { get; set; }
    public string? ScopeKind { get; set; }
    public Guid? ScopeId { get; set; }
}
