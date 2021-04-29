using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cutscene002Zombie : MonoBehaviour {

	public GameObject target;
	public bool walk = false;
	public Vector3 rotationn;
	public bool walking = false;
	public GameObject[] points;
	public int i = 0;
	public bool saw = false;
	public float health = 100;
	public GameObject[] horda;
	public bool did = false;
	// Use this for initialization
	void Start () {
		target = GameObject.Find("Ana");
		walk = false;
		walking = false;
		saw = false;
		did = false;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Animator>().applyRootMotion = true;
		if(health <= 0){
			if(!did){
				Destroy(GameObject.Find("SawTrigger"));
				GameObject.Find("Objective").GetComponent<TextMeshProUGUI>().text = "- Mate a horda";
				did = true;
			}
			return;
		}
		if(saw){
			return;
		}
		RaycastHit hit;
		if(Vector3.Angle (target.transform.position - transform.position, transform.forward) <= 45){
			if(Physics.Linecast(transform.position, target.transform.position, out hit)){
				if (hit.collider.CompareTag("Player") && !saw) {
					//Time.timeScale = 0;
					GameObject.Find("Ana").GetComponent<Animator>().SetBool("Stealth", false);
					saw = true;
					for(int i = 0; i < horda.Length; i++){
						horda[i].SetActive(true);
					}
					GameObject.FindObjectOfType<Cutscene002Manager>().gotSaw();
				}
			}
		}
		if(walk && !walking){
			rotationn = points[i].transform.position;
			StartCoroutine(loop());
			return;
		}
	}

	public IEnumerator loop(){
		walking = true;
		while(Vector3.Distance(transform.position, rotationn) > 0.4F){
			if(saw){
				////Debug.Log("Viu");
				break;
			}
			Vector3 dir = rotationn - transform.position ;
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			Vector3 rotation = lookRotation.eulerAngles;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, rotation.y, transform.rotation.z), 6.0F * Time.deltaTime);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		walking = false;
		i++;
		i = UnityEngine.Random.Range(0, 4);
	}
}
