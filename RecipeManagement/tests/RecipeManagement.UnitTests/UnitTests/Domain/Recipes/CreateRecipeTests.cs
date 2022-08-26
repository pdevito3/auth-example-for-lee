namespace RecipeManagement.UnitTests.UnitTests.Domain.Recipes;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class CreateRecipeTests
{
    private readonly Faker _faker;

    public CreateRecipeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_recipe()
    {
        // Arrange + Act
        var fakeRecipe = FakeRecipe.Generate();

        // Assert
        fakeRecipe.Should().NotBeNull();
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeRecipe = FakeRecipe.Generate();

        // Assert
        fakeRecipe.DomainEvents.Count.Should().Be(1);
        fakeRecipe.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(RecipeCreated));
    }
}