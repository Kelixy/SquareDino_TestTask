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
        
        public int CurrentHealth { get; private set; }
        private bool _killed;

        private static GameController GameController => ControllersManager.Instance.GameController;

        private void Start()
        {
            CurrentHealth = startHealth;
        }

        public void Hit(int damage)
        {
            if (_killed) return;
            
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
            if (_killed) return;
            _killed = true;
            healthLine.fillAmount = 0;
            
            enemyAnimator.Play(GameController.AnimationKeys.DeathAnimationHash);
            GameController.PlusWalkThroughCondition();
        }
    }
}
