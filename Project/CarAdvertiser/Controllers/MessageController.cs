using System;
using System.Net;
using CarAdvertiser.Helpers;
using System.Web.Mvc;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetAllUnreadMessages()
        {
            return Json(MessageHelpers.GetAllUnreadMessages(User.Identity.GetUserId<int>()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllUndeletedMessages()
        {
            return Json(MessageHelpers.GetAllUndeletedMessages(User.Identity.GetUserId<int>()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMessage(int messageId)
        {
            MessagesService.SetRead(messageId);
            MessagesService.Delete(messageId);
            Uow.Commit();
            return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendMessage(string message, int toId)
        {
            if (string.IsNullOrEmpty(message))
            {
                return new JsonErrorResult(HttpStatusCode.InternalServerError)
                {
                    Data = "Missing message!",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            try
            {
                MessagesService.Create(new Messages
                {
                    MessageContent = message,
                    ReceiverId = toId,
                    SenderId = User.Identity.GetUserId<int>()
                });
                Uow.Commit();
            }
            catch (Exception ex)
            {
                return new JsonErrorResult(HttpStatusCode.InternalServerError)
                {
                    Data = ex.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return Json(new { result = "Message sent successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}