using System;
using DG.Tweening;
using Mechanics;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ShootingMechanic shootingMechanic;
        [SerializeField] private AnimationKeys animationKeys;
        [SerializeField] private CanvasGroup curtains;
        [SerializeField] private GameObject instructions;

        public Action OnWayPointReached;
        public Action OnLevelSucceed;
        public AnimationKeys AnimationKeys => animationKeys;
        public ShootingMechanic ShootingMechanic => shootingMechanic;

        private ControllersManager _controllersManager;
        private LevelsController _levelsController;
        private HeroController _heroController;

        private bool _tapIsBlocked = true;

        private void Start()
        {
            ShootingMechanic.Initialize();
            _controllersManager = ControllersManager.Instance;
            _levelsController = _controllersManager.LevelsController;
            _heroController = _controllersManager.HeroController;
            _levelsController.Initialize();
            _heroController.Initialize();

            OnWayPointReached += () =>
            {
                BlockTap(false);
                EndGameIfLastLevel();
                GoNextIfAimReached();
            };
        }

        public void StartGame()
        {
            DOTween.Sequence()
                .AppendCallback(() => {_heroController.MoveToNextPoint();})
                .Append(curtains.DOFade(0, 3));
        }

        private void EndGame()
        {
            DOTween.Sequence()
                .AppendCallback(() => { instructions.SetActive(false); })
                .Append(curtains.DOFade(1, 1))
                .InsertCallback(1,() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
        }

        public void GoNextIfAimReached()
        {
            if (_levelsController.CheckIfLevelAimReached())
            {
                OnLevelSucceed.Invoke();
            }
        }

        public void BlockTap(bool block = true) => _tapIsBlocked = block;

        private void EndGameIfLastLevel()
        {
            if (_levelsController.CheckIfLastLevel())
            {
                EndGame();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_tapIsBlocked)
            {
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    _heroController.Shoot(hit.point);
                }
            }
        }
    }
}
