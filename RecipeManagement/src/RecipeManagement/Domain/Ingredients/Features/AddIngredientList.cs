namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients.Services;
using RecipeManagement.Domain.Recipes.Services;
using RecipeManagement.Services;
using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class AddIngredientList
{
    public class AddIngredientListCommand : IRequest<List<IngredientDto>>
    {
        public readonly IEnumerable<IngredientForCreationDto> IngredientListToAdd;
        public readonly Guid RecipeId;

        public AddIngredientListCommand(IEnumerable<IngredientForCreationDto> ingredientListListToAdd, Guid recipeId)
        {
            IngredientListToAdd = ingredientListListToAdd;
            RecipeId = recipeId;
        }
    }

    public class Handler : IRequestHandler<AddIngredientListCommand, List<IngredientDto>>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IIngredientRepository ingredientRepository, IUnitOfWork unitOfWork, IMapper mapper, IRecipeRepository recipeRepository)
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
            _unitOfWork = unitOfWork;
            _recipeRepository = recipeRepository;
        }

        public async Task<List<IngredientDto>> Handle(AddIngredientListCommand request, CancellationToken cancellationToken)
        {
            // throws error if parent doesn't exist 
            await _recipeRepository.GetById(request.RecipeId, cancellationToken: cancellationToken);

            var ingredientListToAdd = request.IngredientListToAdd
                .Select(i => { i.RecipeId = request.RecipeId; return i; })
                .ToList();
            var ingredientList = new List<Ingredient>();
            ingredientListToAdd.ForEach(ingredient => ingredientList.Add(Ingredient.Create(ingredient)));
            
            // if you have large datasets to add in bulk and have performance concerns, there 
            // are additional methods that could be leveraged in your repository instead (e.g. SqlBulkCopy)
            // https://timdeschryver.dev/blog/faster-sql-bulk-inserts-with-csharp#table-valued-parameter 
            await _ingredientRepository.AddRange(ingredientList, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);

            return _mapper.Map<List<IngredientDto>>(ingredientList);
        }
    }
}