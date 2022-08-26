namespace RecipeManagement.Domain.Authors.Dtos;

using SharedKernel.Dtos;

public class AuthorParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
