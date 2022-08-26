namespace RecipeManagement.UnitTests.UnitTests.Domain.Recipes.Features;

using RecipeManagement.SharedTestHelpers.Fakes.Recipe;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Mappings;
using RecipeManagement.Domain.Recipes.Features;
using RecipeManagement.Domain.Recipes.Services;
using MapsterMapper;
using FluentAssertions;
using HeimGuard;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetRecipeListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IRecipeRepository> _recipeRepository;
      private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetRecipeListTests()
    {
        _recipeRepository = new Mock<IRecipeRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_recipe()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate();
        var fakeRecipeTwo = FakeRecipe.Generate();
        var fakeRecipeThree = FakeRecipe.Generate();
        var recipe = new List<Recipe>();
        recipe.Add(fakeRecipeOne);
        recipe.Add(fakeRecipeTwo);
        recipe.Add(fakeRecipeThree);
        var mockDbData = recipe.AsQueryable().BuildMock();
        
        var queryParameters = new RecipeParametersDto() { PageSize = 1, PageNumber = 2 };

        _recipeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetRecipeList.Query(queryParameters);
        var handler = new GetRecipeList.Handler(_recipeRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_recipe_list_using_Title()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "alpha")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "bravo")
            .Generate());
        var queryParameters = new RecipeParametersDto() { Filters = $"Title == {fakeRecipeTwo.Title}" };

        var recipeList = new List<Recipe>() { fakeRecipeOne, fakeRecipeTwo };
        var mockDbData = recipeList.AsQueryable().BuildMock();

        _recipeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetRecipeList.Query(queryParameters);
        var handler = new GetRecipeList.Handler(_recipeRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_recipe_list_using_Directions()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "alpha")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "bravo")
            .Generate());
        var queryParameters = new RecipeParametersDto() { Filters = $"Directions == {fakeRecipeTwo.Directions}" };

        var recipeList = new List<Recipe>() { fakeRecipeOne, fakeRecipeTwo };
        var mockDbData = recipeList.AsQueryable().BuildMock();

        _recipeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetRecipeList.Query(queryParameters);
        var handler = new GetRecipeList.Handler(_recipeRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_recipe_list_using_Rating()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 1)
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 2)
            .Generate());
        var queryParameters = new RecipeParametersDto() { Filters = $"Rating == {fakeRecipeTwo.Rating}" };

        var recipeList = new List<Recipe>() { fakeRecipeOne, fakeRecipeTwo };
        var mockDbData = recipeList.AsQueryable().BuildMock();

        _recipeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetRecipeList.Query(queryParameters);
        var handler = new GetRecipeList.Handler(_recipeRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Title()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "alpha")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Title, _ => "bravo")
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "-Title" };

        var RecipeList = new List<Recipe>() { fakeRecipeOne, fakeRecipeTwo };
        var mockDbData = RecipeList.AsQueryable().BuildMock();

        _recipeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetRecipeList.Query(queryParameters);
        var handler = new GetRecipeList.Handler(_recipeRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Directions()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "alpha")
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Directions, _ => "bravo")
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "-Directions" };

        var RecipeList = new List<Recipe>() { fakeRecipeOne, fakeRecipeTwo };
        var mockDbData = RecipeList.AsQueryable().BuildMock();

        _recipeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetRecipeList.Query(queryParameters);
        var handler = new GetRecipeList.Handler(_recipeRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_recipe_by_Rating()
    {
        //Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 1)
            .Generate());
        var fakeRecipeTwo = FakeRecipe.Generate(new FakeRecipeForCreationDto()
            .RuleFor(r => r.Rating, _ => 2)
            .Generate());
        var queryParameters = new RecipeParametersDto() { SortOrder = "-Rating" };

        var RecipeList = new List<Recipe>() { fakeRecipeOne, fakeRecipeTwo };
        var mockDbData = RecipeList.AsQueryable().BuildMock();

        _recipeRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetRecipeList.Query(queryParameters);
        var handler = new GetRecipeList.Handler(_recipeRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
    }
}