using System;
using TuioNet.Server;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11Spawner : MonoBehaviour
    {
        [SerializeField] private Tuio11CursorBehaviour _cursorPrefab;
        [SerializeField] private Tuio11ObjectBehaviour _objectPrefab;
        [SerializeField] private MouseClicker _mouseClicker;
        [SerializeField] private CurrentIdSO _currentId;

        private Tuio11Manager _manager;


        private Tuio11CursorBehaviour _cursor;

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
        
        public void SetManager(ITuioManager manager)
        {
            _manager = (Tuio11Manager)manager;
        }
        
        private void AddPointer(PointerEventData pointerEventData)
        {
            _cursor = Instantiate(_cursorPrefab, transform);
            _cursor.Init(_manager, pointerEventData.position);
            _mouseClicker.OnLeftMove += MovePointer;
        }
        
        private void MovePointer(PointerEventData pointerEventData)
        {
            _cursor.Position = pointerEventData.position;
        }
        
        private void RemovePointer(PointerEventData pointerEventData)
        {
            Destroy(_cursor.gameObject);
            _mouseClicker.OnLeftMove -= MovePointer;
        }
        
        private void AddToken(Vector2 position)
        {
            var token = Instantiate(_objectPrefab, transform);
            token.Init(_manager, _currentId.CurrentId, position);
        }
    }
}
