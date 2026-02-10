using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryListApp
{
    /// <summary>
    /// Represents a recipe containing multiple ingredients with quantities.
    /// Demonstrates use of collections (List) and encapsulation.
    /// </summary>
    public class Recipe
    {
        private string _name;
        private int _servings;
        private List<RecipeIngredient> _ingredients;

        public string Name
        {
            get { return _name; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Recipe name cannot be empty.");
                _name = value.Trim();
            }
        }

        public int Servings
        {
            get { return _servings; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Servings must be greater than zero.");
                _servings = value;
            }
        }

        // Read-only access to ingredients (encapsulation)
        public IReadOnlyList<RecipeIngredient> Ingredients
        {
            get { return _ingredients.AsReadOnly(); }
        }

        // Constructor
        public Recipe(string name, int servings = 4)
        {
            Name = name;
            Servings = servings;
            _ingredients = new List<RecipeIngredient>();
        }

        // Add an ingredient to the recipe
        public bool AddIngredient(string ingredientName, decimal quantity)
        {
            if (string.IsNullOrWhiteSpace(ingredientName) || quantity <= 0)
                return false;

            // Check if ingredient already exists in recipe
            var existing = _ingredients.FirstOrDefault(i => 
                i.IngredientName.Equals(ingredientName, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                // Update quantity if it already exists
                existing.Quantity += quantity;
            }
            else
            {
                // Add new ingredient
                _ingredients.Add(new RecipeIngredient(ingredientName, quantity));
            }

            return true;
        }

        // Remove an ingredient from the recipe
        public bool RemoveIngredient(string ingredientName)
        {
            var ingredient = _ingredients.FirstOrDefault(i => 
                i.IngredientName.Equals(ingredientName, StringComparison.OrdinalIgnoreCase));

            if (ingredient != null)
            {
                _ingredients.Remove(ingredient);
                return true;
            }

            return false;
        }

        // Get the count of ingredients
        public int GetIngredientCount()
        {
            return _ingredients.Count;
        }

        // Get all ingredient names
        public List<string> GetIngredientNames()
        {
            return _ingredients.Select(i => i.IngredientName).ToList();
        }

        // Get quantity for a specific ingredient
        public decimal GetIngredientQuantity(string ingredientName)
        {
            var ingredient = _ingredients.FirstOrDefault(i => 
                i.IngredientName.Equals(ingredientName, StringComparison.OrdinalIgnoreCase));

            return ingredient?.Quantity ?? 0;
        }

        public override string ToString()
        {
            return $"{Name} (Serves {Servings}) - {_ingredients.Count} ingredients";
        }

        public bool NameEquals(string otherName)
        {
            return Name.Equals(otherName, StringComparison.OrdinalIgnoreCase);
        }
    }
}