﻿using System.Diagnostics;
using BiznesElektroniczny_scraper.API.Models;
using Newtonsoft.Json;

namespace BiznesElektroniczny_scraper.API.Services;

public class ProductsRetrieverService(IConfiguration configuration) {
    public async Task<List<Product>> GetAllProducts() {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", configuration["Paths:ProductsJsonPath"]!);

        // Read JSON file
        var json = await File.ReadAllTextAsync(jsonFilePath);
        return JsonConvert.DeserializeObject<List<Product>>(json) ?? [];
    }

    public async Task<ExtendedCategory> CreateTree() {
        var products = await GetAllProducts();

        var rootCategory = new ExtendedCategory("main");

        foreach (var product in products) {
            var categoryPath = product.CategoryName.Split('/');
            rootCategory.AddProductToCategory(categoryPath, product);
        }

        return rootCategory;
    }
}