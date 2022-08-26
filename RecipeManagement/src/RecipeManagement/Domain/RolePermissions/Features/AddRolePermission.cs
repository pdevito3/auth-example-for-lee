namespace RecipeManagement.Domain.RolePermissions.Features;

using RecipeManagement.Domain.RolePermissions.Services;
using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddRolePermission
{
    public class Command : IRequest<RolePermissionDto>
    {
        public readonly RolePermissionForCreationDto RolePermissionToAdd;

        public Command(RolePermissionForCreationDto rolePermissionToAdd)
        {
            RolePermissionToAdd = rolePermissionToAdd;
        }
    }

    public class Handler : IRequestHandler<Command, RolePermissionDto>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRolePermissionRepository rolePermissionRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<RolePermissionDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddRolePermission);

            var rolePermission = RolePermission.Create(request.RolePermissionToAdd);
            await _rolePermissionRepository.Add(rolePermission, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var rolePermissionAdded = await _rolePermissionRepository.GetById(rolePermission.Id, cancellationToken: cancellationToken);
            return _mapper.Map<RolePermissionDto>(rolePermissionAdded);
        }
    }
}