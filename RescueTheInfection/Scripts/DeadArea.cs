using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerC>().callTakeDamage(1000.0F);
        }else if(other.gameObject.GetComponent<GrabPlank>()){
            other.gameObject.transform.position = other.gameObject.GetComponent<GrabPlank>().startPosition;
            other.gameObject.transform.rotation = other.gameObject.GetComponent<GrabPlank>().startRotation;
        }
    }
}
