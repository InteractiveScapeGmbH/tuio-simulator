using TuioSimulator.Tuio.Tuio20;
using UnityEngine;

namespace TuioSimulator.Tuio.Sxm
{
    public class SxmSpawner : MonoBehaviour
    {
        [SerializeField] private MqttBehaviour _mqttBehaviour;
        [SerializeField] private Tuio20Spawner _spawner;

        private void Start()
        {
            _mqttBehaviour.Init(_spawner.AddMobile);
        }
    }
}