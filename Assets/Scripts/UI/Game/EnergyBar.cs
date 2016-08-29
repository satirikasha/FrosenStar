using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {

    private Slider _Slider;

	void Start () {
        _Slider = this.GetComponent<Slider>();
	}
	
	void Update () {
        _Slider.value = PlayerController.LocalPlayer.Ship.NormalizedEnergy;
	}
}
