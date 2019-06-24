using CarAdvertiser.DTO;
using System.Collections.Generic;

namespace CarAdvertiser.BLL.Interfaces
{
    public interface IWantedCarService : IService<WantedCar>
    {
        List<WantedCar> GetAllByPriceMileage(int maxPrice, int minYear);
    }
}