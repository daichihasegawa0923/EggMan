using UnityEngine;

namespace Diamond.EggmanSimulator.Gimics
{
    public class Burret : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 10;

        [SerializeField]
        private int _damagePower = 10;

        private void Start()
        {
            Destroy(gameObject, 5.0f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var breakObject = collision.gameObject;
            var damaged = breakObject.GetComponent<IDamaged>();

            if (damaged == null)
                return;

            var damageStatus = new DamageStatus() { DamagedPosition = collision.contacts[0].point, DamagePoint = _damagePower };

            damaged.Damaged(damageStatus);
            Destroy(gameObject);
        }
    }

    public class DamageStatus
    {
        public Vector3 DamagedPosition { set; get; }
        public int DamagePoint { set; get; }
    }
}