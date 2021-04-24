using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Diamond.EggmanSimulator.Characters
{
    /// <summary>
    ///  Chase aim object and transparent intercepted object.
    /// </summary>
    public class ChaseCamera : MonoBehaviour
    {
        [SerializeField]
        private GameObject _aimObject;

        [SerializeField]
        private Material _transparentMaterial;

        [SerializeField]
        private List<MaterialSet> _materialSets = new List<MaterialSet>();

        private void FixedUpdate()
        {
            RayCastAndSetMaterial();
        }

        /// <summary>
        /// Transparent intercepting object.
        /// </summary>
        private void RayCastAndSetMaterial()
        {
            var ray = new Ray(transform.position, transform.forward);
            var hits = Physics.RaycastAll(ray, Vector3.Distance(transform.position, _aimObject.transform.position));
            hits = hits.Where(h => h.collider.gameObject != _aimObject).ToArray();

            if(hits.Length == 0)
            {
                _materialSets.ForEach(m => m.SetOriginMaterial());
                _materialSets.Clear();
                return;
            }

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == _aimObject)
                    continue;

                var renderer = hit.collider.gameObject.GetComponent<MeshRenderer>();

                if (renderer == null)
                    continue;

                if(!_materialSets.Exists(m => m._meshRenderer.Equals(renderer)))
                    _materialSets.Add(new MaterialSet(renderer, _transparentMaterial, renderer.materials));
            }

            _materialSets.ForEach(m => m.SetTransparentMaterial());
            var delete = _materialSets.Where(m =>  !hits.ToList().Exists((hit) =>
              {
                  var renderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                  return renderer != null && m._meshRenderer.Equals(renderer);
  
                  }
            )).ToList();

            delete.ForEach(d => d.SetOriginMaterial());
            _materialSets.RemoveAll(m=>delete.Contains(m));
        }
    }

    /// <summary>
    /// Material and mesh renderer list value.
    /// </summary>
    public class MaterialSet
    {
        public MeshRenderer _meshRenderer;
        public Material _transparentMaterial;
        public Material[] _originMaterial;

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="meshRenderer"></param>
        /// <param name="transparentMaterial"></param>
        /// <param name="originMaterial"></param>
        public MaterialSet(MeshRenderer meshRenderer, Material transparentMaterial, Material[] originMaterial)
        {
            _meshRenderer = meshRenderer;
            _transparentMaterial = transparentMaterial;
            _originMaterial = new Material[_meshRenderer.materials.Length];
            _meshRenderer.materials.CopyTo(_originMaterial, 0);
        }

        /// <summary>
        /// Set material all transparent
        /// </summary>
        public void SetTransparentMaterial()
        {
            _meshRenderer.material = _transparentMaterial;
            var materialArray = _meshRenderer.materials;

            for (var i = 0; i < materialArray.Length; i++)
            {
                materialArray[i] = _transparentMaterial;
            }

            _meshRenderer.materials = materialArray;
        }

        /// <summary>
        /// Reverse materials.
        /// </summary>
        public void SetOriginMaterial()
        {
            _meshRenderer.material = _originMaterial[0];
            _meshRenderer.materials = _originMaterial;
        }
    }
}