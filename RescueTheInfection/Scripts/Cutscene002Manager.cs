using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Cutscene002Manager : MonoBehaviour {

	public GameObject firstMapTrigger;
	public GameObject secondMapTrigger;
	public PlayerC playerScript;
	public GameObject[] thingsToActivate;
	public GameObject explosion;
	public GameObject cutsceneCamera;
	public GameObject mainCamera;
	public GameObject playerTeleportPoint;
	public TextMeshProUGUI text;
	public GameObject cutscene002Borders;
	public GameObject cutscene002Borders2;
	public GameObject blackScreen;
	public bool did = false;
	public bool secondDid = false;
	public GameObject substituteZombie;
	public GameObject audioManager;
	// Use this for initialization
	void Start () {
		playerScript = GameObject.Find("Ana").GetComponent<PlayerC>();
		secondMapTrigger.SetActive(false);
		for(int i = 0; i < thingsToActivate.Length; i++){
			thingsToActivate[i].SetActive(false);
		}
		did = true;
		secondDid = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerScript.inCutscene == 10){
			//GameObject.Find("ActionText").GetComponent<TextMeshProUGUI>().text = "[Q] To Cover";
			if(Input.GetKeyDown(KeyCode.Q)){
				playerScript.animator.SetInteger("Action", 11);
				playerScript.transform.gameObject.GetComponent<CoverSystem>().getInCover();
				text.GetComponent<TextMeshProUGUI>().text = "";
				//GameObject.Find("GameManager").GetComponent<SaveGame>().Save();
				//GameObject.Find("ActionText").GetComponent<TextMeshProUGUI>().text = "";
			}
		}
	}

	public void startFirstMapTriggerIEnumerator(){
		StartCoroutine(firstMapTriggerIEnumerator());
	}

	public void startSecondMapTriggerIEnumerator(){
		StartCoroutine(secondMapTriggerIEnumerator());
	}
	bool didd = false;
	public IEnumerator firstMapTriggerIEnumerator(){
		if(!didd){
			didd = true;
			GameObject.Find("Cutscene002Part0").GetComponent<PlayableDirector>().Play();
			audioManager.GetComponent<AudioManager>().Stop("BeachAmbient");
			audioManager.GetComponent<AudioManager>().Play("ForestAmbient");
			GameObject.Find("Cutscene001Borders").SetActive(false);
			secondMapTrigger.SetActive(true);
			playerScript.animator.SetInteger("Action", 11);
			yield return new WaitForSeconds(5.0F);
			playerScript.transform.gameObject.transform.position = GameObject.Find("Cutscene002Part0AnaTeleportPoint").transform.position;
			playerScript.transform.gameObject.transform.rotation = GameObject.Find("Cutscene002Part0AnaTeleportPoint").transform.rotation;
			playerScript.animator.SetInteger("Cutscene002", 1);
			yield return new WaitForSeconds(3.0F);
			GameObject.Find("MainCamera").GetComponent<CameraCollision>().speed = 10.0F;
			for(int i = 0; i < thingsToActivate.Length; i++){
				thingsToActivate[i].SetActive(true);
			}
			//explosion.SetActive(true);
			GameObject.Find("MainCamera").GetComponent<CameraCollision>().ShakeOnce(1.0F, 0.1F);
			playerScript.animator.SetInteger("Cutscene002", 2);
			while(GameObject.Find("MainCamera").GetComponent<CameraCollision>().distCam < 1.7F){
				GameObject.Find("MainCamera").GetComponent<CameraCollision>().distCam += 0.8F * Time.deltaTime;
				yield return new WaitForSeconds(Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime * Time.deltaTime);
			}
			GameObject.Find("MainCamera").GetComponent<CameraCollision>().inCutscene = false;
			yield return new WaitForSeconds(1.0F);
			yield return new WaitForSeconds(2.0F);
			//explosion.SetActive(false);
			playerScript.inCutscene = 0;
			playerScript.animator.SetInteger("Action", 0);
		}
	}

	public IEnumerator secondMapTriggerIEnumerator(){
		GetComponent<PlayableDirector>().Play();
		playerScript.animator.SetInteger("Action", 11);
		yield return new WaitForSeconds(3.5F);
		thingsToActivate[1].GetComponent<Animator>().SetInteger("Cutscene002", 1);
		yield return new WaitForSeconds(1.0F);
		GameObject.Find("Grenade").GetComponent<Grenade>().go();
		playerScript.transform.gameObject.transform.position = playerTeleportPoint.transform.position;
		playerScript.transform.gameObject.transform.rotation = playerTeleportPoint.transform.rotation;
		playerScript.gameObject.GetComponent<Waypoint>().oldPosition = playerTeleportPoint.transform.position;
		playerScript.animator.SetInteger("Action", 0);
		playerScript.enabled = true;
		playerScript.inCutscene = 0;
		playerScript.animator.SetInteger("Action", 0);
		playerScript.animator.SetBool("Stealth", true);
		yield return new WaitForSeconds(2.2F);
		thingsToActivate[2].GetComponent<Animator>().SetInteger("Cutscene002", 2);
		thingsToActivate[3].GetComponent<Animator>().SetInteger("Cutscene002", 3);
		thingsToActivate[2].GetComponent<AudioSource>().enabled = false;
		thingsToActivate[3].GetComponent<AudioSource>().enabled = false;
		substituteZombie.SetActive(true);
		substituteZombie.transform.position = GameObject.Find("SubstituteZombieTeleportPoint").transform.position;
		substituteZombie.transform.rotation = GameObject.Find("SubstituteZombieTeleportPoint").transform.rotation;
		Destroy(thingsToActivate[1].gameObject);
		yield return new WaitForSeconds(4.0F);
		GameObject.Find("ObjectiveTutorialManager").GetComponent<ObjectiveTutorial>().setObjectiveOrTutorial("Go to the other side");
		GameObject.Find("Ana").GetComponent<Waypoint>().target = GameObject.Find("WaypointCutscene002");
	}

	public void gotSaw(){
		return;
		if(did){
			return;
		}else{
			did = true;
		}
		Vector3 dir = thingsToActivate[1].transform.position - playerScript.transform.gameObject.transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = lookRotation.eulerAngles;
		//thingsToActivate[1].transform.rotation = Quaternion.Slerp(thingsToActivate[1].transform.rotation, Quaternion.Euler(thingsToActivate[1].transform.rotation.x, rotation.y, thingsToActivate[1].transform.rotation.z), 6.0F * Time.deltaTime);
		thingsToActivate[1].transform.LookAt(playerScript.transform.gameObject.transform.position);
		thingsToActivate[1].GetComponent<Animator>().SetInteger("Cutscene002", 4);
		GameObject.Find("Cutscene002Part2GotSaw").GetComponent<PlayableDirector>().Play();
		StartCoroutine(gotSawIEnumerator());
		GameObject.Find("Ana").GetComponent<Animator>().SetBool("Stealth", false);
	}
	public IEnumerator gotSawIEnumerator(){
		yield return new WaitForSeconds(3.0F);
		//playerScript.transform.gameObject.transform.position = playerTeleportPoint.transform.position;
		//playerScript.transform.gameObject.transform.rotation = playerTeleportPoint.transform.rotation;
		//playerScript.animator.SetInteger("Cutscene002", 5);
		thingsToActivate[1].GetComponent<Animator>().SetInteger("Cutscene002", 1);
		substituteZombie.SetActive(true);
		substituteZombie.transform.position = thingsToActivate[1].transform.position;
		substituteZombie.transform.rotation = thingsToActivate[1].transform.rotation;
		Destroy(thingsToActivate[1].gameObject);
		//playerScript.inCutscene = 11;
		thingsToActivate[1].GetComponent<Cutscene002Zombie>().saw = false;
		thingsToActivate[1].GetComponent<Cutscene002Zombie>().walking = false;
		//yield return new WaitForSeconds(1.0F);
		//playerScript.animator.SetInteger("Cutscene002", 2);
	}
}
