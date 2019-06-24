using CarAdvertiser.BLL.Interfaces;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CarAdvertiser.BLL.Services
{
    public class ExtrasService : Service<CarExtras>, IExtrasService
    {
        public ExtrasService(IRepository<CarExtras> repository, IUnitOfWork uow) : base(repository, uow)
        {
        }
        
        public List<CarExtras> GetAllByAdvertId(int advertId)
        {
            return GetAllNotDeleted(x => x.Advertisement, x => x.AdditionalEquipment)
                .Where(x => x.ExtrasAdvertId == advertId).ToList();
        }
    }
}
