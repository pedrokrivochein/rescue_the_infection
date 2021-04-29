using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrabPlank : MonoBehaviour
{
    public bool onTrigger = false;
    public GameObject player;
    public bool hasPickedUp = false;
    public bool goingToPosition = false;
    public Transform leftHand;
    public Transform rightHand;
    public Animator animator;
    public Vector3 startPosition;
    public Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Ana");
        animator = player.GetComponent<Animator>();
        leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update(){
        if(!onTrigger){
            return;
        }

        if(!hasPickedUp){
            if(Input.GetKeyDown(KeyCode.E) && player.GetComponent<Animator>().GetInteger("WalkingMode") == 0){
                StartCoroutine(pickUp());
            }
        }else{
            if(Input.GetKeyDown(KeyCode.E) && player.GetComponent<Animator>().GetInteger("WalkingMode") == 5){
                StartCoroutine(letGo());
            }
        }
        if(goingToPosition){
            transform.position = Vector3.Lerp(transform.position, player.transform.Find("Plank").transform.position /*- new Vector3(0.0F, 0.0F, 2.0F)*/, 2.0F * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.Find("Plank").transform.rotation, 2.0F * Time.deltaTime);
        }
    }
    public IEnumerator pickUp(){
        StopCoroutine(letGo());
        player.GetComponent<Animator>().SetInteger("Action", 5);
        hasPickedUp = true;
        GameObject.Find("ActionText").GetComponent<TextMeshProUGUI>().text = "";
        yield return new WaitForSeconds(4.0F);
        GetComponent<Collider>().enabled = false;
        goingToPosition = true;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = player.transform;
        yield return new WaitForSeconds(3.0F);
        player.GetComponent<Animator>().SetInteger("Action", 0);
        player.GetComponent<Animator>().SetInteger("WalkingMode", 5);
    }

    public IEnumerator letGo(){
        //player.GetComponent<Animator>().SetInteger("Action", 5);
        GetComponent<Collider>().enabled = false;
        goingToPosition = false;
        transform.parent = player.transform.parent;
        GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(0.2F);
        GetComponent<Collider>().enabled = true;
        hasPickedUp = false;
        player.GetComponent<Animator>().SetInteger("WalkingMode", 0);
        yield return null;
    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            onTrigger = true;
            GameObject.Find("ActionText").GetComponent<TextMeshProUGUI>().text = "Press [E] To Grab";
        }
    }

    public void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            onTrigger = false;
            GameObject.Find("ActionText").GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
