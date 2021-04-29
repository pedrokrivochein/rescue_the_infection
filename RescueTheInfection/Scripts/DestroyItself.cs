using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        Destroy(this.gameObject, 30.0F);
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
