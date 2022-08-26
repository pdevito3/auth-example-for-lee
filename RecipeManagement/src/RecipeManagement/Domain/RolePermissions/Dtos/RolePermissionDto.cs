namespace RecipeManagement.Domain.RolePermissions.Dtos;

public class RolePermissionDto 
{
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string Permission { get; set; }
}
