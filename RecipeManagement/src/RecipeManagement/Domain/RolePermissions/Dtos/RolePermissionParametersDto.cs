namespace RecipeManagement.Domain.RolePermissions.Dtos;

using SharedKernel.Dtos;

public class RolePermissionParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
