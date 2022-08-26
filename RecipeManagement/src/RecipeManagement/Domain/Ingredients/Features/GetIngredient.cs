namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class GetIngredient
{
    public class Query : IRequest<IngredientDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<Query, IngredientDto>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public Handler(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<IngredientDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _ingredientRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<IngredientDto>(result);
        }
    }
}