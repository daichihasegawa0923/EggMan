using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diamond.Extensions.AnimatorExtension;
using Diamond.EggmanSimulator.BreakGimic;

namespace Diamond.EggmanSimulator.Characters
{
    public class Eggman : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private Vector3 _cameraDistance;

        [SerializeField]
        private float _speed = 1.0f;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private MeshBreaker _meshBreaker;

        private void Update()
        {
            OperateCamera();
            OperateCharacterMove();
            OperateAnimation();
        }

        private void OperateCamera()
        {
            _camera.transform.position = transform.position + _cameraDistance;
            _camera.transform.LookAt(this.transform.position);
        }

        private void OperateCharacterMove()
        {

            var hMotion = Input.GetAxis("Horizontal");
            var vMotion = Input.GetAxis("Vertical");

            var velocity = _rigidbody.velocity;
            velocity.x = hMotion * _speed;
            velocity.z = vMotion * _speed;
            _rigidbody.velocity = velocity;


            if (Mathf.Abs(hMotion) > 0 || Mathf.Abs(vMotion) > 0)
            {
                transform.LookAt(transform.position + _rigidbody.velocity);
                var spin = transform.eulerAngles;
                spin.x = 0;
                spin.z = 0;
                transform.eulerAngles = spin;
            }

            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void OperateAnimation()
        {
            var hMotion = Input.GetAxis("Horizontal");
            var vMotion = Input.GetAxis("Vertical");

            if (Mathf.Abs(hMotion) > 0 || Mathf.Abs(vMotion) > 0)
                _animator.SetBoolTrueOnly("isWalk");
            else
                _animator.SetBoolTrueOnly("isStay");

        }

        private void OnCollisionEnter(Collision collision)
        {
            var go = collision.gameObject;
            if(!go.isStatic)
                _meshBreaker.BreakMesh(go);
        }
    }
}