namespace RecipeManagement.Domain.Authors.DomainEvents;

public class AuthorUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            