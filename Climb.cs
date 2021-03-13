using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攀爬：参考UP主 伦敦街区的福尔摩斯 大家可以去搜一下他的视频
/// </summary>
public class Climb : MonoBehaviour
{
    public bool onWall;

    private Vector3 targetPos;
    public float wallRayLength = 1;
    public float wallOffset = 0.5f;

    private Animator anim;

    public Transform climbHelper;
    private Vector3 headPos;
    private RaycastHit hitInfo;

    public float climbSpeed = 0.5f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        CheckClimb();
    }

    private bool CheckClimb()
    {
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;
        RaycastHit hit;
        Debug.DrawRay(origin, dir * wallRayLength, Color.green);
        if (Physics.Raycast(origin, dir, out hit, wallRayLength))
        {
            InitClimb(hit);
            return true;
        }
        return false;
    }

    private void InitClimb(RaycastHit hit)
    {
        onWall = false;
        targetPos = hit.point + hit.normal * wallOffset;

        anim.CrossFade("EnterClimb", 0.2f);
        Debug.Log("Hit Wall");
    }

    public Vector2 input;
    private void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        anim.SetFloat("hor", input.x);
        anim.SetFloat("ver", input.y);
        if (!onWall)
        {
            SetBodyPositionToWall();
        }
        else
        {
            FixBodyPos();
            MoveHandle();
        }

    }

    private void SetBodyPositionToWall()
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            onWall = true;
            transform.position = targetPos;
            return;
        }
        Vector3 lerpTargetPos = Vector3.MoveTowards(transform.position, targetPos, 0.2f);
        transform.position = lerpTargetPos;
    }

    public void FixBodyPos()
    {
        Vector3 localClimbHelperPos = transform.InverseTransformPoint(climbHelper.position);
        Vector3 localHeadPos = new Vector3(0, localClimbHelperPos.y, 0);
        headPos = transform.TransformPoint(localHeadPos);
        Debug.DrawRay(headPos, transform.forward * 1f, Color.red);
        if(Physics.SphereCast(headPos,0.1f,transform.forward,out hitInfo,1))
        {
            Vector3 tempVector = transform.position - climbHelper.position;
            if(Vector3.Distance(transform.position,hitInfo.point+tempVector)>0.05f)
            {
                transform.position = hitInfo.point + tempVector;
            }
        }
    }

    
    public void MoveHandle()
    {
        bool canMove = true;
        if(input.magnitude > 0.05f && canMove)
        {
            
            transform.Translate(input.x*climbSpeed*Time.deltaTime, input.y*climbSpeed*Time.deltaTime, 0);
        }
        else
        {
            anim.SetFloat("hor", 0);
            anim.SetFloat("ver", 0);
        }
    }

    private Vector3 LeftHandIK, RightHandIK, LeftFootIK, RightFootIK;
    private Vector3 LeftHandPos, RightHandPos, LeftFootPos, RightFootPos;
    private Quaternion LeftHandRot, RightHandRot, LeftFootRot, RightFootRot;
    public bool enableIK;
    /// <summary>
    /// 脚部IK位置与实际射线检测位置的Y轴差
    /// </summary>
    [Range(0, 0.2f)] public float GroundOffset;
    /// <summary>
    /// 射线向下检测距离
    /// </summary>
    public float GroundDistance;
    /// <summary>
    /// IK交互层
    /// </summary>
    public LayerMask EnvLayer;
    public float climbRotateSpeed = 5f;
    public float offset = 0.2f;

    private void FixedUpdate()
    {

        Debug.DrawLine(LeftHandIK - transform.forward, LeftHandIK + transform.forward*GroundDistance, Color.blue, Time.fixedDeltaTime);
        Debug.DrawLine(RightHandIK - transform.forward, RightHandIK + transform.forward * GroundDistance, Color.blue, Time.fixedDeltaTime);

        if (Physics.Raycast(LeftHandIK - transform.forward, transform.forward, out RaycastHit hit, GroundDistance + 1, EnvLayer))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red, Time.fixedDeltaTime);

            LeftHandPos = hit.point - transform.forward * GroundOffset;
            LeftHandRot = Quaternion.FromToRotation(-transform.forward, hit.normal) * transform.rotation;
        }
        if (Physics.Raycast(RightHandIK - transform.forward, transform.forward, out RaycastHit hit1, GroundDistance + 1, EnvLayer))
        {
            Debug.DrawRay(hit1.point, hit1.normal, Color.red, Time.fixedDeltaTime);

            RightHandPos = hit1.point - transform.forward * GroundOffset;

            RightHandRot = Quaternion.FromToRotation(-transform.forward, hit1.normal) * transform.rotation;
        }

        if(Physics.Raycast(transform.position + transform.up*offset, transform.forward, out RaycastHit hit2,0.5f))
        {
            if (Vector3.Angle(hit2.normal, hit.normal) > 1f && input.y < 0)
            {
                transform.Rotate(transform.right, climbRotateSpeed * Time.deltaTime);
            }
            if (Vector3.Angle(-transform.forward, hit.normal) > 1f && input.y > 0)
            {
                transform.Rotate(transform.right, -climbRotateSpeed * Time.deltaTime);
            }
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        LeftHandIK = anim.GetIKPosition(AvatarIKGoal.LeftHand);
        RightHandIK = anim.GetIKPosition(AvatarIKGoal.RightHand);

        if (enableIK == false)
            return;

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);


        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandPos);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandRot);

        anim.SetIKPosition(AvatarIKGoal.RightHand, RightHandPos);
        anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandRot);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(headPos, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(hitInfo.point, 0.05f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(climbHelper.position, 0.05f);
    }
}
