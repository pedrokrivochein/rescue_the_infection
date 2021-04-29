using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsIkAttach : MonoBehaviour
{
    public Animator animator;
    public Transform chest;
    public Transform hips;
    public float ikWeight = 0.0F;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        hips = animator.GetBoneTransform(HumanBodyBones.Spine);
    }

    // Update is called once per frame
    void Update()
    {
        if(ikWeight < 1.0F && transform.Find("PlankObject")){
            ikWeight += Time.deltaTime;
        }else if(ikWeight > 0.0F && !transform.Find("PlankObject")){
            ikWeight -= 6.0F * Time.deltaTime;
        }
    }
    void OnAnimatorIK(){
        if(transform.Find("PlankObject")){
            animator.SetIKPosition(AvatarIKGoal.RightHand, GameObject.Find("PlankRH").transform.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, GameObject.Find("PlankLH").transform.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
            return;
        }
    }
}
