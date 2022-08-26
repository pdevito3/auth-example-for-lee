namespace RecipeManagement.Domain.RolePermissions.Validators;

using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentValidation;

public class RolePermissionForManipulationDtoValidator<T> : AbstractValidator<T> where T : RolePermissionForManipulationDto
{
    public RolePermissionForManipulationDtoValidator()
    {
        RuleFor(rp => rp.Permission)
            .Must(BeAnExistingPermission)
            .WithMessage("Please use a valid permission.");
        RuleFor(rp => rp.Role)
            .Must(BeAnExistingRole)
            .WithMessage("Please use a valid role.");
    }
    
    private static bool BeAnExistingPermission(string permission)
    {
        return Permissions.List().Contains(permission, StringComparer.InvariantCultureIgnoreCase);
    }

    private static bool BeAnExistingRole(string role)
    {
        return Roles.List().Contains(role, StringComparer.InvariantCultureIgnoreCase);
    }
}