using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSound : MonoBehaviour {

	public AudioManager audioManager;
	public GameObject foot;

	// Use this for initialization
	void Start () {
		audioManager = GetComponent<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnTriggerEnter(Collider other){
		//audioManager.Play("Sand");
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.forward, out hit)){
			GameObject go = Instantiate(foot, hit.point, transform.rotation) as GameObject;
			//Destroy(go, 15.0F);
		}
	}
}
