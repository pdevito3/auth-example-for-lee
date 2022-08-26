namespace RecipeManagement.Domain.Recipes.DomainEvents;

public class RecipeUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            