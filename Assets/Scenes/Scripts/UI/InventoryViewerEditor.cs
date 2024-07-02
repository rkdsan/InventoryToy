using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryViewer))]
public class InventoryViewerEditor : Editor
{
    public ItemData addItemData;
    public int addItemCount;

    private InventoryViewer inventoryViewer;

    private void OnEnable()
    {
        inventoryViewer = (InventoryViewer)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("天天天天天Editor Test天天天天天");

        addItemData = (ItemData)EditorGUILayout.ObjectField("AddData", addItemData, typeof(ItemData), true);
        addItemCount = EditorGUILayout.IntField("AddAmount", addItemCount);
        if (GUILayout.Button("Additem"))
        {
            AddItem();
        }

        if (GUILayout.Button("Clear Save Data"))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void AddItem()
    {
        var addItem = addItemData.CreateItem();
        if (addItem is ConsumableItem consumableItem)
            consumableItem.AddAmount(addItemCount);

        inventoryViewer.AddItem(addItem);
    }
}
