using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour {

	public AudioManager audioManager;
	// Use this for initialization
	void Start () {
		audioManager = GetComponent<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!audioManager.isPlaying("BeachAmbient")){
			audioManager.Play("BeachAmbient");
		}
	}
}
