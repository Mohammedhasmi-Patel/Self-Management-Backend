

using Microsoft.EntityFrameworkCore.Storage;
using SelfManagement.Application.RepositoryInterface.Common;
using SelfManagement.Infrastructure.Database;

namespace SelfManagement.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
           _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("No active transaction.");
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
           if (_transaction == null)
            return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public Task<int> SaveChangesAsync()
        {
           return _context.SaveChangesAsync();
        }
    }
}
