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
		InitializeValues ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitializeValues (){
		if (btnImage == null) {
			//Set orange's values
			orange.r = 1f;
			orange.g = 0.478f;
			orange.b = 0f;
			orange.a = 1f;
			btnImage = GetComponent<Image> ();
		}
	}

	public void Select() {
		//Update button color and change temp in main screen
		if (!isSelected) {
			InitializeValues();
			btnImage.color = orange;
			isSelected = true;
			otherBtnScript.Deselect();
			weatherMan.IsFarenheit = isFarenheit;
			weatherMan.SetWeatherTemp();
		}
	}

	public void Deselect(){
		InitializeValues();
		isSelected = false;
		btnImage.color = Color.white;
	}
}
