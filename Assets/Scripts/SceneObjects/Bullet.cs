using Controllers;
using DG.Tweening;
using UnityEngine;

namespace SceneObjects
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [Range(0,1f)][SerializeField] private float damage;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float speed;

        public float Damage => damage;
        public float Speed => speed;
        public Sequence Sequence { get; set; }

        public Rigidbody Rigidbody => rigidbody;
        
        private void OnCollisionEnter(Collision collision)
        {
            var enemyComponent = collision.gameObject.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.Kill();
            }
            ControllersManager.Instance.GameController.ShootingMechanic.StopBullet(this);
        }
    }
}
