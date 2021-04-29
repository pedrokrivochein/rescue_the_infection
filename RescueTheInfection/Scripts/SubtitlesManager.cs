using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitlesManager : MonoBehaviour {
	public TextMeshProUGUI text;
	public GameObject audioManager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startSubtitle(string subtitle){
		StartCoroutine(subtitleIEnumerator(subtitle));
	}

	public IEnumerator subtitleIEnumerator(string subtitle){
		text.text = subtitle;
		audioManager.GetComponent<AudioManager>().Play(subtitle);
		yield return new WaitForSeconds(3.0F);
		text.text = " ";
	}
}
