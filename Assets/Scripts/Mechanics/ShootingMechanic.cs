using Models;
using SceneObjects;
using UnityEngine;

namespace Mechanics
{
    public class ShootingMechanic : MonoBehaviour
    {
        [SerializeField] private Transform poolTransform;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;

        private PoolOfObjects<IBullet> _poolOfBullets;

        public void Initialize()
        {
            _poolOfBullets = new PoolOfObjects<IBullet>(bulletPrefab, poolTransform);
        }

        public void RunBullet(Vector3 aimPointPos)
        {
            var firePos = firePoint.position;
            var bullet = _poolOfBullets.Get();
            var direction = (aimPointPos - firePos).normalized;
            bullet.SetFlightSettings(firePos, direction * bullet.Speed);
            bullet.Switch(true);
        }

        public void StopBullet(IBullet bullet)
        {
            _poolOfBullets.Put(bullet);
            bullet.Switch(false);
        }
    }
}