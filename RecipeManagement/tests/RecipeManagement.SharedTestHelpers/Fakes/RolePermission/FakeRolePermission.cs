namespace RecipeManagement.SharedTestHelpers.Fakes.RolePermission;

using AutoBogus;
using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.RolePermissions.Dtos;

public class FakeRolePermission
{
    public static RolePermission Generate(RolePermissionForCreationDto rolePermissionForCreationDto)
    {
        return RolePermission.Create(rolePermissionForCreationDto);
    }

    public static RolePermission Generate()
    {
        return RolePermission.Create(new FakeRolePermissionForCreationDto().Generate());
    }
}