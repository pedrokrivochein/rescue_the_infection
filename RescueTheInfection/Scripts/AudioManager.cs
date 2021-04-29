using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public Sound[] sound;

	void Start(){
		foreach(Sound s in sound){
			s.source = gameObject.AddComponent <AudioSource>();
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.clip = s.clip;
			s.source.name = s.name;
			s.source.loop = s.loop;
			s.source.playOnAwake = s.playOnAwake;
		}
	}
	public void Play(string name){
		foreach (Sound s in sound) {
			if (s.name == name) {
				s.source.Play();
			}
		}
	}
	public void Stop(string name){
		foreach (Sound s in sound) {
			if(s.name == name){
				s.source.Stop ();
			}
		}
	}
	public bool isPlaying(string name){
		foreach (Sound s in sound) {
			if (s.name == name) {
				return s.source.isPlaying;
			}
		}
		return false;
	}
}
