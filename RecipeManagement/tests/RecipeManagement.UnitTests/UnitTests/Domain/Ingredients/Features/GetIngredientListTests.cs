namespace RecipeManagement.UnitTests.UnitTests.Domain.Ingredients.Features;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Mappings;
using RecipeManagement.Domain.Ingredients.Features;
using RecipeManagement.Domain.Ingredients.Services;
using MapsterMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetIngredientListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IIngredientRepository> _ingredientRepository;

    public GetIngredientListTests()
    {
        _ingredientRepository = new Mock<IIngredientRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
    }
    
    [Test]
    public async Task can_get_paged_list_of_ingredient()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate();
        var fakeIngredientTwo = FakeIngredient.Generate();
        var fakeIngredientThree = FakeIngredient.Generate();
        var ingredient = new List<Ingredient>();
        ingredient.Add(fakeIngredientOne);
        ingredient.Add(fakeIngredientTwo);
        ingredient.Add(fakeIngredientThree);
        var mockDbData = ingredient.AsQueryable().BuildMock();
        
        var queryParameters = new IngredientParametersDto() { PageSize = 1, PageNumber = 2 };

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_ingredient_list_using_Name()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "alpha")
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "bravo")
            .Generate());
        var queryParameters = new IngredientParametersDto() { Filters = $"Name == {fakeIngredientTwo.Name}" };

        var ingredientList = new List<Ingredient>() { fakeIngredientOne, fakeIngredientTwo };
        var mockDbData = ingredientList.AsQueryable().BuildMock();

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_ingredient_list_using_Quantity()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "alpha")
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "bravo")
            .Generate());
        var queryParameters = new IngredientParametersDto() { Filters = $"Quantity == {fakeIngredientTwo.Quantity}" };

        var ingredientList = new List<Ingredient>() { fakeIngredientOne, fakeIngredientTwo };
        var mockDbData = ingredientList.AsQueryable().BuildMock();

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_ingredient_list_using_ExpiresOn()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.ExpiresOn, _ => DateTime.Now.AddDays(1))
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.ExpiresOn, _ => DateTime.Parse(DateTime.Now.AddDays(2).ToString("MM/dd/yyyy")))
            .Generate());
        var queryParameters = new IngredientParametersDto() { Filters = $"ExpiresOn == {fakeIngredientTwo.ExpiresOn}" };

        var ingredientList = new List<Ingredient>() { fakeIngredientOne, fakeIngredientTwo };
        var mockDbData = ingredientList.AsQueryable().BuildMock();

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_ingredient_list_using_Measure()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "alpha")
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "bravo")
            .Generate());
        var queryParameters = new IngredientParametersDto() { Filters = $"Measure == {fakeIngredientTwo.Measure}" };

        var ingredientList = new List<Ingredient>() { fakeIngredientOne, fakeIngredientTwo };
        var mockDbData = ingredientList.AsQueryable().BuildMock();

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Name()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "alpha")
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Name, _ => "bravo")
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "-Name" };

        var IngredientList = new List<Ingredient>() { fakeIngredientOne, fakeIngredientTwo };
        var mockDbData = IngredientList.AsQueryable().BuildMock();

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Quantity()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "alpha")
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Quantity, _ => "bravo")
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "-Quantity" };

        var IngredientList = new List<Ingredient>() { fakeIngredientOne, fakeIngredientTwo };
        var mockDbData = IngredientList.AsQueryable().BuildMock();

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_ExpiresOn()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.ExpiresOn, _ => DateTime.Now.AddDays(1))
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.ExpiresOn, _ => DateTime.Now.AddDays(2))
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "-ExpiresOn" };

        var IngredientList = new List<Ingredient>() { fakeIngredientOne, fakeIngredientTwo };
        var mockDbData = IngredientList.AsQueryable().BuildMock();

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_ingredient_by_Measure()
    {
        //Arrange
        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "alpha")
            .Generate());
        var fakeIngredientTwo = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.Measure, _ => "bravo")
            .Generate());
        var queryParameters = new IngredientParametersDto() { SortOrder = "-Measure" };

        var IngredientList = new List<Ingredient>() { fakeIngredientOne, fakeIngredientTwo };
        var mockDbData = IngredientList.AsQueryable().BuildMock();

        _ingredientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetIngredientList.Query(queryParameters);
        var handler = new GetIngredientList.Handler(_ingredientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeIngredientOne, options =>
                options.ExcludingMissingMembers());
    }
}