using System.Collections.Generic;
using CarAdvertiser.BLL.Interfaces;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using System.Linq;

namespace CarAdvertiser.BLL.Services
{
    public class ImageService : Service<Image>, IImageService
    {
        public ImageService(IRepository<Image> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }

        public Image GetByAdvertId(int advertId)
        {
            return GetAllNotDeleted(x => x.Advertisement).FirstOrDefault(x => x.AdvertisementId == advertId);
        }

        public List<Image> GetAllNotDeletedByAdvertId(int advertId)
        {
            return GetAllNotDeleted(x => x.Advertisement).Where(x => x.AdvertisementId == advertId).ToList();
        }
    }
}
