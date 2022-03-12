using Mechanics;
using Settings;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ShootingMechanic shootingMechanic;
        [SerializeField] private AnimationKeys animationKeys;

        public AnimationKeys AnimationKeys => animationKeys;
        public ShootingMechanic ShootingMechanic => shootingMechanic;
        public Camera MainCamera => mainCamera;

        private ControllersManager _controllersManager;
        
        void Start()
        {
            _controllersManager = ControllersManager.Instance;
            ShootingMechanic.Initialize();
            _controllersManager.HeroController.MoveToNextPoint(0);
        }

        public void PlusWalkthroughCondition()
        {
            var levelsController = _controllersManager.LevelsController;
            levelsController.PlusOneEnemyKilled();
            
            if (levelsController.CheckIfLevelAimReached())
                levelsController.GoToNextLevel();
        }

        void Update()
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
