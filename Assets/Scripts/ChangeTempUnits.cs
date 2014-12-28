using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeTempUnits : MonoBehaviour {

	private Color orange = new Color ();
	private Image btnImage;

	public bool isSelected;
	public bool isFarenheit;
	public ChangeTempUnits otherBtnScript;
	public WeatherInfoManager weatherMan;


	// Use this for initialization
	void Start () {
		//Set orange's values
		orange.r = 1f;
		orange.g = 0.478f;
		orange.b = 0f;
		orange.a = 1f;

		btnImage = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetNewTempUnits() {
		//Update button color and change temp in main screen
		if (!isSelected) {
			btnImage.color = orange;
			isSelected = true;
			otherBtnScript.Deselect();
			weatherMan.IsFarenheit = isFarenheit;
			weatherMan.SetWeatherTemp();
		}
	}

	public void Deselect(){
		isSelected = false;
		btnImage.color = Color.white;
	}
}
