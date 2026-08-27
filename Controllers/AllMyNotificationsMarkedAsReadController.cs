using ITBees.Notifications.Controllers.Models;
using ITBees.Notifications.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.Notifications.Controllers;

[Authorize]
public class
    AllMyNotificationsMarkedAsReadController : RestfulControllerBase<AllMyNotificationsMarkedAsReadController>
{
    private readonly INotificationsService _notificationsService;

    public AllMyNotificationsMarkedAsReadController(ILogger<AllMyNotificationsMarkedAsReadController> logger,
        INotificationsService notificationsService) : base(logger)
    {
        _notificationsService = notificationsService;
    }

    [HttpPut]
    [Produces(typeof(NotificationsCounterVm))]
    public IActionResult Put([FromBody] AllMyNotificationsMarkedAsReadUm notificationsMarkedAsReadUm)
    {
        return ReturnOkResult(() => _notificationsService.MarkAllNotyficationsAsRead(notificationsMarkedAsReadUm));
    }
}