namespace RecipeManagement.UnitTests.UnitTests.Domain.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class CreateIngredientTests
{
    private readonly Faker _faker;

    public CreateIngredientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_ingredient()
    {
        // Arrange + Act
        var fakeIngredient = FakeIngredient.Generate();

        // Assert
        fakeIngredient.Should().NotBeNull();
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeIngredient = FakeIngredient.Generate();

        // Assert
        fakeIngredient.DomainEvents.Count.Should().Be(1);
        fakeIngredient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(IngredientCreated));
    }
}