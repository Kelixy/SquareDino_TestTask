using System.Linq;
using Settings;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class HeroController : MonoBehaviour
    {
        [SerializeField] private Animator heroAnimator;
        [SerializeField] private AnimationKeys animationKeys;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform[] wayPoints;

        public void Shoot()
        {
            heroAnimator.Play(animationKeys.ShootAnimationHash);
            MoveToNextPoint();
        }

        public void MoveToNextPoint()
        {
            navMeshAgent.SetDestination(wayPoints[0].position);
        }
    }
}
