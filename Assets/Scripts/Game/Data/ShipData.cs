using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ShipData : IData {

   // public Vector3 Position;
   // public Quaternion Rotation;
    public float Health;
    public float Energy;

    public void GatherData() {
        //Position = PlayerController.LocalPlayer.Ship.transform.position;
        //Rotation = PlayerController.LocalPlayer.Ship.transform.rotation;
        Health = PlayerController.LocalPlayer.Ship.Health;
        Energy = PlayerController.LocalPlayer.Ship.Energy;
        Debug.Log(Energy);
    }

    public void ScatterData() {
       // PlayerController.LocalPlayer.Ship.transform.position = Position;
       // PlayerController.LocalPlayer.Ship.transform.rotation = Rotation;
        PlayerController.LocalPlayer.Ship.SetHealth(Health);
        PlayerController.LocalPlayer.Ship.SetEnergy(Energy);
    }
}
