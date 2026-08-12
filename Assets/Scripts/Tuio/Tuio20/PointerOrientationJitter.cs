using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.Tuio.Tuio20
{
    public class PointerOrientationJitter : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        private static PointerOrientationJitter _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static float GetJitteredValue(float angleInRadians)
        {
            float jitterValueInDegrees = _instance._slider.value;
            float jitterValueInRadians = jitterValueInDegrees * Mathf.Deg2Rad;
            float jitterInRadians = Random.Range(-jitterValueInRadians, jitterValueInRadians);

            return angleInRadians + jitterInRadians;
        }
    }
}