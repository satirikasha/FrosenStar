using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour {

    public ShipItemTemplate TestShip;

	// Use this for initialization
	void Start () {
		var shipItem = (ShipItem)TestShip.GenerateItem();
        var ship = shipItem.Instantiate(true);
        ship.transform.position = this.transform.position;
        ship.transform.rotation = this.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
