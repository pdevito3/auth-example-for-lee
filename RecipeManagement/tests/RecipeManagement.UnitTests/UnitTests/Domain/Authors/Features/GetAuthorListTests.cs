namespace RecipeManagement.UnitTests.UnitTests.Domain.Authors.Features;

using RecipeManagement.SharedTestHelpers.Fakes.Author;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors.Mappings;
using RecipeManagement.Domain.Authors.Features;
using RecipeManagement.Domain.Authors.Services;
using MapsterMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetAuthorListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IAuthorRepository> _authorRepository;

    public GetAuthorListTests()
    {
        _authorRepository = new Mock<IAuthorRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
    }
    
    [Test]
    public async Task can_get_paged_list_of_author()
    {
        //Arrange
        var fakeAuthorOne = FakeAuthor.Generate();
        var fakeAuthorTwo = FakeAuthor.Generate();
        var fakeAuthorThree = FakeAuthor.Generate();
        var author = new List<Author>();
        author.Add(fakeAuthorOne);
        author.Add(fakeAuthorTwo);
        author.Add(fakeAuthorThree);
        var mockDbData = author.AsQueryable().BuildMock();
        
        var queryParameters = new AuthorParametersDto() { PageSize = 1, PageNumber = 2 };

        _authorRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetAuthorList.Query(queryParameters);
        var handler = new GetAuthorList.Handler(_authorRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_author_list_using_Name()
    {
        //Arrange
        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "alpha")
            .Generate());
        var fakeAuthorTwo = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "bravo")
            .Generate());
        var queryParameters = new AuthorParametersDto() { Filters = $"Name == {fakeAuthorTwo.Name}" };

        var authorList = new List<Author>() { fakeAuthorOne, fakeAuthorTwo };
        var mockDbData = authorList.AsQueryable().BuildMock();

        _authorRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAuthorList.Query(queryParameters);
        var handler = new GetAuthorList.Handler(_authorRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAuthorTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_author_by_Name()
    {
        //Arrange
        var fakeAuthorOne = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "alpha")
            .Generate());
        var fakeAuthorTwo = FakeAuthor.Generate(new FakeAuthorForCreationDto()
            .RuleFor(a => a.Name, _ => "bravo")
            .Generate());
        var queryParameters = new AuthorParametersDto() { SortOrder = "-Name" };

        var AuthorList = new List<Author>() { fakeAuthorOne, fakeAuthorTwo };
        var mockDbData = AuthorList.AsQueryable().BuildMock();

        _authorRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAuthorList.Query(queryParameters);
        var handler = new GetAuthorList.Handler(_authorRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeAuthorTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAuthorOne, options =>
                options.ExcludingMissingMembers());
    }
}