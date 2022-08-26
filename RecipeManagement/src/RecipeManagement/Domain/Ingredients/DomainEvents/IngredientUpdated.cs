namespace RecipeManagement.Domain.Ingredients.DomainEvents;

public class IngredientUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            