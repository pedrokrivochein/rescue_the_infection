using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
	public string name;
	public float volume;
	public float pitch;
	public AudioClip clip;
	[HideInInspector]
	public AudioSource source;
	public bool loop;
	public bool playOnAwake = true;
}
