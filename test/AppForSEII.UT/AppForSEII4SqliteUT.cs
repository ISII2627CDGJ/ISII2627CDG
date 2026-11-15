using AppForSEII.API.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace AppForSEII.UT
{
    public class AppForSEII4SqliteUT : IDisposable
    {
        protected readonly DbConnection _connection;
        protected readonly ApplicationDbContext _context;
        protected readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        protected ApplicationDbContext CreateContext() => new(_contextOptions);

        public AppForSEII4SqliteUT()
        {
            
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

           
            _context = new ApplicationDbContext(_contextOptions);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}