using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;

public class WeatherInfoManager : MonoBehaviour {

	private const string degreesSymbol = "\u00B0";
	private const string WEBQUERYURL  = "http://api.openweathermap.org/data/2.5/weather?q=";

	private bool gotWeather = false;
	private string currWebQueryUrl;
	private WWW web;
	private float kelvinTemp;
	private bool isFarenheit = true;

	public bool IsFarenheit 
	{
		set{this.isFarenheit = value;}
	}

	
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
			SetWeatherInfo();
		}
	
	}

	public void FindNewLocationWeather(string location) {
		currWebQueryUrl = WEBQUERYURL + location; 
		input.gameObject.SetActive (false);
		web = new WWW (currWebQueryUrl);
		gotWeather = false;
	}

	public void SetWeatherInfo() {

		JsonData weatherData = JsonMapper.ToObject(web.text);

		//Didn't find city
		if (weatherData.Keys.Contains ("message")) {
			SetLocationText("Not found");
		}
		else{
			//Location
			string locationString = weatherData ["name"].ToString ();
			SetLocationText(locationString);
			
			//Set temparature
			string tempString = weatherData["main"]["temp"].ToString();
			kelvinTemp = float.Parse(tempString);
			SetWeatherTemp();
		}

		
	}

	void SetLocationText(string location){
		locationText.gameObject.SetActive(true);
		locationText.text = location;
	}

	public void SetWeatherTemp(){
		float kelvin = kelvinTemp;

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
