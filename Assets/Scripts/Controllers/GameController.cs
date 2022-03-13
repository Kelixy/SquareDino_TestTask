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

        public AnimationKeys AnimationKeys => animationKeys;
        public ShootingMechanic ShootingMechanic => shootingMechanic;

        private ControllersManager _controllersManager;

        private bool _tapIsBlocked = true;

        private void Start()
        {
            ShootingMechanic.Initialize();
            _controllersManager = ControllersManager.Instance;
            _controllersManager.HeroController.Initialize();
            _controllersManager.LevelsController.Initialize();
        }

        public void StartGame()
        {
            DOTween.Sequence()
                .AppendCallback(() => {_controllersManager.HeroController.MoveToNextPoint();})
                .Append(curtains.DOFade(0, 3));
        }

        private void EndGame()
        {
            DOTween.Sequence()
                .AppendCallback(() => { instructions.SetActive(false); })
                .Append(curtains.DOFade(1, 1))
                .InsertCallback(1,() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
        }

        public void PlusWalkThroughCondition()
        {
            var levelsController = _controllersManager.LevelsController;
            
            levelsController.PlusOneEnemyKilled();

            if (levelsController.CheckIfLevelAimReached())
            {
                levelsController.GoToNextLevel();
            }
        }

        public void BlockTap(bool block = true) => _tapIsBlocked = block;

        public void CheckIfLastLevel()
        {
            if (_controllersManager.LevelsController.CheckIfLastLevel())
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
                    _controllersManager.HeroController.Shoot(hit.point);
                }
            }
        }
    }
}
