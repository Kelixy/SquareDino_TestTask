using UnityEngine;

namespace SceneObjects
{
    public interface IEnemy
    {
        int CurrentHealth { get; }
        bool IsKilled { get; }
        void SetStartParams(Vector3 startPos, Quaternion startRotation);
        void Switch(bool shouldBeOn);
    }
}
