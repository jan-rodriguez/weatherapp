using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocaitonInputManager : MonoBehaviour {

	public WeatherInfoManager weatherManager;
	public InputField input;
	public InputField defaultLocInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PerformSearch() {

		string locationInput = input.text;

		//Assure location input isn't empty
		if(!string.IsNullOrEmpty(locationInput)){
			weatherManager.FindNewLocationWeather (locationInput);
			input.text = "";
		}

		weatherManager.HideInputAndShowText ();
	}

	public void SetDefaultLocation() {

		string locationInput = defaultLocInput.text;

		print ("Trying to set " + locationInput);
		//Assure location input isn't empty
		if(!string.IsNullOrEmpty(locationInput)){
			weatherManager.TrySetDefaultLocation (locationInput);
			input.text = "";
		}
	}
}
