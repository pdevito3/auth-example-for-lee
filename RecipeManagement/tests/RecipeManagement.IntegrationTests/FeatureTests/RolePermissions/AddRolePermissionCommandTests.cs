namespace RecipeManagement.IntegrationTests.FeatureTests.RolePermissions;

using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using RecipeManagement.Domain.RolePermissions.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddRolePermissionCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_rolepermission_to_db()
    {
        // Arrange
        var fakeRolePermissionOne = new FakeRolePermissionForCreationDto().Generate();

        // Act
        var command = new AddRolePermission.Command(fakeRolePermissionOne);
        var rolePermissionReturned = await SendAsync(command);
        var rolePermissionCreated = await ExecuteDbContextAsync(db => db.RolePermissions
            .FirstOrDefaultAsync(r => r.Id == rolePermissionReturned.Id));

        // Assert
        rolePermissionReturned.Should().BeEquivalentTo(fakeRolePermissionOne, options =>
            options.ExcludingMissingMembers());
        rolePermissionCreated.Should().BeEquivalentTo(fakeRolePermissionOne, options =>
            options.ExcludingMissingMembers());
    }
}