namespace RecipeManagement.Domain.RolePermissions.Mappings;

using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Domain.RolePermissions;
using Mapster;

public class RolePermissionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RolePermissionDto, RolePermission>()
            .TwoWays();
        config.NewConfig<RolePermissionForCreationDto, RolePermission>()
            .TwoWays();
        config.NewConfig<RolePermissionForUpdateDto, RolePermission>()
            .TwoWays();
    }
}