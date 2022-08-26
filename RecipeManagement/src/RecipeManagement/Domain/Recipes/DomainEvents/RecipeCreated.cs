namespace RecipeManagement.Domain.Recipes.DomainEvents;

public class RecipeCreated : DomainEvent
{
    public Recipe Recipe { get; set; } 
}
            