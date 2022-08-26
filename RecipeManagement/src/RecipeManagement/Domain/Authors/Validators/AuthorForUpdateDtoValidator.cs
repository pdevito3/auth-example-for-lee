namespace RecipeManagement.Domain.Authors.Validators;

using RecipeManagement.Domain.Authors.Dtos;
using FluentValidation;

public class AuthorForUpdateDtoValidator: AuthorForManipulationDtoValidator<AuthorForUpdateDto>
{
    public AuthorForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}