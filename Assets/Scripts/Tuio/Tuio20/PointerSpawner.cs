using System;
using System.Collections.Generic;
using TuioNet.Server;
using TuioSimulator.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.Tuio.Tuio20
{
    [RequireComponent(typeof(MouseClicker), typeof(MouseDrager))]
    public class PointerSpawner : MonoBehaviour
    {
        [SerializeField] private Tuio20PointerBehaviour _pointerPrefab;
        private MouseClicker _mouseClicker;
        private MouseDrager _mouseDrager;
        
        private readonly Dictionary<int, Tuio20PointerBehaviour> _activePointers = new();
        
        private Tuio20Manager _manager;

        public void SetManager(ITuioManager manager)
        {
            _manager = manager as Tuio20Manager;
        }

        private void Awake()
        {
            _mouseClicker = GetComponent<MouseClicker>();
            _mouseDrager = GetComponent<MouseDrager>();
        }

        private void OnEnable()
        {
            _mouseClicker.OnLeftDown += AddPointer;
            _mouseDrager.OnMove += MovePointer;
            _mouseClicker.OnLeftUp += RemovePointer;
        }

        private void OnDisable()
        {
            _mouseClicker.OnLeftDown -= AddPointer;
            _mouseDrager.OnMove -= MovePointer;
            _mouseClicker.OnLeftUp -= RemovePointer;
        }

        private void AddPointer(PointerEventData eventData)
        {
            var pointer = Instantiate(_pointerPrefab, transform);
            pointer.Init(_manager, eventData.position);
            _activePointers[eventData.pointerId] = pointer;
        }

        private void MovePointer(PointerEventData eventData)
        {
            _activePointers[eventData.pointerId].Position = eventData.position;
        }

        private void RemovePointer(PointerEventData eventData)
        {
            if (_activePointers.Remove(eventData.pointerId, out var pointerBehaviour))
            {
                Destroy(pointerBehaviour.gameObject);
            }
        }
    }
}