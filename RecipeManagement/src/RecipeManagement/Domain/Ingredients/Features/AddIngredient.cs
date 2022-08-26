namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients.Services;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class AddIngredient
{
    public class Command : IRequest<IngredientDto>
    {
        public readonly IngredientForCreationDto IngredientToAdd;

        public Command(IngredientForCreationDto ingredientToAdd)
        {
            IngredientToAdd = ingredientToAdd;
        }
    }

    public class Handler : IRequestHandler<Command, IngredientDto>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IIngredientRepository ingredientRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IngredientDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var ingredient = Ingredient.Create(request.IngredientToAdd);
            await _ingredientRepository.Add(ingredient, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var ingredientAdded = await _ingredientRepository.GetById(ingredient.Id, cancellationToken: cancellationToken);
            return _mapper.Map<IngredientDto>(ingredientAdded);
        }
    }
}