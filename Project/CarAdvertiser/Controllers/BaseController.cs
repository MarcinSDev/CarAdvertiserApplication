using CarAdvertiser.BLL.Interfaces;
using CarAdvertiser.BLL.Services;
using CarAdvertiser.DAL;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DAL.Repository;
using CarAdvertiser.DAL.UnitOfWork;
using CarAdvertiser.DTO;
using CarAdvertiser.DTO.ValueEntities;
using System.Web.Mvc;

namespace CarAdvertiser.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ICarAdvertiserContext _context;
        protected readonly IUnitOfWork Uow;

        protected readonly IRepositoryAppUserV2<AppUserV2> RepositoryAppUserV2;
        protected readonly IRepositoryAppRole<AppRole> RepositoryAppRole;

        private readonly IRepository<Image> _imageRepository;
        protected readonly IImageService ImageService;

        private readonly IRepository<Advertisement> _advertRepository;
        protected readonly IAdvertisementService AdvertisementService;

        private readonly IRepository<CarExtras> _extrasRepository;
        protected readonly IExtrasService ExtrasService;

        private readonly IRepository<AdditionalEquipment> _equipmentRepository;
        protected readonly IService<AdditionalEquipment> EquipmentService;

        private readonly IRepository<BodyType> _bodyTypeRepository;
        protected readonly IService<BodyType> BodyTypeService;

        private readonly IRepository<CarManufacturer> _manufacturerRepository;
        protected readonly IService<CarManufacturer> ManufacturerService;

        private readonly IRepository<CarModel> _modelRepository;
        protected readonly IService<CarModel> ModelService;

        private readonly IRepository<Colour> _colourRepository;
        protected readonly IService<Colour> ColourService;

        private readonly IRepository<DoorAmount> _doorRepository;
        protected readonly IService<DoorAmount> DoorService;

        private readonly IRepository<DriveTrain> _driveTrainRepository;
        protected readonly IService<DriveTrain> DriveTrainService;

        private readonly IRepository<EngineSize> _engineSizeRepository;
        protected readonly IService<EngineSize> EngineSizeService;

        private readonly IRepository<FuelType> _fuelTypeRepository;
        protected readonly IService<FuelType> FuelTypeService;

        private readonly IRepository<ListingTime> _listingTimeRepository;
        protected readonly IService<ListingTime> ListingTimeService;

        private readonly IRepository<SeatAmount> _seatAmountRepository;
        protected readonly IService<SeatAmount> SeatAmountService;

        private readonly IRepository<Transmission> _transmissionRepository;
        protected readonly IService<Transmission> TransmissionService;

        private readonly IRepository<Messages> _messagesRepository;
        protected readonly IMessageService MessagesService;

        private readonly IRepository<Booking> _bookingRepository;
        protected readonly IService<Booking> BookingService;

        private readonly IRepository<BookingAvailability> _bookingAvailabilityRepository;
        protected readonly IService<BookingAvailability> BookingAvailabilityService;

        private readonly IRepository<WantedCar> _wantedCarRepository;
        protected readonly IWantedCarService WantedCarService;




        protected BaseController()
        {
            _context = new CarAdvertiserContext();
            Uow = new UnitOfWork(_context);

            RepositoryAppUserV2 = new RepositoryAppUserV2<AppUserV2>(_context);

            RepositoryAppRole = new RepositoryAppRole<AppRole>(_context);

            _imageRepository = new Repository<Image>(_context);
            ImageService = new ImageService(_imageRepository, Uow);

            _advertRepository = new Repository<Advertisement>(_context);
            AdvertisementService = new AdvertisementService(_advertRepository, Uow);

            _extrasRepository = new Repository<CarExtras>(_context);
            ExtrasService = new ExtrasService(_extrasRepository, Uow);

            _equipmentRepository = new Repository<AdditionalEquipment>(_context);
            EquipmentService = new Service<AdditionalEquipment>(_equipmentRepository, Uow);

            _bodyTypeRepository = new Repository<BodyType>(_context);
            BodyTypeService = new Service<BodyType>(_bodyTypeRepository, Uow);

            _manufacturerRepository = new Repository<CarManufacturer>(_context);
            ManufacturerService = new Service<CarManufacturer>(_manufacturerRepository, Uow);

            _modelRepository = new Repository<CarModel>(_context);
            ModelService = new Service<CarModel>(_modelRepository, Uow);

            _colourRepository = new Repository<Colour>(_context);
            ColourService = new Service<Colour>(_colourRepository, Uow);

            _doorRepository = new Repository<DoorAmount>(_context);
            DoorService = new Service<DoorAmount>(_doorRepository, Uow);

            _driveTrainRepository = new Repository<DriveTrain>(_context);
            DriveTrainService = new Service<DriveTrain>(_driveTrainRepository, Uow);

            _engineSizeRepository = new Repository<EngineSize>(_context);
            EngineSizeService = new Service<EngineSize>(_engineSizeRepository, Uow);

            _fuelTypeRepository = new Repository<FuelType>(_context);
            FuelTypeService = new Service<FuelType>(_fuelTypeRepository, Uow);

            _listingTimeRepository = new Repository<ListingTime>(_context);
            ListingTimeService = new Service<ListingTime>(_listingTimeRepository, Uow);

            _seatAmountRepository = new Repository<SeatAmount>(_context);
            SeatAmountService = new Service<SeatAmount>(_seatAmountRepository, Uow);

            _transmissionRepository = new Repository<Transmission>(_context);
            TransmissionService = new Service<Transmission>(_transmissionRepository, Uow);

            _messagesRepository = new Repository<Messages>(_context);
            MessagesService = new MessageService(_messagesRepository, Uow);

            _bookingRepository = new Repository<Booking>(_context);
            BookingService = new Service<Booking>(_bookingRepository, Uow);

            _bookingAvailabilityRepository = new Repository<BookingAvailability>(_context);
            BookingAvailabilityService = new Service<BookingAvailability>(_bookingAvailabilityRepository, Uow);

            _wantedCarRepository = new Repository<WantedCar>(_context);
            WantedCarService = new WantedCarService(_wantedCarRepository, Uow);
        }
    }
}