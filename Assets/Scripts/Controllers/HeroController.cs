using Counters;
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

        private GameController _gameController;
        private bool _checkIfStopped;

        public void Initialize()
        {
            _gameController = ControllersManager.Instance.GameController;
        }

        public void Shoot(Vector3 targetPos)
        {
            var currentHeroPosition = navMeshAgent.transform.position;
            var angle = ValuesCounter.CountTurningAngle(currentHeroPosition, targetPos);
            DOTween.Sequence()
                .Append(heroAnimator.transform.DORotate(new Vector3(0, angle, 0), 0.2f))
                .AppendCallback(() =>
                {
                    _gameController.ShootingMechanic.RunBullet(targetPos);
                });

        }

        public void MoveToNextPoint(int level)
        {
            _gameController.BlockTap();
            
            DOTween.Sequence()
                .Append(heroAnimator.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f))
                .AppendCallback(() =>
                {
                    heroAnimator.Play(_gameController.AnimationKeys.RunAnimationHash);
                })
                .AppendInterval(startRunningPauseDuration)
                .AppendCallback(() =>
                {
                    navMeshAgent.SetDestination(wayPoints[level].position);
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
                    _gameController.BlockTap(false);
                    _gameController.CheckIfLastLevel();
                    heroAnimator.Play(_gameController.AnimationKeys.IdleAnimationHash);
                }
            }
        }
    }
}