using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace SceneObjects
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [Range(0,100)][SerializeField] private int startHealth;
        
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private Image healthLine;

        public bool IsKilled { get; private set; }

        public int CurrentHealth { get; private set; }

        public void SetStartParams(Vector3 startPos, Quaternion startRotation)
        {
            transform.position = startPos;
            transform.rotation = startRotation;
            healthLine.fillAmount = 1;
        }

        public void Switch(bool shouldBeOn)
        {
            gameObject.SetActive(shouldBeOn);
        }

        private static GameController GameController => ControllersManager.Instance.GameController;

        private void Start()
        {
            CurrentHealth = startHealth;
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
            
            enemyAnimator.Play(GameController.AnimationKeys.DeathAnimationHash);
            GameController.PlusWalkThroughCondition();
        }
    }
}
