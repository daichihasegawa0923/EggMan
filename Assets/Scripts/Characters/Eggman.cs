using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diamond.Extensions.AnimatorExtension;
using Diamond.EggmanSimulator.BreakGimic;
using Diamond.EggmanSimulator.Gimics;
using System.Linq;

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

        [SerializeField]
        private Burret _burret;

        [SerializeField]
        private Transform _burretMouse;

        [SerializeField]
        private float _burretSpeed = 10;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _walkAudioClip;

        private bool _isThrowing = false;

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
            if (_isThrowing)
                return;

            var hMotion = Input.GetAxis("Horizontal");
            var vMotion = Input.GetAxis("Vertical");

            var velocity = _rigidbody.velocity;
            velocity.x = IsFoot ? (hMotion * _speed) : 0;
            velocity.z = IsFoot ? (vMotion * _speed) : 0;
            _rigidbody.velocity = velocity;


            if (Mathf.Abs(hMotion) > 0 || Mathf.Abs(vMotion) > 0)
            {
                var direction = Vector3.zero;
                direction.x = hMotion;
                direction.z = vMotion;

                transform.LookAt(transform.position + direction);
                var spin = transform.eulerAngles;
                spin.x = 0;
                spin.z = 0;
                transform.eulerAngles = spin;

                if(_audioSource.clip != null && (!_audioSource.clip.Equals(_walkAudioClip)) || ! _audioSource.isPlaying)
                {
                    _audioSource.clip = _walkAudioClip;
                    _audioSource.pitch = 3.0f;
                    _audioSource.loop = true;
                    _audioSource.Play();
                }

                if(IsFoot && !_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
                else if(!IsFoot && _audioSource.isPlaying)
                {
                    _audioSource.Pause();
                }
            }
            else
            {
                _audioSource.pitch = 1.0f;
                _audioSource.clip = null;
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

            if(Input.GetKeyDown(KeyCode.Space) && !_isThrowing)
            {
                _animator.SetTrigger("Throw");
            }

            if(!IsFoot)
            {
                _animator.SetBoolTrueOnly("isSky");
            }
        }

        /// <summary>
        /// Invoked by animation.
        /// </summary>
        public void Fire()
        {
            var burret = Instantiate(_burret.gameObject);
            burret.transform.position = _burretMouse.position;
            var rigidbody = burret.GetComponent<Rigidbody>();
            if (rigidbody == null)
                rigidbody = burret.AddComponent<Rigidbody>();

            rigidbody.velocity = transform.forward * _burretSpeed;
            rigidbody.velocity += Vector3.up;
        }

        /// <summary>
        /// Character is foot on the floor
        /// </summary>
        /// <returns>is foot</returns>
        public bool IsFoot
        {
            get
            {
                var all = Physics.BoxCastAll (transform.position, Vector3.one * 0.5f, Vector3.down,Quaternion.identity,1.0f);
                var result = all.ToList().Where(a => !a.collider.gameObject.Equals(gameObject));
                return result.Count() >= 1;
            }
        }

        /// <summary>
        /// ?A?j???[?V????????????
        /// </summary>
        public void StartThrowing()
        {
            _isThrowing = true;
        }

        /// <summary>
        /// ?A?j???[?V????????????
        /// </summary>
        public void EndThrowing()
        {
            _isThrowing = false;
        }
    }
}