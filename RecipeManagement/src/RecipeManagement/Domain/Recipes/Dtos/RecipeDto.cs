namespace RecipeManagement.Domain.Recipes.Dtos;

public class RecipeDto 
{
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Visibility { get; set; }
        public string Directions { get; set; }
        public int? Rating { get; set; }

}
