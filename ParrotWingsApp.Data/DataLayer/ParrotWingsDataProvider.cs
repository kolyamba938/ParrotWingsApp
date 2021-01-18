using Microsoft.EntityFrameworkCore;
using ParrotWingsApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWingsApp.Data.DataLayer
{
    internal class ParrotWingsDataProvider : IDisposable
    {
        private readonly ParrotWingsContext _context;

        public ParrotWingsDataProvider(String connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ParrotWingsContext>();

            var options = optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString)
                .Options;

            _context = new ParrotWingsContext(options);
        }

        public Repository<T> GetRepository<T>() where T : BaseEntity
        {
            return new Repository<T>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
