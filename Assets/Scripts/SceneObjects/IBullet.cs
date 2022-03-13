using DG.Tweening;
using UnityEngine;

namespace SceneObjects
{
    public interface IBullet
    {
        float Damage { get; }
        float Speed { get; }
        Sequence Sequence { get; set; }

        void SetSettings(Vector3 startPos, Vector3 velocityValue);
        void Switch(bool shouldBeOn);
    }
}
