using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Container : Inventory {

    public List<StackableItemWrapper> Templates;

    void Awake() {
        AddItems(Templates.Select(_ => _.GenerateItem()), this);
    }

    public void OnTriggerEnter(Collider other) {
        var player = other.GetComponentInParent<PlayerController>();
        if(player != null) {
            TransitAll(this, player.GetComponent<Inventory>());
        }
    }
}
