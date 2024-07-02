using System;
using UnityEngine;

[Serializable]
public abstract class ItemData : ScriptableObject
{
    public int ID;
    public string Name;
    public string Desc;
    public Sprite Sprite;

    public abstract Item CreateItem();
}



