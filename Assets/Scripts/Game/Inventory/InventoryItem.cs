using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class InventoryItem {
    public string Name;
    public Texture2D Preview;
    [TextArea(1, 5)]
    public string Description;
}
