using CarAdvertiser.DAL.Identity;
using CarAdvertiser.DTO;
using CarAdvertiser.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarAdvertiser.Helpers;

namespace CarAdvertiser.Controllers
{
    public class HomeController : BaseController
    {
        [RequireHttps]
        public ActionResult Index()
        {
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            CreateDefaultAdminUser();
            FillSmallSearch();
            return View();
        }

        public ActionResult GetAllPremiumAdverts(int featured)
        {
            List<PremiumAdvertViewModel> result = new List<PremiumAdvertViewModel>();
            foreach (Advertisement item in AdvertisementService.GetRandomPremiumAdvert(featured))
            {
                PremiumAdvertViewModel model = new PremiumAdvertViewModel
                {
                    Model = item.CarModel.Value,
                    Colour = item.Colour.Value,
                    Price = item.Price,
                    Description = item.AdvertDescription,
                    EngineSize = item.EngineSize.Size,
                    FuelType = item.FuelType.Value,
                    BodyType = item.BodyType.Value,
                    AdvertId = item.Id,
                    Make = item.CarModel.CarManufacturer.Value,
                    RegYear = item.RegYear,
                    SellerName = item.AppUser.Email,
                    Images = item.Images.Select(x => x.ImageData)
                };

                result.Add(model);
            }

            return PartialView("_PremiumAdvertPartial", result);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private void CreateDefaultAdminUser()
        {
            string admin = "admin@caradvertiser.co.uk";
            if (!RepositoryAppRole.RoleExists("Admin"))
            {
                RepositoryAppRole.Create("Admin");
                RepositoryAppRole.Save();
            }

            if (RepositoryAppUserV2.FindByEmail(admin) == null)
            {
                var adminUser = RepositoryAppUserV2.RegisterAsync(new AppUserV2
                {
                    UserName = admin,
                    Email = admin
                }, "Aa,12345").Result;

                RepositoryAppRole.AddUsersToRoles(new[] { admin }, new[] { "Admin" });
                RepositoryAppRole.Save();
            }
        }

        private void FillSmallSearch()
        {
            ViewData["Make"] = ManufacturerService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            });

            List<SelectListItem> fromYear = new List<SelectListItem>();
            for (int i = 1960; i <= DateTime.Now.Year; i++)
            {
                fromYear.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
            ViewData["FromYear"] = fromYear.OrderBy(x => x.Text);

            List<SelectListItem> toYear = new List<SelectListItem>();
            for (int i = 1960; i <= DateTime.Now.Year; i++)
            {
                toYear.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
            ViewData["ToYear"] = toYear.OrderByDescending(x => x.Text);

            List<SelectListItem> minPrice = new List<SelectListItem>();
            for (int i = 0; i < 50000; i += 500)
            {
                minPrice.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            ViewData["MinPrice"] = minPrice;

            List<SelectListItem> maxPrice = new List<SelectListItem>();
            for (int i = 0; i < 50000; i += 500)
            {
                {
                    maxPrice.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }

            ViewData["MaxPrice"] = maxPrice;

        }
    }
}