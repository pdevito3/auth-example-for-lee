namespace RecipeManagement.Domain.Authors;

using SharedKernel.Exceptions;
using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors.Validators;
using RecipeManagement.Domain.Authors.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using RecipeManagement.Domain.Recipes;


public class Author : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Recipe")]
    public virtual Guid RecipeId { get; private set; }
    public virtual Recipe Recipe { get; private set; }


    public static Author Create(AuthorForCreationDto authorForCreationDto)
    {
        new AuthorForCreationDtoValidator().ValidateAndThrow(authorForCreationDto);

        var newAuthor = new Author();

        newAuthor.Name = authorForCreationDto.Name;
        newAuthor.RecipeId = authorForCreationDto.RecipeId;

        newAuthor.QueueDomainEvent(new AuthorCreated(){ Author = newAuthor });
        
        return newAuthor;
    }

    public void Update(AuthorForUpdateDto authorForUpdateDto)
    {
        new AuthorForUpdateDtoValidator().ValidateAndThrow(authorForUpdateDto);

        Name = authorForUpdateDto.Name;
        RecipeId = authorForUpdateDto.RecipeId;

        QueueDomainEvent(new AuthorUpdated(){ Id = Id });
    }
    
    protected Author() { } // For EF + Mocking
}