namespace RecipeManagement.Services;

using System.Security.Claims;
using RecipeManagement.Domain.RolePermissions.Services;
using SharedKernel.Domain;
using RecipeManagement.Domain;
using HeimGuard;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

public class UserPolicyHandler : IUserPolicyHandler
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly ICurrentUserService _currentUserService;

    public UserPolicyHandler(IRolePermissionRepository rolePermissionRepository, ICurrentUserService currentUserService)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<IEnumerable<string>> GetUserPermissions()
    {
        var user = _currentUserService.User;
        if (user == null) throw new ArgumentNullException(nameof(user));

        var traditionalRoles = user.Claims
            .Where(c => c.Type is ClaimTypes.Role or "client_role")
            .Select(r => r.Value)
            .Distinct()
            .ToArray();

        var realmRoles = user.Claims
            .Where(c => c.Type is "realm_access")
            .Select(r => JsonConvert.DeserializeObject<RealmAccess>(r.Value))
            .SelectMany(x => x?.Roles);
            
        var roles = traditionalRoles.Concat(realmRoles).ToArray();
        
        if(roles.Length == 0)
            return Array.Empty<string>();

        // super admins can do everything
        if(roles.Contains(Roles.SuperAdmin))
            return Permissions.List();

        var permissions = await _rolePermissionRepository.Query()
            .Where(rp => roles.Contains(rp.Role))
            .Select(rp => rp.Permission)
            .Distinct()
            .ToArrayAsync();

        return await Task.FromResult(permissions);
    }

    private class RealmAccess
    {
        public string[] Roles { get; set; }
    }
}