using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UniAttend.Core.Interfaces.Repositories;
using UniAttend.Core.Enums;
using UniAttend.Core.Entities;
using UniAttend.Application.Features.Users.DTOs;

namespace UniAttend.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

                public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // Start with IQueryable to build query
            var query = _unitOfWork.Users.GetQueryable();
        
            // Include necessary navigation properties with explicit joins
            query = query
                .Include(u => u.Professor)
                    .ThenInclude(p => p!.Department)
                .Include(u => u.Student)
                    .ThenInclude(s => s!.Department);
        
            // Single user by ID
            if (request.Id.HasValue)
                query = query.Where(u => u.Id == request.Id.Value);
        
            // Apply filters
            if (request.Role.HasValue)
                query = query.Where(u => u.Role == request.Role.Value);
        
            if (request.DepartmentId.HasValue)
            {
                query = query.Where(u =>
                    (u.Role == UserRole.Professor && u.Professor != null && u.Professor.DepartmentId == request.DepartmentId) ||
                    (u.Role == UserRole.Student && u.Student != null && u.Student.DepartmentId == request.DepartmentId));
            }
        
            if (request.IsActive.HasValue)
                query = query.Where(u => u.IsActive == request.IsActive.Value);
        
            var users = await query.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}