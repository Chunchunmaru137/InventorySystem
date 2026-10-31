using InventorySystem;

var wood = new Item("wood", "Wood", MaxStack: 8);
var stone = new Item("stone", "Stone", MaxStack: 8);
var stick = new Item("stick", "Stick", MaxStack: 16);
var pickaxe = new Item("pickaxe", "Stone Pickaxe", MaxStack: 1);

var stickRecipe = new Recipe("Sticks",
    new Dictionary<Item, int> { [wood] = 2 }, stick, ResultCount: 4);

var pickaxeRecipe = new Recipe("Stone Pickaxe",
    new Dictionary<Item, int> { [stone] = 3, [stick] = 2 }, pickaxe);

var inventory = new Inventory(capacity: 4);
inventory.ItemAdded += (item, n) => Console.WriteLine($"  + {n} x {item.Name}");
inventory.ItemRemoved += (item, n) => Console.WriteLine($"  - {n} x {item.Name}");

Console.WriteLine("Collecting resources:");
inventory.Add(wood, 10);
inventory.Add(stone, 5);
Print(inventory);

Craft(stickRecipe);
Craft(pickaxeRecipe);
Craft(pickaxeRecipe); // not enough stone

Console.WriteLine("Inventory overflow test:");
int left = inventory.Add(stone, 100);
Console.WriteLine($"  {left} x Stone did not fit");
Print(inventory);

void Craft(Recipe recipe)
{
    Console.WriteLine($"\nCrafting '{recipe.Name}':");
    bool ok = Crafter.TryCraft(inventory, recipe);
    Console.WriteLine(ok ? "  Success" : "  Failed (missing ingredients or no space)");
    Print(inventory);
}

static void Print(Inventory inv)
{
    Console.WriteLine("  Inventory:");
    for (int i = 0; i < inv.Slots.Count; i++)
    {
        var slot = inv.Slots[i];
        string text = slot.IsEmpty ? "(empty)" : $"{slot.Item!.Name} x{slot.Count}";
        Console.WriteLine($"   [{i}] {text}");
    }
    Console.WriteLine();
}

