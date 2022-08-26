namespace RecipeManagement.FunctionalTests.FunctionalTests.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using Microsoft.AspNetCore.JsonPatch;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Bogus;

public class PartialRecipeUpdateTests : TestBase
{
    private readonly Faker _faker = new Faker();

    [Test]
    public async Task patch_recipe_returns_nocontent_when_using_valid_patchdoc_on_existing_entity_and__valid_auth_credentials()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        var patchDoc = new JsonPatchDocument<RecipeForUpdateDto>();
        patchDoc.Replace(r => r.Title, _faker.Lorem.Word());

        _client.AddAuth(new[] {Roles.SuperAdmin});
        await InsertAsync(fakeRecipe);

        // Act
        var route = ApiRoutes.Recipes.Patch.Replace(ApiRoutes.Recipes.Id, fakeRecipe.Id.ToString());
        var result = await _client.PatchJsonRequestAsync(route, patchDoc);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task patch_recipe_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        var patchDoc = new JsonPatchDocument<RecipeForUpdateDto>();
        patchDoc.Replace(r => r.Title, _faker.Lorem.Word());

        await InsertAsync(fakeRecipe);

        // Act
        var route = ApiRoutes.Recipes.Patch.Replace(ApiRoutes.Recipes.Id, fakeRecipe.Id.ToString());
        var result = await _client.PatchJsonRequestAsync(route, patchDoc);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task patch_recipe_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        var patchDoc = new JsonPatchDocument<RecipeForUpdateDto>();
        patchDoc.Replace(r => r.Title, _faker.Lorem.Word());
        _client.AddAuth();

        await InsertAsync(fakeRecipe);

        // Act
        var route = ApiRoutes.Recipes.Patch.Replace(ApiRoutes.Recipes.Id, fakeRecipe.Id.ToString());
        var result = await _client.PatchJsonRequestAsync(route, patchDoc);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}