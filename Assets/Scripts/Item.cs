using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    Etc,
}

public interface IUsableItem
{
    public bool Use();
}

public abstract class Item
{
    public ItemData Data { get; protected set; }

    public abstract ItemType GetItemType();

    public abstract int GetAmount();

    public Item(ItemData data)
    {
        Data = data;
    }
}

public class EquipmentItem : Item
{
    public EquipmentItem(EquipmentItemData data) : base(data)
    {
        
    }

    public override int GetAmount()
    {
        return 1;
    }

    public override ItemType GetItemType()
    {
        return ItemType.Equipment;
    }
}

public class ConsumableItem : Item, IUsableItem
{
    public int Amount { get; protected set; }
    public bool IsMax => Amount >= ConsumableData.MaxCount;
    public ConsumableItemData ConsumableData { get; protected set; }

    public ConsumableItem(ConsumableItemData data) : base(data)
    {
        ConsumableData = data;
        Amount = 0;
    }

    public override ItemType GetItemType()
    {
        return ItemType.Consumable;
    }
    public override int GetAmount()
    {
        return Amount;
    }

    /// <returns>더하고 남은 값을 반환</returns>
    public int AddAmount(int addValue)
    {
        int newCount = Amount + addValue;
        Amount = Mathf.Clamp(newCount, 0, ConsumableData.MaxCount);

        return Mathf.Max(0, newCount - Amount);
    }

    public bool Use()
    {
        if (Amount <= 0)
            return false;

        AddAmount(-1);
        return true;
    }
}
