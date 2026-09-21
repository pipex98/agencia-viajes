using AgenciaViajes.Application.Interfaces;
using AgenciaViajes.Infrastructure.Data;

namespace AgenciaViajes.Infrastructure
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
