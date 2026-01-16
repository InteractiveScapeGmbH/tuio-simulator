using System.Collections.Generic;
using TuioNet.Server;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Spawner : MonoBehaviour
    {
        [SerializeField] private Tuio20PointerBehaviour _pointerPrefab;
        [SerializeField] private Tuio20TokenBehaviour _tokenPrefab;
        [SerializeField] private Tuio20Mobile _mobilePrefab;
        [SerializeField] private MouseClicker _mouseClicker;
        [SerializeField] private MouseDrager _mouseDrager;
        [SerializeField] private CurrentIdSO _currentId;

        private Tuio20Manager _manager;
        private readonly Dictionary<int, Tuio20PointerBehaviour> _activePointers = new();
        
        private void OnEnable()
        {
            _mouseClicker.OnLeftDown += AddPointer;
            _mouseClicker.OnLeftUp += RemovePointer;

            _mouseClicker.OnLeftDoubleClick += AddToken;
            _mouseClicker.OnRightDoubleClick += AddMobile;

            _mouseDrager.OnMove += MovePointer;
            
        }

        private void OnDisable()
        {
            _mouseClicker.OnLeftDown -= AddPointer;
            _mouseClicker.OnLeftUp -= RemovePointer;

            _mouseClicker.OnLeftDoubleClick -= AddToken;
            _mouseClicker.OnRightDoubleClick -= AddMobile;

            _mouseDrager.OnMove -= MovePointer;
        }

        private void MovePointer(PointerEventData eventData)
        {
            _activePointers[eventData.pointerId].Position = eventData.position;
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


        private void AddPointer(PointerEventData pointerEventData)
        {
            var pointer = Instantiate(_pointerPrefab, transform);
            pointer.Init(_manager, pointerEventData.position);
            _activePointers[pointerEventData.pointerId] = pointer;
        }
        

        private void RemovePointer(PointerEventData pointerEventData)
        {
            if (_activePointers.Remove(pointerEventData.pointerId, out var pointerBehaviour))
            {
                Destroy(pointerBehaviour.gameObject);
            }
        }
    }
}
