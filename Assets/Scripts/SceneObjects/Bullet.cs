using Controllers;
using UnityEngine;

namespace SceneObjects
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [Range(0,100)][SerializeField] private int damage;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float speed;

        public float Damage => damage;
        public float Speed => speed;
        
        public void SetFlightSettings(Vector3 startPos, Vector3 velocityValue)
        {
            transform.position = startPos;
            rigidbody.velocity = velocityValue;
        }

        public void Switch(bool shouldBeOn)
        {
            gameObject.SetActive(shouldBeOn);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var enemyComponent = collision.gameObject.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.Hit(damage);
            }
            ControllersManager.Instance.GameController.ShootingMechanic.StopBullet(this);
        }
    }
}
