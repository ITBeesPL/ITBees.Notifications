using ITBees.Interfaces.CodeGeneration;
using ITBees.Notifications.DbModels;

namespace ITBees.Notifications.Controllers.Models;

public class MyNotificationVm
{
    public MyNotificationVm()
    {
        
    }

    public MyNotificationVm(Notification x)
    {
        Guid = x.Guid;
        Received = x.Received;
        Title = x.Title;
        Message = x.Message;
        Link = x.Link;
        HasBeenRead = x.HasBeenRead;
        HasBeenClicked = x.HasBeenClicked;
        LinkOpenInNewWindow = x.LinkOpenInNewWindow;
    }

    public Guid Guid { get; set; }
    public DateTime Received { get; set; }
    public string Title { get; set; }

    [NullableStringProperty]
    public string? Message { get; set; }

    [NullableStringProperty]
    public string? Link { get; set; }


    public bool HasBeenRead { get; set; }
    public bool HasBeenClicked { get; set; }
    public bool LinkOpenInNewWindow { get; set; }
}