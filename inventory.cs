namespace InventorySystem;

public class Slot
{
    public Item? Item { get; set; }
    public int Count { get; set; }
    public bool IsEmpty => Item is null;
}

public class Inventory
{
    private readonly Slot[] _slots;

    public event Action<Item, int>? ItemAdded;
    public event Action<Item, int>? ItemRemoved;

    public IReadOnlyList<Slot> Slots => _slots;

    public Inventory(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        _slots = new Slot[capacity];
        for (int i = 0; i < capacity; i++)
            _slots[i] = new Slot();
    }

    public int CountOf(Item item) => _slots.Where(s => s.Item == item).Sum(s => s.Count);

    /// <summary>Adds items, filling existing stacks first. Returns how many did NOT fit.</summary>
    public int Add(Item item, int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        int remaining = amount;

        // 1. Top up existing stacks
        foreach (var slot in _slots)
        {
            if (remaining == 0) break;
            if (slot.Item == item && slot.Count < item.MaxStack)
            {
                int toAdd = Math.Min(item.MaxStack - slot.Count, remaining);
                slot.Count += toAdd;
                remaining -= toAdd;
            }
        }

        // 2. Use empty slots
        foreach (var slot in _slots)
        {
            if (remaining == 0) break;
            if (slot.IsEmpty)
            {
                int toAdd = Math.Min(item.MaxStack, remaining);
                slot.Item = item;
                slot.Count = toAdd;
                remaining -= toAdd;
            }
        }

        int added = amount - remaining;
        if (added > 0)
            ItemAdded?.Invoke(item, added);

        return remaining;
    }

    /// <summary>Removes items. Returns false (and changes nothing) if there are not enough.</summary>
    public bool Remove(Item item, int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));
        if (CountOf(item) < amount)
            return false;

        int remaining = amount;
        for (int i = _slots.Length - 1; i >= 0 && remaining > 0; i--)
        {
            var slot = _slots[i];
            if (slot.Item != item) continue;

            int toTake = Math.Min(slot.Count, remaining);
            slot.Count -= toTake;
            remaining -= toTake;

            if (slot.Count == 0)
                slot.Item = null;
        }

        ItemRemoved?.Invoke(item, amount);
        return true;
    }
}
