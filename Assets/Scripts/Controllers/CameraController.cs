using Counters;
using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector3 distanceToHero;

        public Camera MainCamera => mainCamera;
        
        private bool _followHero = true;

        public void TurnToPos(Vector3 targetPos)
        {
            var angle = ValuesCounter.CountTurningAngle(transform.position, targetPos);
            DOTween.Sequence()
                .Append(transform.DORotate(new Vector3(0, angle, 0), 1));
        }
    
        void FixedUpdate()
        {
            if (_followHero)
            {
                MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position,
                    ControllersManager.Instance.HeroController.CurrentHeroPosition + distanceToHero, 0.1f);
            }
        }
    }
}
