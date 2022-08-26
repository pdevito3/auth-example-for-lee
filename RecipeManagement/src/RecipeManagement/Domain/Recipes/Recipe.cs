namespace RecipeManagement.Domain.Recipes;

using SharedKernel.Exceptions;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Validators;
using RecipeManagement.Domain.Recipes.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Ingredients;


public class Recipe : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Title { get; private set; }

    private VisibilityEnum _visibility;
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Visibility
    {
        get => _visibility.Name;
        private set
        {
            if (!VisibilityEnum.TryFromName(value, true, out var parsed))
                throw new InvalidSmartEnumPropertyName(nameof(Visibility), value);

            _visibility = parsed;
        }
    }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Directions { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual int? Rating { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual Author Author { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<Ingredient> Ingredients { get; private set; }


    public static Recipe Create(RecipeForCreationDto recipeForCreationDto)
    {
        new RecipeForCreationDtoValidator().ValidateAndThrow(recipeForCreationDto);

        var newRecipe = new Recipe();

        newRecipe.Title = recipeForCreationDto.Title;
        newRecipe.Visibility = recipeForCreationDto.Visibility;
        newRecipe.Directions = recipeForCreationDto.Directions;
        newRecipe.Rating = recipeForCreationDto.Rating;

        newRecipe.QueueDomainEvent(new RecipeCreated(){ Recipe = newRecipe });
        
        return newRecipe;
    }

    public void Update(RecipeForUpdateDto recipeForUpdateDto)
    {
        new RecipeForUpdateDtoValidator().ValidateAndThrow(recipeForUpdateDto);

        Title = recipeForUpdateDto.Title;
        Visibility = recipeForUpdateDto.Visibility;
        Directions = recipeForUpdateDto.Directions;
        Rating = recipeForUpdateDto.Rating;

        QueueDomainEvent(new RecipeUpdated(){ Id = Id });
    }
    
    protected Recipe() { } // For EF + Mocking
}