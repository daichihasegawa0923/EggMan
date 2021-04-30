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
                if (count == 0)
                {
                    return;
                }

                count--;

                var direction = Vector3.one;
                direction.x = UnityEngine.Random.Range(-1, 1);
                direction.y = UnityEngine.Random.Range(-1, 1);
                direction.z = UnityEngine.Random.Range(-1, 1);

                var objects = MeshCut.Cut(gObject, gObject.transform.position, direction, gObject.GetComponent<MeshRenderer>().material);

                objects.ToList().ForEach(o => RebreakObject(o, count));
            }
            catch(Exception e)
            {
                Debug.LogAssertion(e.Message);
                return;
            }
        }
        /// <summary>
        /// brake objects mesh
        /// </summary>
        /// <param name="gObject">object will be broken</param>
        /// <param name="position">broken point</param>
        /// <param name="count">re-brake count</param>
        /// <returns>broken objects</returns>
        public void BreakMesh(GameObject gObject, Vector3 position ,int count)
        {
            try
            {
                if (count == 0)
                {
                    return;
                }

                count--;

                var direction = Vector3.one;
                direction.x = UnityEngine.Random.Range(-1, 1);
                direction.y = UnityEngine.Random.Range(-1, 1);
                direction.z = UnityEngine.Random.Range(-1, 1);

                var objects = MeshCut.Cut(gObject, position, direction, gObject.GetComponent<MeshRenderer>().material);

                objects.ToList().ForEach(o => RebreakObject(o, count));
            }
            catch (Exception e)
            {
                Debug.LogAssertion(e.Message);
                return;
            }
        }

        private void RebreakObject(GameObject gObject, int count)
        {
            gObject.transform.parent = null;


            if (gObject.GetComponent<BoxCollider>())
                Destroy(gObject.GetComponent<BoxCollider>());

            if (gObject.GetComponent<MeshCollider>())
                Destroy(gObject.GetComponent<MeshCollider>());

            var boxCollider = gObject.AddComponent<BoxCollider>();
            boxCollider.size = Vector3.one * 0.5f;

            if (gObject.GetComponent<Rigidbody>() == null)
                gObject.AddComponent<Rigidbody>();

            BreakMesh(gObject, count);
        }
    }
}