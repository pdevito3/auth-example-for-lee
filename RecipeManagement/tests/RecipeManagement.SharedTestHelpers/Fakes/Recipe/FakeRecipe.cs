namespace RecipeManagement.SharedTestHelpers.Fakes.Recipe;

using AutoBogus;
using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;

public class FakeRecipe
{
    public static Recipe Generate(RecipeForCreationDto recipeForCreationDto)
    {
        return Recipe.Create(recipeForCreationDto);
    }

    public static Recipe Generate()
    {
        return Recipe.Create(new FakeRecipeForCreationDto().Generate());
    }
}