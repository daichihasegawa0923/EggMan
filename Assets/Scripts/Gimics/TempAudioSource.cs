using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diamond.EggmanSimulator.Gimics
{
    public class TempAudioSource : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private float _startTime = 0.25f;

        private void Start()
        {
            _audioSource.time = _startTime;
            _audioSource.Play();
            Destroy(gameObject, 5.0f);
        }
    }
}