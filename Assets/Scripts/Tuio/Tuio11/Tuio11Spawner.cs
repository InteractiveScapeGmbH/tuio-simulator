using System;
using System.Collections.Generic;
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
        [SerializeField] private MouseDrager _mouseDrager;
        [SerializeField] private CurrentIdSO _currentId;

        private Tuio11Manager _manager;
        private readonly Dictionary<int, Tuio11CursorBehaviour> _activeCursors = new();

        private void OnEnable()
        {
            _mouseClicker.OnLeftDown += AddPointer;
            _mouseClicker.OnLeftUp += RemovePointer;

            _mouseClicker.OnLeftDoubleClick += AddToken;

            _mouseDrager.OnMove += MovePointer;
        }
        
        private void OnDisable()
        {
            _mouseClicker.OnLeftDown -= AddPointer;
            _mouseClicker.OnLeftUp -= RemovePointer;

            _mouseClicker.OnLeftDoubleClick -= AddToken;
            _mouseDrager.OnMove -= MovePointer;
        }
        
        public void SetManager(ITuioManager manager)
        {
            _manager = (Tuio11Manager)manager;
        }
        
        private void AddPointer(PointerEventData pointerEventData)
        {
            var cursor = Instantiate(_cursorPrefab, transform);
            cursor.Init(_manager, pointerEventData.position);
            _activeCursors[pointerEventData.pointerId] = cursor;
        }
        
        private void MovePointer(PointerEventData pointerEventData)
        {
            _activeCursors[pointerEventData.pointerId].Position = pointerEventData.position;
        }
        
        private void RemovePointer(PointerEventData pointerEventData)
        {
            if (_activeCursors.Remove(pointerEventData.pointerId, out var cursorBehaviour))
            {
                Destroy(cursorBehaviour.gameObject);
            }
        }
        
        private void AddToken(Vector2 position)
        {
            var token = Instantiate(_objectPrefab, transform);
            token.Init(_manager, _currentId.CurrentId, position);
        }
    }
}
