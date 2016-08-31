using UnityEngine;
using System.Collections;
using Tools;
using System.Collections.Generic;

public class InventoryItemsConfig : SingletonScriptableObject<InventoryItemsConfig> {
    public List<InventoryItem> Items;
}
