using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryViewer : MonoBehaviour, IItemSlotEventHandler
{
    [SerializeField] private ItemTooltipViewer tooltipViewer;
    [SerializeField] private ItemSlot slotPrefab;
    [SerializeField] private Transform slotParent;

    [Header("天天天天天Button天天天天天")]
    [SerializeField] private Button _equipmentItemTabButton;
    [SerializeField] private Button _consumableItemTabButton;
    [SerializeField] private Button _sortItemButton;

    private InventoryPresenter _inventoryPresenter;
    private List<ItemSlot> _slots;
    private ItemType _showItemType;

    private void Awake()
    {
        _equipmentItemTabButton.onClick.AddListener(() => SetShowItemType(ItemType.Equipment));
        _consumableItemTabButton.onClick.AddListener(() => SetShowItemType(ItemType.Consumable));
        _sortItemButton.onClick.AddListener(() => SortByItem());
        tooltipViewer.Hide();
        Init();
    }

    public void Init()
    {
        int size = 30;

        _slots = new List<ItemSlot>(size);
        for (int i = 0; i < size; i++)
        {
            var newSlot = Instantiate(slotPrefab, slotParent);
            newSlot.Init(this);
            _slots.Add(newSlot);
        }

        _inventoryPresenter = new InventoryPresenter(this, size);
        SetShowItemType(ItemType.Equipment);
    }

    public void AddItem(Item item)
    {
        _inventoryPresenter.AddItem(item);
    }

    public void OnUpdateItem(ItemType itemType, Item item)
    {
        if (_showItemType != itemType)
            return;

        var slot = _slots.FirstOrDefault(t => t.ItemInfo == item);
        if (slot == null)
        {
            slot = _slots.FirstOrDefault(t => t.ItemInfo == null);
        }

        slot.SetItem(item);
    }

    public void OnRemoveItem(ItemType itemType, Item item)
    {
        if (_showItemType != itemType)
            return;

        var slot = _slots.First(t => t.ItemInfo == item);
        slot.SetItem(null);
    }

    public void OnItemSlotPointerEnter(ItemSlot itemSlot)
    {
        if (itemSlot.ItemInfo == null)
            return;

        tooltipViewer.SetItemInfo(itemSlot.ItemInfo.Data);
        tooltipViewer.SetPosition(itemSlot.SlotRectTransform);
        tooltipViewer.Show();
    }

    public void OnItemSlotPointerExit(ItemSlot itemSlot)
    {
        tooltipViewer.Hide();
    }

    public void OnItemSlotPointerClick(ItemSlot itemSlot)
    {
        if(itemSlot.ItemInfo == null) 
            return;

        _inventoryPresenter.UseItem(itemSlot.ItemInfo);
    }

    private void UpdateItems(Item[] items)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            var item = i < items.Length ? items[i] : null;
            _slots[i].SetItem(item);
        }
    }

    private void SetShowItemType(ItemType showItemType)
    {
        _showItemType = showItemType;
        var items = _inventoryPresenter.GetAllItems(_showItemType);
        UpdateItems(items);
    }

    private void SortByItem()
    {
        var sortedItems = _inventoryPresenter.GetAllItems(_showItemType, InventoryPresenter.ItemSortType.Name);
        UpdateItems(sortedItems);
    }
}
