using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrowingBoss2Script : MonoBehaviour
{
    public bool canStuck = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Industry") && canStuck){
            GetComponent<Rigidbody>().isKinematic = true;
            transform.GetChild(0).transform.gameObject.GetComponent<Animator>().SetBool("ThrowingAxe", false);
            transform.position = collision.contacts[0].point + new Vector3(0.0F, 0.3F, 0.0F);
            transform.rotation = Quaternion.Euler(30.0F, transform.GetChild(0).transform.rotation.y, 175.0F);
            canStuck = false;
            GameObject.Find("Marcos").GetComponent<Boss2>().keepMachadoLastPosition = transform.position;
        }
    }
}
