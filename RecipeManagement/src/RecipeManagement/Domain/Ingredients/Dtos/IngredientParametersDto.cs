namespace RecipeManagement.Domain.Ingredients.Dtos;

using SharedKernel.Dtos;

public class IngredientParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
