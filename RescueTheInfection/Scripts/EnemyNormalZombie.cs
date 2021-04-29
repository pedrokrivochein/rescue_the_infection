using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*Thigs To Do
- Search System
- Injure System
- Injure Animations
- Trow Things
 */

public class EnemyNormalZombie : MonoBehaviour {
	public float health;
	public GameObject target;
	public float maxDistanceToSee = 200;
	public enum Actions{Idle, Following, Searching, Attacking};
	public Actions currentAction;
	public NavMeshAgent navMeshAgent;
	public bool attacking = false;
	public Animator animator;
	public Vector3 oldPosition;
	public float angle;
	public bool justFollow = false;
	public Transform[] waypoints;
	public bool followWaypoints = false;
	public bool hasSeen = false;
	public bool startAnimation = false;
	public string startAnimationString = "";
	public bool useDistance = false;
	public float useDistanceFloat = 0.0F;
	public bool willCrawl = false;
	public Vector3 startPosition;
	Transform head;
	public GameObject hips;
	Renderer rend;
	// Use this for initialization
	void Start () {
		target = GameObject.Find("Ana");
		currentAction = Actions.Idle;
		navMeshAgent = GetComponent<NavMeshAgent>();
		attacking = false;
		animator = GetComponent<Animator>();
		startPosition = transform.position;
		head = animator.GetBoneTransform(HumanBodyBones.Head);
		rend = GetComponent<Renderer>();
		navMeshAgent.angularSpeed = 1500.0F;
	}
	void Awake(){
		animator = GetComponent<Animator>();
	}

	int nextWaypoint = 0;
	bool did = false;
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(transform.position, target.transform.position);
		angle = Vector3.Angle (target.transform.position - transform.position, transform.forward);
		if(startAnimation){
			if(!did && startAnimationString != ""){
				if(startAnimationString == "WalkAround"){
					currentAction = Actions.Searching;
					animator.SetFloat("AnimationFloat", 1.0F);
					StartCoroutine(Searching());
					did = true;
				}else{
					animator.Play(startAnimationString);
					did = true;
				}
			}
			if(useDistance){
				if(Vector3.Distance(transform.position, target.transform.position) <= useDistanceFloat){
					startAnimation = false;
					hasSeen = true;
					justFollow = true;
					animator.SetInteger("Hurt", 0);
					if(willCrawl){
						animator.SetInteger("Hurt", 6);
					}
				}
				return;
			}
		}
		if(followWaypoints && !hasSeen){
			if(currentAction == Actions.Searching){
				StartCoroutine(waypointEnumerator());
			}else{
				currentAction = Actions.Searching;
			}
		}
		////Debug.Log("Aconteceu");
		if(justFollow){
			currentAction = Actions.Following;
			justFollow = false;
		}
		if(distance > maxDistanceToSee){
			return;
		}
		if(currentAction == Actions.Idle){
			if(Vector3.Angle (target.transform.position - transform.position, transform.forward) <= 60){
				RaycastHit hit;
				if(Physics.Linecast(transform.position + new Vector3(0.0F, 1.0F, 0.0F), target.transform.position + new Vector3(0.0F, 1.0F, 0.0F), out hit)){
					if(hit.collider.gameObject.CompareTag("Player")){
						currentAction = Actions.Following;
						if(!hasSeen){
							hasSeen = true;
							startAnimation = false;
						}
					}
				}
			}
		}
		
		else if(currentAction == Actions.Following) {
			if(hasSeen){
				StopCoroutine(Searching());
				if(distance < 1.0F){
					currentAction = Actions.Attacking;
					StartCoroutine(Attack(Mathf.RoundToInt(UnityEngine.Random.Range(1.0F, 4.0F)), 3.0F));
				}else if(!attacking){
					navMeshAgent.SetDestination(target.transform.position);
				}
			}
		}

		else if(currentAction == Actions.Attacking){
			navMeshAgent.SetDestination(transform.position);
			animator.SetFloat("AnimationFloat", 0.0F);
		}

		else if(currentAction == Actions.Searching){
			if(Vector3.Angle (target.transform.position - transform.position, transform.forward) <= 60){
				RaycastHit hit;
				if(Physics.Linecast(transform.position + new Vector3(0.0F, 1.0F, 0.0F), target.transform.position + new Vector3(0.0F, 1.0F, 0.0F), out hit)){
					if(hit.collider.gameObject.CompareTag("Player")){
						/*if(!hasSeen){
							if(startAnimationString != "WalkAround"){
							return;
							}
						}*/
						currentAction = Actions.Following;
						if(!hasSeen){
							hasSeen = true;
						}
						startAnimation = false;
						StopCoroutine(Searching());
					}
				}
			}
		}

