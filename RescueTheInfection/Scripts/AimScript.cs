using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    public Animator animator;
    public PlayerC states;
    public Transform targetLook;

    public Transform l_hand;
    public Transform l_hand_Target;
    public Transform r_hand;
    
    public Quaternion lh_rot;

    public float rh_Weight;
    public Transform shoulder;
    public Transform aimPivot;
    public bool isPlayer = false;
    public string currentWeapon;
    public GameObject[] lHand_Targets;
    public GameObject test;
    public bool grabbingWeapon = false;
    public float grabbingWeight = 0.0F;
    public float recoilAmount = 0.0F;
    public bool aimingOn = true;
    // Start is called before the first frame update
    void Start()
    {
        if (isPlayer)
        {
            shoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);

            aimPivot = new GameObject().transform;
            aimPivot.name = "Aim Pivot";
            aimPivot.transform.parent = transform;
            aimPivot.transform.localRotation = Quaternion.Euler(0.0F, 0.0F, 0.0F);

            r_hand = new GameObject().transform;
            r_hand.name = "Right Hand";

            l_hand = new GameObject().transform;
            l_hand.name = "Left Hand";

            //aimPivot.transform.rotation = Quaternion.Euler(0.0F, 90.0F, 0.0F);
            r_hand.transform.parent = aimPivot;
            l_hand.transform.parent = aimPivot;

            r_hand.localPosition = new Vector3(0.215F, 0.046F, 0.488F);
            r_hand.localPosition = new Vector3(0.274F, -0.06F, 0.14F);
            Quaternion rotRight = Quaternion.Euler(GameObject.Find("Pistol").transform.localRotation.x,GameObject.Find("Pistol").transform.localRotation.y, GameObject.Find("Pistol").transform.localRotation.z);
            r_hand.localRotation = rotRight;
            Quaternion rotLeft = Quaternion.Euler(GameObject.Find("Escopeta").transform.localRotation.x,GameObject.Find("Escopeta").transform.localRotation.y, GameObject.Find("Escopeta").transform.localRotation.z);
            l_hand.localRotation = rotLeft;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWeapon == "Pistol" && r_hand.localPosition != new Vector3(0.215F, 0.046F, 0.488F)){
            r_hand.localPosition = new Vector3(0.133F/*0.2F*/, -0.02F, 0.488F);
            r_hand.localRotation = Quaternion.Euler(-7.602F, 16.837F, -85.145F);
            l_hand_Target = lHand_Targets[0].transform;
        }else if(currentWeapon == "Shotgun" && r_hand.localPosition != new Vector3(0.274F, -0.06F, 0.14F)){
            r_hand.localPosition = new Vector3(0.2F, -0.05F, 0.14F);
            l_hand_Target = lHand_Targets[1].transform;
        }
        lh_rot = l_hand.rotation;
        l_hand.position = l_hand_Target.position;
        l_hand.rotation = lh_rot;

        if(animator.GetBool("Aiming")){
            rh_Weight += 0.25F * Time.deltaTime;
        }else{
            rh_Weight -= 0.5F * Time.deltaTime;
        }
        rh_Weight = Mathf.Clamp(rh_Weight, 0.0F, 1.0F);

        if(grabbingWeapon){
            grabbingWeight += 4.0F * Time.deltaTime;
        }else{
            grabbingWeight -= 4.0F * Time.deltaTime;
        }
        grabbingWeight = Mathf.Clamp(grabbingWeight, 0.0F, 1.0F);
    }

    void OnAnimatorIK()
    {
        aimPivot.position = shoulder.position;

        if(animator.GetBool("Aiming") && aimingOn){
            aimPivot.LookAt(targetLook.position);

            animator.SetLookAtWeight(1.0F, 1.0F, 1.0F);
            animator.SetLookAtPosition(targetLook.position - new Vector3(0.0F, recoilAmount * 4.0F, 0.0F));

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            if(currentWeapon == "Shotgun"){
                animator.SetIKPosition(AvatarIKGoal.LeftHand, l_hand.position - new Vector3(0.0F, 0.01F, 0.0F));
            }else{
                animator.SetIKPosition(AvatarIKGoal.LeftHand, l_hand.position);
            }

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rh_Weight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, r_hand.position - new Vector3(0.0F, recoilAmount, 0.0F));
            if(currentWeapon == "Pistol"){
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rh_Weight);
                animator.SetIKRotation(AvatarIKGoal.RightHand, r_hand.rotation);
            }
        }else{
            if(currentWeapon == "Shotgun" && !grabbingWeapon){
                //animator.SetLookAtWeight(1.0F, 1.0F, 1.0F);
                //animator.SetLookAtPosition(targetLook.position - new Vector3(0.0F, recoilAmount * 2.5F, 0.0F));

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, l_hand.position /*+ new Vector3(0.0F, 0.02F, -0.1F)*/);
            }
            //animator.SetLookAtWeight(0.3F, 0.3F, 0.3F);
            //animator.SetLookAtPosition(targetLook.position);
            if(grabbingWeapon){
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, grabbingWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, test.transform.position);
            }
        }
    }
}
