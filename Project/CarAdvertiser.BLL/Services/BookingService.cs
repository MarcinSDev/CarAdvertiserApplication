using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CarAdvertiser.BLL.Interfaces;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;

namespace CarAdvertiser.BLL.Services
{
    public class BookingService : Service<Booking>, IBookingService
    {
        public BookingService(IRepository<Booking> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }

        public override IEnumerable<Booking> GetAll()
        {
            //return base.GetAll();
            return Repository.GetAll().Where(x => !x.IsDeleted);
        }

        public override IEnumerable<Booking> GetAll(params Expression<Func<Booking, object>>[] includeProps)
        {
            return Repository.GetAll(includeProps).Where(x => !x.IsDeleted);
        }

        public List<Booking> GetAll(int? advertId, int? availabilityId)
        {
            IEnumerable<Booking> result = GetAll(x => x.BookingAvailability.Advertisement);

            if (availabilityId.HasValue)
            {
                result = result.Where(x => x.AvailabilityId == availabilityId);
            }

            if (advertId.HasValue)
            {
                result = result.Where(x => x.BookingAvailability.AdvertId == advertId);
            }

            return result.ToList();
        }
    }
}