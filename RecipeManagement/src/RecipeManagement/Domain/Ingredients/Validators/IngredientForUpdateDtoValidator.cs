namespace RecipeManagement.Domain.Ingredients.Validators;

using RecipeManagement.Domain.Ingredients.Dtos;
using FluentValidation;

public class IngredientForUpdateDtoValidator: IngredientForManipulationDtoValidator<IngredientForUpdateDto>
{
    public IngredientForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}