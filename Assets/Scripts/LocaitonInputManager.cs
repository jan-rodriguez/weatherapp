using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocaitonInputManager : MonoBehaviour {

	public WeatherInfoManager weatherManager;
	public InputField input;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PerformSearch() {
		string locationInput = input.text;

		weatherManager.FindNewLocationWeather (locationInput);
	}
}
