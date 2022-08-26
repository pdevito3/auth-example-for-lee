namespace RecipeManagement.Domain.Authors.DomainEvents;

public class AuthorCreated : DomainEvent
{
    public Author Author { get; set; } 
}
            