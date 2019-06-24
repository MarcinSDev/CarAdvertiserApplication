using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.DTO;

namespace CarAdvertiser.BLL.Interfaces
{
    public interface IAdvertisementService : IService<Advertisement>
    {
        Advertisement FindById(int id, params Expression<Func<Advertisement, object>>[] includeProps);
        List<Advertisement> GetAllActiveAdvertsInYearRange(int? yearFrom, int? yearTo);
        List<Advertisement> GetAllActiveAdvertsInPriceRange(int? priceFrom, int? priceTo);
        List<Advertisement> FilterBHP(int? bhpFrom, IEnumerable<Advertisement> advertList);
        List<Advertisement> FilterMileage(int? maxMileage, IEnumerable<Advertisement> advertList);
        List<Advertisement> FilterPriceRange(int? priceFrom, int? priceTo, IEnumerable<Advertisement> advertList);
        List<Advertisement> FilterYearRange(int? yearFrom, int? yearTo, IEnumerable<Advertisement> advertList);
        List<Advertisement> GetAllSold();
        List<Advertisement> GetAllActiveAdverts();
        List<Advertisement> GetAllActivePremiumAdverts();
        List<Advertisement> GetRandomPremiumAdvert(int advertToShow);
        List<Advertisement> GetAllExpiredAdverts();
        List<DateTime> GenerateDates(DateTime startDate, DateTime endDate);
        List<Advertisement> GetAllByWanted(int? makeId, int? modelId, int? minYear, int? maxPrice);
    }
}
