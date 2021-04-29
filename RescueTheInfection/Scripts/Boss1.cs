using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Boss1 : MonoBehaviour {
	public float health;
	public float maxHealth;
	public enum Actions { cutscene, following, normalAttack, jumpAttack, dash, runTowards, runBackwards, confused, takeDown, agony};
 	public Actions currentAction;
 	public Animator animator;
 	public NavMeshAgent navMeshAgent;
 	public GameObject player;
	public float distance;
 	public float specialAttackCooldown;
	public float healthBeforeNormalAttack;
	public Transform head;
	public Transform chest;
	public GameObject dustParicle;
	public Material bloodOnDamage;

 	void Start () {
 		health = maxHealth;
 		animator = GetComponent<Animator> ();
 		navMeshAgent = GetComponent<NavMeshAgent> ();
 		player = GameObject.Find ("Ana");
		head = animator.GetBoneTransform(HumanBodyBones.Head);
		chest = animator.GetBoneTransform(HumanBodyBones.Chest);
		specialAttackCooldown = 7.0F;
	}

	void Update () {
		if (currentAction == Actions.cutscene) {
			return;
		}
		if(health <= 0){
			GameObject.FindObjectOfType<Boss1TriggerLeave>().dead = true;
			return;
		}
		distance = Vector3.Distance (transform.position, player.transform.position);
		if (currentAction == Actions.agony) {
			return;
		}
		if (currentAction == Actions.following) {
			if (distance < 1.0F) {
				animator.SetBool ("Following", false);
				navMeshAgent.SetDestination (player.transform.position);
				StartCoroutine(normalAttack());
				healthBeforeNormalAttack = health;
				return;
			}
			animator.SetBool ("Following", true);
			navMeshAgent.SetDestination (player.transform.position);
			//navMeshAgent.speed = 5.0F;
			if (specialAttackCooldown <= 0.0F) {
				int attackInt = (int) UnityEngine.Random.Range (0.0F, 4.0F);
				specialAttackCooldown = UnityEngine.Random.Range (2.0F, 5.0F);
				StartCoroutine (specialAttack (attackInt));
			} else {
				specialAttackCooldown -= Time.deltaTime;
			}
			return;
		}

		if (currentAction == Actions.normalAttack) {
			if(health < healthBeforeNormalAttack - 50){
				currentAction = Actions.runBackwards;
			}
			
			return;
		}

		if (currentAction == Actions.jumpAttack) {
			return;
		}

		if (currentAction == Actions.runTowards) {
			navMeshAgent.SetDestination(player.transform.position);
			if(distance < 1.0F){
				currentAction = Actions.takeDown;
				animator.SetBool("LegTakeDown", true);
				StopAllCoroutines();
				StartCoroutine(takeDown());
			}
			return;
		}

		if (currentAction == Actions.runBackwards) {
			if(distance > 4.0F){
				animator.SetBool ("RunBackwards", false);
				navMeshAgent.SetDestination(transform.position);
				StartCoroutine(specialAttack(0));
				return;
			}
			animator.SetBool ("RunBackwards", true);
			//transform.LookAt(player.transform.position);
			navMeshAgent.SetDestination(transform.position + new Vector3(-40, 0, 0));
			return;
		}
	}
	
	public IEnumerator takeDown(){
		navMeshAgent.SetDestination(player.transform.position);
		navMeshAgent.speed = 5.0F;
		player.GetComponent<Animator>().SetBool("Aiming", false);
		player.GetComponent<PlayerC>().cmCam.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 50.0F;
		player.GetComponent<Animator>().SetInteger("Action", 11);
		player.GetComponent<Animator>().Play("DoubleLegTakedown", 0, 0.0F);
		yield return new WaitForSeconds(0.6F);
		foreach(Collider c in GetComponentsInChildren<Collider>())c.enabled = false;
		navMeshAgent.SetDestination(player.transform.position);
		yield return new WaitForSeconds(1.0F);
		navMeshAgent.SetDestination(player.transform.position);
		yield return new WaitForSeconds(1.0F);
		if(distance < 2.0F){
			player.GetComponent<PlayerC>().callTakeDamage(UnityEngine.Random.Range(20.0F, 50.0F));
		}
		navMeshAgent.SetDestination(player.transform.position);
		yield return new WaitForSeconds(4.0F);
		foreach(Collider c in GetComponentsInChildren<Collider>())c.enabled = true;
		currentAction = Actions.following;
		animator.SetBool ("RunTowards", false);
		animator.SetBool ("LegTakeDown", false);
		player.GetComponent<Animator>().SetInteger("Action", 0);
	}
	public void LateUpdate(){
		/*if(Vector3.Angle(player.transform.position - transform.position, transform.forward) < 120){
			head.LookAt(player.transform.position + new Vector3(0, 1, 0));
			if(Vector3.Angle(player.transform.position - transform.position, transform.forward) < 90){
				chest.LookAt(player.transform.position + new Vector3(0, 1, 0));
			}
		}*/
	}
	public IEnumerator specialAttack (int attack) {
		if (attack == 0) {
			//Jump Attack Animation
			if (distance > 4.0F && distance < 7.0F) {
				currentAction = Actions.jumpAttack;
				//navMeshAgent.speed = 0.0F;
				navMeshAgent.SetDestination(player.transform.position);
				animator.SetBool ("JumpAttack", true);
				animator.SetBool ("Following", false);
				yield return new WaitForSeconds (2.0F);
				GameObject dP = Instantiate(dustParicle, transform.Find("dP").transform.position, Quaternion.identity) as GameObject;
				Destroy(dP, 5.0F);
				if(distance < 2.0F){
					player.GetComponent<PlayerC>().callTakeDamage(UnityEngine.Random.Range(15.0F, 50.0F));
				}
				yield return new WaitForSeconds (2.0F);
				currentAction = Actions.following;
				//navMeshAgent.speed = 5.0F;
				animator.SetBool ("JumpAttack", false);
			}
		} else if (attack == 1) {
			//Run Towards Attack
			//Distance 6.0F > x < 13.0F
			//SetDestination to player;
			if (distance > 4.0F && distance < 8.0F) {
				currentAction = Actions.runTowards;
				//navMeshAgent.speed = 10.0F;
				animator.SetBool ("RunTowards", true);
				animator.SetBool ("Following", false);
				navMeshAgent.SetDestination (player.transform.position);
				yield return new WaitForSeconds (7.0F);
				if(currentAction != Actions.takeDown){
					animator.SetBool ("RunTowards", false);
					currentAction = Actions.agony;
					animator.Play("AgonyStart", 0, 0.0F);
					navMeshAgent.SetDestination(transform.position + new Vector3(-40, 0, 0));
					yield return new WaitForSeconds(5.0F);
					currentAction = Actions.following;
					//navMeshAgent.speed = 5.0F;
				}
			}
		} else if (attack == 2) {
			//Dash Attack
			//After Dash Run Towards
			if (distance > 5.0F && distance < 8.0F) {
				currentAction = Actions.dash;
				//navMeshAgent.speed = 0.0F;
				animator.SetBool ("Dash", true);
				animator.SetBool ("Following", false);
				yield return new WaitForSeconds (2.0F);
				currentAction = Actions.runTowards;
				//navMeshAgent.speed = 10.0F;
				animator.SetBool ("RunTowards", true);
				animator.SetBool ("Dash", false);
				navMeshAgent.SetDestination (player.transform.position);
				yield return new WaitForSeconds (0.5F);
				float time = 0.0F;
				while(distance > 2.0F && time < 7){
					navMeshAgent.SetDestination (player.transform.position);
					time += 2 * Time.deltaTime; 
					yield return new WaitForSeconds(0.1F);
				}
				currentAction = Actions.following;
				navMeshAgent.speed = 5.0F;
				animator.SetBool ("RunTowards", false);
			}
		}
		//Normal Attack;
	}
	public IEnumerator normalAttack(){
		currentAction = Actions.normalAttack;
		navMeshAgent.SetDestination (player.transform.position);
		animator.SetBool("NormalAttack", true);
		yield return new WaitForSeconds(2.4F);
		if(distance < 2.0F){
			GameObject.Find("Ana").GetComponent<PlayerC>().callTakeDamage(20);
		}
		if(currentAction == Actions.normalAttack){
			currentAction = Actions.following;
		}
		animator.SetBool("NormalAttack", false);
	}

	void OnAnimatorMove(){
		if(animator && currentAction != Actions.takeDown){
    		navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude * UnityEngine.Random.RandomRange(0.8F, 1.0F);
		}
 	}
}