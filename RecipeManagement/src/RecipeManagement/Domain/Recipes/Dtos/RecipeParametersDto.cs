namespace RecipeManagement.Domain.Recipes.Dtos;

using SharedKernel.Dtos;

public class RecipeParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
