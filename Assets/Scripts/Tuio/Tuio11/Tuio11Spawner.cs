using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11Spawner : MonoBehaviour
    {
        [SerializeField] private TuioTransmitter _transmitter;
        [SerializeField] private Tuio11CursorBehaviour _cursorPrefab;
        [SerializeField] private Tuio11ObjectBehaviour _objectPrefab;

        [ContextMenu("Spawn Cursor")]
        public void SpawnCursor()
        {
            var cursor = Instantiate(_cursorPrefab, transform);
            cursor.Init((Tuio11Manager)_transmitter.Manager);
        }
        
        [ContextMenu("Spawn Object")]
        public void SpawnObject()
        {
            var tuioObject = Instantiate(_objectPrefab, transform);
            tuioObject.Init((Tuio11Manager)_transmitter.Manager, 5);
        }
    }
}
