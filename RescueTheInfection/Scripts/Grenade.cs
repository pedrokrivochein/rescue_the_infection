using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	public Rigidbody rg;
	public GameObject fire;
	// Use this for initialization
	void Start () {
		//rg = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void go(){
		Vector3 force = new Vector3(0.0F, 350.0F, 0.0F);
		//rg.AddForce(transform.forward * 350.0F);
		//rg.AddForce(force);
		StartCoroutine(destroy());
	}

	public IEnumerator destroy(){
		yield return new WaitForSeconds(3.0F);
		transform.GetChild(0).GetComponent<ParticleSystem>().Play();
		GetComponent<AudioSource>().Play();
		Destroy(this.gameObject, 2.0F);
	}
}
