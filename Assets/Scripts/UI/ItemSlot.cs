using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IItemSlotEventHandler
{
    void OnItemSlotPointerEnter(ItemSlot itemSlot);

    void OnItemSlotPointerExit(ItemSlot itemSlot);

    void OnItemSlotPointerClick(ItemSlot itemSlot);
}

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemCount;
    
    public Item ItemInfo { get; private set; }
    public RectTransform SlotRectTransform { get; private set; }

    private IItemSlotEventHandler _itemSlotEventHandler;

    public void Init(IItemSlotEventHandler eventHandler)
    {
        SlotRectTransform = GetComponent<RectTransform>();
        _itemSlotEventHandler = eventHandler;
        SetItem(null);
    }

    public void SetItem(Item item)
    {
        ItemInfo = item;

        bool isEmptySlot = ItemInfo == null;
        _itemImage.gameObject.SetActive(!isEmptySlot);
        _itemCount.gameObject.SetActive(!isEmptySlot);

        if (isEmptySlot)
            return;

        _itemImage.sprite = ItemInfo.Data.Sprite;

        if (item is ConsumableItem consumableItem)
            SetConsumableItem(consumableItem);
        else if (item is EquipmentItem equipmentItem)
            SetEquipmentItem(equipmentItem);
    }

    private void SetConsumableItem(ConsumableItem item)
    {
        _itemCount.text = item.Amount.ToString();
    }

    private void SetEquipmentItem(EquipmentItem item)
    {
        _itemCount.gameObject.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _itemSlotEventHandler.OnItemSlotPointerEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _itemSlotEventHandler.OnItemSlotPointerExit(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _itemSlotEventHandler.OnItemSlotPointerClick(this);
    }
}
