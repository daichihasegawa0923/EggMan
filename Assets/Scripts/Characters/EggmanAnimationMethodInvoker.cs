using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diamond.EggmanSimulator.Characters
{
    public class EggmanAnimationMethodInvoker : MonoBehaviour
    {
        [SerializeField]
        private Eggman _eggman;
        
        public void StartThrowing()
        {
            _eggman.StartThrowing();
        }

        public void EndThrowing()
        {
            _eggman.EndThrowing();
        }

        public void Fire()
        {
            _eggman.Fire();
        }
    }
}