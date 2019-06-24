using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using CarAdvertiser.DTO.BaseEntities;
using CarAdvertiser.DTO.ValueEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarAdvertiser.DAL
{
    public class CarAdvertiserContext : DbContext, ICarAdvertiserContext
    {
        public CarAdvertiserContext() : base("name=CarAdvertiser")
        {
            Database.CreateIfNotExists();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public IDbSet<T> GetDbSet<T>() where T : BaseEntity
        {
            return base.Set<T>();
        }

        public new DbEntityEntry<T> Entry<T>(T entity) where T : BaseEntity
        {
            return base.Entry(entity);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<AppUserV2>().ToTable("AspNetUsersV2");
            modelBuilder.Entity<AppRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<AppUserRole>().ToTable("AspNetUserRoles");

            modelBuilder.Entity<BodyType>();
            modelBuilder.Entity<CarManufacturer>();
            modelBuilder.Entity<CarModel>();
            modelBuilder.Entity<Colour>();
            modelBuilder.Entity<DoorAmount>();
            modelBuilder.Entity<DriveTrain>();
            modelBuilder.Entity<EngineSize>();
            modelBuilder.Entity<FuelType>();
            modelBuilder.Entity<SeatAmount>();
            modelBuilder.Entity<Transmission>();
            modelBuilder.Entity<Advertisement>();
            modelBuilder.Entity<Image>();
            modelBuilder.Entity<CarExtras>();
            modelBuilder.Entity<WantedCar>();
            modelBuilder.Entity<BookingAvailability>();
            modelBuilder.Entity<Booking>();
            modelBuilder.Entity<ListingTime>();
            modelBuilder.Entity<AdditionalEquipment>();
            modelBuilder.Entity<Messages>();

        }

        public static CarAdvertiserContext Create()
        {
            return new CarAdvertiserContext();
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
        
        private void AddTimestamps()
        {
            IEnumerable<DbEntityEntry> entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (DbEntityEntry entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedDate = DateTime.Now;
                    ((BaseEntity)entity.Entity).CreateUser = CurrentUser;
                }

                ((BaseEntity)entity.Entity).LastModificationDate = DateTime.Now;
                ((BaseEntity)entity.Entity).LastModificationUser = CurrentUser;
            }
        }

        public static string CurrentUser => !string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name)
            ? Thread.CurrentPrincipal.Identity.Name
            : "Anonymous";

        public static string CurrentSamAccountName
        {
            get
            {
                string username = CurrentUser;
                if (username.Contains(@"\"))
                {
                    int lastIndex = username.LastIndexOf(@"\");
                    username = username.Substring(lastIndex + 1);
                }

                return username;
            }
        }
    }
}
