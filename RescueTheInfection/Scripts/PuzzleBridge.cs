using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBridge : MonoBehaviour
{
    public bool onTrigger = false;
    public bool used = false;
    public GameObject[] deactivateObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(onTrigger){
            if(Input.GetKeyDown(KeyCode.E)){
                GetComponent<Rigidbody>().isKinematic = false;
                foreach(GameObject dObject in deactivateObjects){
                    Destroy(dObject);
                }
                this.enabled = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            onTrigger = true;
        }
    }
    public void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            onTrigger = false;
        }
    }
}
