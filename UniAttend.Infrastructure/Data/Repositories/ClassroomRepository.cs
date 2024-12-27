using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing classroom entities in the database.
    /// Provides CRUD operations and specialized queries for Classroom entities.
    /// </summary>
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassroomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Classroom?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Set<Classroom>()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<IEnumerable<Classroom>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Set<Classroom>()
                .ToListAsync(cancellationToken);

        public async Task<Classroom> AddAsync(Classroom entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<Classroom>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(Classroom entity, CancellationToken cancellationToken = default)
        {
            _context.Set<Classroom>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var classroom = await GetByIdAsync(id, cancellationToken);
            if (classroom != null)
            {
                _context.Set<Classroom>().Remove(classroom);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Set<Classroom>()
                .AnyAsync(c => c.Id == id, cancellationToken);

        public async Task<Classroom?> GetByReaderDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
            => await _context.Set<Classroom>()
                .FirstOrDefaultAsync(c => c.ReaderDeviceId == deviceId, cancellationToken);
    }
}