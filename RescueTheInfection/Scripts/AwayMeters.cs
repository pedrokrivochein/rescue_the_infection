using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AwayMeters : MonoBehaviour {

	public float meters;
	public GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Ana");
	}
	
	// Update is called once per frame
	void Update () {
		if(player != null){
			meters = Vector3.Distance(player.transform.position, player.GetComponent<Waypoint>().target.transform.position);
			GetComponent<TextMeshProUGUI>().text = meters.ToString("#" + "m");
		}
	}
}
