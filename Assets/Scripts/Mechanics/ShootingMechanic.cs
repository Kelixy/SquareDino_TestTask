using Controllers;
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

        private PoolOfObjects<Bullet> _poolOfBullets;

        public void Initialize()
        {
            _poolOfBullets = new PoolOfObjects<Bullet>(bulletPrefab, poolTransform);
        }

        public void RunBullet(Vector3 aimPointPos)
        {
            var firePos = firePoint.position;
            var bullet = _poolOfBullets.Get();
            var direction = (aimPointPos - firePos).normalized;
            bullet.transform.position = firePos;
            bullet.Rigidbody.velocity = direction * bullet.Speed;
            bullet.gameObject.SetActive(true);
            bullet.Sequence = DOTween.Sequence()
                .AppendInterval(bulletLifeTime)
                .AppendCallback(() => { StopBullet(bullet); });
        }

        public void StopBullet(Bullet bullet)
        {
            bullet.Sequence?.Kill();
            _poolOfBullets.Put(bullet);
            bullet.gameObject.SetActive(false);
        }
    }
}