namespace SharedKernel.Messages
{
    using System;
    using System.Text;

    public interface IRecipeAdded
    {
        public Guid RecipeId { get; set; }
    }

    public class RecipeAdded : IRecipeAdded
    {
        public Guid RecipeId { get; set; }
    }
}