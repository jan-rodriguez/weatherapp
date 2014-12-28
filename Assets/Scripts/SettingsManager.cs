using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsManager : MonoBehaviour {

	public GameObject settingsPanel;
	public GameObject weatherPanel;
	public GameObject settingsButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowSettingsMenu () {
		settingsPanel.animation.Play ("SettingScrollIn");
		weatherPanel.animation.Play ("ScrollOut");
		settingsButton.animation.Play ("SetBtnScrollOut");
	}

	public void HideSettingsMenu () {
		settingsPanel.animation.Play ("SettingScrollOut");
		weatherPanel.animation.Play ("ScrollIn");
		settingsButton.animation.Play ("SetBtnScrollIn");
	}
}
