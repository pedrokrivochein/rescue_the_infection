using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealthManager : MonoBehaviour {

	public GameObject[] horda;
	public bool inCutscene = false;
	public bool did = false;
	public Animator anim;
	public GameObject leftHand;
	public bool leftArm = false;
	public GameObject[] blood;
	public LayerMask ignoreRay;
	public GameObject[] bleedingFeet;
	public Transform hips;
	public bool hasFallen = false;
	public Rigidbody deadForce;
	public Vector3 deadDirection;
	public float deadTime = 0.0F;
	// Update is called once per frame
	void Start(){
		anim = GetComponent<Animator>();
		did = false;
		leftArm = false;
		hips = anim.GetBoneTransform(HumanBodyBones.Hips);
		InvokeRepeating("BloodOnFloor", 0.0F, 2.0F);
		deadForce = null;
	}

	void FixedUpdate () {
		if(deadForce && deadTime < 3.0F){
			deadForce.AddForce(deadDirection * 70.0F);
			deadTime += Time.deltaTime;
		}
	}

	public void takeDamage(string name, float damage, RaycastHit hit, Ray ray){
		if(inCutscene){
			if(!did){
				GetComponent<Cutscene002Zombie>().saw = true;
				GameObject.FindObjectOfType<Cutscene002Manager>().did = true;
				for(int i = 0; i < horda.Length; i++){
					horda[i].SetActive(true);
				}
				GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().Play("Action");
				GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().Stop("Cover");
				did = true;
			}
			if(name == "Right"){
				//StartCoroutine(hit2());
				GetComponent<Cutscene002Zombie>().health -= damage;
				////Debug.Log("Got Here");
			}else if(name == "Left"){
				//StartCoroutine(hit());
				GetComponent<Cutscene002Zombie>().health -= damage;
				////Debug.Log("Got Here");
			}else if(name == "Head"){
				GetComponent<Animator>().SetBool("Dead", true);
				Rigidbody[] list = GetComponentsInChildren<Rigidbody>();
  				foreach(Rigidbody comp in list){
      				comp.isKinematic = false;
  				}
				GetComponent<Animator>().enabled = false;
				GetComponent<Cutscene002Zombie>().health = 0;
				////Debug.Log("Got Here");
			}
			return;
		}










		if(this.gameObject.CompareTag("NormalZombie")){
			if(!hasFallen){
				GetComponent<EnemyNormalZombie>().health -= damage;
			}
			if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), -transform.up, out hit, 10.0F, ignoreRay)){
				GameObject decalBlood = Instantiate (blood[UnityEngine.Random.Range(0, blood.Length - 1)], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
				decalBlood.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
				Destroy(decalBlood, 30.0F);
			}
			/*if(GetComponent<EnemyNormalZombie>().health > 0){
				kill();
				bool willFall = (UnityEngine.Random.value > 0.1F);
				if(willFall){
				}
				return;
			}*/
			if(GetComponent<EnemyNormalZombie>().health <= 0.0F){
				Rigidbody[] list = GetComponentsInChildren<Rigidbody>();
  				foreach(Rigidbody comp in list){
      				comp.isKinematic = false;
					if(comp.gameObject.name.Contains(name)){
						deadForce = comp;
						deadDirection = ray.direction;
					}
  				}
				StopAllCoroutines();
				GetComponent<Animator>().enabled = false;
				GetComponent<RagdollHelper>().enabled = false;
				GetComponent<EnemyNormalZombie>().enabled = false;
				GetComponent<NavMeshAgent>().SetDestination(transform.position);
				GetComponent<NavMeshAgent>().enabled = false;
				//bool willFall = (UnityEngine.Random.value > 0.1F);
				/*if(willFall && !hasFallen){
					hasFallen = true;
					StartCoroutine(gettingUp());
					return;
				}else if(hasFallen){
					return;
				}*/
				hasFallen = true;
				return;
			}
			if(hasFallen){
				return;
			}
			bool did = (UnityEngine.Random.value > 0.5F);
			if(name.Contains("RightArm")){
				if(did) GetComponent<Animator>().SetInteger("Hurt", 3);

			}else if(name.Contains("LeftArm")){
				if(did) GetComponent<Animator>().SetInteger("Hurt", 2);

			}else if(name.Contains("RightLeg")){
				if(did) GetComponent<Animator>().SetInteger("Hurt", 6);

			}else if(name.Contains("LeftLeg")){
				if(did) GetComponent<Animator>().SetInteger("Hurt", 5);

			}else if(name.Contains("Head")){
				Rigidbody[] list = GetComponentsInChildren<Rigidbody>();
  				foreach(Rigidbody comp in list){
      				comp.isKinematic = false;
					if(comp.gameObject.name.Contains("Head")){
						Debug.Log("AAAAA");
						deadForce = comp;
						deadDirection = ray.direction;
					}
  				}
				GetComponent<Animator>().enabled = false;
				GetComponent<EnemyNormalZombie>().enabled = false;
				GetComponent<NavMeshAgent>().SetDestination(transform.position);
				GetComponent<NavMeshAgent>().enabled = false;
			}
			GetComponent<EnemyNormalZombie>().hasSeen= true;
			GetComponent<EnemyNormalZombie>().justFollow = true;
		}








		if(this.gameObject.CompareTag("Boss1")){
			GetComponent<Boss1>().health -= damage;
			if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), -transform.up, out hit, 10.0F, ignoreRay)){
				GameObject decalBlood = Instantiate (blood[UnityEngine.Random.Range(0, blood.Length - 1)], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
				decalBlood.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
			}
			if(GetComponent<Boss1>().health <= 0.0F){
				Rigidbody[] list = GetComponentsInChildren<Rigidbody>();
  				foreach(Rigidbody comp in list){
      				comp.isKinematic = false;
  				}
				GetComponent<Animator>().enabled = false;
				GetComponent<Boss1>().enabled = false;
				GetComponent<NavMeshAgent>().SetDestination(transform.position);
				GetComponent<NavMeshAgent>().enabled = false;
				return;
			}
			bool did = (UnityEngine.Random.value > 0.5F);
			if(name.Contains("Head")){
				GetComponent<Boss1>().health -= damage * 2.0F;
			}
		}








		if(this.gameObject.CompareTag("Boss2")){
			GetComponent<Boss2>().health -= damage;
			if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), -transform.up, out hit, 10.0F, ignoreRay)){
				GameObject decalBlood = Instantiate (blood[UnityEngine.Random.Range(0, blood.Length - 1)], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
				decalBlood.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
			}
			if(GetComponent<Boss2>().health <= 0.0F){
				Rigidbody[] list = GetComponentsInChildren<Rigidbody>();
  				foreach(Rigidbody comp in list){
      				comp.isKinematic = false;
  				}
				GetComponent<Animator>().enabled = false;
				GetComponent<Boss2>().enabled = false;
				GetComponent<NavMeshAgent>().SetDestination(transform.position);
				GetComponent<NavMeshAgent>().enabled = false;
				return;
			}
			bool did = (UnityEngine.Random.value > 0.5F);
			if(name.Contains("Head")){
				GetComponent<Boss2>().health -= damage * 2.0F;
			}
		}









		if(this.gameObject.CompareTag("Boss3")){
			GetComponent<Boss3>().health -= damage;
			if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), -transform.up, out hit, 10.0F, ignoreRay)){
				GameObject decalBlood = Instantiate (blood[UnityEngine.Random.Range(0, blood.Length - 1)], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
				decalBlood.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
			}
			if(GetComponent<Boss3>().health <= 0.0F){
				Rigidbody[] list = GetComponentsInChildren<Rigidbody>();
  				foreach(Rigidbody comp in list){
      				comp.isKinematic = false;
  				}
				GetComponent<Animator>().enabled = false;
				GetComponent<Boss3>().enabled = false;
				GetComponent<NavMeshAgent>().SetDestination(transform.position);
				GetComponent<NavMeshAgent>().enabled = false;
				return;
			}
			bool did = (UnityEngine.Random.value > 0.5F);
			if(name.Contains("Head")){
				GetComponent<Boss3>().health -= damage * 2.0F;
			}
		}
	}

	void SetKinematic(bool newValue)
	{
		//Get an array of components that are of type Rigidbody
		Rigidbody[] bodies=GetComponentsInChildren<Rigidbody>();

		//For each of the components in the array, treat the component as a Rigidbody and set its isKinematic property
		foreach (Rigidbody rb in bodies)
		{
			rb.isKinematic=newValue;
		}
	}

	public void kill(){
		Rigidbody[] list = GetComponentsInChildren<Rigidbody>();
  		foreach(Rigidbody comp in list){
      		comp.isKinematic = false;
  		}
		StopAllCoroutines();
		GetComponent<EnemyNormalZombie>().StopAllCoroutines();
		GetComponent<Animator>().enabled = false;
		GetComponent<EnemyNormalZombie>().enabled = false;
		GetComponent<NavMeshAgent>().SetDestination(transform.position);
		GetComponent<NavMeshAgent>().enabled = false;
	}

	public bool getUp = false;
	public IEnumerator gettingUp(){
		yield return new WaitForSeconds(4.0F);
		GetComponent<RagdollHelper>().ragdolled = false;
		GetComponent<Animator>().enabled = true;
		int state = UnityEngine.Random.Range(0, 5);
		GetComponent<Animator>().SetInteger("State", state);
		switch(state){
			case 0:
				GetComponent<Animator>().Play("GettingUpFront", 0, 0.0F);
				break;
			case 1:
				GetComponent<Animator>().Play("Crawling", 0, 0.0F);
				break;
			case 2:
				GetComponent<Animator>().Play("GettingUpFront", 0, 0.0F);
				break;
			case 3:
				GetComponent<Animator>().Play("GettingUpFront", 0, 0.0F);
				break;
			case 4:
				GetComponent<Animator>().Play("GettingUpFront", 0, 0.0F);
				break;

		}
		GetComponent<NavMeshAgent>().enabled = true;
		GetComponent<EnemyNormalZombie>().enabled = true;
		GetComponent<EnemyNormalZombie>().health = 30.0F;
	}

	public IEnumerator hit(){
		//GetComponent<Animator>().Play("HitLeft");
		yield return new WaitForSeconds(1.0F);
		//GetComponent<Animator>().SetInteger("Hit", 0);
	}

	public IEnumerator hit2(){
		//GetComponent<Animator>().Play("HitLeft");
		yield return new WaitForSeconds(1.0F);
		//GetComponent<Animator>().SetInteger("Hit", 0);
	}

	public void BloodOnFloor(){
		RaycastHit hit;
		if(GetComponent<EnemyNormalZombie>() != null && GetComponent<EnemyNormalZombie>().health < 100){
			if(Physics.Raycast(transform.position + new Vector3(0, 1, 0), -transform.up, out hit, 10.0F, ignoreRay)){
				GameObject decalBlood = Instantiate (blood[UnityEngine.Random.Range(0, blood.Length - 1)], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
				decalBlood.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
				/*GameObject decalBlood2 = Instantiate (bleedingFeet[0], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
				decalBlood2.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
				GameObject decalBlood3 = Instantiate (bleedingFeet[1], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
				decalBlood3.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);*/
			}
		}
	}
}