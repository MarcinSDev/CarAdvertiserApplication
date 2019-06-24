using CarAdvertiser.DTO;
using System.Collections.Generic;

namespace CarAdvertiser.BLL.Interfaces
{
    public interface IExtrasService : IService<CarExtras>
    {
        List<CarExtras> GetAllByAdvertId(int advertId);
    }
}
