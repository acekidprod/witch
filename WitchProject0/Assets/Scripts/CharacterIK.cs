using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{

    Animator anim;

    Transform leftFoot;
    Transform rightFoot;

    Vector3 leftFoot_pos;
    Vector3 rightFoot_pos;

    Quaternion leftFoot_rot;
    Quaternion rightFoot_rot;

    float leftFoot_Weight;
    float rightFoot_Weight;

    public Transform lookAtThis;

    private void Start()
    {
        anim = GetComponent<Animator>();
        leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // foot IK
        leftFoot_Weight = anim.GetFloat("leftFoot");
        rightFoot_Weight = anim.GetFloat("rightFoot");
        // find raycast positions
        FindFloorPositions(leftFoot, ref leftFoot_pos, ref leftFoot_rot, Vector3.up);
        FindFloorPositions(rightFoot, ref rightFoot_pos, ref rightFoot_rot, Vector3.up);

        // replace the weights with , and  when you've set them in your animation's Curves
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFoot_Weight);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFoot_Weight);

        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFoot_Weight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFoot_Weight);

        // set the position of the feet
        anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot_pos);
        anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot_pos);

        // set the rotation of the feet
        anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFoot_rot);
        anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFoot_rot);

        // head IK
        if (lookAtThis != null)
        {
            // distance between face and object to look at
            float distanceFaceObject = Vector3.Distance(anim.GetBoneTransform(HumanBodyBones.Head).position, lookAtThis.position);

            anim.SetLookAtPosition(lookAtThis.position);
            // blend based on the distance
            anim.SetLookAtWeight(Mathf.Clamp01(2 - distanceFaceObject), Mathf.Clamp01(1 - distanceFaceObject));
        }
    }

    void FindFloorPositions(Transform t, ref Vector3 targetPosition, ref Quaternion targetRotation, Vector3 direction)
    {
        RaycastHit hit;
        Vector3 rayOrigin = t.position;
        // move the ray origin back a bit
        rayOrigin += direction * 0.3f;

        // raycast in the given direction
        Debug.DrawRay(rayOrigin, -direction, Color.green);
        if (Physics.Raycast(rayOrigin, -direction, out hit, 3))
        {
            // the hit point is the position of the hand/foot
            targetPosition = hit.point;
            // then rotate based on the hit normal
            Quaternion rot = Quaternion.LookRotation(transform.forward);
            targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * rot;
        }
    }
}