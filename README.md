# InventorySystem

A console app in pure C# (.NET 8, no external libraries) that implements a game-style **inventory with stacking** and a **crafting system**.

## Why this project
Inventories and crafting are in almost every game. This project shows the logic behind them, independent of any engine, so it can be moved into Unity as-is.

## Features
- Fixed-size inventory with slots
- Item stacking with per-item `MaxStack`
- `Add` returns the amount that did not fit (overflow handling)
- Safe `Remove` (no changes if not enough items)
- Events: `ItemAdded` / `ItemRemoved` (observer pattern, like UI updates in Unity)
- Recipes and crafting with **rollback** if the result does not fit

## Run
```bash
dotnet run
```

## Project structure
| File | Purpose |
|------|---------|
| `Item.cs` | Item definition (immutable record) |
| `Inventory.cs` | Slots, stacking, add/remove, events |
| `Crafting.cs` | Recipes and crafting logic |
| `Program.cs` | Console demo |

## Possible improvements
- Unit tests (xUnit)
- Item weight / inventory weight limit
- Save/load to JSON
- Port to Unity with ScriptableObject items and a UI view subscribed to the events
