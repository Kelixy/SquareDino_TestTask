using UnityEngine;

namespace Counters
{
    public static class ValuesCounter
    {
        public static float CountTurningAngle(Vector3 objectPos, Vector3 targetPos)
        {
            var direction = targetPos - objectPos;
            return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }
    }
}
