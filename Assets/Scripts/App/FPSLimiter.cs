using UnityEngine;

namespace TuioSimulator
{
    public class FPSLimiter : MonoBehaviour
    {
        void Awake()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 120;
        }
    }
}
