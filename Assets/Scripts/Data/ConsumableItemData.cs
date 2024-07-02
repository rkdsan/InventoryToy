using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Item Data/ConsumableItemData")]
public class ConsumableItemData : ItemData
{
    public int MaxCount;

    public override Item CreateItem()
    {
        return new ConsumableItem(this);
    }
}