using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Animation_Keys", menuName = "Animation_Keys", order = 51)]
    public class AnimationKeys : ScriptableObject
    {
        [SerializeField] private string runAnimationName;
        [SerializeField] private string deathAnimationName;
        [SerializeField] private string shootAnimationName;

        private int _runAnimationHash;
        private int _deathAnimationHash;
        private int _shootAnimationHash;

        public int RunAnimationHash => _runAnimationHash == default
            ? _runAnimationHash = Animator.StringToHash(runAnimationName)
            : _runAnimationHash;

        public int DeathAnimationHash => _deathAnimationHash == default
            ? _deathAnimationHash = Animator.StringToHash(deathAnimationName)
            : _deathAnimationHash;

        public int ShootAnimationHash => _shootAnimationHash == default
            ? _shootAnimationHash = Animator.StringToHash("GunShot")
            : _shootAnimationHash;
    }
}
