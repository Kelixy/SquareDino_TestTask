using Controllers;
using UnityEngine;

namespace SceneObjects
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private int health;
        [SerializeField] private Animator enemyAnimator;

        private bool _killed;

        public float Health => health;
        
        public void Kill()
        {
            if (_killed) return;
            _killed = true;
            enemyAnimator.Play(ControllersManager.Instance.GameController.AnimationKeys.DeathAnimationHash);
            ControllersManager.Instance.GameController.PlusWalkthroughCondition();
        }
    }
}
