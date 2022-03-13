using System;
using UnityEngine;

namespace SceneObjects
{
    public interface IBullet
    {
        float Damage { get; }
        float Speed { get; }

        void SetFlightSettings(Vector3 startPos, Vector3 velocityValue);
        void Switch(bool shouldBeOn);
    }
}
