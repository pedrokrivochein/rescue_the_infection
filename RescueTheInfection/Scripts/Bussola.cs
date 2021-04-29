using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bussola : MonoBehaviour {

	public GameObject norte;
	public int toprota;

	// Use this for initialization
	void Start () {
		norte = GameObject.Find ("NorteX");
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion newRotation = Quaternion.LookRotation (norte.transform.position - transform.position);
			
		newRotation.x = 0.0F;
		newRotation.z = 0.0F;

		toprota = 10;

		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(0.0F, newRotation.y, 0.0F),Time.deltaTime*toprota);

	}
}
