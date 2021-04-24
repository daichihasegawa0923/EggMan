using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Diamond.EggmanSimulator.Characters
{
    public class ChaseCamera : MonoBehaviour
    {
        [SerializeField]
        private GameObject _aimObject;

        [SerializeField]
        private Material _transparentMaterial;

        [SerializeField]
        private List<MaterialSet> _materialSets = new List<MaterialSet>();

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            RayCastAndSetMaterial();
        }

        private void RayCastAndSetMaterial()
        {
            var ray = new Ray(transform.position, transform.forward);
            var hits = Physics.RaycastAll(ray, Vector3.Distance(transform.position, _aimObject.transform.position));

            var meshRenderers = new List<MeshRenderer>();

            foreach(var hit in hits)
            {
                if (hit.collider.gameObject == _aimObject)
                    continue;

                var renderer = hit.collider.gameObject.GetComponent<MeshRenderer>();

                if (renderer == null)
                    continue;

                _materialSets.Add(new MaterialSet(renderer, _transparentMaterial, renderer.material));
            }

            var refreshedMaterialSets = _materialSets.Where(
                m => hits.ToList().Exists
                (h => h.collider.gameObject.GetComponent<MeshRenderer>()
                && h.collider.gameObject.GetComponent<MeshRenderer>() == m._meshRenderer)).ToList();

            Debug.Log(refreshedMaterialSets.Count);

            _materialSets.Where(m => !refreshedMaterialSets.Contains(m)).ToList().ForEach(m => m.SetOriginMaterial());

            _materialSets = refreshedMaterialSets;

            _materialSets.ForEach(m => m.SetTransparentMaterial());
        }
    }

    public class MaterialSet
    {
        public MeshRenderer _meshRenderer;
        public Material _transparentMaterial;
        public Material _originMaterial;

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="meshRenderer"></param>
        /// <param name="transparentMaterial"></param>
        /// <param name="originMaterial"></param>
        public MaterialSet(MeshRenderer meshRenderer, Material transparentMaterial, Material originMaterial)
        {
            _meshRenderer = meshRenderer;
            _transparentMaterial = transparentMaterial;
            _originMaterial = originMaterial;
        }

        public void SetTransparentMaterial()
        {
            _meshRenderer.material = _transparentMaterial;
        }

        public void SetOriginMaterial()
        {
            _meshRenderer.material = _originMaterial;
        }
    }
}