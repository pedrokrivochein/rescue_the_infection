using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneBoss01Manager : MonoBehaviour
{
    public bool once = false;
    public PlayerC playerScript;
    // Start is called before the first frame update
    void Start(){
		playerScript = GameObject.Find("Ana").GetComponent<PlayerC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startCutscene(){
        GameObject.Find("CutsceneBoss01Manager").GetComponent<PlayableDirector>().Play();
        StartCoroutine(firstCutscene());
    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            if(!once){
                startCutscene();
                once = true;
            }
        }
    }

    public IEnumerator firstCutscene () {
		playerScript.inCutscene = 4;
		playerScript.StopAim();
		GameObject.FindObjectOfType<Waypoint>().inCutscene = true;
		////Debug.Log ("Boss01");
		yield return new WaitForSeconds (1.75F);
		////Debug.Log ("Boss01MonsterPrefab");
		playerScript.animator.SetInteger ("Action", 11);
		GameObject go = Instantiate (Resources.Load<GameObject> ("MonsterPrefab/ZumbiToStrangle")) as GameObject;
		go.transform.SetParent (playerScript.gameObject.transform);
		go.transform.localPosition = GameObject.Find ("pointToStrangleSpawn").transform.localPosition;
		go.transform.localRotation = GameObject.Find ("pointToStrangleSpawn").transform.localRotation;
		go.transform.position = GameObject.Find ("pointToStrangleSpawn").transform.position;
		go.transform.rotation = GameObject.Find ("pointToStrangleSpawn").transform.rotation;
		go.GetComponent<Animator> ().SetInteger ("Start", 1);
		playerScript.animator.SetInteger ("Start", 1);
		yield return new WaitForSeconds (13.25F);
		GameObject.Find ("BlackScreen").GetComponent<Image> ().enabled = true;
		GameObject.FindObjectOfType<CameraControl>().ChangeMode();
		//inCutscene = 1; - Maybe
		yield return new WaitForSeconds (0.3F);
		Destroy (GameObject.Find ("MiguelZombie"));
		Destroy (GameObject.Find ("pointToStrangleSpawn"));
		Destroy (go);
		//Miguel Grabbing Thing Animation
		playerScript.gameObject.transform.position = GameObject.Find ("AnaStartPointBoss01").transform.position;
		playerScript.gameObject.transform.rotation = GameObject.Find ("AnaStartPointBoss01").transform.rotation;
		//Ana Getting Up Start = 2;
		playerScript.animator.SetInteger ("Start", 2);
		yield return new WaitForSeconds (0.8F);
		yield return new WaitForSeconds (1.7F);
		GameObject.Find("BlackScreen").GetComponent<Image>().enabled = false;
		yield return new WaitForSeconds (4.5F);
		playerScript.animator.SetInteger ("Start", 3);
		playerScript.adjustCamera = 5;
		//StartCoroutine(GameObject.Find("Miguel").GetComponent<Boss1>().specialAttack(1));
		yield return new WaitForSeconds (0.4F);
		//Time.timeScale = 0.05F;
		//Time.fixedDeltaTime = Time.timeScale * 0.02F;
		//playerScript.inSlowMotion = true;
		GameObject.Find ("Canvas").transform.GetChild (2).transform.gameObject.SetActive (true);
		yield return new WaitForSeconds (1.0F);
		playerScript.inCutscene = 0;
		playerScript.animator.SetInteger ("Action", 0);
		yield return new WaitForSeconds (1.0F);
		GameObject.Find("Miguel").GetComponent<Boss1>().currentAction = Boss1.Actions.following;
		GameObject.Find("Miguel").GetComponent<Animator>().SetBool ("Following", true);
	}
}
