using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour {
 
	public GameObject[] disableHere;
	public GameObject[] enableHere;
 
	// Use this for initialization
	void Start () {
	}
 
	void Update () {
	}

	/*public void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			foreach(GameObject o in disableHere){
				o.SetActive(false);
			}
			foreach(GameObject o in enableHere){
				o.SetActive(true);
			}
		}
	}*/
}
