using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Animation_Keys", menuName = "Animation_Keys", order = 51)]
    public class AnimationKeys : ScriptableObject
    {
        [SerializeField] private string runAnimationName;
        [SerializeField] private string shootAnimationName;
        [SerializeField] private string idleAnimationName;
        [SerializeField] private string hitAnimationName;

        private int _runAnimationHash;
        private int _shootAnimationHash;
        private int _idleAnimationHash;
        private int _hitAnimationHash;

        public int RunAnimationHash => _runAnimationHash == default
            ? _runAnimationHash = Animator.StringToHash(runAnimationName)
            : _runAnimationHash;

        public int ShootAnimationHash => _shootAnimationHash == default
            ? _shootAnimationHash = Animator.StringToHash(shootAnimationName)
            : _shootAnimationHash;
        
        public int IdleAnimationHash => _idleAnimationHash == default
            ? _idleAnimationHash = Animator.StringToHash(idleAnimationName)
            : _idleAnimationHash;
        
        public int HitAnimationHash => _hitAnimationHash == default
            ? _hitAnimationHash = Animator.StringToHash(hitAnimationName)
            : _hitAnimationHash;
    }
}
