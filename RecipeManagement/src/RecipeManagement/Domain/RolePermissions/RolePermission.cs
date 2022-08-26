namespace RecipeManagement.Domain.RolePermissions;

using SharedKernel.Exceptions;
using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.Domain.RolePermissions.Validators;
using RecipeManagement.Domain.RolePermissions.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;


public class RolePermission : BaseEntity
{
    public virtual string Role { get; private set; }

    public virtual string Permission { get; private set; }


    public static RolePermission Create(RolePermissionForCreationDto rolePermissionForCreationDto)
    {
        new RolePermissionForCreationDtoValidator().ValidateAndThrow(rolePermissionForCreationDto);

        var newRolePermission = new RolePermission();

        newRolePermission.Role = rolePermissionForCreationDto.Role;
        newRolePermission.Permission = rolePermissionForCreationDto.Permission;

        newRolePermission.QueueDomainEvent(new RolePermissionCreated(){ RolePermission = newRolePermission });
        
        return newRolePermission;
    }

    public void Update(RolePermissionForUpdateDto rolePermissionForUpdateDto)
    {
        new RolePermissionForUpdateDtoValidator().ValidateAndThrow(rolePermissionForUpdateDto);

        Role = rolePermissionForUpdateDto.Role;
        Permission = rolePermissionForUpdateDto.Permission;

        QueueDomainEvent(new RolePermissionUpdated(){ Id = Id });
    }
    
    protected RolePermission() { } // For EF + Mocking
}