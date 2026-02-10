using System;

namespace GroceryListApp
{
    /// <summary>
    /// Represents an ingredient with a specific quantity in a recipe.
    /// Links to the ingredient name rather than storing the full ingredient object
    /// to avoid data duplication and maintain consistency.
    /// </summary>
    public class RecipeIngredient
    {
        private string _ingredientName;
        private decimal _quantity;

        public string IngredientName
        {
            get { return _ingredientName; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ingredient name cannot be empty.");
                _ingredientName = value.Trim();
            }
        }

        public decimal Quantity
        {
            get { return _quantity; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.");
                _quantity = value;
            }
        }

        // Constructor
        public RecipeIngredient(string ingredientName, decimal quantity)
        {
            IngredientName = ingredientName;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Quantity} unit(s) of {IngredientName}";
        }
    }
}