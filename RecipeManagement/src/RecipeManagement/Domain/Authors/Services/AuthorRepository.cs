namespace RecipeManagement.Domain.Authors.Services;

using RecipeManagement.Domain.Authors;
using RecipeManagement.Databases;
using RecipeManagement.Services;

public interface IAuthorRepository : IGenericRepository<Author>
{
}

public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    private readonly RecipesDbContext _dbContext;

    public AuthorRepository(RecipesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
