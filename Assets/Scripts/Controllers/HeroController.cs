using Settings;
using UnityEngine;

namespace Controllers
{
    public class HeroController : MonoBehaviour
    {
        [SerializeField] private Animator heroAnimator;
        [SerializeField] private AnimationKeys animationKeys;

        public void Shoot()
        {
            heroAnimator.Play("Shoot");
        }
    }
}
