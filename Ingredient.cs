using System;

namespace GroceryListApp
{
    /// <summary>
    /// Represents a single ingredient with a name, price, and unit of measurement.
    /// Demonstrates encapsulation with private fields and public properties.
    /// </summary>
    public class Ingredient
    {
        // Private fields (encapsulation)
        private string _name;
        private decimal _pricePerUnit;
        private string _unit;

        // Public properties with validation
        public string Name
        {
            get { return _name; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ingredient name cannot be empty.");
                _name = value.Trim();
            }
        }

        public decimal PricePerUnit
        {
            get { return _pricePerUnit; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _pricePerUnit = value;
            }
        }

        public string Unit
        {
            get { return _unit; }
            private set
            {
                _unit = string.IsNullOrWhiteSpace(value) ? "unit" : value.Trim();
            }
        }

        // Constructor
        public Ingredient(string name, decimal pricePerUnit, string unit = "unit")
        {
            Name = name;
            PricePerUnit = pricePerUnit;
            Unit = unit;
        }

        // Calculate cost for a specific quantity
        public decimal CalculateCost(decimal quantity)
        {
            return PricePerUnit * quantity;
        }

        // Override ToString for easy display
        public override string ToString()
        {
            return $"{Name} - ${PricePerUnit:F2} per {Unit}";
        }

        // For case-insensitive comparison
        public bool NameEquals(string otherName)
        {
            return Name.Equals(otherName, StringComparison.OrdinalIgnoreCase);
        }
    }
}