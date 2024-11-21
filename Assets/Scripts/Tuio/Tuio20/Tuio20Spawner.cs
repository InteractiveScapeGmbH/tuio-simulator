using TuioNet.Server;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Spawner : MonoBehaviour
    {
        // [SerializeField] private TuioTransmitter _transmitter;
        [SerializeField] private Tuio20PointerBehaviour _pointerPrefab;
        [SerializeField] private Tuio20TokenBehaviour _tokenPrefab;
        [SerializeField] private Tuio20Mobile _mobilePrefab;
        [SerializeField] private MouseClicker _mouseClicker;
        [SerializeField] private CurrentIdSO _currentId;

        private Tuio20Manager _manager;
        private Tuio20PointerBehaviour _pointer;
        
        private void OnEnable()
        {
            _mouseClicker.OnLeftDown += AddPointer;
            _mouseClicker.OnLeftUp += RemovePointer;

            // _mouseClicker.OnLeftDoubleClick += AddToken;
            // _mouseClicker.OnRightDoubleClick += AddMobile;
        }

        private void OnDisable()
        {
            _mouseClicker.OnLeftDown -= AddPointer;
            _mouseClicker.OnLeftUp -= RemovePointer;

            // _mouseClicker.OnLeftDoubleClick -= AddToken;
            // _mouseClicker.OnRightDoubleClick -= AddMobile;
        }

        private void AddMobile(Vector2 position)
        {
            var mobile = Instantiate(_mobilePrefab, transform);
            mobile.Init(_manager, 1, position);
        }

        public void SetManager(ITuioManager manager)
        {
            _manager = (Tuio20Manager)manager;
        }

        private void AddToken(Vector2 position)
        {
            var token = Instantiate(_tokenPrefab, transform);
            token.Init(_manager, _currentId.CurrentId, position);
        }


        private void AddPointer(Vector2 position)
        {
            _pointer = Instantiate(_pointerPrefab, transform);
            _pointer.Init(_manager, position);
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

        public void SpawnMobile()
        {
            var mobile = Instantiate(_mobilePrefab, transform);
            mobile.Init(_manager,0, Vector2.zero);
        }
    }
}
