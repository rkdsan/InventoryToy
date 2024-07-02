
public class InventoryManager
{
    private ConsumableItemInventory _consumableItemInventory;
    private EquipmentItemInventory _equipmentItemInventory;

    public InventoryManager(int size, IInventoryEventHandler eventHandler)
    {
        _consumableItemInventory = new ConsumableItemInventory(size, eventHandler);
        _equipmentItemInventory = new EquipmentItemInventory(size, eventHandler);
    }

    public void AddItem(Item item)
    {
        var targetInventory = GetSubInventory(item.GetItemType());
        targetInventory.AddItem(item);
    }

    public void RemoveItem(Item item)
    {
        var targetInventory = GetSubInventory(item.GetItemType());
        targetInventory.RemoveItem(item);
    }

    public Item[] GetAllItems(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Equipment: return _equipmentItemInventory.GetItemArray();
            case ItemType.Consumable: return _consumableItemInventory.GetItemArray();
        }

        return null;
    }

    public void UseItem(Item item)
    {
        if (item is ConsumableItem consumableItem)
        {
            _consumableItemInventory.ConsumeItem(consumableItem);
        }
    }

    private ItemInventoryBase GetSubInventory(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Equipment: return _equipmentItemInventory;
            case ItemType.Consumable: return _consumableItemInventory;
        }

        return null;
    }
}
