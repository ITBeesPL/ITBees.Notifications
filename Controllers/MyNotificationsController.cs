using ITBees.Interfaces.Repository;
using ITBees.Notifications.Controllers.Models;
using ITBees.Notifications.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.Notifications.Controllers;

[Authorize]
public class MyNotificationsController : RestfulControllerBase<MyNotificationsController>
{
    private readonly INotificationsService _notificationsService;

    public MyNotificationsController(ILogger<MyNotificationsController> logger,
        INotificationsService notificationsService) : base(logger)
    {
        _notificationsService = notificationsService;
    }

    [HttpGet]
    [Produces(typeof(PaginatedResult<MyNotificationVm>))]
    public IActionResult Get(bool onlyUnread, int? page, int? pageSize, string? sortColumn, SortOrder? sortOrder,
        [FromQuery] string? discriminator = null, [FromQuery] string? scopeKind = null,
        [FromQuery] Guid? scopeId = null)
    {
        return ReturnOkResult(() => _notificationsService.GetMyNotifications(onlyUnread, page, pageSize,
            sortColumn, sortOrder, discriminator, scopeKind, scopeId));
    }
}
