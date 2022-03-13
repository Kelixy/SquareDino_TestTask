using Counters;
using DG.Tweening;
using Settings;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class HeroController : MonoBehaviour
    {
        private const float WayPointDestinationDelta = 0.1f;
        
        [SerializeField] private Animator heroAnimator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform[] wayPoints;
        
        [SerializeField] private float startRunningPauseDuration;

        private GameController _gameController;
        private AnimationKeys _animationKeys;
        private bool _mustGoToWaypoint;

        public void Initialize()
        {
            _gameController = ControllersManager.Instance.GameController;
            _animationKeys = _gameController.AnimationKeys;
        }

        public void Shoot(Vector3 targetPos)
        {
            var currentHeroPosition = navMeshAgent.transform.position;
            var angle = ValuesCounter.CountTurningAngle(currentHeroPosition, targetPos);
            
            DOTween.Sequence()
                .Append(heroAnimator.transform.DORotate(new Vector3(0, angle, 0), 0.2f))
                .AppendCallback(() =>
                {
                    heroAnimator.Play(_animationKeys.ShootAnimationHash);
                    _gameController.ShootingMechanic.RunBullet(targetPos);
                });

        }

        public void MoveToNextPoint(int level)
        {
            _gameController.BlockTap();
            
            DOTween.Sequence()
                .Append(heroAnimator.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f))
                .AppendCallback(() => { heroAnimator.Play(_animationKeys.RunAnimationHash); })
                .AppendInterval(startRunningPauseDuration)
                .AppendCallback(() =>
                {
                    navMeshAgent.SetDestination(wayPoints[level].position);
                    _mustGoToWaypoint = true;
                });
        }

        private void StopHeroOnWayPoint()
        {
            _mustGoToWaypoint = false;
            _gameController.BlockTap(false);
            _gameController.CheckIfLastLevel();
            heroAnimator.Play(_animationKeys.IdleAnimationHash);
        }

        private bool CheckIfNearWaypoint() =>
            Vector3.Distance(navMeshAgent.destination, navMeshAgent.transform.position) <= WayPointDestinationDelta;

        private void FixedUpdate()
        {
            if (_mustGoToWaypoint && CheckIfNearWaypoint())
            {
                StopHeroOnWayPoint();
            }
        }
    }
}