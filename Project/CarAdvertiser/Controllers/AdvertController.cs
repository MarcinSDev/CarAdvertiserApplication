using CarAdvertiser.DTO;
using CarAdvertiser.DTO.ValueEntities;
using CarAdvertiser.Helpers;
using CarAdvertiser.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CarAdvertiser.Controllers
{
    public class AdvertController : BaseController
    {
        [HttpGet]
        [Authorize]
        public ActionResult AddAdvert()
        {
            AddAdvertViewModel model = new AddAdvertViewModel();
            FillAdvertView();
            FillCheckBoxes(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AddAdvert(AddAdvertViewModel model, HttpPostedFileBase[] files)
        {
            TempData["error"] = null;
            TempData["success"] = null;

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Please fill all mandatory fields!";
                FillAdvertView();
                model.Additionals = new List<CheckBoxList>();
                FillCheckBoxes(model);
                return View(model);
            }

            int saved = 0;

            Advertisement advert = new Advertisement
            {
                AppUserId = User.Identity.GetUserId<int>(),
                CarModelId = model.CarModelId,
                Price = model.Price,
                CurrentMileage = model.CurrentMileage,
                HorsePower = model.HorsePower,
                AmountOfPrevOwners = model.AmountOfPrevOwners,
                AdvertDescription = model.AdvertDescription,
                RegYear = model.RegYear,
                AdvertOpenDate = model.AdvertOpenDate,
                EngineSizeId = model.EngineSizeId,
                BodyTypeId = model.BodyTypeId,
                TransmissionId = model.TransmissionId,
                SeatAmountId = model.SeatAmountId,
                DriveTrainId = model.DriveTrainId,
                FuelTypeId = model.FuelTypeId,
                DoorAmountId = model.DoorAmountId,
                ColourId = model.ColourId,
                AdvertCloseDate = model.AdvertOpenDate.AddDays(model.AdvertAliveDays),
                IsPremium = model.IsPremium,
                IsSold = false,
            };

            List<DateTime> dates = AdvertisementService.GenerateDates(model.AdvertOpenDate,
                model.AdvertOpenDate.AddDays(model.AdvertAliveDays));

            AdvertisementService.Create(advert);
            saved++;

            foreach (DateTime date in dates)
            {
                BookingAvailabilityService.Create(new BookingAvailability
                {
                    AvailableDate = date,
                    AdvertId = advert.Id
                });
                saved++;
            }

            foreach (CheckBoxList addItem in model.Additionals.Where(x => x.IsChecked))
            {
                ExtrasService.Create(new CarExtras
                {
                    EquipmentId = addItem.Id,
                    ExtrasAdvertId = advert.Id
                });
                saved++;
            }

            foreach (HttpPostedFileBase file in files)
            {
                if (file.ContentLength > 0)
                {
                    byte[] data = null;
                    using (MemoryStream target = new MemoryStream())
                    {
                        file.InputStream.CopyTo(target);
                        data = target.ToArray();
                    }

                    ImageService.Create(new Image
                    {
                        ContentType = file.ContentType,
                        ImageName = file.FileName,
                        ImageData = data
                    });
                    saved++;
                }
            }

            if (Uow.Commit() == saved)
            {
                List<WantedCar> wantedList = WantedCarService.GetAllByPriceMileage(model.Price, model.RegYear);

                if (wantedList.Any())
                {
                    List<string> emails = new List<string>();
                    foreach (int userId in wantedList.Select(x => x.UserId))
                    {
                        emails.Add(RepositoryAppUserV2.FindById(userId).Email);
                    }
                    Email.InterestedAdvertAdded(emails.ToArray(), advert);
                }

                TempData["success"] = $"Thank you! Confirmation email has been sent to your inbox";

                Email.Send(new[] { User.Identity.GetEmail() }, $"Greetings from Car Advertiser. Your advert will be live from {model.AdvertOpenDate} and will close on {model.AdvertOpenDate.AddDays(model.AdvertAliveDays)}");

                return RedirectToAction("AddAdvert");
            }

            TempData["error"] = $"Error Occured When Trying To Add The Advert. Please try again!";
            return View(model);
        }

        private void FillAdvertView()
        {
            ViewData["Make"] = ManufacturerService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            });

            ViewData["Model"] = ModelService.GetAllNotDeleted(x => x.CarManufacturer).Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            });

            ViewData["Engine"] = EngineSizeService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Size.ToString(),
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text);

            ViewData["BodyType"] = BodyTypeService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text);

            ViewData["Transmission"] = TransmissionService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text);

            ViewData["Seat"] = SeatAmountService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.NumberOfSeats.ToString(),
                Value = x.Id.ToString()
            });

            ViewData["DriveTrain"] = DriveTrainService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text);

            ViewData["FuelType"] = FuelTypeService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text);

            ViewData["Door"] = DoorService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.NumberOfDoors.ToString(),
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text);

            ViewData["Colour"] = ColourService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text);

            List<SelectListItem> years = new List<SelectListItem>();
            {
                for (int i = 1960; i <= DateTime.Now.Year; i++)
                {
                    years.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }
            ViewData["Year"] = years.ToList().OrderByDescending(x => x.Text);

            List<SelectListItem> days = new List<SelectListItem>();
            {
                for (int i = 1; i < 31; i++)
                {
                    days.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }
            ViewData["Days"] = days.ToList();
        }

        private void FillCheckBoxes(AddAdvertViewModel model)
        {
            foreach (AdditionalEquipment item in EquipmentService.GetAllNotDeleted())
            {
                CheckBoxList newItem = new CheckBoxList
                {
                    Id = item.Id,
                    Display = item.Value,
                    IsChecked = false
                };

                if (model.Additionals.All(x => x.Id != newItem.Id))
                    model.Additionals.Add(newItem);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddWantedCar()
        {
            WantedCarViewModel wanted = new WantedCarViewModel();
            FillWantedCarView();
            return View(wanted);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddWantedCar(WantedCarViewModel wanted)
        {
            TempData["error"] = null;
            TempData["success"] = null;

            if (!ModelState.IsValid)
            {
                FillWantedCarView();
                return View(wanted);
            }

            int saved = 0;
            WantedCar wantedCar = new WantedCar
            {
                MinimumYear = wanted.MinYear,
                MaxPrice = wanted.MaxPrice,
                IsWanted = true,
                UserId = User.Identity.GetUserId<int>(),
                ModelId = wanted.Model
            };
            WantedCarService.Create(wantedCar);
            saved++;
            if (Uow.Commit() == saved)
            {
                List<Advertisement> existInDb = AdvertisementService.GetAllByWanted(wanted.Make, wanted.Model, wanted.MinYear, wanted.MaxPrice);

                if (existInDb.Any())
                {
                    TempData["success"] = $"Thank you! Database contains car(s) you want. Found {existInDb.Count} car matches!";
                }
                else
                {
                    TempData["success"] = $"Thank you! As soon as chosen car appears in our database we notify you via an email.";
                }

                return RedirectToAction("AddWantedCar");
            }

            TempData["error"] = $"Error Occured When Trying To Add The Advert. Please try again!";
            return View(wanted);
        }

        private void FillWantedCarView()
        {
            ViewData["Make"] = ManufacturerService.GetAllNotDeleted().Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            });

            ViewData["Model"] = ModelService.GetAllNotDeleted(x => x.CarManufacturer).Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Id.ToString()
            });

            List<SelectListItem> years = new List<SelectListItem>();
            {
                for (int i = 1960; i <= DateTime.Now.Year; i++)
                {
                    years.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }
            ViewData["Year"] = years.ToList().OrderByDescending(x => x.Text);

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

        [HttpPost]
        public JsonResult GetModels(int makeId)
        {
            List<SelectListItem> result = ModelService.GetAllNotDeleted().Where(x => x.ManufacturerId == makeId).Select(
                x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Value
                    }).ToList();
            return Json(new SelectList(result, "Value", "Text"));
        }

        [HttpGet]
        public ActionResult AdvertSearch()
        {
            AdvertSearchViewModel result = new AdvertSearchViewModel();
            FillAdvertSearchView(result);
            return View(result);
        }

        private void FillAdvertSearchView(AdvertSearchViewModel result)
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

            List<SelectListItem> maxMileage = new List<SelectListItem>();
            for (int i = 0; i < 250000; i += 1000)
            {
                {
                    maxMileage.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }

            ViewData["Mileage"] = maxMileage;

            List<SelectListItem> minHorsePower = new List<SelectListItem>();
            for (int i = 0; i < 500; i += 20)
            {
                {
                    minHorsePower.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }

            ViewData["HorsePower"] = minHorsePower;

            foreach (BodyType item in BodyTypeService.GetAllNotDeleted())
            {
                result.Styles.Add(new CheckBoxList
                {
                    Id = item.Id,
                    Display = item.Value,
                    IsChecked = false
                });
            }

            foreach (EngineSize item in EngineSizeService.GetAllNotDeleted())
            {
                result.Engines.Add(new CheckBoxList
                {
                    Id = item.Id,
                    Display = Convert.ToString(item.Size),
                    IsChecked = false
                });
            }

            foreach (DoorAmount item in DoorService.GetAllNotDeleted())
            {
                result.Doors.Add(new CheckBoxList
                {
                    Id = item.Id,
                    Display = Convert.ToString(item.NumberOfDoors),
                    IsChecked = false
                });
            }

            foreach (SeatAmount item in SeatAmountService.GetAllNotDeleted())
            {
                result.Seats.Add(new CheckBoxList
                {
                    Id = item.Id,
                    Display = Convert.ToString(item.NumberOfSeats),
                    IsChecked = false
                });
            }

            foreach (Colour item in ColourService.GetAllNotDeleted())
            {
                result.Colours.Add(new CheckBoxList
                {
                    Id = item.Id,
                    Display = item.Value,
                    IsChecked = false
                });
            }

            foreach (Transmission item in TransmissionService.GetAllNotDeleted())
            {
                result.Transmissions.Add(new CheckBoxList
                {
                    Id = item.Id,
                    Display = item.Value,
                    IsChecked = false
                });
            }

            foreach (DriveTrain item in DriveTrainService.GetAllNotDeleted())
            {
                result.Drivetrains.Add(new CheckBoxList
                {
                    Id = item.Id,
                    Display = item.Value,
                    IsChecked = false
                });
            }

            foreach (FuelType item in FuelTypeService.GetAllNotDeleted())
            {
                result.Fueltypes.Add(new CheckBoxList
                {
                    Id = item.Id,
                    Display = item.Value,
                    IsChecked = false
                });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdvertSearch(AdvertSearchViewModel searchModel, string frontendMakeId, string frontendModelId)
        {
            IEnumerable<Advertisement> searchResult = AdvertisementService.GetAllActiveAdverts().ToList();

            if (!string.IsNullOrEmpty(frontendModelId))
            {
                searchModel.CarModel = ModelService.FindById(int.Parse(frontendModelId));
                searchResult = searchResult.Where(x => x.CarModelId == searchModel.CarModel.Id).ToList();
            }

            if (!string.IsNullOrEmpty(frontendMakeId))
            {
                searchModel.CarManufacturer = ManufacturerService.FindById(int.Parse(frontendMakeId));
                searchResult = searchResult.Where(x => x.CarModel?.ManufacturerId == searchModel.CarManufacturer.Id)
                    .ToList();
            }

            searchResult =
                AdvertisementService.FilterPriceRange(searchModel.MinPrice, searchModel.MaxPrice, searchResult);

            searchResult =
                AdvertisementService.FilterYearRange(searchModel.MinRegYear, searchModel.MaxRegYear, searchResult);

            searchResult = AdvertisementService.FilterBHP(searchModel.MinHorsePower, searchResult);

            searchResult = AdvertisementService.FilterMileage(searchModel.MaxMilleage, searchResult);


            searchResult = FilterSearch(searchResult.ToList(), searchModel);

            List<SmallSearchResultViewModel> result = new List<SmallSearchResultViewModel>();
            foreach (Advertisement item in searchResult)
            {
                result.Add(new SmallSearchResultViewModel
                {
                    Model = item.CarModel.Value,
                    Make = item.CarModel.CarManufacturer.Value,
                    Price = item.Price,
                    EngineSize = item.EngineSize.Size,
                    RegYear = item.RegYear,
                    Description = item.AdvertDescription,
                    Images = item.Images.Select(x => x.ImageData),
                    IsSold = item.IsSold,
                    AdvertisementId = item.Id
                });
            }


            return View("MainActiveAdverts", result);
        }


        private List<Advertisement> FilterSearch(List<Advertisement> searchResult, AdvertSearchViewModel model)
        {
            List<int> id = new List<int>();
            bool required = false;
            foreach (CheckBoxList item in model.Styles)
            {
                if (item.IsChecked)
                {
                    required = true;
                    id.Add(item.Id);
                }
            }
            if (required)
            {
                searchResult = searchResult.Where(x => id.Contains(x.BodyTypeId)).ToList();
            }

            id = new List<int>();
            required = false;
            foreach (CheckBoxList item in model.Engines)
            {
                if (item.IsChecked)
                {
                    required = true;
                    id.Add(item.Id);
                }
            }
            if (required)
            {
                searchResult = searchResult.Where(x => id.Contains(x.EngineSizeId)).ToList();
            }

            id = new List<int>();
            required = false;
            foreach (CheckBoxList item in model.Doors)
            {
                if (item.IsChecked)
                {
                    required = true;
                    id.Add(item.Id);
                }
            }
            if (required)
            {
                searchResult = searchResult.Where(x => id.Contains(x.DoorAmountId)).ToList();
            }

            id = new List<int>();
            required = false;
            foreach (CheckBoxList item in model.Seats)
            {
                if (item.IsChecked)
                {
                    required = true;
                    id.Add(item.Id);
                }
            }
            if (required)
            {
                searchResult = searchResult.Where(x => id.Contains(x.SeatAmountId)).ToList();
            }

            id = new List<int>();
            required = false;
            foreach (CheckBoxList item in model.Colours)
            {
                if (item.IsChecked)
                {
                    required = true;
                    id.Add(item.Id);
                }
            }
            if (required)
            {
                searchResult = searchResult.Where(x => id.Contains(x.ColourId)).ToList();
            }

            id = new List<int>();
            required = false;
            foreach (CheckBoxList item in model.Transmissions)
            {
                if (item.IsChecked)
                {
                    required = true;
                    id.Add(item.Id);
                }
            }
            if (required)
            {
                searchResult = searchResult.Where(x => id.Contains(x.TransmissionId)).ToList();
            }

            id = new List<int>();
            required = false;
            foreach (CheckBoxList item in model.Drivetrains)
            {
                if (item.IsChecked)
                {
                    required = true;
                    id.Add(item.Id);
                }
            }
            if (required)
            {
                searchResult = searchResult.Where(x => id.Contains(x.DriveTrainId)).ToList();
            }

            id = new List<int>();
            required = false;
            foreach (CheckBoxList item in model.Fueltypes)
            {
                if (item.IsChecked)
                {
                    required = true;
                    id.Add(item.Id);
                }
            }
            if (required)
            {
                searchResult = searchResult.Where(x => id.Contains(x.FuelTypeId)).ToList();
            }

            return searchResult;
        }

        public ActionResult SelectedAdvert(int id)
        {
            Advertisement selectedAdvertisement = AdvertisementService.FindById(id, x => x.CarModel.CarManufacturer, x => x.BodyType, x => x.Transmission, x => x.SeatAmount, x => x.DriveTrain, x => x.FuelType, x => x.DoorAmount, x => x.Colour, x => x.EngineSize, x => x.Images, x => x.Extras);
            if (selectedAdvertisement == null) throw new ArgumentNullException(nameof(selectedAdvertisement));

            var result = new SelectedAdvertViewModel
            {
                Id = selectedAdvertisement.Id,
                Model = selectedAdvertisement.CarModel.Value,
                Make = selectedAdvertisement.CarModel.CarManufacturer.Value,
                HorsePower = selectedAdvertisement.HorsePower,
                AmountOfOwners = selectedAdvertisement.AmountOfPrevOwners,
                Description = selectedAdvertisement.AdvertDescription,
                AdvertCloseDate = selectedAdvertisement.AdvertCloseDate,
                BodyType = selectedAdvertisement.BodyType.Value,
                Transmission = selectedAdvertisement.Transmission.Value,
                SeatAmount = selectedAdvertisement.SeatAmount.NumberOfSeats,
                DriveTrain = selectedAdvertisement.DriveTrain.Value,
                FuelType = selectedAdvertisement.FuelType.Value,
                DoorAmount = selectedAdvertisement.DoorAmount.NumberOfDoors,
                Colour = selectedAdvertisement.Colour.Value,
                RegYear = selectedAdvertisement.RegYear,
                Price = selectedAdvertisement.Price,
                EngineSize = selectedAdvertisement.EngineSize.Size,
                Images = selectedAdvertisement.Images.Select(x => x.ImageData),
                AdvertiserId = selectedAdvertisement.AppUserId
            };

            result.Extras = ExtrasService.GetAllByAdvertId(id);
            
            foreach (BookingAvailability item in BookingAvailabilityService.GetAllNotDeleted(x => x.Advertisement).Where(x => x.AdvertId == selectedAdvertisement.Id))
            {
                IEnumerable<Booking> alreadyBookedDates = BookingService.GetAllNotDeleted(x => x.BookingAvailability)
                    .Where(x => x.AvailabilityId == item.Id);

                if (!alreadyBookedDates.Any())
                {
                    result.BookingDates.Add(new CheckBoxList
                    {
                        Id = item.Id,
                        Display = item.AvailableDate.ToShortDateString()
                    });
                }
            }

            return View(result);
        }

        [HttpGet]
        [NoDirectAccess]
        public ActionResult MainActiveAdverts()
        {
            return View();
        }

        [HttpPost]
        [NoDirectAccess]
        public ActionResult MainActiveAdverts(AdvertSearchViewModel model, string frontendMakeId, string frontendModelId)
        {
            List<SmallSearchResultViewModel> result = new List<SmallSearchResultViewModel>();
            List<Advertisement> adverts = new List<Advertisement>();

            if (!string.IsNullOrEmpty(frontendModelId) && int.TryParse(frontendModelId, out int tempModel))
            {
                adverts = AdvertisementService
                    .GetAllActiveAdverts().Where(x => x.CarModelId == tempModel).ToList();
            }
            else if (!string.IsNullOrEmpty(frontendMakeId) && int.TryParse(frontendMakeId, out int tempMake))
            {
                adverts = AdvertisementService.GetAllActiveAdverts().Where(x => x.CarModel.ManufacturerId == tempMake)
                    .ToList();
            }
            else
            {
                adverts = AdvertisementService.GetAllActiveAdverts();
            }

            adverts =
                AdvertisementService.FilterPriceRange(model.MinPrice, model.MaxPrice, adverts);

            adverts =
                AdvertisementService.FilterYearRange(model.MinRegYear, model.MaxRegYear, adverts);

            foreach (Advertisement item in adverts)
            {
                result.Add(new SmallSearchResultViewModel
                {
                    Model = item.CarModel.Value,
                    Make = item.CarModel.CarManufacturer.Value,
                    Price = item.Price,
                    EngineSize = item.EngineSize.Size,
                    RegYear = item.RegYear,
                    Description = item.AdvertDescription,
                    Images = item.Images.Select(x => x.ImageData),
                    IsSold = item.IsSold,
                    AdvertisementId = item.Id
                });
            }
            return View(result);
        }

        [HttpGet]
        public ActionResult GetAvailableDates(int advertId)
        {
            List<CheckBoxList> resultDates = new List<CheckBoxList>();

            foreach (BookingAvailability item in BookingAvailabilityService.GetAllNotDeleted(x => x.Advertisement)
                .Where(x => x.AdvertId == advertId))
            {
                List<Booking> alreadyBookedDates = BookingService.GetAllNotDeleted(x => x.BookingAvailability)
                    .Where(x => x.AvailabilityId == item.Id).ToList();

                if (!alreadyBookedDates.Any())
                {
                    resultDates.Add(new CheckBoxList
                    {
                        Id = item.Id,
                        Display = item.AvailableDate.ToShortDateString()
                    });
                }
            }

            return Json(new { result = resultDates }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendBooking(SelectedAdvertViewModel model)
        {
            try
            {
                foreach (CheckBoxList item in model.BookingDates.Where(x => x.IsChecked))
                {
                    BookingService.Create(new Booking
                    {
                        AvailabilityId = item.Id,
                        BookingUserId = User.Identity.GetUserId<int>()
                    });
                }

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


            return Json(new { result = model.BookingDates.Count(x => x.IsChecked) }, JsonRequestBehavior.AllowGet);
        }
    }
}