namespace RecipeManagement.Domain.RolePermissions.DomainEvents;

public class RolePermissionCreated : DomainEvent
{
    public RolePermission RolePermission { get; set; } 
}
            