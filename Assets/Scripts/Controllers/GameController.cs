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
        public static bool TapIsBlocked = true;
        
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
                TapIsBlocked = false;
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

        private bool EndGameIfLastLevel()
        {
            if (_levelsController.CheckIfLastLevel())
            {
                EndGame();
                return true;
            }
            
            return false;
        }
    }
}
