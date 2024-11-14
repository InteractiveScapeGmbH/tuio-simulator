using System;
using TuioNet.Server;
using TuioSimulation.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11Spawner : MonoBehaviour
    {
        // [SerializeField] private TuioTransmitter _transmitter;
        [SerializeField] private Tuio11CursorBehaviour _cursorPrefab;
        [SerializeField] private Tuio11ObjectBehaviour _objectPrefab;
        [SerializeField] private MouseClicker _mouseClicker;

        private Tuio11Manager _manager;

        // private Tuio11Manager Manager
        // {
        //     get
        //     {
        //         if (_manager == null)
        //         {
        //             _manager = (Tuio11Manager)_transmitter.Manager;
        //         }
        //
        //         return _manager;
        //     }
        // }

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
        
        private void AddPointer(Vector2 position)
        {
            _cursor = Instantiate(_cursorPrefab, transform);
            _cursor.Init(_manager, position);
            _mouseClicker.OnLeftMove += MovePointer;
        }
        
        private void MovePointer(Vector2 position)
        {
            _cursor.Position = position;
        }
        
        private void RemovePointer(Vector2 position)
        {
            Destroy(_cursor.gameObject);
            _mouseClicker.OnLeftMove -= MovePointer;
        }
        
        private void AddToken(Vector2 position)
        {
            var token = Instantiate(_objectPrefab, transform);
            token.Init(_manager, 2, position);
        }
        

        // [ContextMenu("Spawn Cursor")]
        // public void SpawnCursor()
        // {
        //     var cursor = Instantiate(_cursorPrefab, transform);
        //     cursor.Init((Tuio11Manager)_transmitter.Manager);
        // }
        //
        // [ContextMenu("Spawn Object")]
        // public void SpawnObject()
        // {
        //     var tuioObject = Instantiate(_objectPrefab, transform);
        //     tuioObject.Init((Tuio11Manager)_transmitter.Manager, 5);
        // }
    }
}
