using UnityEngine;

namespace Misc
{
    public static class ClampAngleHelper
    {
        public static float ClampAngle(this float angle, float min, float max) {
            float start = (min + max) * 0.5f - 180;
            float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
            min += floor;
            max += floor;
            
            return Mathf.Clamp(angle, min, max);
        }
    }
}