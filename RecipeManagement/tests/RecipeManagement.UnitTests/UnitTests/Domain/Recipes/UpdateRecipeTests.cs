namespace RecipeManagement.UnitTests.UnitTests.Domain.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class UpdateRecipeTests
{
    private readonly Faker _faker;

    public UpdateRecipeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_recipe()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate();
        var updatedRecipe = new FakeRecipeForUpdateDto().Generate();
        
        // Act
        fakeRecipe.Update(updatedRecipe);

        // Assert
        fakeRecipe.Should().BeEquivalentTo(updatedRecipe, options =>
            options.ExcludingMissingMembers());
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeRecipe = FakeRecipe.Generate();
        var updatedRecipe = new FakeRecipeForUpdateDto().Generate();
        fakeRecipe.DomainEvents.Clear();
        
        // Act
        fakeRecipe.Update(updatedRecipe);

        // Assert
        fakeRecipe.DomainEvents.Count.Should().Be(1);
        fakeRecipe.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RecipeUpdated));
    }
}