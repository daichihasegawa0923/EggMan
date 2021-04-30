using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Diamond.EggmanSimulator.BreakGimic
{
    public class MeshBreaker : MonoBehaviour
    {
        /// <summary>
        /// brake objects mesh
        /// </summary>
        /// <param name="gObject">object will be broken</param>
        /// <param name="count">re-brake count</param>
        /// <returns>broken objects</returns>
        public void BreakMesh(GameObject gObject,int count)
        {
            try
            {
                if (count == 0 || gObject.tag == "IsBroken")
                {
                    gObject.tag = "IsBroken";
                    return;
                }

                count--;

                var direction = Vector3.one;
                direction.x = UnityEngine.Random.Range(-1, 1);
                direction.y = UnityEngine.Random.Range(-1, 1);
                direction.z = UnityEngine.Random.Range(-1, 1);

                var objects = MeshCut.Cut(gObject, gObject.transform.position, direction, gObject.GetComponent<MeshRenderer>().material);

                objects.ToList().ForEach(o =>
                {
                    o.transform.parent = null;

                    if (o.GetComponent<BoxCollider>())
                        Destroy(o.GetComponent<BoxCollider>());

                    if (o.GetComponent<MeshCollider>())
                        Destroy(o.GetComponent<MeshCollider>());

                    var meshCollider = o.AddComponent<MeshCollider>();
                    meshCollider.convex = true;

                    if (o.GetComponent<Rigidbody>() == null)
                        o.AddComponent<Rigidbody>();

                    BreakMesh(o, count);
                });
            }
            catch(Exception e)
            {
                Debug.LogAssertion(e.Message);
                return;
            }

            /*
            count--;

            var gObjects = new GameObject[] { };

            if (count == 0)
            {
                var currentObject = new GameObject[] { gObject };
                return currentObject;
            }

            foreach (var g in gObjects)
            {

                var cutObjects = CutMesh(g);

                foreach(var c in cutObjects)
                {
                    gObjects.Concat(BreakMesh(c,count));
                }
            }

            return gObjects;
            */
        }

        private GameObject[] CutMesh(GameObject gObject)
        {


            var cutObjects = new GameObject[] { };

            var meshFilter = gObject.GetComponent<MeshFilter>();
            var meshRenderer = gObject.GetComponent<MeshRenderer>();

            if (meshFilter == null || meshRenderer == null)
                return cutObjects;



            return cutObjects;
        }
    }
}