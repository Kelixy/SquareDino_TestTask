using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace SceneObjects
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [Range(0,100)][SerializeField] private int startHealth;

        [SerializeField] private CapsuleCollider hitCollider;
        [SerializeField] private Rigidbody[] rigidBodies;
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private Image healthLine;

        public bool IsKilled { get; private set; }
        public int CurrentHealth { get; private set; }        
        private static GameController GameController => ControllersManager.Instance.GameController;

        public void SetStartParams(Vector3 startPos, Quaternion startRotation)
        {
            enemyAnimator.enabled = true;
            hitCollider.enabled = true;
            SwitchKinematic(true);
            transform.position = startPos;
            transform.rotation = startRotation;
            healthLine.fillAmount = 1;
            CurrentHealth = startHealth;
        }

        private void SwitchKinematic(bool isOn)
        {
            for (var i = 0; i < rigidBodies.Length; i++)
            {
                rigidBodies[i].isKinematic = isOn;
            }
        }

        public void Switch(bool shouldBeOn)
        {
            gameObject.SetActive(shouldBeOn);
        }

        public void Hit(int damage)
        {
            if (IsKilled) return;
            
            CurrentHealth -= damage;
            
            if (CurrentHealth <= 0)
            {
                Kill();
                return;
            }
            
            enemyAnimator.Play(GameController.AnimationKeys.HitAnimationHash);
            healthLine.fillAmount = (float)CurrentHealth / startHealth;
        }

        private void Kill()
        {
            if (IsKilled) return;
            IsKilled = true;
            healthLine.fillAmount = 0;

            enemyAnimator.enabled = false;
            hitCollider.enabled = false;
            SwitchKinematic(false);
            GameController.GoNextIfAimReached();
        }
    }
}
