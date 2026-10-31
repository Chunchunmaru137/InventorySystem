namespace InventorySystem;

public sealed record Recipe(
    string Name,
    IReadOnlyDictionary<Item, int> Ingredients,
    Item Result,
    int ResultCount = 1);

public static class Crafter
{
    public static bool CanCraft(Inventory inventory, Recipe recipe) =>
        recipe.Ingredients.All(i => inventory.CountOf(i.Key) >= i.Value);

    /// <summary>Crafts the recipe. If the result does not fit, everything is rolled back.</summary>
    public static bool TryCraft(Inventory inventory, Recipe recipe)
    {
        if (!CanCraft(inventory, recipe))
            return false;

        foreach (var (item, amount) in recipe.Ingredients)
            inventory.Remove(item, amount);

        int notFitted = inventory.Add(recipe.Result, recipe.ResultCount);
        if (notFitted == 0)
            return true;

        // Rollback: take back what was added, return the ingredients
        int fitted = recipe.ResultCount - notFitted;
        if (fitted > 0)
            inventory.Remove(recipe.Result, fitted);

        foreach (var (item, amount) in recipe.Ingredients)
            inventory.Add(item, amount);

        return false;
    }
}
