using DG.Tweening;
using Pools;
using SceneObjects;
using UnityEngine;

namespace Mechanics
{
    public class ShootingMechanic : MonoBehaviour
    {
        [SerializeField] private Transform poolTransform;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletLifeTime;

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
            bullet.SetSettings(firePos, direction * bullet.Speed);
            bullet.Switch(true);
            bullet.Sequence = DOTween.Sequence()
                .AppendInterval(bulletLifeTime)
                .AppendCallback(() => { StopBullet(bullet); });
        }

        public void StopBullet(IBullet bullet)
        {
            bullet.Sequence?.Kill();
            _poolOfBullets.Put(bullet);
            bullet.Switch(false);
        }
    }
}