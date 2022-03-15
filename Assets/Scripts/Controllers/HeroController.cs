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
        
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Animator heroAnimator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform[] wayPoints;
        
        [SerializeField] private float startRunningPauseDuration;

        private GameController _gameController;
        private AnimationKeys _animationKeys;
        private bool _mustGoToWaypoint;
        private int _currentWayPointInd;

        public void Initialize()
        {
            _gameController = ControllersManager.Instance.GameController;
            _animationKeys = _gameController.AnimationKeys;
            _gameController.OnLevelSucceed += MoveToNextPoint;
        }

        private void Shoot(Vector3 targetPos)
        {
            var currentPosition = navMeshAgent.transform.position;
            var angle = ValuesCounter.CountTurningAngle(currentPosition, targetPos);
            
            DOTween.Sequence()
                .Append(heroAnimator.transform.DORotate(new Vector3(0, angle, 0), 0.2f))
                .AppendCallback(() =>
                {
                    heroAnimator.Play(_animationKeys.ShootAnimationHash);
                    _gameController.ShootingMechanic.RunBullet(targetPos);
                });

        }

        public void MoveToNextPoint()
        {
            GameController.TapIsBlocked = true;
            
            DOTween.Sequence()
                .Append(heroAnimator.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f))
                .AppendCallback(() => { heroAnimator.Play(_animationKeys.RunAnimationHash); })
                .AppendInterval(startRunningPauseDuration)
                .AppendCallback(() =>
                {
                    navMeshAgent.SetDestination(wayPoints[_currentWayPointInd++].position);
                    _mustGoToWaypoint = true;
                });
        }

        private void StopOnWayPointIfIsGoing()
        {
            if (!_mustGoToWaypoint || !CheckIfNearWaypoint()) return;
            _mustGoToWaypoint = false;
            _gameController.OnWayPointReached.Invoke();
            heroAnimator.Play(_animationKeys.IdleAnimationHash);
        }

        private bool CheckIfNearWaypoint() =>
            Vector3.Distance(navMeshAgent.destination, navMeshAgent.transform.position) <= WayPointDestinationDelta;

        private void ShootOnTapIfPossible()
        {
            if (Input.GetMouseButtonDown(0) && !GameController.TapIsBlocked)
            {
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    Shoot(hit.point);
                }
            }
        }

        private void FixedUpdate()
        {
            ShootOnTapIfPossible();
            StopOnWayPointIfIsGoing();
        }
    }
}