namespace RecipeManagement.Domain.Ingredients.Mappings;

using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients;
using Mapster;

public class IngredientMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<IngredientDto, Ingredient>()
            .TwoWays();
        config.NewConfig<IngredientForCreationDto, Ingredient>()
            .TwoWays();
        config.NewConfig<IngredientForUpdateDto, Ingredient>()
            .TwoWays();
    }
}