using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Identity
{
    public class AppRoleStore<T> : IAppRoleStore<T> where T : AppRole
    {
        private readonly RoleStore<IdentityRole> _roleStore;
        private readonly CarAdvertiserContext _context;
        private bool _disposed;

        public AppRoleStore()
        {
            _context = new CarAdvertiserContext();
            _roleStore = new RoleStore<IdentityRole>(_context);
        }

        public AppRoleStore(ICarAdvertiserContext context)
        {
            _context = context as CarAdvertiserContext;
            _roleStore = new RoleStore<IdentityRole>(_context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _roleStore?.Dispose();
                _context?.Dispose();
                _disposed = true;
            }
        }

        public Task CreateAsync(T role)
        {
            _context.GetDbSet<T>().Add(role);
            _context.Configuration.ValidateOnSaveEnabled = false;
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(T role)
        {
            _context.GetDbSet<T>().Attach(role);
            _context.Entry(role).State = EntityState.Modified;
            _context.Configuration.ValidateOnSaveEnabled = false;
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(T role)
        {
            _context.GetDbSet<T>().Remove(role);
            _context.Configuration.ValidateOnSaveEnabled = false;
            return _context.SaveChangesAsync();
        }

        public Task<T> FindByIdAsync(int roleId)
        {
            return QueryableExtensions.FirstOrDefaultAsync(_context.GetDbSet<T>(), x => x.Id == roleId);
        }

        public Task<T> FindByIdAsync(string roleId)
        {
            return QueryableExtensions.FirstOrDefaultAsync<T>(_context.GetDbSet<T>(),
                x => x.Id.Equals(roleId.Trim().ToLower()));
        }

        public Task<T> FindByNameAsync(string roleName)
        {
            return QueryableExtensions.FirstOrDefaultAsync<T>(_context.GetDbSet<T>(),
                x => x.Name.ToLower().Equals(roleName.Trim().ToLower()));
        }
    }
}