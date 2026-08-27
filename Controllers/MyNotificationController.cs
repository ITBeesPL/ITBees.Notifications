using ITBees.Notifications.Controllers.Models;
using ITBees.Notifications.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.Notifications.Controllers;

[Authorize]
public class MyNotificationController : RestfulControllerBase<MyNotificationController>
{
    private readonly INotificationsService _notificationsService;

    public MyNotificationController(ILogger<MyNotificationController> logger,
        INotificationsService notificationsService) : base(logger)
    {
        _notificationsService = notificationsService;
    }

    [HttpDelete]
    public IActionResult Delete([FromBody] MyNotificationDeleteDm notificationDeleteDm)
    {
        return ReturnOkResult(() => _notificationsService.Delete(notificationDeleteDm));
    }
}