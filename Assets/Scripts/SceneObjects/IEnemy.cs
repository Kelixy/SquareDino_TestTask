using UnityEngine;

namespace SceneObjects
{
    public interface IEnemy
    {
        int CurrentHealth { get; }
        void SetStartParams(Vector3 startPos, Quaternion startRotation);
    }
}
