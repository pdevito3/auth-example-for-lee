namespace RecipeManagement.Domain.RolePermissions.Features;

using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Domain.RolePermissions.Validators;
using RecipeManagement.Domain.RolePermissions.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateRolePermission
{
    public class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly RolePermissionForUpdateDto RolePermissionToUpdate;

        public Command(Guid rolePermission, RolePermissionForUpdateDto newRolePermissionData)
        {
            Id = rolePermission;
            RolePermissionToUpdate = newRolePermissionData;
        }
    }

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRolePermissionRepository rolePermissionRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateRolePermission);

            var rolePermissionToUpdate = await _rolePermissionRepository.GetById(request.Id, cancellationToken: cancellationToken);

            rolePermissionToUpdate.Update(request.RolePermissionToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}