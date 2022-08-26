namespace RecipeManagement.Domain.RolePermissions.Services;

using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Databases;
using RecipeManagement.Services;

public interface IRolePermissionRepository : IGenericRepository<RolePermission>
{
}

public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
{
    private readonly RecipesDbContext _dbContext;

    public RolePermissionRepository(RecipesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
