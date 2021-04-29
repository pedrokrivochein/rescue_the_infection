using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public bool isSelected = false;
	public Text amountText;
	// Use this for initialization
	void Start () {
		GetComponent<Outline> ().enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (transform.childCount > 1 && transform.GetChild(1).GetComponent<ItemSelf>().amount > 1) {
			int amountChildInt = transform.GetChild (1).GetComponent<ItemSelf> ().amount;
			amountText.text = "x" + amountChildInt;
		} else {
			amountText.text = ""; 
		}
	}
	public void OnPointerEnter(PointerEventData eventData){
		isSelected = true;
		GetComponent<Outline> ().enabled = true;
	}
	public void OnPointerExit(PointerEventData eventData){
		isSelected = false;
		GetComponent<Outline> ().enabled = false;
	}
}
