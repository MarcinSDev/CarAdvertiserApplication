using System.Collections.Generic;
using System.Linq;
using CarAdvertiser.BLL.Interfaces;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;

namespace CarAdvertiser.BLL.Services
{
    public class WantedCarService : Service<WantedCar>, IWantedCarService
    {
        public WantedCarService(IRepository<WantedCar> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }

        public List<WantedCar> GetAllByPriceMileage(int maxPrice, int minYear)
        {
            return GetAllNotDeleted().Where(x => x.MaxPrice <= maxPrice && x.MinimumYear >= minYear).ToList();
        }
    }
}