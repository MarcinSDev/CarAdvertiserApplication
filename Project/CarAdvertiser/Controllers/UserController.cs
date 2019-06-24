using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CarAdvertiser.BLL.Services;
using CarAdvertiser.DTO;
using CarAdvertiser.Helpers;
using CarAdvertiser.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace CarAdvertiser.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        [Authorize]
        [HttpGet]
        public ActionResult UserPanel()
        {
            ViewBag.UserAdvertCount = AdvertisementService
                .GetAllActiveAdverts().Count(x => x.AppUserId == User.Identity.GetUserId<int>());

            ViewBag.UserExpiredAdvertCount = AdvertisementService
                .GetAllExpiredAdverts().Count(x => x.AppUserId == User.Identity.GetUserId<int>());

            ViewBag.UserSoldCars = AdvertisementService
                .GetAll().Where(x => x.AppUserId == User.Identity.GetUserId<int>()).Count(x => x.IsSold == true);

            ViewBag.MyBookingsCount = BookingService.GetAll().Count(x => x.BookingUserId == User.Identity.GetUserId<int>());

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ManageAdverts()
        {
            IEnumerable<Advertisement> userAdverts = AdvertisementService.GetAllActiveAdverts().Where(x => x.AppUserId == User.Identity.GetUserId<int>());

            List<UserAdvertsViewModel> result = new List<UserAdvertsViewModel>();
            foreach (Advertisement item in userAdverts)
            {
                result.Add(new UserAdvertsViewModel
                {
                    AdvertId = item.Id,
                    Make = item.CarModel.CarManufacturer.Value,
                    Model = item.CarModel.Value,
                    Price = item.Price,
                    ClosingDate = item.AdvertCloseDate,
                    IsSold = item.IsSold
                });
            }
            return View("ManageAdverts", result);
        }

        //[HttpPost]
        [Authorize]
        [NoDirectAccess]
        [HttpGet]
        public ActionResult ExpiredAdverts()
        {
            IEnumerable<Advertisement> expiredAdverts = AdvertisementService.GetAllExpiredAdverts()
                .Where(x => x.AppUserId == User.Identity.GetUserId<int>());

            List<UserAdvertsViewModel> result = new List<UserAdvertsViewModel>();
            foreach (Advertisement item in expiredAdverts)
            {
                result.Add(new UserAdvertsViewModel
                {
                    AdvertId = item.Id,
                    Make = item.CarModel.CarManufacturer.Value,
                    Model = item.CarModel.Value,
                    Price = item.Price,
                    ClosingDate = item.AdvertCloseDate,
                    IsSold = item.IsSold
                });
            }

            return View(result);
        }

        [Authorize]
        [NoDirectAccess]
        [HttpGet]
        public ActionResult SoldCars()
        {
            IEnumerable<Advertisement> soldCars = AdvertisementService.GetAll()
                .Where(x => x.AppUserId == User.Identity.GetUserId<int>() & x.IsSold);

            List<UserAdvertsViewModel> result = new List<UserAdvertsViewModel>();
            foreach (Advertisement item in soldCars)
            {
                result.Add(new UserAdvertsViewModel
                {
                    AdvertId = item.Id,
                    Price = item.Price,
                    ClosingDate = item.AdvertCloseDate
                });
            }

            return View(result);
        }

        [HttpPost]
        public ActionResult SetAsSold(int id)
        {
            if (ModelState.IsValid)
            {
                Advertisement adv = AdvertisementService.FindById(id);
                adv.IsSold = true;
                AdvertisementService.Update(adv);
                Uow.Commit();

            }
            return View("UserPanel");
        }

        [Authorize]
        [NoDirectAccess]
        [HttpGet]
        public ActionResult MyBookings()
        {
            IEnumerable<Booking> myBookings = BookingService.GetAll()
                .Where(x => x.BookingUserId == User.Identity.GetUserId<int>());

            List<MyBookingsViewModel> result = new List<MyBookingsViewModel>();
            foreach (Booking item in myBookings)
            {
                result.Add(new MyBookingsViewModel
                {
                    BookingId = item.Id,
                    AvailabilityId = item.AvailabilityId,
                    BookingUserId = item.BookingUserId,
                });
            }

            return View(result);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditExpiredAdvert()
        {

            return View();
        }

        [Authorize]
        public ActionResult AdminPanel()
        {
            return View();
        }
    }
}