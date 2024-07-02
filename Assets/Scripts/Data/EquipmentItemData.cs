using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EquipmentItemData", menuName = "Item Data/EquipmentItemData")]
public class EquipmentItemData : ItemData
{
    public override Item CreateItem()
    {
        return new EquipmentItem(this);
    }
}