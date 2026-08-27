using ITBees.Notifications.Controllers.Models;
using ITBees.Notifications.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.Notifications.Controllers;

[Authorize]
public class
    MyNotificationReadStatusChangedController : RestfulControllerBase<MyNotificationReadStatusChangedController>
{
    private readonly INotificationsService _notificationsService;

    public MyNotificationReadStatusChangedController(ILogger<MyNotificationReadStatusChangedController> logger,
        INotificationsService notificationsService) : base(logger)
    {
        _notificationsService = notificationsService;
    }

    [HttpPost]
    [Produces(typeof(MyNotificationVm))]
    public IActionResult Get([FromBody] MyNotificationReadStatusIm notificationIm)
    {
        return ReturnOkResult(() => _notificationsService.SetReadStatus(notificationIm));
    }
}