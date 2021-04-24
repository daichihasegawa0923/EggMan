using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diamond.EggmanSimulator.BreakGimic
{
    public class MeshBreaker : MonoBehaviour
    {
        [SerializeField]
        MeshFilter _meshFilter;

        public void BreakMesh(GameObject gameObject)
        {
            var mesh = gameObject.GetComponent<MeshFilter>();
            var mate = gameObject.GetComponent<MeshRenderer>();

            var vertices = mesh.mesh.vertices;
        }
    }
}