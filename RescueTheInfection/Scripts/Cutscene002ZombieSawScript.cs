using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class Cutscene002ZombieSawScript : MonoBehaviour
{
    public EnemyNormalZombie zombieScript;
    public GameObject zombiePrefab;
    public GameObject zombieInScene;
    public GameObject[] horda;
    public bool did = false;
    public GameObject substituteZombie;
    // Start is called before the first frame update
    void Start()
    {
        zombieScript = GetComponent<EnemyNormalZombie>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(!zombieScript.enabled){
            return;
        }
        if(did){
            return;
        }
        if(Vector3.Angle (zombieScript.target.transform.position - transform.position, transform.forward) <= 45){
			zombieScript.currentAction = EnemyNormalZombie.Actions.Following;
            zombieScript.enabled = false;
            GetComponent<Animator>().SetFloat("AnimationFloat", 0.0F);
            GetComponent<NavMeshAgent>().SetDestination(transform.position);
            GetComponent<NavMeshAgent>().speed = 0.0F;
            StartCoroutine(sawPlayer());
		}
	}

    public IEnumerator sawPlayer(){
        GetComponent<Animator>().SetInteger("Cutscene002", 1);
        GameObject.Find("Cutscene002Part2GotSaw").GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(8.0F);
        zombieScript.hasSeen = true;
        GameObject newZombie = Instantiate(zombiePrefab, zombieInScene.transform.position, zombieInScene.transform.rotation) as GameObject;
        newZombie.GetComponent<EnemyNormalZombie>().hasSeen = true;
        newZombie.GetComponent<EnemyNormalZombie>().justFollow = true;
        newZombie.GetComponent<Animator>().enabled = false;
        newZombie.GetComponent<Animator>().enabled = true;
        newZombie.GetComponent<Animator>().SetFloat("AnimationFloat", 2.0F);
        GetComponent<EnemyNormalZombie>().followWaypoints = false;
        GetComponent<EnemyNormalZombie>().justFollow = true;
        GameObject go = Instantiate(substituteZombie, transform.position, transform.rotation) as GameObject;
        GetComponent<EnemyNormalZombie>().hasSeen = true;
        GetComponent<EnemyNormalZombie>().justFollow = true;
        foreach(GameObject zombie in horda){
            zombie.SetActive(true);
        }
        Destroy(zombieInScene);
        GameObject.Find("Ana").GetComponent<Animator>().SetBool("Stealth", false);
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetFloat("AnimationFloat", 2.0F);
        did = true;
        Destroy(gameObject);
        }
}
