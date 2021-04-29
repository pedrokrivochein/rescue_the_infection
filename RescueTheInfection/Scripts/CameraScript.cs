using System.Security.Cryptography.X509Certificates;
using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {
	Vector2 mouseLook;
	Vector2 smoothV;
	public float sensitivity = 3.0f;
	public float smoothing = 2.0f;
	public bool walking = false;
	public GameObject player;
	public bool turning = false;
	public PlayerC playerScript;
	public Vector2 mda;
	public float keepSensitivity;
	public Slider slider;
	public Quaternion freeRotation;
	public Vector3 targetDirection;
	public GameObject head;
	public GameObject[] aimBones;
	public GameObject playerObj;
	public GameObject mainC;
	// Use this for initialization
	void Start () {
		walking = false;
		turning = false;
		playerScript = player.GetComponent<PlayerC>();
		playerObj = GameObject.Find("Ana");
		mainC = GameObject.Find("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if(player == null){
			return;
		}
		if(playerScript.inCutscene == 2){
			return;
		}
		mda = new Vector2 (Input.GetAxisRaw ("Mouse X") * sensitivity, Input.GetAxisRaw ("Mouse Y") * sensitivity);
		//if(Input.GetAxisRaw ("HorizontalJoystick") <= 0.3F && Input.GetAxisRaw ("HorizontalJoystick") >= -0.3F && Input.GetAxisRaw ("VerticalJoystick") <= 0.3F && Input.GetAxisRaw ("VerticalJoystick") >= -0.3F){
		//	sensitivity = 0.0F;
		//}else{
		//	mda = new Vector2 (Input.GetAxisRaw ("HorizontalJoystick") * sensitivity, Input.GetAxisRaw ("VerticalJoystick") * sensitivity);
		//}
		sensitivity = slider.value;
		float movhh = Input.GetAxis ("Horizontal") * 5F;
		float movvh = Input.GetAxis ("Vertical") * 5F; 
		mda = Vector2.Scale (mda, new Vector2 (sensitivity * smoothing, 2.0F * smoothing));
		smoothV.x = Mathf.Lerp (smoothV.x, mda.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp (smoothV.y, mda.y, 1f / smoothing);
		mouseLook += smoothV;
		mouseLook.y = Mathf.Clamp (mouseLook.y, -75f, 75f);
		if(playerObj.GetComponent<CoverSystem>().inCover){
			if(playerObj.GetComponent<CoverSystem>().right){
				//mouseLook.x = Mathf.Clamp (mouseLook.x, -player.transform.rotation.y, -player.transform.rotation.y);
			}else{
				//mouseLook.x = Mathf.Clamp (mouseLook.x, -player.transform.rotation.y, -player.transform.rotation.y);
			}
		}
		//transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0.0F, 3.0F, 0.0F), 23F * Time.deltaTime);
		mouseLook.x = Mathf.Clamp(mouseLook.x, -80.0F, 80.0F);
		if(playerObj.GetComponent<Animator>().GetBool("Aiming") && !player.GetComponent<CoverSystem>().inCover){
			//GetComponent<Animator>().SetBool("Aiming", true);
			//isSprinting = false;
			/*Cursor.lockState = CursorLockMode.Locked;
			sensitivity = 0.7F;
			Vector2 input;
			input.x = Input.GetAxis("Horizontal");
	    	input.y = Input.GetAxis("Vertical");
			var forward = mainC.transform.TransformDirection(Vector3.forward);
			forward.y = 0;
            var right = mainC.transform.TransformDirection(Vector3.right);
            targetDirection = input.x * right + input.y * forward;
            Vector3 lookDirection = targetDirection.normalized;
            freeRotation = Quaternion.LookRotation(lookDirection, player.transform.up);
            var diferenceRotation = freeRotation.eulerAngles.y - player.transform.eulerAngles.y;
            var eulerY = transform.eulerAngles.y;

            if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
            var euler = new Vector3(0, eulerY - 9.0F, 0);*/
			var forward = mainC.transform.TransformDirection(Vector3.forward);
			forward.y = 0;
            playerObj.transform.rotation = Quaternion.LookRotation(forward);
			////Debug.Log("AAAKKKAKAOAKOKAKAKKA");
		}else{
			//GetComponent<Animator>().SetBool("Aiming", false);
			sensitivity = slider.value;
			//Cursor.lockState = CursorLockMode.Locked;
		}
		if(!walking && playerScript.inCutscene != 10){
			transform.localRotation = Quaternion.Euler (-mouseLook.y, mouseLook.x, 0.0F);
		}
	}
	public void LateUpdate(){
		if(player.GetComponent<Animator>().GetBool("Aiming")){
		}
		//head.transform.rotation = Quaternion.Slerp (head.transform.rotation, Quaternion.LookRotation(GameObject.Find("CM_ThirdCamera").transform.forward, Vector3.up), 6.0F * Time.deltaTime);
	}
}
