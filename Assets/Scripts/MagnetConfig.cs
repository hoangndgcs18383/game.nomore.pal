using UnityEngine;

namespace NoMorePals
{
    [CreateAssetMenu(fileName = "MagnetConfig", menuName = "Configs/MagnetConfig")]
    public class MagnetConfig : ScriptableObject
    {
        public float magnetRange = 100f;
        public float magnetSpeedDefault = 10f;
        public float magnetSpeedMax = 20f;
        public float stopOffset = 1f;
    }
}
