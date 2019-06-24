using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarAdvertiser.DTO;
using CarAdvertiser.DTO.ValueEntities;
using Newtonsoft.Json;

namespace CarAdvertiser.Controllers
{
    [Authorize]
    public class DatabaseController : BaseController
    {
        public ActionResult InitializeDatabase()
        {
            if (!ManufacturerService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Manufacturers.json")))
                {
                    var manufacturers = JsonConvert.DeserializeObject<List<TempValue>>(StreamToString(r));
                    foreach (TempValue item in manufacturers)
                    {
                        ManufacturerService.Create(new CarManufacturer
                        {
                            Value = item.Value
                        });
                        ManufacturerService.Save();
                    }
                }
            }

            if (!ModelService.GetAllNotDeleted().Any())
            {
                var manufacturers = ManufacturerService.GetAllNotDeleted().ToList();
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Models.json")))
                {
                    var models = JsonConvert.DeserializeObject<List<TempModel>>(StreamToString(r));
                    foreach (TempModel item in models)
                    {
                        var manufacturer = manufacturers.FirstOrDefault(x => x.Value.Equals(item.CarManufacturer.Value));
                        ModelService.Create(new CarModel
                        {
                            ManufacturerId = manufacturer.Id,
                            Value = item.Value
                        });
                        ModelService.Save();
                    }
                }
            }

            if (!ColourService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Colours.json")))
                {
                    var colours = JsonConvert.DeserializeObject<List<TempValue>>(StreamToString(r));
                    foreach (TempValue item in colours)
                    {
                        ColourService.Create(new Colour
                        {
                            Value = item.Value
                        });
                        ColourService.Save();
                    }
                }
            }

            if (!TransmissionService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Transmissions.json")))
                {
                    var transmissions = JsonConvert.DeserializeObject<List<TempValue>>(StreamToString(r));
                    foreach (TempValue item in transmissions)
                    {
                        TransmissionService.Create(new Transmission
                        {
                            Value = item.Value
                        });
                        TransmissionService.Save();
                    }
                }
            }

            if (!BodyTypeService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Bodies.json")))
                {
                    var bodies = JsonConvert.DeserializeObject<List<TempValue>>(StreamToString(r));
                    foreach (TempValue item in bodies)
                    {
                        BodyTypeService.Create(new BodyType
                        {
                            Value = item.Value
                        });
                        BodyTypeService.Save();
                    }
                }
            }

            if (!EngineSizeService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/EngineSizes.json")))
                {
                    var engines = JsonConvert.DeserializeObject<List<TempEngine>>(StreamToString(r));
                    foreach (TempEngine item in engines)
                    {
                        EngineSizeService.Create(new EngineSize
                        {
                            Size = item.Size
                        });
                        EngineSizeService.Save();
                    }
                }
            }

            if (!FuelTypeService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Fuels.json")))
                {
                    var fuels = JsonConvert.DeserializeObject<List<TempValue>>(StreamToString(r));
                    foreach (TempValue item in fuels)
                    {
                        FuelTypeService.Create(new FuelType
                        {
                            Value = item.Value
                        });
                        FuelTypeService.Save();
                    }
                }
            }

            if (!DriveTrainService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Drives.json")))
                {
                    var drives = JsonConvert.DeserializeObject<List<TempValue>>(StreamToString(r));
                    foreach (TempValue item in drives)
                    {
                        DriveTrainService.Create(new DriveTrain
                        {
                            Value = item.Value
                        });
                        DriveTrainService.Save();
                    }
                }
            }

            if (!EquipmentService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/AdditionalEqu.json")))
                {
                    var equipments = JsonConvert.DeserializeObject<List<TempValue>>(StreamToString(r));
                    foreach (TempValue item in equipments)
                    {
                        EquipmentService.Create(new AdditionalEquipment
                        {
                            Value = item.Value
                        });
                        EquipmentService.Save();
                    }
                }
            }

            if (!SeatAmountService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Seats.json")))
                {
                    var seats = JsonConvert.DeserializeObject<List<TempSeat>>(StreamToString(r));
                    foreach (TempSeat item in seats)
                    {
                        SeatAmountService.Create(new SeatAmount
                        {
                            NumberOfSeats = item.NumberOfSeats
                        });
                        SeatAmountService.Save();
                    }
                }
            }

            if (!DoorService.GetAllNotDeleted().Any())
            {
                using (StreamReader r = new StreamReader(Server.MapPath("~/App_Data/Doors.json")))
                {
                    var doors = JsonConvert.DeserializeObject<List<TempDoor>>(StreamToString(r));
                    foreach (TempDoor item in doors)
                    {
                        DoorService.Create(new DoorAmount
                        {
                            NumberOfDoors = item.NumberOfDoors
                        });
                        DoorService.Save();
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Purge()
        {
            ListingTimeService.PurgeAll();
            ImageService.PurgeAll();
            BookingService.PurgeAll();
            BookingAvailabilityService.PurgeAll();
            MessagesService.PurgeAll();
            WantedCarService.PurgeAll();
            ExtrasService.PurgeAll();

            AdvertisementService.PurgeAll();

            ModelService.PurgeAll();
            ManufacturerService.PurgeAll();

            EngineSizeService.PurgeAll();
            FuelTypeService.PurgeAll();
            ColourService.PurgeAll();
            SeatAmountService.PurgeAll();
            BodyTypeService.PurgeAll();
            DriveTrainService.PurgeAll();
            TransmissionService.PurgeAll();
            DoorService.PurgeAll();
            EquipmentService.PurgeAll();

            Uow.Commit();

            return RedirectToAction("Index", "Home");
        }

        private string StreamToString(StreamReader stream)
        {
            return stream.ReadToEnd();
        }

        internal class TempValue
        {
            public string Value { get; set; }
        }

        internal class TempEngine
        {
            public decimal Size { get; set; }
        }

        internal class TempSeat
        {
            public decimal NumberOfSeats { get; set; }
        }

        internal class TempDoor
        {
            public int NumberOfDoors { get; set; }

        }

        internal class TempModel
        {
            public string Value { get; set; }
            public TempValue CarManufacturer { get; set; }
        }
    }
}