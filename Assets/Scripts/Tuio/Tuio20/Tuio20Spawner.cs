using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Spawner : MonoBehaviour
    {
        [SerializeField] private TuioTransmitter _transmitter;
        [SerializeField] private Tuio20PointerBehaviour _pointerPrefab;
        [SerializeField] private Tuio20TokenBehaviour _tokenPrefab;
        [SerializeField] private Tuio20Mobile _mobilePrefab;

        [ContextMenu("Spawn Cursor")]
        public void SpawnCursor()
        {
            var cursor = Instantiate(_pointerPrefab, transform);
            cursor.Init((Tuio20Manager)_transmitter.Manager);
        }

        [ContextMenu("Spawn Token")]
        public void SpawnToken()
        {
            var token = Instantiate(_tokenPrefab, transform);
            token.Init((Tuio20Manager)_transmitter.Manager, 20);
        }

        [ContextMenu("Spawn Phone")]
        public void SpawnMobile()
        {
            var mobile = Instantiate(_mobilePrefab, transform);
            mobile.Init((Tuio20Manager)_transmitter.Manager,0);
        }
    }
}
