namespace RecipeManagement.Domain.Ingredients.Services;

using RecipeManagement.Domain.Ingredients;
using RecipeManagement.Databases;
using RecipeManagement.Services;

public interface IIngredientRepository : IGenericRepository<Ingredient>
{
}

public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
{
    private readonly RecipesDbContext _dbContext;

    public IngredientRepository(RecipesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
