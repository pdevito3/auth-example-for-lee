namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Services;
using RecipeManagement.Wrappers;
using SharedKernel.Exceptions;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetIngredientList
{
    public class Query : IRequest<PagedList<IngredientDto>>
    {
        public readonly IngredientParametersDto QueryParameters;

        public Query(IngredientParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public class Handler : IRequestHandler<Query, PagedList<IngredientDto>>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;

        public Handler(IIngredientRepository ingredientRepository, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<PagedList<IngredientDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = _ingredientRepository.Query();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "Id",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<IngredientDto>();

            return await PagedList<IngredientDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}