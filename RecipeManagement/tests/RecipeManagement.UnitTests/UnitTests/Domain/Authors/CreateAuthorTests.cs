namespace RecipeManagement.UnitTests.UnitTests.Domain.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class CreateAuthorTests
{
    private readonly Faker _faker;

    public CreateAuthorTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_author()
    {
        // Arrange + Act
        var fakeAuthor = FakeAuthor.Generate();

        // Assert
        fakeAuthor.Should().NotBeNull();
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeAuthor = FakeAuthor.Generate();

        // Assert
        fakeAuthor.DomainEvents.Count.Should().Be(1);
        fakeAuthor.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AuthorCreated));
    }
}