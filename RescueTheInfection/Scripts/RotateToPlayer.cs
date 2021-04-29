using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour {

	public GameObject camera;
	public bool x = false;
	public bool y = false;
	public bool z = false;
	public float xf;
	public float yf;
	public float zf;
	// Use this for initialization
	void Start () {
		camera = GameObject.Find("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = camera.transform.position - transform.parent.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = lookRotation.eulerAngles;
		if(x){
			transform.rotation = Quaternion.Euler(rotation.y, yf, zf);
		}else if(y){
			transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
		}else if(z){
			transform.rotation = Quaternion.Euler(xf, yf, rotation.y);
		}	
	}
}
