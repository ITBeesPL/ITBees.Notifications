using ITBees.Notifications.Controllers.Models;
using ITBees.Notifications.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.Notifications.Controllers;

[Authorize]
public class MyNotificationHasBeenClickedController : RestfulControllerBase<MyNotificationHasBeenClickedController>
{
    private readonly INotificationsService _notificationsService;

    public MyNotificationHasBeenClickedController(ILogger<MyNotificationHasBeenClickedController> logger,
        INotificationsService notificationsService) : base(logger)
    {
        _notificationsService = notificationsService;
    }

    [HttpPut]
    [Produces(typeof(MyNotificationVm))]
    public IActionResult Put([FromBody] MyNotificationClickedUm notificationUm)
    {
        return ReturnOkResult(() => _notificationsService.MarkNotificationAsClicked(notificationUm));
    }
}