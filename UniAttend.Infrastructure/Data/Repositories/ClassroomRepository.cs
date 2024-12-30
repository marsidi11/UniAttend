using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Entities;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Infrastructure.Data.Repositories.Base;

namespace UniAttend.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository implementation for managing classroom entities in the database.
    /// Provides CRUD operations and specialized queries for Classroom entities.
    /// </summary>
    public class ClassroomRepository : BaseRepository<Classroom>, IClassroomRepository  
    {
        public ClassroomRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Classroom?> GetByReaderDeviceIdAsync(
            string deviceId, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(c => c.ReaderDeviceId == deviceId, 
                    cancellationToken);
        }
    }
}