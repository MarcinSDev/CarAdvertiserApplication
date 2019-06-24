using CarAdvertiser.BLL.Interfaces;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CarAdvertiser.BLL.Services
{
    public class AdvertisementService : Service<Advertisement>, IAdvertisementService
    {
        public AdvertisementService(IRepository<Advertisement> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }

        public override IEnumerable<Advertisement> GetAll()
        {
            return Repository.GetAll().Where(x => x.AdvertOpenDate <= DateTime.Now && x.AdvertCloseDate >= DateTime.Now);
        }

        public override IEnumerable<Advertisement> GetAll(params Expression<Func<Advertisement, object>>[] includeProps)
        {
            return Repository.GetAll(includeProps).Where(
                x => x.AdvertOpenDate <= DateTime.Now &&
                     x.AdvertCloseDate >= DateTime.Now);
        }

        public override IEnumerable<Advertisement> GetAllNotDeleted()
        {
            return Repository.GetAll().Where(
                x => !x.IsDeleted &&
                     x.AdvertOpenDate <= DateTime.Now &&
                     x.AdvertCloseDate >= DateTime.Now);
        }

        public override IEnumerable<Advertisement> GetAllNotDeleted(params Expression<Func<Advertisement, object>>[] includeProps)
        {
            return Repository.GetAll(includeProps).Where(
                x => !x.IsDeleted &&
                     x.AdvertOpenDate <= DateTime.Now &&
                     x.AdvertCloseDate >= DateTime.Now);
        }

        public override IEnumerable<Advertisement> GetAllDeleted()
        {
            return Repository.GetAll().Where(
                x => x.IsDeleted &&
                     x.AdvertOpenDate <= DateTime.Now &&
                     x.AdvertCloseDate >= DateTime.Now);
        }

        public override IEnumerable<Advertisement> GetAllDeleted(params Expression<Func<Advertisement, object>>[] includeProps)
        {
            return Repository.GetAll(includeProps).Where(x => x.IsDeleted && x.AdvertOpenDate <= DateTime.Now && x.AdvertCloseDate >= DateTime.Now);
        }

        public Advertisement FindById(int id, params Expression<Func<Advertisement, object>>[] includeProps)
        {
            return Repository.GetById(id, includeProps);
        }

        public List<Advertisement> GetAllActiveAdvertsInYearRange(int? yearFrom, int? yearTo)
        {
            IEnumerable<Advertisement> result = GetAllNotDeleted(
                    x => x.CarModel.CarManufacturer,
                    x => x.EngineSize,
                    x => x.BodyType,
                    x => x.Transmission,
                    x => x.SeatAmount,
                    x => x.DriveTrain,
                    x => x.FuelType,
                    x => x.DoorAmount,
                    x => x.Colour,
                    x => x.Images,
                    x => x.AppUser)
                .ToList();

            if (yearFrom.HasValue)
            {
                result = result.Where(x => x.RegYear >= yearFrom.Value);
            }

            if (yearTo.HasValue)
            {
                result = result.Where(x => x.RegYear <= yearTo.Value);
            }

            return result.ToList();
        }

        public List<Advertisement> GetAllActiveAdvertsInPriceRange(int? priceFrom, int? priceTo)
        {
            IEnumerable<Advertisement> result = GetAllNotDeleted(
                    x => x.CarModel.CarManufacturer,
                    x => x.EngineSize,
                    x => x.BodyType,
                    x => x.Transmission,
                    x => x.SeatAmount,
                    x => x.DriveTrain,
                    x => x.FuelType,
                    x => x.DoorAmount,
                    x => x.Colour,
                    x => x.Images,
                    x => x.AppUser)
                .ToList();

            if (priceFrom.HasValue)
            {
                result = result.Where(x => x.RegYear >= priceFrom.Value);
            }

            if (priceTo.HasValue)
            {
                result = result.Where(x => x.RegYear <= priceTo.Value);
            }

            return result.ToList();
        }

        public List<Advertisement> FilterBHP(int? bhpFrom, IEnumerable<Advertisement> advertList)
        {
            if (bhpFrom.HasValue && bhpFrom != 0)
            {
                advertList = advertList.Where(x => x.HorsePower >= bhpFrom.Value);
            }
            return advertList.ToList();
        }

        public List<Advertisement> FilterMileage(int? maxMileage, IEnumerable<Advertisement> advertList)
        {
            if (maxMileage.HasValue && maxMileage != 0)
            {
                advertList = advertList.Where(x => x.CurrentMileage <= maxMileage.Value);
            }
            return advertList.ToList();
        }

        public List<Advertisement> FilterPriceRange(int? priceFrom, int? priceTo, IEnumerable<Advertisement> advertList)
        {
            if (priceFrom.HasValue && priceFrom != 0)
            {
                advertList = advertList.Where(x => x.Price >= priceFrom.Value);
            }

            if (priceTo.HasValue && priceTo != 0)
            {
                advertList = advertList.Where(x => x.Price <= priceTo.Value);
            }

            return advertList.ToList();
        }

        public List<Advertisement> FilterYearRange(int? yearFrom, int? yearTo, IEnumerable<Advertisement> advertList)
        {
            if (yearFrom.HasValue && yearFrom != 0)
            {
                advertList = advertList.Where(x => x.RegYear >= yearFrom.Value);
            }

            if (yearTo.HasValue && yearTo != 0)
            {
                advertList = advertList.Where(x => x.RegYear <= yearTo.Value);
            }

            return advertList.ToList();
        }

        public List<Advertisement> GetAllSold()
        {
            return GetAll(
                x => x.CarModel.CarManufacturer,
                x => x.EngineSize,
                x => x.BodyType,
                x => x.Transmission,
                x => x.SeatAmount,
                x => x.DriveTrain,
                x => x.FuelType,
                x => x.DoorAmount,
                x => x.Colour,
                x => x.Images,
                x => x.AppUser)
                .Where(x => x.IsSold).ToList();
        }

        public List<Advertisement> GetAllActiveAdverts()
        {
            return GetAll(
                    x => x.CarModel.CarManufacturer,
                    x => x.EngineSize,
                    x => x.BodyType,
                    x => x.Transmission,
                    x => x.SeatAmount,
                    x => x.DriveTrain,
                    x => x.FuelType,
                    x => x.DoorAmount,
                    x => x.Colour,
                    x => x.Images,
                    x => x.AppUser)
                .ToList();
        }

        public List<Advertisement> GetAllActivePremiumAdverts()
        {
            return GetAll(
                    x => x.CarModel.CarManufacturer,
                    x => x.EngineSize,
                    x => x.BodyType,
                    x => x.Transmission,
                    x => x.SeatAmount,
                    x => x.DriveTrain,
                    x => x.FuelType,
                    x => x.DoorAmount,
                    x => x.Colour,
                    x => x.Images,
                    x => x.AppUser)
                .Where(x => x.IsPremium)
                .ToList();
        }

        public List<Advertisement> GetRandomPremiumAdvert(int advertsToDisplay)
        {
            List<Advertisement> adverts = GetAllActivePremiumAdverts();
            List<Advertisement> result = new List<Advertisement>();

            Random rand = new Random();
            for (int i = 0; i < advertsToDisplay; i++)
            {
                int position = rand.Next(0, adverts.Count());
                if (adverts.Any())
                {
                    Advertisement taken = adverts.ElementAt(position);
                    result.Add(taken);
                    adverts.RemoveAt(position);
                }
            }

            return result;
        }

        public List<Advertisement> GetAllExpiredAdverts()
        {
            return Repository.GetAll(
                 x => x.CarModel.CarManufacturer,
                 x => x.EngineSize,
                 x => x.BodyType,
                 x => x.Transmission,
                 x => x.SeatAmount,
                 x => x.DriveTrain,
                 x => x.FuelType,
                 x => x.DoorAmount,
                 x => x.Colour,
                 x => x.Images,
                 x => x.AppUser).Where(x => x.AdvertCloseDate < DateTime.Now).ToList();
        }

        public List<DateTime> GenerateDates(DateTime startDate, DateTime endDate)
        {
            var datesRange = new List<DateTime>();
            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                datesRange.Add(dt);
            }

            return datesRange;
        }

        public List<Advertisement> GetAllByWanted(int? makeId, int? modelId, int? minYear, int? maxPrice)
        {
            var result = GetAllNotDeleted(x => x.CarModel).ToList();

            if (makeId.HasValue && makeId.Value > 0)
            {
                result = result.Where(x => x.CarModel.ManufacturerId == makeId.Value).ToList();
            }

            if (modelId.HasValue && modelId.Value > 0)
            {
                result = result.Where(x => x.CarModelId == modelId.Value).ToList();
            }

            if (minYear.HasValue && minYear.Value >= 1960)
            {
                result = result.Where(x => x.RegYear >= minYear.Value).ToList();
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                result = result.Where(x => x.Price <= maxPrice.Value).ToList();
            }

            return result;
        }
    }
}
