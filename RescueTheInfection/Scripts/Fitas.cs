using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fitas : MonoBehaviour {
	public AudioManager audioManager;
	public bool nearToCatch = false;
	public bool playing = false;

	// Use this for initialization
	void Start () {
		audioManager = GetComponent<AudioManager>();
		nearToCatch = false;
		playing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(playing){
			if(!audioManager.isPlaying("Fita")){
				Destroy(this.gameObject);
			}else{
				if(Input.GetKeyDown(KeyCode.Escape)){
					audioManager.Stop("Fita");
					Destroy(this.gameObject);
				}
			}
			return;
		}
		if(Input.GetKeyDown(KeyCode.E) && nearToCatch){
			Play();
			GetComponent<Renderer>().enabled = false;
		}
	}

	public void Play(){
		audioManager.Play("Fita");
		playing = true;
	}

	public void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			nearToCatch = true;
		}
	}

	public void OnTriggerExit(Collider other){
		if(other.CompareTag("Player")){
			nearToCatch = false;
		}
	}
}
