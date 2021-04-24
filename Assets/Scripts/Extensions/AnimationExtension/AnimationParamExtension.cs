using UnityEngine;
using System.Collections;

namespace Diamond.Extensions.AnimatorExtension
{

    public static class AnimationParamOperator
    {
        /// <summary>
        ///  Change value of parametor is type of bool to true
        /// </summary>
        /// <param name="animator">Animator</param>
        /// <param name="parametorName">Parametor name you want to set true</param>
        public static void SetBoolTrueOnly(this Animator animator, string parametorName)
        {
            var paras = animator.parameters;
            foreach (var para in paras)
            {
                if (para.type != AnimatorControllerParameterType.Bool)
                    continue;

                animator.SetBool(para.name, para.name == parametorName);
            }
        }

        /// <summary>
        /// Change value of parametor is type of bool to false
        /// </summary>
        /// <param name="animator"></param>
        public static void SetBoolFalseAll(this Animator animator)
        {
            if (!animator)
                return;

            var paras = animator.parameters;
            foreach (var para in paras)
            {
                if (para.type != AnimatorControllerParameterType.Bool)
                    continue;

                animator.SetBool(para.name,false);
            }
        }
    }
}