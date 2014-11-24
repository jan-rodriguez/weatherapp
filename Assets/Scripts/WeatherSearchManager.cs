using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;

public class WeatherSearchManager : MonoBehaviour {

	private const string degreesSymbol = "\u00B0";
	private const string WEBQUERYURL  = "http://api.openweathermap.org/data/2.5/weather?q=";

	private bool gotWeather = false;
	private bool isFarenheit = true;
	private string currWebQueryUrl;

	WWW web;
	
	public Text tempText;
	public Text locationText;
	public InputField input;

	// Use this for initialization
	void Start () {
		currWebQueryUrl = WEBQUERYURL + "cambridge,ma";
		web = new WWW (currWebQueryUrl);
	}
	
	// Update is called once per frame
	void Update () {

		if (!gotWeather && web.isDone) {
			gotWeather = true;
			setWeatherInfo();
		}
	
	}

	public void FindNewLocationWeather(string location) {
		currWebQueryUrl = WEBQUERYURL + location; 

		web = new WWW (currWebQueryUrl);
		gotWeather = false;
	}

	void setWeatherInfo() {

		JsonData weatherData = JsonMapper.ToObject(web.text);

		//Didn't find city
		if (weatherData.Keys.Contains ("message")) {
			tempText.text = "Not found";
		}
		else{
			//Location
			string locationString = weatherData ["name"].ToString ();
			SetLocationText(locationString);
			
			//Set temparature
			string tempString = weatherData["main"]["temp"].ToString();
			SetTemperatureText(tempString);
		}

		
	}

	void SetLocationText(string location){
		input.gameObject.SetActive(false);
		locationText.gameObject.SetActive(true);
		locationText.text = location;
	}

	void SetTemperatureText(string kelvinTemp){
		float kelvin = float.Parse(kelvinTemp);


		if (isFarenheit) {
			int farenheit = KelvinToFarenheit(kelvin);
			tempText.text = farenheit + "°F";

		}
		else{
			int celcius = KelvinToCelcius(kelvin);
			tempText.text = celcius + "°C";
		}

	}
	
	int KelvinToFarenheit(float kelvin) {
		int celcius = (int)kelvin - 273;
		int farenheit = (int)((celcius * 1.8) + 32);
		return farenheit;
	}

	int KelvinToCelcius(float kelvin) {
		int celcius = (int)kelvin - 273;
		return celcius;
	}

	public void HideTextAndShowInput() {
		locationText.gameObject.SetActive (false);
		input.gameObject.SetActive (true);
	}
	
}
