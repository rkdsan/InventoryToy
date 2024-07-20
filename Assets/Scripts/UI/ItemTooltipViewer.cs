using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _ItemDesc;
    [SerializeField] private Image _itemImage;

    private RectTransform _rectTransform
    {
        get
        {
            if (_cachedRectTransform == null)
                _cachedRectTransform = GetComponent<RectTransform>();

            return _cachedRectTransform;
        }
    }
    private RectTransform _cachedRectTransform;

    public void SetItemInfo(Item item)
    {
        var itemData = item.Data;
        _itemName.text = itemData.Name;
        _ItemDesc.text = itemData.Desc;
        _itemImage.sprite = itemData.Sprite;
    }

    public void SetPosition(RectTransform slotRect)
    {
        var newPos = slotRect.position;

        var xRatio = transform.lossyScale.x;
        var yRatio = transform.lossyScale.y;

        var halfSlotX = slotRect.rect.width * 0.5f * xRatio;
        var halfSlotY = slotRect.rect.height * 0.5f * yRatio;

        var tooltipWidth = _rectTransform.rect.width * xRatio;
        var tooltipHeight = _rectTransform.rect.height * yRatio;

        //tooltip의 앵커는 Left Top
        //아래쪽에 붙어있도록 offset를 조정해준다.
        var widthOffset = -halfSlotX;
        var heightOffset = -halfSlotY;

        //화면 넘어간 경우 보정
        bool isOverX = newPos.x + widthOffset + tooltipWidth > Screen.width;
        bool isOverY = newPos.y + heightOffset - tooltipHeight < 0;
        if (isOverX)
        {
            widthOffset = halfSlotX - tooltipWidth;
        }

        if (isOverY)
        {
            heightOffset = halfSlotY + tooltipHeight;
        }

        newPos.x += widthOffset;
        newPos.y += heightOffset;

        _rectTransform.position = newPos;
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);

}
