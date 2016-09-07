using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class StackableItemWrapper {
    public int Quantity;
    public InventoryItemTemplate Template;

    public InventoryItem GenerateItem() {
        if (Template != null) {
            var item = Template.GenerateItem();
            var quantity = item.GetType().GetField("Quantity");
            if (quantity != null) {
                if (Quantity > 0) {
                    quantity.SetValue(item, Quantity);
                }
                else {
                    return null;
                }
            }
            return item;
        }
        return null;
    }
}
