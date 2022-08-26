namespace RecipeManagement.Domain.Authors.Dtos;

public abstract class AuthorForManipulationDto 
{
        public string Name { get; set; }
        public Guid RecipeId { get; set; }
}
