using ITBees.Notifications.Controllers.Models;
using ITBees.Notifications.Interfaces;
using ITBees.RestfulApiControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ITBees.Notifications.Controllers
{
    [Authorize]
    public class NotificationCounterController : RestfulControllerBase<NotificationCounterController>
    {
        private readonly INotificationsService _notificationsService;

        public NotificationCounterController(ILogger<NotificationCounterController> logger,
            INotificationsService notificationsService) : base(logger)
        {
            _notificationsService = notificationsService;
        }

        [HttpGet]
        [Produces(typeof(NotificationsCounterVm))]
        public IActionResult Get()
        {
            return ReturnOkResult(() => _notificationsService.GetMyNotificationsCounters());
        }
    }
}