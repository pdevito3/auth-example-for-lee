namespace RecipeManagement.IntegrationTests.FeatureTests.Authors;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.Author;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.Authors.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class AuthorListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_author_list()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
    await InsertAsync(fakeRecipeOne, fakeRecipeTwo);

        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        var fakeAuthorTwo = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.RecipeId, _ => fakeRecipeTwo.Id)
            .Generate());
        var queryParameters = new AuthorParametersDto();

        await InsertAsync(fakeAuthorOne, fakeAuthorTwo);

        // Act
        var query = new GetAuthorList.Query(queryParameters);
        var authors = await SendAsync(query);

        // Assert
        authors.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}