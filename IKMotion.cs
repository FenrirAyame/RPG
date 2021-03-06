using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class IKMotion : MonoBehaviour
{
    private Animator anim;
    /// <summary>
    /// 射线检测IK位置
    /// </summary>
    private Vector3 LeftFootIK, RightFootIK;
    /// <summary>
    /// IK位置
    /// </summary>
    private Vector3 LeftFootPos, RightFootPos;
    /// <summary>
    /// IK旋转
    /// </summary>
    private Quaternion LeftFootRot, RightFootRot;

    /// <summary>
    /// IK交互层
    /// </summary>
    public LayerMask EnvLayer;
    /// <summary>
    /// 脚部IK位置与实际射线检测位置的Y轴差
    /// </summary>
    [Range(0,0.2f)] public float GroundOffset;
    /// <summary>
    /// 射线向下检测距离
    /// </summary>
    public float GroundDistance;

    public bool enableIK;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(LeftFootIK + Vector3.up, LeftFootIK + Vector3.down*GroundDistance, Color.blue, Time.fixedDeltaTime);
        Debug.DrawLine(RightFootIK + Vector3.up, RightFootIK + Vector3.down * GroundDistance, Color.blue, Time.fixedDeltaTime);

        if (Physics.Raycast(LeftFootIK + Vector3.up,Vector3.down,out RaycastHit hit,GroundDistance + 1,EnvLayer))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red, Time.fixedDeltaTime);

            LeftFootPos = hit.point + Vector3.up * GroundOffset;

            LeftFootRot = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
        }

        if (Physics.Raycast(RightFootIK + Vector3.up, Vector3.down, out RaycastHit hit1, GroundDistance + 1, EnvLayer))
        {
            Debug.DrawRay(hit1.point, hit1.normal, Color.red, Time.fixedDeltaTime);

            RightFootPos = hit1.point + Vector3.up * GroundOffset;

            RightFootRot = Quaternion.FromToRotation(Vector3.up, hit1.normal) * transform.rotation;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        LeftFootIK = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
        RightFootIK = anim.GetIKPosition(AvatarIKGoal.RightFoot);

        if (enableIK == false)
            return;

        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LIK"));
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LIK"));
        

        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RIK"));
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RIK"));

        anim.SetIKPosition(AvatarIKGoal.LeftFoot, LeftFootPos);
        anim.SetIKRotation(AvatarIKGoal.LeftFoot, LeftFootRot);

        anim.SetIKPosition(AvatarIKGoal.RightFoot, RightFootPos);
        anim.SetIKRotation(AvatarIKGoal.RightFoot, RightFootRot);
    }
}
