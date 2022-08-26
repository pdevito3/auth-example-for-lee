namespace RecipeManagement.Domain.Authors.Mappings;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors;
using Mapster;

public class AuthorMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthorDto, Author>()
            .TwoWays();
        config.NewConfig<AuthorForCreationDto, Author>()
            .TwoWays();
        config.NewConfig<AuthorForUpdateDto, Author>()
            .TwoWays();
    }
}