namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors.Dtos;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.Authors.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class UpdateAuthorCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_author_in_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        var updatedAuthorDto = new FakeAuthorForUpdateDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate();
        await InsertAsync(fakeAuthorOne);

        var author = await ExecuteDbContextAsync(db => db.Authors
            .FirstOrDefaultAsync(a => a.Id == fakeAuthorOne.Id));
        var id = author.Id;

        // Act
        var command = new UpdateAuthor.Command(id, updatedAuthorDto);
        await SendAsync(command);
        var updatedAuthor = await ExecuteDbContextAsync(db => db.Authors.FirstOrDefaultAsync(a => a.Id == id));

        // Assert
        updatedAuthor.Should().BeEquivalentTo(updatedAuthorDto, options =>
            options.ExcludingMissingMembers());
    }
}