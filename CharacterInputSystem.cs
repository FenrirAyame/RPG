using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    /// <summary>
    /// 角色输入系统
    /// </summary>
    public class CharacterInputSystem : MonoBehaviour
    {
        private float hor;
        private float ver;
        private CharacterMotor motor;

        private void Start()
        {
            motor = this.GetComponent<CharacterMotor>();
        }

        private void Update()
        {
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");
            motor.Move(ver);
            motor.Rotate(hor);
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                motor.ChangeMoveState();
            }
        }
    }
}
