using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    /// <summary>
    /// 视线跟随
    /// </summary>
    public class GazeFollow : MonoBehaviour
    {
        /// <summary>
        /// 整体权重
        /// </summary>
        [Range(0, 1)] public float weight = 1.0f;
        /// <summary>
        /// 身体权重
        /// </summary>
        [Range(0, 1)] public float bodyWeight = 0.2f;
        /// <summary>
        /// 头部权重
        /// </summary>
        [Range(0, 1)] public float headWeight = 1.0f;
        /// <summary>
        /// 眼睛权重
        /// </summary>
        [Range(0, 1)] public float eyesWeight = 0.6f;
        /// <summary>
        /// 最大视角
        /// </summary>
        [Range(0, 90)] public float maxSightAngle = 50f;
        /// <summary>
        /// 跟随速度
        /// </summary>
        public float followSpeed = 2f;
        /// <summary>
        /// 注视目标标签
        /// </summary>
        public string targetTag = "Player";

        /// <summary>
        /// 注视目标：目标物体需要有GazeTarget子物体
        /// </summary>
        private Transform target;
        /// <summary>
        /// 动画状态机
        /// </summary>
        private Animator anim;
        /// <summary>
        /// 目标位置：在目标离开范围后记录目标最后的位置
        /// </summary>
        private Vector3 targetPos;
        /// <summary>
        /// 当前整体权重：用于插值调整权重
        /// </summary>
        private float currentWeight = 0;
        /// <summary>
        /// 注视目标与自身前方的夹角
        /// </summary>
        private float angle;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                target = other.transform.Find("GazeTarget");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                if (target != null) targetPos = target.position;
                target = null;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (target != null)
            {
                angle = Vector3.Angle(target.position - transform.position, transform.forward);
                if (angle > maxSightAngle)
                    targetPos = target.position;
            }
            if (target != null && angle <= maxSightAngle)
            {
                currentWeight = Mathf.Clamp01(currentWeight + followSpeed * Time.deltaTime) * weight;
                anim.SetLookAtWeight(currentWeight, bodyWeight, headWeight, eyesWeight);
                anim.SetLookAtPosition(target.position);
            }
            else
            {
                currentWeight = Mathf.Clamp01(currentWeight - followSpeed * Time.deltaTime) * weight;
                anim.SetLookAtWeight(currentWeight, bodyWeight, headWeight, eyesWeight);
                anim.SetLookAtPosition(targetPos);
            }
        }
    }
}
