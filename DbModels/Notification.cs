using ITBees.Models.Users;

namespace ITBees.Notifications.DbModels;

public class Notification
{
    public Guid Guid { get; set; }
    public DateTime Received { get; set; }
    public string Title { get; set; }
    public string? Message { get; set; }
    public string? Link { get; set; }
    public bool HasBeenRead { get; set; }
    public DateTime? HasBeenReadDate { get; set; }
    public bool HasBeenClicked { get; set; }
    public DateTime? HasBeenClickedDate { get; set; }
    public UserAccount UserAccount { get; set; }
    public Guid UserAccountGuid { get; set; }
    public bool LinkOpenInNewWindow { get; set; }
}