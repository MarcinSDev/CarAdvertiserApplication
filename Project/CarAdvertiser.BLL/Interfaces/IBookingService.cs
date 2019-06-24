using System.Collections.Generic;
using CarAdvertiser.DTO;

namespace CarAdvertiser.BLL.Interfaces
{
    public interface IBookingService : IService<Booking>
    {
        List<Booking> GetAll(int? advertId, int? availabilityId);
    }
}