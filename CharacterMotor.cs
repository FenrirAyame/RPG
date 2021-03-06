using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    /// <summary>
    /// 角色马达
    /// </summary>
    public class CharacterMotor : MonoBehaviour
    {
        /// <summary>
        /// 行走速度
        /// </summary>
        public float walkSpeed = 1f;
        /// <summary>
        /// 跑步速度
        /// </summary>
        public float runSpeed = 3f;
        /// <summary>
        /// 旋转速度
        /// </summary>
        public float rotateSpeed = 0.05f;
        /// <summary>
        /// 是否跑步
        /// </summary>
        public bool isRun;

        private CharacterAnimation chAnim;

        private void Start()
        {
            chAnim = this.GetComponent<CharacterAnimation>();
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="x">水平方向输入</param>
        /// <param name="z">垂直方向输入</param>
        public void Move(float z)
        {
            if(z!=0)
            {
                this.transform.Translate(Vector3.forward * (z > 0 && isRun ? runSpeed : (z > 0 ? walkSpeed : -walkSpeed)) * Time.deltaTime, Space.Self);
            }
            chAnim.PlayAnimation(z, isRun);
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="target">目标方向</param>
        public void Rotate(float x)
        {
            this.transform.Rotate(Vector3.up, x * rotateSpeed * Time.deltaTime, Space.Self);
            chAnim.PlayRotateAnimation(x);
        }

        /// <summary>
        /// 改变移动状态
        /// </summary>
        public void ChangeMoveState()
        {
            isRun = !isRun;
        }
    }
}
