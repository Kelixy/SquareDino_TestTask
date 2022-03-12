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
        
        void Start()
        {
            ShootingMechanic.Initialize();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    ControllersManager.Instance.HeroController.Shoot(hit.point);
                }
            }
        }
    }
}
