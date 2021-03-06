using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    /// <summary>
    /// 角色动画
    /// </summary>
    public class CharacterAnimation : MonoBehaviour
    {
        public string moveXName = "hor";
        public string moveYName = "ver";
        public string isRunName = "isRun";

        private Animator anim;

        private void Start()
        {
            anim = this.GetComponent<Animator>();
        }

        public void PlayAnimation(float ver,bool isRun)
        {
            anim.SetBool(isRunName, isRun);
            anim.SetFloat(moveYName, ver);
        }

        public void PlayRotateAnimation(float hor)
        {
            anim.SetFloat(moveXName, hor);
        }
    }
}
