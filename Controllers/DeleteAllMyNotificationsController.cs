using ITBees.Notifications.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.Notifications.Controllers;

[Authorize]
public class DeleteAllMyNotificationsController : RestfulControllerBase<DeleteAllMyNotificationsController>
{
    private readonly INotificationsService _notificationsService;

    public DeleteAllMyNotificationsController(ILogger<DeleteAllMyNotificationsController> logger,
        INotificationsService notificationsService) : base(logger)
    {
        _notificationsService = notificationsService;
    }

    [HttpDelete]
    public IActionResult Delete()
    {
        return ReturnOkResult(() => _notificationsService.DeleteAllMyNotifications());
    }
}