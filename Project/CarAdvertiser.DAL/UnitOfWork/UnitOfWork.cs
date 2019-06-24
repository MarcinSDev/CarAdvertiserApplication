using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.DAL.Interfaces;

namespace CarAdvertiser.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ICarAdvertiserContext _context;
        private bool _disposed;

        public UnitOfWork(ICarAdvertiserContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }
    }
}
