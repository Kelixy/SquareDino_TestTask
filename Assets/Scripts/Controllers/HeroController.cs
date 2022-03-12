using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class HeroController : MonoBehaviour
    {
        private const float DestinationDelta = 0.1f;
        
        [SerializeField] private Animator heroAnimator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform[] wayPoints;
        
        [SerializeField] private float startRunningPauseDuration;

        private bool _checkIfStopped;

        public void Shoot(Vector3 direction)
        {
            ControllersManager.Instance.GameController.ShootingMechanic.RunBullet(direction);
        }

        public void MoveToNextPoint()
        {
            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    heroAnimator.Play(ControllersManager.Instance.GameController.AnimationKeys.RunAnimationHash);
                })
                .AppendInterval(startRunningPauseDuration)
                .AppendCallback(() =>
                {
                    navMeshAgent.SetDestination(wayPoints[0].position);
                    _checkIfStopped = true;
                });
        }

        private void Update()
        {
            if (_checkIfStopped)
            {
                if (Vector3.Distance( navMeshAgent.destination, navMeshAgent.transform.position) <= DestinationDelta)
                {
                    _checkIfStopped = false;
                    heroAnimator.Play(ControllersManager.Instance.GameController.AnimationKeys.IdleAnimationHash);
                }
            }
        }
    }
}