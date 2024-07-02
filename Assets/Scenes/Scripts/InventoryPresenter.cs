
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryPresenter : IInventoryEventHandler
{
    public enum ItemSortType
    {
        None,
        Name,
        Trim,
    }

    private InventoryManager _inventoryManager;
    private InventoryViewer _inventoryViewer;

    public InventoryPresenter(InventoryViewer inventoryViewer, int size)
    {
        _inventoryViewer = inventoryViewer;
        _inventoryManager = new InventoryManager(size, this);

        LoadInventory(ItemType.Equipment);
        LoadInventory(ItemType.Consumable);
    }

    public void AddItem(Item item)
    {
        _inventoryManager.AddItem(item);
        SaveInventory(item.GetItemType());
    }

    public void UseItem(Item item)
    {
        _inventoryManager.UseItem(item);
        SaveInventory(item.GetItemType());
    }

    public Item[] GetAllItems(ItemType itemType, ItemSortType sortType = ItemSortType.None)
    {
        var allItems = _inventoryManager.GetAllItems(itemType);
        switch (sortType)
        {
            case ItemSortType.Trim: return allItems.Where(t => t != null).ToArray();
            case ItemSortType.Name: return allItems.Where(t => t != null)
                                                   .OrderBy(t => t.Data.name)
                                                   .ToArray();
        }

        return allItems;
    }

    public void OnUpdateItem(ItemType itemType, Item item)
    {
        _inventoryViewer.OnUpdateItem(itemType, item);
    }

    public void OnRemoveItem(ItemType itemType, Item item)
    {
        _inventoryViewer.OnRemoveItem(itemType, item);
    }

    private void LoadInventory(ItemType itemType)
    {
        var saveKey = GetSaveKey(itemType);
        if (!PlayerPrefs.HasKey(saveKey))
            return;

        var json = PlayerPrefs.GetString(saveKey);
        var saveData = JsonUtility.FromJson<DataListWrapper<ItemSaveData>>(json);
        var items = saveData.DataList.Select(saveData =>
        {
            var itemData = DataManager.GetItemData(saveData.ItemDataID);
            var item = itemData.CreateItem();
            if(item is ConsumableItem consumableItem)
            {
                consumableItem.AddAmount(saveData.Amount);
            }

            return item;
        });

        foreach (var item in items)
        {
            _inventoryManager.AddItem(item);
        }
    }

    private void SaveInventory(ItemType itemType)
    {
        var items = _inventoryManager.GetAllItems(itemType);
        var saveItemData = items.Where(item => item != null).Select(item => new ItemSaveData(item.Data.ID, item.GetAmount())).ToList();
        string json = JsonUtility.ToJson(new DataListWrapper<ItemSaveData>(saveItemData));

        var saveKey = GetSaveKey(itemType);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    private string GetSaveKey(ItemType itemType)
    {
        return $"{itemType}Inventory";
    }
}

[System.Serializable]
public class DataListWrapper<T>
{
    public List<T> DataList;
    
    public DataListWrapper(List<T> dataList)
    {
        DataList = dataList;
    }
}

[System.Serializable]
public class ItemSaveData
{
    public int ItemDataID;
    public int Amount;

    public ItemSaveData(int dataID, int amount)
    {
        ItemDataID = dataID;
        Amount = amount;
    }
}