		if(transform.position != oldPosition){
			float speed = animator.GetFloat("AnimationFloat");
			if(currentAction == Actions.Following){
				if(speed < 2.0F){
					animator.SetFloat("AnimationFloat", speed + 3.0F * Time.deltaTime);
				}
			}else if(currentAction == Actions.Searching){
				if(speed < 1.0F){
					animator.SetFloat("AnimationFloat", speed + 3.0F * Time.deltaTime);
				}else if(speed > 1.0F){
					animator.SetFloat("AnimationFloat", speed - 3.0F * Time.deltaTime);
				}
			}else if(currentAction == Actions.Idle){
				if(speed > 0.0F){
					animator.SetFloat("AnimationFloat", speed - 3.0F * Time.deltaTime);
				}
			}else if(currentAction == Actions.Attacking){
			}
		}
		oldPosition = transform.position;
	}
	public void LateUpdate(){
		/*if(angle < 60){
			head.LookAt(target.transform.position + new Vector3(0.0F, 1.5F, 0.0F));
		}*/
	}
	public IEnumerator Attack(int i, float time){
		attacking = true;
		if(i == 1){
			time = 4.6F;
		}else if(i == 2){
			time = 2.6F;
		}else if(i == 3){
			time = 3.8F;
		}else if(i == 4){
			time = 3.36F;
		}
		animator.SetInteger("AttackAnimation", i);
		//transform.LookAt(target.transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 10.0F * Time.deltaTime);
		//for(int r = 0; r < 10; r++)transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 6.0F * Time.deltaTime);
		yield return new WaitForSeconds(0.4F);
		target.GetComponent<PlayerC>().health -= UnityEngine.Random.Range(7, 30);
		yield return new WaitForSeconds(time - 0.4F);
		animator.SetInteger("AttackAnimation", 0);
		if(Vector3.Angle (target.transform.position - transform.position, transform.forward) <= 120.0F){
			currentAction = Actions.Following;
		}else{
			currentAction = Actions.Searching;
			StartCoroutine(Searching());
		}
		attacking = false;
	}
	public IEnumerator Searching(){
		float range = 10.0F;
		Vector3 keepPosition = transform.position;
		if(startAnimationString == "WalkAround"){
			keepPosition = startPosition;
			range = 15.0F;
		}
		Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += keepPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, range, 1);
        Vector3 finalPosition = hit.position;
		navMeshAgent.SetDestination(finalPosition);
		yield return new WaitForSeconds(3.0F);
		if(currentAction == Actions.Following){
			yield return true;
		}
		randomDirection = Random.insideUnitSphere * range;
        randomDirection += keepPosition;
        NavMesh.SamplePosition(randomDirection, out hit, range, 1);
        finalPosition = hit.position;
		navMeshAgent.SetDestination(finalPosition);
		yield return new WaitForSeconds(3.0F);
		if(currentAction == Actions.Following){
			yield return true;
		}
		randomDirection = Random.insideUnitSphere * range;
        randomDirection += keepPosition;
        NavMesh.SamplePosition(randomDirection, out hit, range, 1);
        finalPosition = hit.position;
		navMeshAgent.SetDestination(finalPosition);
		if(startAnimationString == "WalkAround" && !hasSeen){
			currentAction = Actions.Searching;
			while(animator.GetFloat("AnimationFloat") > 0.1F){
				animator.SetFloat("AnimationFloat", animator.GetFloat("AnimationFloat") - Time.deltaTime);
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(1.5F, 3.7F));
			while(animator.GetFloat("AnimationFloat") < 1.0F){
				animator.SetFloat("AnimationFloat", animator.GetFloat("AnimationFloat") + Time.deltaTime);
			}
			StartCoroutine(Searching());
		}else if(hasSeen){
			currentAction = Actions.Following;
			
		}else{
			currentAction = Actions.Idle;
		}
	}
	public IEnumerator waitForSearch(){
		yield return null;
	}

	public void takeHealth(float damage){
		health -= damage;
	}
	void OnAnimatorMove(){
		if(navMeshAgent)
    	navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude * UnityEngine.Random.Range(0.8F, 1.0F);
		////Debug.Log("A");
 	}
	public IEnumerator waypointEnumerator(){
		while (Vector3.Distance(transform.position, waypoints[nextWaypoint].position) > 0.5F){
			if(!hasSeen){
				navMeshAgent.SetDestination(waypoints[nextWaypoint].position);
			}
			yield return new WaitForSeconds(1.0F);
		}
		if(nextWaypoint == waypoints.Length - 1){
			nextWaypoint = 0;
		}else{
			nextWaypoint++;
		}
	}
}
