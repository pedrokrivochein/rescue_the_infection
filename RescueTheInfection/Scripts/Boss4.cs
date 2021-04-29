using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4 : MonoBehaviour {

	public float speed = 1.0F;
	public enum Actions{Following, Idle, Attacking, Defending, Hit};
	public float health = 2000F;
	public Animator animator;
	public Vector3 oldPosition;
	public Actions currentAction;
	public GameObject player;
	public bool canAttack = false;
	public bool canSpecialAttack = false;
	public bool canDash = false;
	public int damageInt = 0;
	public bool damaging = false;
	public int attackNumber = 0;
	public int runningNumber = 1;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		currentAction = Actions.Idle;
		canAttack = true;
		speed = 1.0F;
		canSpecialAttack = false;
		InvokeRepeating("canChange", 0.0F, 1.0F);
		health = 2000F;
		canDash = false;
		damageInt = 0;
		damaging = false;
		runningNumber = 1;
	}
	// Update is called once per frame
	void Update () {
		if(health <= 0){
			animator.SetInteger("Dead", 1);
			return;
		}
		oldPosition = transform.position;
		float distance = Vector3.Distance(transform.position, player.transform.position);
		if(currentAction == Actions.Following){
			if(distance < 3){
				if(canAttack){
					StartCoroutine(attack(UnityEngine.Random.Range(1, 2)));
				}
			}else if(distance < 6){
				if(canAttack){
					if(distance > 6){
						if(attackNumber == 2){
							StartCoroutine(specialAttack(3));
						}
					}else{
					}
				}else{
					if(health < 1000){
					}else{
						if(canDash){
							StartCoroutine(dash());
						}
					}
				}
			}else if(distance < 11){
				if(canAttack){
					if(distance < 10){
						if(attackNumber == 3){
							StartCoroutine(specialAttack(1));
						}
					}
				}else{
					if(health < 1000){
					}else{
						if(canDash){
							StartCoroutine(dash());
						}
					}
				}
			}
			if(!GetComponent<CharacterController>().isGrounded){
				transform.position -= new Vector3(0.0F, 2.0F * Time.deltaTime, 0.0F);
			}
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
			Vector3 dir = player.transform.position + new Vector3(0.0F, 1.0F, 0.0F) - transform.position + new Vector3(0.0F, 1.0F, 0.0F);
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			Vector3 rotation = lookRotation.eulerAngles;
			transform.rotation = Quaternion.Euler(transform.rotation.x, rotation.y, transform.rotation.z);
		}else if(currentAction == Actions.Attacking){
			RaycastHit hit;
			Vector3 dir = player.transform.position + new Vector3(0.0F, 1.0F, 0.0F) - transform.position + new Vector3(0.0F, 1.0F, 0.0F);
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			Vector3 rotation = lookRotation.eulerAngles;
			transform.rotation = Quaternion.Euler(transform.rotation.x, rotation.y, transform.rotation.z);
		}
		if(transform.position != oldPosition){
			animator.SetInteger("Running", runningNumber);
			speed = 1.0F;
		}else{
			animator.SetInteger("Running", 0);
		}
	}

	public IEnumerator attack(int attackNumber){
		animator.SetInteger("Attack", attackNumber);
		canAttack = false;
		currentAction = Actions.Attacking;
		if(attackNumber == 1){
			yield return new WaitForSeconds(1.7F);
		}else if(attackNumber == 2){
			yield return new WaitForSeconds(2.5F);
		}
		animator.SetInteger("Attack", 0);
		canAttack = true;
		currentAction = Actions.Following;
	}

	public IEnumerator specialAttack(int specialAttackNumber){
		RaycastHit hit;
		if(Physics.Linecast(transform.position + new Vector3(0.0F, 1.0F, 0.0F), player.transform.position + new Vector3(0.0F, 1.0F, 0.0F), out hit)){
			transform.LookAt(hit.transform.position);
		}
		animator.SetInteger("SpecialAttack", specialAttackNumber);
		canAttack = false;
		canSpecialAttack = false;
		currentAction = Actions.Attacking;
		if(specialAttackNumber == 1){
			yield return new WaitForSeconds(1.5F);
		}else if(specialAttackNumber == 2){
			yield return new WaitForSeconds(3.7F);
		}else if(specialAttackNumber == 3){
			yield return new WaitForSeconds(2.1F);
		}
		animator.SetInteger("SpecialAttack", 0);
		canAttack = true;
		currentAction = Actions.Following;
	}

	public IEnumerator dash(){
		animator.SetInteger("Dash", 1);
		canAttack = false;
		canDash = false;
		currentAction = Actions.Attacking;
		yield return new WaitForSeconds(4.6F);
		animator.SetInteger("Dash", 0);
		canAttack = true;
		currentAction = Actions.Following;
	}

	public void canChange(){
		attackNumber = UnityEngine.Random.Range(1, 4);
	}

	public IEnumerator damage(int damageNumber, float damageToTake){
		animator.SetInteger("Damage", damageNumber);
		health -= damageToTake;
		damaging = true;
		yield return new WaitForSeconds(1.1F);
		animator.SetInteger("Damage", 0);
		damaging = false;
	}

	public void takeDamage(RaycastHit hit, float damageToTake){
		if(currentAction == Actions.Following){
			if(damageInt == 3){
				canChange();
			}
			if(damaging){
				StopCoroutine("damage");
			}
			if(hit.collider.gameObject.name == "Chest"){
				StartCoroutine(damage(1, damageToTake));
			}else if(hit.collider.gameObject.name == "Left"){
				StartCoroutine(damage(2, damageToTake));
			}else if(hit.collider.gameObject.name == "Right"){
				StartCoroutine(damage(3, damageToTake));
			}
			damageInt++;
		}
	}
}
