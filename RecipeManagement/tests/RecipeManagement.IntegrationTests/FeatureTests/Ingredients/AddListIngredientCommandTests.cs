namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Ingredients.Features;
using SharedKernel.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AddListIngredientCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_ingredient_list_to_db()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipe);
        var fakeIngredientOne = new FakeIngredientForCreationDto().Generate();
        var fakeIngredientTwo = new FakeIngredientForCreationDto().Generate();

        // Act
        var command = new AddIngredientList.AddIngredientListCommand(new List<IngredientForCreationDto>() {fakeIngredientOne, fakeIngredientTwo}, fakeRecipe.Id);
        var ingredientReturned = await SendAsync(command);
        var ingredientDb = await ExecuteDbContextAsync(db => db.Ingredients.ToListAsync());

        // Assert
        ingredientReturned.Should().ContainEquivalentOf(fakeIngredientOne, options =>
            options.ExcludingMissingMembers());
        ingredientDb.Should().ContainEquivalentOf(fakeIngredientOne, options =>
            options.ExcludingMissingMembers());

        ingredientReturned.Should().ContainEquivalentOf(fakeIngredientTwo, options =>
            options.ExcludingMissingMembers());
        ingredientDb.Should().ContainEquivalentOf(fakeIngredientTwo, options =>
            options.ExcludingMissingMembers());
    }
}