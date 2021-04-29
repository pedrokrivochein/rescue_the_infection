using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSystem : MonoBehaviour {

	public PlayerC player;
	public bool inCover = false;
	Vector3 rotationn;
	public GameObject raycast;
	public bool right = true;
	public Transform closestObject;
	public Quaternion rotationKeep;
	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerC>();
		right = true;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(raycast.transform.position, raycast.transform.forward, Color.red, 10.0F);
		if(inCover){
			transform.rotation = new Quaternion(transform.rotation.x , rotationn.y, transform.rotation.z, transform.rotation.w);
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			if(!inCover){
				getInCover();
			}else{
				getOutOfCover();
			}
		}
		Vector3 v;
		if(inCover){
			v = GameObject.Find("CoverPoint").transform.position;
		}
		if(Input.GetKeyUp(KeyCode.A) && inCover){
			if(right){
				right = false;
				player.animator.SetInteger("CoverAnimation", 2);
				return;
			}
		}
		if(Input.GetKeyUp(KeyCode.D) && inCover){
			if(!right){
				right = true;
				player.animator.SetInteger("CoverAnimation", 1);
				return;
			}
		}
		if(Input.GetKey(KeyCode.A) && inCover){
			right = false;
			player.animator.SetInteger("CoverAnimation", 3);
			var newRotation = Quaternion.LookRotation(transform.position - closestObject.position);
    		newRotation.z = 0.0F;
   			newRotation.x = 0.0F;
			rotationKeep = newRotation;
			if(newRotation.y != 0.0F){
    			transform.rotation = newRotation;
			}
    		//transform.position = closestObject.position;
    		//transform.position += transform.rotation * Vector3.forward * 1.0F;
    		//transform.position += -transform.right * 0.5F * Time.deltaTime;*/
			Quaternion rotationDelta = Quaternion.FromToRotation(transform.forward, closestObject.transform.forward);
			//transform.rotation = transform.rotation * rotationDelta;
			player.character.GetComponent<CharacterController> ().Move (transform.right * 0.5F * Time.deltaTime);
			player.character.GetComponent<CharacterController> ().Move (new Vector3(0.0F, -1.0F * Time.deltaTime, 0.0F));
		}else if(!Input.GetKey(KeyCode.A) && inCover && !right){
			player.animator.SetInteger("CoverAnimation", 2);
			transform.rotation = rotationKeep;
		}
		if(Input.GetKey(KeyCode.D) && inCover){
			right = true;
			player.animator.SetInteger("CoverAnimation", 4);
			var newRotation = Quaternion.LookRotation(transform.position - closestObject.position);
    		newRotation.z = 0.0F;
   			newRotation.x = 0.0F;
			rotationKeep = newRotation;
			if(newRotation.y != 0.0F){
    			transform.rotation = newRotation;
			}
    		//transform.position = closestObject.position;
    		//transform.position += transform.rotation * Vector3.forward * 1.0F;
    		//transform.position += -transform.right * 0.5F * Time.deltaTime;*/
			Quaternion rotationDelta = Quaternion.FromToRotation(transform.forward, closestObject.transform.forward);
			//transform.rotation = transform.rotation * rotationDelta;
			player.character.GetComponent<CharacterController> ().Move (-transform.right * 0.5F * Time.deltaTime);
			player.character.GetComponent<CharacterController> ().Move (new Vector3(0.0F, -1.0F * Time.deltaTime, 0.0F));
		}else if(!Input.GetKey(KeyCode.D) && inCover && right){
			player.animator.SetInteger("CoverAnimation", 1);
			transform.rotation = rotationKeep;
		}
	}

	public void getInCover(){
		RaycastHit hit;
		Collider[] objects = Physics.OverlapSphere(transform.position + new Vector3(0.0F, 6.0F, 0.0F), 0.8F);
		for(int i = 0; i < objects.Length; i++){
			if(objects[i].transform.gameObject.CompareTag("Cover")){
				closestObject = objects[i].transform;
				player.animator.SetInteger("CoverAnimation", 1);
				Vector3 dir = transform.position + new Vector3(0.0F, 1.0F, 0.0F) - player.transform.gameObject.transform.position + new Vector3(0.0F, 1.0F, 0.0F);
				Quaternion lookRotation = Quaternion.LookRotation(dir);
				Vector3 rotation = lookRotation.eulerAngles;
				rotationn = rotation;
				inCover = true;
				player.inCover = true;
				RaycastHit hitt;
				rightMethod();
				if(Physics.Raycast(raycast.transform.position, raycast.transform.forward, out hitt, 10.0F)){
					if(hitt.collider.transform.gameObject.name.StartsWith("Tree")){
					}
				}
				break;
			}
		}
	}

	public void getOutOfCover(){
		player.animator.SetInteger("CoverAnimation", 0);
		inCover = false;
		player.inCover = false;
	}

	public void rightMethod(){
		right = false;
		player.animator.SetInteger("CoverAnimation", 3);
		var newRotation = Quaternion.LookRotation(transform.position - closestObject.position);
		newRotation.z = 0.0F;
		newRotation.x = 0.0F;
		rotationKeep = newRotation;
		if(newRotation.y != 0.0F){
			transform.rotation = newRotation;
		}
		//transform.position = closestObject.position;
		//transform.position += transform.rotation * Vector3.forward * 1.0F;
		//transform.position += -transform.right * 0.5F * Time.deltaTime;*/
		Quaternion rotationDelta = Quaternion.FromToRotation(transform.forward, closestObject.transform.forward);
		//transform.rotation = transform.rotation * rotationDelta;
		player.character.GetComponent<CharacterController> ().Move (transform.right * 0.5F * Time.deltaTime);
		player.character.GetComponent<CharacterController> ().Move (new Vector3(0.0F, -1.0F * Time.deltaTime, 0.0F));
	}
}
