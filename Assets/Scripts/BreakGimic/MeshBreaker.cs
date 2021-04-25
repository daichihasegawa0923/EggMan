using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Diamond.EggmanSimulator.BreakGimic
{
    public class MeshBreaker : MonoBehaviour
    {
        public void BreakMesh(GameObject gObject)
        {
            if (gObject.tag == "IsBroken")
                return;

            var meshFilter = gObject.GetComponent<MeshFilter>();
            var meshRenderer = gObject.GetComponent<MeshRenderer>();

            if (meshFilter == null || meshRenderer == null)
                return;

            if (meshFilter.mesh.triangles.Length < 12)
                return;

            var meshComponent = new MeshComponent(meshFilter.mesh);

            var brokenObjectParts = new List<GameObject> { new GameObject(gObject.name + "01"), new GameObject(gObject.name + "02") };

            var dividedMeshComponents = DivideMeshComponent(meshComponent);

            brokenObjectParts.ForEach(b =>
            {
                var createdMeshFilter = b.AddComponent<MeshFilter>();
                var createdMeshRenderer = b.AddComponent<MeshRenderer>();

                createdMeshRenderer.material = meshRenderer.material;
                b.transform.position = gObject.transform.position;
                b.tag = "IsBroken";
            }
            );

            for(var i = 0; i < dividedMeshComponents.Length;i++)
            {
                SetMeshComponentToMeshFilter(dividedMeshComponents[i], brokenObjectParts[i].GetComponent<MeshFilter>());
                var collider = brokenObjectParts[i].AddComponent<MeshCollider>();
                collider.convex = true;
                var rigidbody = brokenObjectParts[i].AddComponent<Rigidbody>();
            }

            Destroy(gObject);
        }

        public void SetMeshComponentToMeshFilter(MeshComponent meshComponent,MeshFilter meshFilter)
        {
            var mesh = meshFilter.mesh;

            mesh.vertices = meshComponent.Vertices;
            mesh.normals = meshComponent.Normals;
            mesh.uv = meshComponent.Uvs;
            mesh.triangles = meshComponent.Triangles;

            meshFilter.mesh = mesh;
        }

        public MeshComponent[] DivideMeshComponent(MeshComponent meshComponent)
        {
            var dividedMeshComponents = new MeshComponent[] { new MeshComponent(), new MeshComponent() };

            var vertices = GetDividedArray<Vector3>(meshComponent.Vertices);
            var normals = GetDividedArray<Vector3>(meshComponent.Normals);
            var uvs = GetDividedArray<Vector2>(meshComponent.Uvs);
            var triangles = GetTriangle(meshComponent.Triangles, vertices[0]);

            Action<int> setValues = (int index) =>
            {
                dividedMeshComponents[index].Vertices = vertices[index];
                dividedMeshComponents[index].Normals = normals[index];
                dividedMeshComponents[index].Uvs = uvs[index];
                dividedMeshComponents[index].Triangles = triangles[index];
            };

            setValues(0);
            setValues(1);

            return dividedMeshComponents;
        }

        public List<T[]> GetDividedArray<T>(T[] array, bool isTriangle = false)
        {
            if(array.Length == 0)
            {
                return new List<T[]>() { array, array };
            }

            var array01 = new T[array.Length/2];
            var array02 = new T[array.Length - array01.Length];

            Array.Copy(array, array01, array01.Length);
            Array.Copy(array, array01.Length - 1, array02, 0, array02.Length);

            return new List<T[]>() { array01, array02 };
        }


        public List<int[]> GetTriangle(int[] preDivideArray,Vector3[] vertices_01)
        {
            var triangles_01 = preDivideArray.Where(pa => pa < vertices_01.Length).ToArray();
            var triangles_02 = preDivideArray.Where(pa => !triangles_01.Contains(pa)).ToList().Select(pa => pa - (triangles_01.Max() + 1)).ToArray();

            return new List<int[]>() { triangles_01, triangles_02 };

        }
    }

    public class MeshComponent
    {
        public Vector3[] Vertices { set; get; }
        public Vector3[] Normals { set; get; }
        public Vector2[] Uvs { set; get; }
        public int[] Triangles { set; get; }

        /// <summary>
        /// Optional Constructer
        /// </summary>
        /// <param name="mesh"></param>
        public MeshComponent(Mesh mesh)
        {
            this.Vertices = mesh.vertices;
            this.Normals = mesh.normals;
            this.Uvs = mesh.uv;
            this.Triangles = mesh.triangles;
        }

        /// <summary>
        /// Default Constructer
        /// </summary>
        public MeshComponent() { }
    }
}