namespace RecipeManagement.IntegrationTests.FeatureTests.RolePermissions;

using RecipeManagement.Domain.RolePermissions.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.RolePermission;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.RolePermissions.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class RolePermissionListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_rolepermission_list()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var fakeRolePermissionTwo = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var queryParameters = new RolePermissionParametersDto();

        await InsertAsync(fakeRolePermissionOne, fakeRolePermissionTwo);

        // Act
        var query = new GetRolePermissionList.Query(queryParameters);
        var rolePermissions = await SendAsync(query);

        // Assert
        rolePermissions.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}