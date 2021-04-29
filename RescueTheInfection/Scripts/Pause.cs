using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	public GameObject pause;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(Time.timeScale != 1){
				//Time.timeScale = 1;
				//pause.SetActive(false);
				//Cursor.lockState = CursorLockMode.Locked;
				////Debug.Log("A");
			}else{
				//Time.timeScale = 0.2F;
				//Cursor.lockState = CursorLockMode.None;
				//pause.SetActive(true);
				//foreach(Transform item in pause.transform){
				//	item.gameObject.GetComponent<Animator>().Play("Animation", 0, 0.0F);
				//}
				////Debug.Log("A");
			}
		}
	}
}
