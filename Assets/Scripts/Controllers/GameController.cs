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

        public AnimationKeys AnimationKeys => animationKeys;
        public ShootingMechanic ShootingMechanic => shootingMechanic;

        private ControllersManager _controllersManager;

        private void Start()
        {
            _controllersManager = ControllersManager.Instance;
            ShootingMechanic.Initialize();
            _controllersManager.HeroController.Initialize();

            StartGame();
        }

        private void StartGame()
        {
            _controllersManager.HeroController.MoveToNextPoint(0);
        }

        public void EndGame()
        {
            DOTween.Sequence()
                .Append(curtains.DOFade(1, 1))
                .AppendCallback(() => {SceneManager.LoadScene(SceneManager.GetActiveScene().name);});
        }

        public void PlusWalkthroughCondition()
        {
            var levelsController = _controllersManager.LevelsController;
            levelsController.PlusOneEnemyKilled();
            
            if (levelsController.CheckIfLevelAimReached())
                levelsController.GoToNextLevel();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    _controllersManager.HeroController.Shoot(hit.point);
                }
            }
        }
    }
}
