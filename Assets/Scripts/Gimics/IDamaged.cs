using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diamond.EggmanSimulator.Gimics
{
    public abstract class IDamaged : MonoBehaviour
    {
        public abstract void Damaged(DamageStatus damageStatus);
    }
}
