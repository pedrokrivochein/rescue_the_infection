using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1TriggerLeave : MonoBehaviour
{
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player") && !dead){
            //other.gameObject.GetComponent<PlayerC>().animator.SetInteger("Action", 1);
            //other.gameObject.GetComponent<PlayerC>().animator.Play("Trip", 0, 0.0F);
        }
        //Spawn Enemies To Kill Her
    }
}
