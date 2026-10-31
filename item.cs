namespace InventorySystem;

/// <summary>Item definition (like a ScriptableObject in Unity): shared data, not a specific stack.</summary>
public sealed record Item(string Id, string Name, int MaxStack);
