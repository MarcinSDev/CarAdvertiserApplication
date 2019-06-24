using System.Collections.Generic;
using CarAdvertiser.DTO;

namespace CarAdvertiser.BLL.Interfaces
{
    public interface IImageService : IService<Image>
    {
        Image GetByAdvertId(int advertId);
        List<Image> GetAllNotDeletedByAdvertId(int advertId);
    }
}
