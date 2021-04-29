using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public bool inCombat = false;
    public CinemachineFreeLook cfl;
    // Start is called before the first frame update
    void Start()
    {
        cfl = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMode(){
        if(!inCombat){
            inCombat = true;
            cfl.m_Lens.FieldOfView = 50.0F;
        }else{
            inCombat = false;
            cfl.m_Lens.FieldOfView = 40.0F;
        }
    }
}
