namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

public static class PatchRecipe
{
    public class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly JsonPatchDocument<RecipeForUpdateDto> PatchDoc;

        public Command(Guid recipe, JsonPatchDocument<RecipeForUpdateDto> patchDoc)
        {
            Id = recipe;
            PatchDoc = patchDoc;
        }
    }

    public class Handler : IRequestHandler<Command, bool>
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

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanPatchRecipe);

            if (request.PatchDoc == null)
                throw new ValidationException(
                    new List<ValidationFailure>()
                    {
                        new ValidationFailure("Patch Document","Invalid patch doc.")
                    });

            var recipeToUpdate = await _recipeRepository.GetById(request.Id, cancellationToken: cancellationToken);

            var recipeToPatch = _mapper.Map<RecipeForUpdateDto>(recipeToUpdate);
            request.PatchDoc.ApplyTo(recipeToPatch);

            recipeToUpdate.Update(recipeToPatch);
            await _unitOfWork.CommitChanges(cancellationToken);

            return true;
        }
    }
}