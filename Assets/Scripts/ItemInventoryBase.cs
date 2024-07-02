using System;
using System.Linq;

public interface IInventoryEventHandler
{
    public void OnUpdateItem(ItemType type, Item item);

    public void OnRemoveItem(ItemType type, Item item);
}

public abstract class ItemInventoryBase
{
    public abstract ItemType InventoryType { get; }

    public int Size { get; private set; }
    private IInventoryEventHandler _eventHandler;

    public ItemInventoryBase(int size, IInventoryEventHandler eventHandler)
    {
        Size = size;

        _eventHandler = eventHandler;
    }

    protected int GetEmptyItemIndex(Item[] itemArray)
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            var item = itemArray[i];
            if (item == null)
                return i;
        }

        return -1;
    }

    protected void OnChangeItem(ItemType inventoryType, Item item)
    {
        _eventHandler.OnUpdateItem(inventoryType, item);
    }

    protected void OnRemoveItem(ItemType inventoryType, Item item)
    {
        _eventHandler.OnRemoveItem(inventoryType, item);
    }

    protected void AddNewItem(Item item)
    {
        var itemArray = GetItemArray();
        int emptyIndex = GetEmptyItemIndex(GetItemArray());
        if (emptyIndex == -1)
        {
            return;
        }

        itemArray[emptyIndex] = item;
        OnChangeItem(InventoryType, item);
    }

    public abstract void AddItem(Item item);

    public void RemoveItem(Item item)
    {
        var itemArray = GetItemArray();
        int index = Array.IndexOf(itemArray, item);
        if (index == -1)
            return;

        itemArray[index] = null;
        OnRemoveItem(InventoryType, item);
    }

    public abstract Item[] GetItemArray();

}

public class ConsumableItemInventory : ItemInventoryBase
{
    private ConsumableItem[] _consumableItems;

    public ConsumableItemInventory(int size, IInventoryEventHandler eventHandler) : base(size, eventHandler)
    {
        _consumableItems = new ConsumableItem[size];
    }

    public override ItemType InventoryType => ItemType.Consumable;

    public override void AddItem(Item item)
    {
        var consumableItem = (ConsumableItem)item;

        int count = consumableItem.Amount;
        while (count > 0)
        {
            var targetItem = GetSameItem(consumableItem.Data);
            if (targetItem != null)
            {
                int beforeCount = count;
                count = targetItem.AddAmount(count);
                consumableItem.AddAmount(count - beforeCount);

                OnChangeItem(InventoryType, targetItem);
            }
            else
            {
                AddNewItem(consumableItem);
                break;
            }
        }
    }

    private ConsumableItem GetSameItem(ItemData itemData)
    {
        for (int i = 0; i < _consumableItems.Length; i++)
        {
            var item = _consumableItems[i];
            if (item != null && !item.IsMax && item.Data == itemData)
                return item;
        }

        return null;
    }

    public override Item[] GetItemArray()
    {
        return _consumableItems;
    }

    public void ConsumeItem(ConsumableItem item)
    {
        var targetItem = _consumableItems.FirstOrDefault(t => t == item);
        if (targetItem == null)
            return;

        targetItem.Use();
        if(targetItem.Amount <= 0)
            RemoveItem(targetItem);
    }
}

public class EquipmentItemInventory : ItemInventoryBase
{
    private EquipmentItem[] _equipmentItems;

    public EquipmentItemInventory(int size, IInventoryEventHandler eventHandler) : base(size, eventHandler)
    {
        _equipmentItems = new EquipmentItem[size];
    }
    public override ItemType InventoryType => ItemType.Equipment;

    public override void AddItem(Item item)
    {
        AddNewItem(item);
    }

    public override Item[] GetItemArray()
    {
        return _equipmentItems;
    }
}
