namespace RecipeManagement.Domain.Ingredients.Features;

using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Validators;
using RecipeManagement.Domain.Ingredients.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class UpdateIngredient
{
    public class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly IngredientForUpdateDto IngredientToUpdate;

        public Command(Guid ingredient, IngredientForUpdateDto newIngredientData)
        {
            Id = ingredient;
            IngredientToUpdate = newIngredientData;
        }
    }

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IIngredientRepository ingredientRepository, IUnitOfWork unitOfWork)
        {
            _ingredientRepository = ingredientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var ingredientToUpdate = await _ingredientRepository.GetById(request.Id, cancellationToken: cancellationToken);

            ingredientToUpdate.Update(request.IngredientToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}