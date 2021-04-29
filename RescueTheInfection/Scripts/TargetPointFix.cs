
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointFix : MonoBehaviour
{
    public GameObject mainC;
    // Start is called before the first frame update
    void Start()
    {
        mainC = GameObject.Find("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mainC.transform.position;
        transform.rotation = mainC.transform.rotation;
    }
}
