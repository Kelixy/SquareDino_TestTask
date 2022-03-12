using UnityEngine;

namespace SceneObjects
{
    public interface IBullet
    {
        float Damage { get; }
        float Speed { get; }
    }
}
