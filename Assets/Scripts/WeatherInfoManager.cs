using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;
using System.IO;

public class WeatherInfoManager : MonoBehaviour {

	private const string DEGREESSYMBOL = "\u00B0";
	private const string WEBQUERYURL  = "http://api.openweathermap.org/data/2.5/weather?q=";
	private string LOCATIONFILEPATH;
	private string TEMPUNITFILEPATH;

	private bool gotWeather = false;
	private string currWebQueryUrl;
	private WWW web;
	private float kelvinTemp;
	private bool isFarenheit = true;

	public bool IsFarenheit 
	{
		set{
			this.isFarenheit = value;
			string tempUnits = value ? "f" : "c";

			//Write the default temp units to file
			if(!File.Exists(TEMPUNITFILEPATH)){
				FileStream stream = File.Create(TEMPUNITFILEPATH);
				stream.Close();
			}

			File.WriteAllText(TEMPUNITFILEPATH, tempUnits);

		}
	}

	
	public Text tempText;
	public Text locationText;
	public InputField input;

	public ChangeTempUnits celciusBtnScript;

	// Use this for initialization
	void Awake () {
		LOCATIONFILEPATH = Application.persistentDataPath + "/location.txt";
		TEMPUNITFILEPATH = Application.persistentDataPath + "/temp.txt";

		SetUpDefaults ();
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

	void SetUpDefaults(){
		if(File.Exists(TEMPUNITFILEPATH)){
			string tempUnits = File.ReadAllText (TEMPUNITFILEPATH);
			if (tempUnits == "c") {
				isFarenheit = false;
				celciusBtnScript.Select();
			}
		}

	}

	public void FindNewLocationWeather(string location) {
		currWebQueryUrl = WEBQUERYURL + location;
		web = new WWW (currWebQueryUrl);
		gotWeather = false;
	}

	public void SetWeatherInfo() {

		JsonData weatherData = JsonMapper.ToObject(web.text);

		//Didn't find city
		if (weatherData.Keys.Contains ("message")) {
			SetLocationText("Not found");
			SetWeatherTemp("");
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

	void SetWeatherTemp (string temp){
		tempText.text = temp;
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

	public void HideInputAndShowText(){
		locationText.gameObject.SetActive (true);
		input.gameObject.SetActive (false);
	}


}
