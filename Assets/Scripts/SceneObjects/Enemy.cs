using Controllers;
using UnityEngine;

namespace SceneObjects
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private int health;
        [SerializeField] private Animator enemyAnimator;

        public float Health => health;
        
        public void Kill()
        {
            enemyAnimator.Play(ControllersManager.Instance.GameController.AnimationKeys.DeathAnimationHash);
        }
    }
}
