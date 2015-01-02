using UnityEngine;
using System.Collections;

public class ObjectToggler : MonoBehaviour {

	public GameObject object1;
	public GameObject object2;

	public void Toggle(){
		object1.SetActive (!object1.activeSelf);
		object2.SetActive (!object2.activeSelf);
	}
}
