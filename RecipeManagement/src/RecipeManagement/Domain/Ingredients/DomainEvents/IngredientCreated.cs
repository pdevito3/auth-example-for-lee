namespace RecipeManagement.Domain.Ingredients.DomainEvents;

public class IngredientCreated : DomainEvent
{
    public Ingredient Ingredient { get; set; } 
}
            