using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private Vector3 _latestPosition;


        private void Start()
        {
            _latestPosition = transform.position;
        }

        private void Update()
        {
            OperateCamera();
            OperateCharacterMove();
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
            velocity.x = hMotion;
            velocity.z = vMotion;
            _rigidbody.velocity = velocity;

            var diff = transform.position - _latestPosition;

            if (diff.magnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(diff);

            _latestPosition = transform.position;
        }
    }
}