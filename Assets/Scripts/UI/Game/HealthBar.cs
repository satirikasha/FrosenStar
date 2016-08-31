using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private Slider _Slider;

	void Start () {
        _Slider = this.GetComponent<Slider>();
	}
	
	void Update () {
        _Slider.value = PlayerController.LocalPlayer.Ship.NormalizedHealth;
        if (_Slider.value == 0)
            _Slider.fillRect.gameObject.SetActive(false);

	}
}
