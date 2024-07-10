using TuioNet.Server;
using TuioSimulation.Input;
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
        [SerializeField] private MouseClicker _mouseClicker;

        private Tuio20Manager _manager;

        private Tuio20Manager Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = (Tuio20Manager)_transmitter.Manager;
                }

                return _manager;
            }
        }
        private Tuio20PointerBehaviour _pointer;


        private void OnEnable()
        {
            _mouseClicker.OnLeftDown += AddPointer;
            _mouseClicker.OnLeftUp += RemovePointer;

            _mouseClicker.OnLeftDoubleClick += AddToken;
        }

        private void OnDisable()
        {
            _mouseClicker.OnLeftDown -= AddPointer;
            _mouseClicker.OnLeftUp -= RemovePointer;

            _mouseClicker.OnLeftDoubleClick -= AddToken;
        }

        private void AddToken(Vector2 position)
        {
            var token = Instantiate(_tokenPrefab, transform);
            token.Init(Manager, 2, position);
        }


        private void AddPointer(Vector2 position)
        {
            _pointer = Instantiate(_pointerPrefab, transform);
            _pointer.Init(Manager, position);
            _mouseClicker.OnLeftMove += MovePointer;
            
        }
        
        private void MovePointer(Vector2 position)
        {
            _pointer.Position = position;
        }

        private void RemovePointer(Vector2 position)
        {
            Destroy(_pointer.gameObject);
            _mouseClicker.OnLeftMove -= MovePointer;
        }

        // [ContextMenu("Spawn Cursor")]
        // public void SpawnCursor()
        // {
        //     var cursor = Instantiate(_pointerPrefab, transform);
        //     cursor.Init((Tuio20Manager)_transmitter.Manager);
        // }

        // [ContextMenu("Spawn Token")]
        // public void SpawnToken()
        // {
        //     var token = Instantiate(_tokenPrefab, transform);
        //     token.Init((Tuio20Manager)_transmitter.Manager, 20);
        // }

        [ContextMenu("Spawn Phone")]
        public void SpawnMobile()
        {
            var mobile = Instantiate(_mobilePrefab, transform);
            mobile.Init((Tuio20Manager)_transmitter.Manager,0);
        }
    }
}
