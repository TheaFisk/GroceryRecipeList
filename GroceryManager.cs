using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace GroceryListApp
{
    /// <summary>
    /// Main manager class that coordinates ingredients, recipes, and grocery list generation.
    /// Demonstrates use of collections (Dictionary and List), file I/O, and business logic.
    /// </summary>
    public class GroceryManager
    {
        // Dictionary for fast ingredient lookup by name
        private Dictionary<string, Ingredient> _ingredients;
        
        // Dictionary for fast recipe lookup by name
        private Dictionary<string, Recipe> _recipes;

        // File paths for persistence
        private const string INGREDIENTS_FILE = "ingredients.json";
        private const string RECIPES_FILE = "recipes.json";

        public GroceryManager()
        {
            _ingredients = new Dictionary<string, Ingredient>(StringComparer.OrdinalIgnoreCase);
            _recipes = new Dictionary<string, Recipe>(StringComparer.OrdinalIgnoreCase);
        }

        #region Ingredient Management

        public bool AddIngredient(string name, decimal price, string unit = "unit")
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            string key = name.Trim();
            
            if (_ingredients.ContainsKey(key))
                return false;

            _ingredients[key] = new Ingredient(name, price, unit);
            return true;
        }

        public bool UpdateIngredientPrice(string name, decimal newPrice)
        {
            if (_ingredients.TryGetValue(name, out Ingredient ingredient))
            {
                ingredient.PricePerUnit = newPrice;
                return true;
            }
            return false;
        }

        public bool DeleteIngredient(string name)
        {
            return _ingredients.Remove(name);
        }

        public Ingredient GetIngredient(string name)
        {
            _ingredients.TryGetValue(name, out Ingredient ingredient);
            return ingredient;
        }

        public void DisplayAllIngredients()
        {
            if (_ingredients.Count == 0)
            {
                Console.WriteLine("\n✗ No ingredients available.");
                return;
            }

            Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║                  INGREDIENT LIST                      ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
            
            var sortedIngredients = _ingredients.Values.OrderBy(i => i.Name);
            
            foreach (var ingredient in sortedIngredients)
            {
                Console.WriteLine($"  • {ingredient}");
            }
            
            Console.WriteLine($"\nTotal Ingredients: {_ingredients.Count}");
        }

        #endregion

        #region Recipe Management

        public bool AddRecipe(Recipe recipe)
        {
            if (recipe == null || string.IsNullOrWhiteSpace(recipe.Name))
                return false;

            if (_recipes.ContainsKey(recipe.Name))
                return false;

            _recipes[recipe.Name] = recipe;
            return true;
        }

        public bool DeleteRecipe(string name)
        {
            return _recipes.Remove(name);
        }

        public Recipe GetRecipe(string name)
        {
            _recipes.TryGetValue(name, out Recipe recipe);
            return recipe;
        }

        public List<string> GetAllRecipeNames()
        {
            return _recipes.Keys.OrderBy(name => name).ToList();
        }

        public void DisplayAllRecipes()
        {
            if (_recipes.Count == 0)
            {
                Console.WriteLine("\n✗ No recipes available.");
                return;
            }

            Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     RECIPE LIST                       ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
            
            var sortedRecipes = _recipes.Values.OrderBy(r => r.Name);
            
            foreach (var recipe in sortedRecipes)
            {
                Console.WriteLine($"  • {recipe}");
            }
            
            Console.WriteLine($"\nTotal Recipes: {_recipes.Count}");
        }

        public void DisplayRecipeDetails(string recipeName)
        {
            var recipe = GetRecipe(recipeName);
            
            if (recipe == null)
            {
                Console.WriteLine($"\n✗ Recipe '{recipeName}' not found.");
                return;
            }

            Console.WriteLine($"\n╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine($"║  {recipe.Name.ToUpper().PadRight(51)} ║");
            Console.WriteLine($"╚═══════════════════════════════════════════════════════╝");
            Console.WriteLine($"Servings: {recipe.Servings}");
            Console.WriteLine($"\nIngredients ({recipe.GetIngredientCount()}):");
            
            decimal totalCost = 0;
            bool missingIngredients = false;

            foreach (var recipeIngredient in recipe.Ingredients)
            {
                var ingredient = GetIngredient(recipeIngredient.IngredientName);
                
                if (ingredient != null)
                {
                    decimal cost = ingredient.CalculateCost(recipeIngredient.Quantity);
                    totalCost += cost;
                    Console.WriteLine($"  • {recipeIngredient.Quantity} {ingredient.Unit} of {ingredient.Name} (${cost:F2})");
                }
                else
                {
                    Console.WriteLine($"  • {recipeIngredient} [INGREDIENT NOT IN SYSTEM]");
                    missingIngredients = true;
                }
            }

            if (!missingIngredients)
            {
                Console.WriteLine($"\nEstimated Total Cost: ${totalCost:F2}");
                Console.WriteLine($"Cost Per Serving: ${(totalCost / recipe.Servings):F2}");
            }
            else
            {
                Console.WriteLine("\n⚠ Some ingredients are not in the system. Add them to calculate cost.");
            }
        }

        #endregion

        #region Grocery List Generation

        public void GenerateGroceryList(List<string> selectedRecipeNames)
        {
            if (selectedRecipeNames == null || selectedRecipeNames.Count == 0)
            {
                Console.WriteLine("\n✗ No recipes selected.");
                return;
            }

            // Dictionary to aggregate ingredient quantities
            var groceryList = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            var validRecipes = new List<Recipe>();

            // Collect ingredients from all selected recipes
            foreach (var recipeName in selectedRecipeNames)
            {
                var recipe = GetRecipe(recipeName);
                if (recipe == null)
                {
                    Console.WriteLine($"\n⚠ Warning: Recipe '{recipeName}' not found. Skipping.");
                    continue;
                }

                validRecipes.Add(recipe);

                foreach (var recipeIngredient in recipe.Ingredients)
                {
                    if (groceryList.ContainsKey(recipeIngredient.IngredientName))
                    {
                        groceryList[recipeIngredient.IngredientName] += recipeIngredient.Quantity;
                    }
                    else
                    {
                        groceryList[recipeIngredient.IngredientName] = recipeIngredient.Quantity;
                    }
                }
            }

            if (groceryList.Count == 0)
            {
                Console.WriteLine("\n✗ No ingredients found in selected recipes.");
                return;
            }

            // Display grocery list
            Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   GROCERY LIST                        ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
            Console.WriteLine($"\nRecipes selected: {validRecipes.Count}");
            
            foreach (var recipe in validRecipes)
            {
                Console.WriteLine($"  • {recipe.Name} (serves {recipe.Servings})");
            }

            Console.WriteLine($"\n─────────────────────────────────────────────────────────");
            Console.WriteLine($"Items to purchase: {groceryList.Count}");
            Console.WriteLine($"─────────────────────────────────────────────────────────\n");

            decimal estimatedTotal = 0;
            bool hasMissingIngredients = false;

            var sortedGroceryList = groceryList.OrderBy(kvp => kvp.Key);

            foreach (var item in sortedGroceryList)
            {
                var ingredient = GetIngredient(item.Key);
                
                if (ingredient != null)
                {
                    decimal itemCost = ingredient.CalculateCost(item.Value);
                    estimatedTotal += itemCost;
                    Console.WriteLine($"  [ ] {item.Value} {ingredient.Unit} of {ingredient.Name}");
                    Console.WriteLine($"      ${ingredient.PricePerUnit:F2} per {ingredient.Unit} = ${itemCost:F2}");
                }
                else
                {
                    Console.WriteLine($"  [ ] {item.Value} unit(s) of {item.Key}");
                    Console.WriteLine($"      [Price not available - ingredient not in system]");
                    hasMissingIngredients = true;
                }
                
                Console.WriteLine();
            }

            Console.WriteLine($"═════════════════════════════════════════════════════════");
            
            if (!hasMissingIngredients)
            {
                Console.WriteLine($"ESTIMATED TOTAL COST: ${estimatedTotal:F2}");
            }
            else
            {
                Console.WriteLine($"PARTIAL ESTIMATED COST: ${estimatedTotal:F2}");
                Console.WriteLine("⚠ Some ingredients missing from system - total may be higher");
            }
            
            Console.WriteLine($"═════════════════════════════════════════════════════════");

            // Offer to save to file
            Console.Write("\nWould you like to save this grocery list to a file? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                SaveGroceryListToFile(groceryList, validRecipes, estimatedTotal, hasMissingIngredients);
            }
        }

        private void SaveGroceryListToFile(Dictionary<string, decimal> groceryList, 
                                          List<Recipe> recipes, 
                                          decimal estimatedTotal, 
                                          bool hasMissingIngredients)
        {
            try
            {
                string fileName = $"GroceryList_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine("═══════════════════════════════════════════════════════");
                    writer.WriteLine("              GROCERY LIST");
                    writer.WriteLine($"              Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
                    writer.WriteLine("═══════════════════════════════════════════════════════");
                    writer.WriteLine();
                    writer.WriteLine($"RECIPES SELECTED: {recipes.Count}");
                    
                    foreach (var recipe in recipes)
                    {
                        writer.WriteLine($"  • {recipe.Name} (serves {recipe.Servings})");
                    }
                    
                    writer.WriteLine();
                    writer.WriteLine("───────────────────────────────────────────────────────");
                    writer.WriteLine($"ITEMS TO PURCHASE: {groceryList.Count}");
                    writer.WriteLine("───────────────────────────────────────────────────────");
                    writer.WriteLine();

                    var sortedList = groceryList.OrderBy(kvp => kvp.Key);
                    
                    foreach (var item in sortedList)
                    {
                        var ingredient = GetIngredient(item.Key);
                        
                        if (ingredient != null)
                        {
                            decimal itemCost = ingredient.CalculateCost(item.Value);
                            writer.WriteLine($"[ ] {item.Value} {ingredient.Unit} of {ingredient.Name}");
                            writer.WriteLine($"    ${ingredient.PricePerUnit:F2} per {ingredient.Unit} = ${itemCost:F2}");
                        }
                        else
                        {
                            writer.WriteLine($"[ ] {item.Value} unit(s) of {item.Key}");
                            writer.WriteLine($"    [Price not available]");
                        }
                        
                        writer.WriteLine();
                    }

                    writer.WriteLine("═══════════════════════════════════════════════════════");
                    
                    if (!hasMissingIngredients)
                    {
                        writer.WriteLine($"ESTIMATED TOTAL COST: ${estimatedTotal:F2}");
                    }
                    else
                    {
                        writer.WriteLine($"PARTIAL ESTIMATED COST: ${estimatedTotal:F2}");
                        writer.WriteLine("(Some ingredients missing - total may be higher)");
                    }
                    
                    writer.WriteLine("═══════════════════════════════════════════════════════");
                }

                Console.WriteLine($"\n✓ Grocery list saved to: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error saving grocery list: {ex.Message}");
            }
        }

        #endregion

        #region File I/O (Persistence)

        public void SaveData()
        {
            try
            {
                // Save ingredients
                var ingredientData = _ingredients.Values.Select(i => new
                {
                    Name = i.Name,
                    PricePerUnit = i.PricePerUnit,
                    Unit = i.Unit
                }).ToList();

                string ingredientsJson = JsonSerializer.Serialize(ingredientData, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(INGREDIENTS_FILE, ingredientsJson);

                // Save recipes
                var recipeData = _recipes.Values.Select(r => new
                {
                    Name = r.Name,
                    Servings = r.Servings,
                    Ingredients = r.Ingredients.Select(ri => new
                    {
                        IngredientName = ri.IngredientName,
                        Quantity = ri.Quantity
                    }).ToList()
                }).ToList();

                string recipesJson = JsonSerializer.Serialize(recipeData, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(RECIPES_FILE, recipesJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error saving data: {ex.Message}");
            }
        }

        public void LoadData()
        {
            try
            {
                // Load ingredients
                if (File.Exists(INGREDIENTS_FILE))
                {
                    string ingredientsJson = File.ReadAllText(INGREDIENTS_FILE);
                    var ingredientData = JsonSerializer.Deserialize<List<IngredientData>>(ingredientsJson);

                    _ingredients.Clear();
                    
                    if (ingredientData != null)
                    {
                        foreach (var data in ingredientData)
                        {
                            AddIngredient(data.Name, data.PricePerUnit, data.Unit);
                        }
                    }
                }

                // Load recipes
                if (File.Exists(RECIPES_FILE))
                {
                    string recipesJson = File.ReadAllText(RECIPES_FILE);
                    var recipeData = JsonSerializer.Deserialize<List<RecipeData>>(recipesJson);

                    _recipes.Clear();
                    
                    if (recipeData != null)
                    {
                        foreach (var data in recipeData)
                        {
                            var recipe = new Recipe(data.Name, data.Servings);
                            
                            foreach (var ingredient in data.Ingredients)
                            {
                                recipe.AddIngredient(ingredient.IngredientName, ingredient.Quantity);
                            }
                            
                            AddRecipe(recipe);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error loading data: {ex.Message}");
            }
        }

        // Helper classes for JSON serialization
        private class IngredientData
        {
            public string Name { get; set; }
            public decimal PricePerUnit { get; set; }
            public string Unit { get; set; }
        }

        private class RecipeData
        {
            public string Name { get; set; }
            public int Servings { get; set; }
            public List<RecipeIngredientData> Ingredients { get; set; }
        }

        private class RecipeIngredientData
        {
            public string IngredientName { get; set; }
            public decimal Quantity { get; set; }
        }

        #endregion
    }
}