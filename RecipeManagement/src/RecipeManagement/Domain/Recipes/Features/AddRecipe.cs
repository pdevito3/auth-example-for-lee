namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes.Services;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddRecipe
{
    public class Command : IRequest<RecipeDto>
    {
        public readonly RecipeForCreationDto RecipeToAdd;

        public Command(RecipeForCreationDto recipeToAdd)
        {
            RecipeToAdd = recipeToAdd;
        }
    }

    public class Handler : IRequestHandler<Command, RecipeDto>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRecipeRepository recipeRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<RecipeDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddRecipe);

            var recipe = Recipe.Create(request.RecipeToAdd);
            await _recipeRepository.Add(recipe, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var recipeAdded = await _recipeRepository.GetById(recipe.Id, cancellationToken: cancellationToken);
            return _mapper.Map<RecipeDto>(recipeAdded);
        }
    }
}