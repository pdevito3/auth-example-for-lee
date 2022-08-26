namespace RecipeManagement.Domain.Ingredients;

using SharedKernel.Exceptions;
using RecipeManagement.Domain.Ingredients.Dtos;
using RecipeManagement.Domain.Ingredients.Validators;
using RecipeManagement.Domain.Ingredients.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using RecipeManagement.Domain.Recipes;


public class Ingredient : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Quantity { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual DateTime? ExpiresOn { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Measure { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Recipe")]
    public virtual Guid RecipeId { get; private set; }
    public virtual Recipe Recipe { get; private set; }


    public static Ingredient Create(IngredientForCreationDto ingredientForCreationDto)
    {
        new IngredientForCreationDtoValidator().ValidateAndThrow(ingredientForCreationDto);

        var newIngredient = new Ingredient();

        newIngredient.Name = ingredientForCreationDto.Name;
        newIngredient.Quantity = ingredientForCreationDto.Quantity;
        newIngredient.ExpiresOn = ingredientForCreationDto.ExpiresOn;
        newIngredient.Measure = ingredientForCreationDto.Measure;
        newIngredient.RecipeId = ingredientForCreationDto.RecipeId;

        newIngredient.QueueDomainEvent(new IngredientCreated(){ Ingredient = newIngredient });
        
        return newIngredient;
    }

    public void Update(IngredientForUpdateDto ingredientForUpdateDto)
    {
        new IngredientForUpdateDtoValidator().ValidateAndThrow(ingredientForUpdateDto);

        Name = ingredientForUpdateDto.Name;
        Quantity = ingredientForUpdateDto.Quantity;
        ExpiresOn = ingredientForUpdateDto.ExpiresOn;
        Measure = ingredientForUpdateDto.Measure;
        RecipeId = ingredientForUpdateDto.RecipeId;

        QueueDomainEvent(new IngredientUpdated(){ Id = Id });
    }
    
    protected Ingredient() { } // For EF + Mocking
}