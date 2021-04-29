using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoLimitations : MonoBehaviour {

	public GameObject player;
	public SubtitlesManager subtitlesManager;
	public int warning = 0;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Ana");
		subtitlesManager = GameObject.Find("SubtitlesManager").GetComponent<SubtitlesManager>();
		warning = 0;
		InvokeRepeating("warn", 0.0F, 5.0F);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void warn(){
		if(Vector3.Distance(player.transform.position, transform.position) < 10.0F){
			if(warning == 0){
				subtitlesManager.startSubtitle("Ana caminho errado.");
				warning++;
			}else if(warning == 1){
				subtitlesManager.startSubtitle("Ana a cabana fica seguindo norte.");
				warning++;
			}else if(warning == 2){
				subtitlesManager.startSubtitle("Ana caminho errado.");
				warning = 0;
			}
		}
	}
}
