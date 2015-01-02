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
	private string currLocation = "New York, NY";
	private WWW web;

	//Web connection used to test whether a locatin exists
	private WWW testWeb;
	private bool finishedTestLocSearch = false;
	private bool foundTestLocation = false;
	private string curTestLocation;

	private float kelvinTemp;
	private bool isFarenheit = true;

	//Setter for private isFarenheit field. When changed, saves to a file containing the default units
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
	public InputField locationInput;

	public ChangeTempUnits celciusBtnScript;

	// Use this for initialization
	void Awake () {
		LOCATIONFILEPATH = Application.persistentDataPath + "/location.txt";
		TEMPUNITFILEPATH = Application.persistentDataPath + "/temp.txt";

		SetUpDefaults ();

		currWebQueryUrl = WEBQUERYURL + currLocation;
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
		if (File.Exists (LOCATIONFILEPATH)) {
			currLocation = File.ReadAllText(LOCATIONFILEPATH);
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
		if (!FoundLocation(weatherData)) {
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
		locationInput.gameObject.SetActive (true);
	}

	public void HideInputAndShowText(){
		locationText.gameObject.SetActive (true);
		locationInput.gameObject.SetActive (false);
	}

	public void TrySetDefaultLocation (string location) {
		curTestLocation = location;

		//Starts Coroutine checking for the test default location
		StartCoroutine(CheckTestLocation (location));
	}

	private IEnumerator CheckTestLocation (string location){

		print ("Checking for " + curTestLocation);
		//Trying to find new test location
		finishedTestLocSearch = false;
		foundTestLocation = false;
		curTestLocation = location;

		testWeb = new WWW (WEBQUERYURL + location);

		//Search until we find whether or not the test location exists
		while (!finishedTestLocSearch) {
			if(testWeb.error != null){
				print("Error in testWeb");
				break;
			}
			//Finished the web search and can now check wether or not the location exists
			if(testWeb.isDone){
				JsonData locData = JsonMapper.ToObject(testWeb.text);
				if(FoundLocation(locData)){
					foundTestLocation = true;
					SaveDefaultLocation (location);
					print ("I have done it!!");
				}else{
					print ("Didn't find it!!");
					foundTestLocation = false;
				}
				finishedTestLocSearch = true;
				break;
			}
			yield return new WaitForSeconds(.1f);
		}
		yield return null;
	}

	private bool FoundLocation (JsonData weatherData){
		return !weatherData.Keys.Contains ("message");
	}

	private void SaveDefaultLocation (string location){
		File.WriteAllText(LOCATIONFILEPATH, location);
	}


}
