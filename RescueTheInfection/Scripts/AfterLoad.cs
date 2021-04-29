using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find("LoadForItem").SetActive(false);
		GameObject.Find("GameManager").GetComponent<LocalizationManager>().LoadLocalizedText("en.json");
		GameObject.Find("Teste").GetComponent<LocalizedText>().key = "Eu Tenho";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
