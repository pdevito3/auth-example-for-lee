namespace RecipeManagement.UnitTests.UnitTests.Domain.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class UpdateAuthorTests
{
    private readonly Faker _faker;

    public UpdateAuthorTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_author()
    {
        // Arrange
        var fakeAuthor = FakeAuthor.Generate();
        var updatedAuthor = new FakeAuthorForUpdateDto().Generate();
        
        // Act
        fakeAuthor.Update(updatedAuthor);

        // Assert
        fakeAuthor.Should().BeEquivalentTo(updatedAuthor, options =>
            options.ExcludingMissingMembers());
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeAuthor = FakeAuthor.Generate();
        var updatedAuthor = new FakeAuthorForUpdateDto().Generate();
        fakeAuthor.DomainEvents.Clear();
        
        // Act
        fakeAuthor.Update(updatedAuthor);

        // Assert
        fakeAuthor.DomainEvents.Count.Should().Be(1);
        fakeAuthor.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AuthorUpdated));
    }
}