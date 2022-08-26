namespace RecipeManagement.UnitTests.UnitTests.Domain.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class UpdateIngredientTests
{
    private readonly Faker _faker;

    public UpdateIngredientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_ingredient()
    {
        // Arrange
        var fakeIngredient = FakeIngredient.Generate();
        var updatedIngredient = new FakeIngredientForUpdateDto().Generate();
        
        // Act
        fakeIngredient.Update(updatedIngredient);

        // Assert
        fakeIngredient.Should().BeEquivalentTo(updatedIngredient, options =>
            options.ExcludingMissingMembers());
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeIngredient = FakeIngredient.Generate();
        var updatedIngredient = new FakeIngredientForUpdateDto().Generate();
        fakeIngredient.DomainEvents.Clear();
        
        // Act
        fakeIngredient.Update(updatedIngredient);

        // Assert
        fakeIngredient.DomainEvents.Count.Should().Be(1);
        fakeIngredient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(IngredientUpdated));
    }
}