namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class DeleteIngredientCommandTests : TestBase
{
    [Test]
    public async Task can_delete_ingredient_from_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        await InsertAsync(fakeIngredientOne);
        var ingredient = await ExecuteDbContextAsync(db => db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == fakeIngredientOne.Id));

        // Act
        var command = new DeleteIngredient.Command(ingredient.Id);
        await SendAsync(command);
        var ingredientResponse = await ExecuteDbContextAsync(db => db.Ingredients.CountAsync(i => i.Id == ingredient.Id));

        // Assert
        ingredientResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_ingredient_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteIngredient.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_ingredient_from_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        await InsertAsync(fakeIngredientOne);
        var ingredient = await ExecuteDbContextAsync(db => db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == fakeIngredientOne.Id));

        // Act
        var command = new DeleteIngredient.Command(ingredient.Id);
        await SendAsync(command);
        var deletedIngredient = await ExecuteDbContextAsync(db => db.Ingredients
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == ingredient.Id));

        // Assert
        deletedIngredient?.IsDeleted.Should().BeTrue();
    }
}