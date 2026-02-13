# Overview
This project is a C# console application that generates grocery lists based on recipes. The program stores recipes made up of ingredient lists, where each ingredient has an associated price and unit of measurement. Users can add new ingredients, assign prices to them, create recipes using those ingredients, and select which recipes to cook. Based on the selected recipes, the program automatically compiles a consolidated grocery list and calculates the total estimated cost. The application also supports saving and loading data between sessions using JSON file storage.

My goal with this software was to create a practical tool that solves a real-world problem I face regularly: planning meals and creating efficient grocery lists. I wanted to demonstrate my understanding of object-oriented programming principles, data structures, and file I/O while building something genuinely useful for my daily life. This project showcases my ability to design class hierarchies, implement encapsulation, manage collections effectively, and create a user-friendly interface.

[Software Demo Video](http://youtube.link.goes.here)

# Development Environment
This project was developed using the following tools and technologies:

* .NET 9.0 SDK
* C# Programming Language
* Visual Studio Code IDE
* Git and GitHub for version control
* System.Text.Json for JSON serialization

The application is built as a console application targeting .NET 6.0, making it cross-platform compatible with Windows, macOS, and Linux.

# Useful Websites
The following resources were helpful while developing this project:

* [Microsoft C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
* [C# Programming Guide - Collections](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/collections)
* [System.Text.Json Documentation](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview)
* [LINQ Tutorial](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/)

# AI Disclosure
I used Claude AI as a development partner throughout this project. I primarily used it to help structure the initial class architecture, review C# best practices for encapsulation and property validation, explore different collection types (Dictionary vs List) for optimal performance, and understand JSON serialization patterns.

In one specific part of the project, I asked for direct assistance with implementing the grocery list consolidation logic that combines repeated ingredients from multiple recipes. The AI suggested using a Dictionary with case-insensitive string comparison to aggregate quantities. I did not copy the implementation verbatim. I adapted the approach to fit my existing GroceryManager class structure, added additional validation for missing ingredients, integrated cost calculation logic, and added the ability to export the consolidated list to a formatted text file.

Through this process, I learned how to use StringComparer.OrdinalIgnoreCase for case-insensitive dictionary keys, implement the TryGetValue pattern for safe dictionary access, and use LINQ's OrderBy method to sort output. I felt confident using the suggestion because I fully understood the algorithm, could trace through the consolidation logic with example data, and successfully extended it with additional features like cost estimation and file export.

# Future Work
If I were to continue improving this project, I would like to:

* Add a graphical user interface (GUI) using Windows Forms or WPF
* Implement nutrition tracking (calories, protein, carbohydrates, fats)
* Add recipe scaling functionality to adjust ingredient quantities based on serving size
* Support multiple grocery stores with different prices per ingredient
* Add meal planning calendar to schedule recipes by day/week
* Implement recipe categories and search/filter functionality
* Add barcode scanning integration for quick ingredient price updates
* Create a mobile companion app for use while shopping
* Add recipe import from popular cooking websites
* Implement user accounts and cloud synchronization for multi-device access