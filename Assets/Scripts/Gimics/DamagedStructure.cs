using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diamond.EggmanSimulator.BreakGimic;

namespace Diamond.EggmanSimulator.Gimics
{
    public class DamagedStructure :IDamaged
    {
        private MeshBreaker _meshBreaker;

        private void Start()
        {
            _meshBreaker = GetComponent<MeshBreaker>();

            if (_meshBreaker == null)
                _meshBreaker = gameObject.AddComponent<MeshBreaker>();
        }

        public override void Damaged(DamageStatus damageStatus)
        {
            var se = Resources.Load("BreakSE");
            var seIns = (GameObject)Instantiate(se);
            seIns.transform.position = transform.position;
            _meshBreaker.BreakMesh(gameObject,damageStatus.DamagedPosition,4);
            Destroy(gameObject);
        }
    }
}