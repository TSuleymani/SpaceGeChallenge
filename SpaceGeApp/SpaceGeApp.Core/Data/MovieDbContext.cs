using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SpaceGeApp.Core.Data.Models;
using SpaceGeApp.Core.Models;

namespace SpaceGeApp.Core.Data
{
    public class MovieDbContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable($"{nameof(User)}s");
            modelBuilder.Entity<Movie>().ToTable($"{nameof(Movie)}s");
            modelBuilder.Entity<WatchList>().ToTable($"{nameof(WatchList)}s");
            modelBuilder.Entity<MovieProvider>().ToTable($"{nameof(MovieProvider)}s");
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieProvider> MovieProviders { get; set; }
        public DbSet<WatchList> WatchList { get; set; }
    }
}
