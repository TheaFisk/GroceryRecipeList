using System;

namespace GroceryListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GroceryManager manager = new GroceryManager();
            bool running = true;

            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║   Welcome to Smart Grocery List Manager   ║");
            Console.WriteLine("╔════════════════════════════════════════════╗\n");

            while (running)
            {
                Console.WriteLine("\n═══════════════ MAIN MENU ═══════════════");
                Console.WriteLine("1. Manage Ingredients");
                Console.WriteLine("2. Manage Recipes");
                Console.WriteLine("3. Select Recipes & Generate Grocery List");
                Console.WriteLine("4. Save Data");
                Console.WriteLine("5. Load Data");
                Console.WriteLine("6. Exit");
                Console.WriteLine("═════════════════════════════════════════\n");
                Console.Write("Select an option (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageIngredientsMenu(manager);
                        break;
                    case "2":
                        ManageRecipesMenu(manager);
                        break;
                    case "3":
                        GenerateGroceryList(manager);
                        break;
                    case "4":
                        manager.SaveData();
                        Console.WriteLine("\n✓ Data saved successfully!");
                        break;
                    case "5":
                        manager.LoadData();
                        Console.WriteLine("\n✓ Data loaded successfully!");
                        break;
                    case "6":
                        Console.Write("\nWould you like to save before exiting? (y/n): ");
                        if (Console.ReadLine()?.ToLower() == "y")
                        {
                            manager.SaveData();
                            Console.WriteLine("✓ Data saved!");
                        }
                        Console.WriteLine("\nThank you for using Smart Grocery List Manager!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("\n✗ Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void ManageIngredientsMenu(GroceryManager manager)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n─────── INGREDIENT MANAGEMENT ───────");
                Console.WriteLine("1. Add New Ingredient");
                Console.WriteLine("2. View All Ingredients");
                Console.WriteLine("3. Update Ingredient Price");
                Console.WriteLine("4. Delete Ingredient");
                Console.WriteLine("5. Back to Main Menu");
                Console.WriteLine("─────────────────────────────────────\n");
                Console.Write("Select an option (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddIngredient(manager);
                        break;
                    case "2":
                        manager.DisplayAllIngredients();
                        break;
                    case "3":
                        UpdateIngredientPrice(manager);
                        break;
                    case "4":
                        DeleteIngredient(manager);
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("\n✗ Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void ManageRecipesMenu(GroceryManager manager)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n─────── RECIPE MANAGEMENT ───────");
                Console.WriteLine("1. Create New Recipe");
                Console.WriteLine("2. View All Recipes");
                Console.WriteLine("3. View Recipe Details");
                Console.WriteLine("4. Delete Recipe");
                Console.WriteLine("5. Back to Main Menu");
                Console.WriteLine("─────────────────────────────────────\n");
                Console.Write("Select an option (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateRecipe(manager);
                        break;
                    case "2":
                        manager.DisplayAllRecipes();
                        break;
                    case "3":
                        ViewRecipeDetails(manager);
                        break;
                    case "4":
                        DeleteRecipe(manager);
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("\n✗ Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddIngredient(GroceryManager manager)
        {
            Console.Write("\nEnter ingredient name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("✗ Ingredient name cannot be empty.");
                return;
            }

            Console.Write("Enter price per unit ($): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price) && price >= 0)
            {
                Console.Write("Enter unit (e.g., lb, oz, cup, each): ");
                string unit = Console.ReadLine();

                if (manager.AddIngredient(name, price, unit))
                {
                    Console.WriteLine($"\n✓ Ingredient '{name}' added successfully!");
                }
                else
                {
                    Console.WriteLine($"\n✗ Ingredient '{name}' already exists.");
                }
            }
            else
            {
                Console.WriteLine("\n✗ Invalid price. Must be a non-negative number.");
            }
        }

        static void UpdateIngredientPrice(GroceryManager manager)
        {
            Console.Write("\nEnter ingredient name to update: ");
            string name = Console.ReadLine();

            Console.Write("Enter new price per unit ($): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price) && price >= 0)
            {
                if (manager.UpdateIngredientPrice(name, price))
                {
                    Console.WriteLine($"\n✓ Price for '{name}' updated successfully!");
                }
                else
                {
                    Console.WriteLine($"\n✗ Ingredient '{name}' not found.");
                }
            }
            else
            {
                Console.WriteLine("\n✗ Invalid price. Must be a non-negative number.");
            }
        }

        static void DeleteIngredient(GroceryManager manager)
        {
            Console.Write("\nEnter ingredient name to delete: ");
            string name = Console.ReadLine();

            Console.Write($"Are you sure you want to delete '{name}'? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                if (manager.DeleteIngredient(name))
                {
                    Console.WriteLine($"\n✓ Ingredient '{name}' deleted successfully!");
                }
                else
                {
                    Console.WriteLine($"\n✗ Ingredient '{name}' not found.");
                }
            }
        }

        static void CreateRecipe(GroceryManager manager)
        {
            Console.Write("\nEnter recipe name: ");
            string recipeName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(recipeName))
            {
                Console.WriteLine("✗ Recipe name cannot be empty.");
                return;
            }

            Console.Write("Enter number of servings: ");
            if (!int.TryParse(Console.ReadLine(), out int servings) || servings <= 0)
            {
                Console.WriteLine("✗ Invalid number of servings.");
                return;
            }

            Recipe recipe = new Recipe(recipeName, servings);

            Console.WriteLine("\nAdd ingredients to the recipe (type 'done' when finished):");
            
            while (true)
            {
                Console.Write("\nIngredient name (or 'done'): ");
                string ingredientName = Console.ReadLine();

                if (ingredientName?.ToLower() == "done")
                    break;

                Console.Write("Quantity: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal quantity) || quantity <= 0)
                {
                    Console.WriteLine("✗ Invalid quantity. Skipping this ingredient.");
                    continue;
                }

                if (recipe.AddIngredient(ingredientName, quantity))
                {
                    Console.WriteLine($"✓ Added {quantity} unit(s) of {ingredientName}");
                }
                else
                {
                    Console.WriteLine($"✗ Failed to add ingredient.");
                }
            }

            if (manager.AddRecipe(recipe))
            {
                Console.WriteLine($"\n✓ Recipe '{recipeName}' created successfully with {recipe.GetIngredientCount()} ingredients!");
            }
            else
            {
                Console.WriteLine($"\n✗ Recipe '{recipeName}' already exists.");
            }
        }

        static void ViewRecipeDetails(GroceryManager manager)
        {
            Console.Write("\nEnter recipe name: ");
            string name = Console.ReadLine();

            manager.DisplayRecipeDetails(name);
        }

        static void DeleteRecipe(GroceryManager manager)
        {
            Console.Write("\nEnter recipe name to delete: ");
            string name = Console.ReadLine();

            Console.Write($"Are you sure you want to delete '{name}'? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                if (manager.DeleteRecipe(name))
                {
                    Console.WriteLine($"\n✓ Recipe '{name}' deleted successfully!");
                }
                else
                {
                    Console.WriteLine($"\n✗ Recipe '{name}' not found.");
                }
            }
        }

        static void GenerateGroceryList(GroceryManager manager)
        {
            var recipes = manager.GetAllRecipeNames();
            if (recipes.Count == 0)
            {
                Console.WriteLine("\n✗ No recipes available. Please create recipes first.");
                return;
            }

            Console.WriteLine("\n─────── AVAILABLE RECIPES ───────");
            for (int i = 0; i < recipes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {recipes[i]}");
            }
            Console.WriteLine("─────────────────────────────────\n");

            Console.WriteLine("Enter recipe numbers to include (comma-separated, e.g., 1,3,4): ");
            string input = Console.ReadLine();

            var selectedRecipes = new List<string>();
            foreach (string numStr in input.Split(','))
            {
                if (int.TryParse(numStr.Trim(), out int num) && num > 0 && num <= recipes.Count)
                {
                    selectedRecipes.Add(recipes[num - 1]);
                }
            }

            if (selectedRecipes.Count == 0)
            {
                Console.WriteLine("\n✗ No valid recipes selected.");
                return;
            }

            manager.GenerateGroceryList(selectedRecipes);
        }
    }
}
