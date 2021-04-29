using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour {

	public bool onTrigger = false;
	public GameObject[] horda;
	// Use this for initialization
	void Start () {
		onTrigger = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnTriggerEnter(Collider other){
		if(other.transform.gameObject.CompareTag("Player")){
			//other.gameObject.transform.rotation = other.gameObject.GetComponent<PlayerC>();
			GameObject.Find("Ana").GetComponent<PlayerC>().onBorder = true;
			//onTrigger = true;
			/*for(int i = 0; i < horda.Length; i++){
				horda[i].SetActive(true);
				Destroy(GameObject.Find("PistolAmmo"));
				GameObject.Find("Pistola").GetComponent<Weapon>().bulletsLoaded = 0;
				GameObject.Find("Pistola").GetComponent<Weapon>().bulletsNotLoaded = 0;
			}*/
		}
	}

	public void OnTriggerExit(Collider other){
		if(other.transform.gameObject.CompareTag("Player")){
			onTrigger = false;
			GameObject.Find("Ana").GetComponent<PlayerC>().onBorder = false;
		}
	}
}
