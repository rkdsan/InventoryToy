using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> _itemDataList;
    private static Dictionary<int, ItemData> _itemDataByID;

    private void Awake()
    {
        _itemDataByID = _itemDataList.ToDictionary(data => data.ID, data => data);
    }

    public static ItemData GetItemData(int id)
    {
        return _itemDataByID[id];
    }

}
