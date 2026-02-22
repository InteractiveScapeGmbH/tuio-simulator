using System;
using System.Collections.Generic;
using TuioNet.Server;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TuioSimulator.Tuio.Tuio20
{
    public struct MobileData
    {
        public Vector2 Position;
        public string Data;

        public MobileData(Vector2 position, string data)
        {
            Position = position;
            Data = data;
        }
    }
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
        
        private readonly Queue<MobileData> _mobilesToAdd = new();
        private readonly Queue<string> _mobilesToRemove = new();

        private Dictionary<string, Tuio20Mobile> _appMobiles = new();

        private void OnEnable()
        {
            _mouseClicker.OnLeftDown += AddPointer;
            _mouseClicker.OnLeftUp += RemovePointer;

            _mouseClicker.OnLeftDoubleClick += AddToken;
            _mouseClicker.OnRightDoubleClick += SpawnMobile;

            _mouseDrager.OnMove += MovePointer;
            
        }

        private void OnDisable()
        {
            _mouseClicker.OnLeftDown -= AddPointer;
            _mouseClicker.OnLeftUp -= RemovePointer;

            _mouseClicker.OnLeftDoubleClick -= AddToken;
            _mouseClicker.OnRightDoubleClick -= SpawnMobile;

            _mouseDrager.OnMove -= MovePointer;
        }

        private void MovePointer(PointerEventData eventData)
        {
            _activePointers[eventData.pointerId].Position = eventData.position;
        }

        public void AddMobile(Vector2 position, string data)
        {
            _mobilesToAdd.Enqueue(new MobileData(position, data));
        }

        public void RemoveMobile(string id)
        {
            _mobilesToRemove.Enqueue(id);
        }

        private void DestroyMobile(string id)
        {
            if (_appMobiles.Remove(id, out var mobile))
            {
                print($"Destroy {id}");
                Destroy(mobile.gameObject);
            }
        }

        private void Update()
        {
            while (_mobilesToRemove.Count > 0)
            {
                DestroyMobile(_mobilesToRemove.Dequeue());
            }
            
            while (_mobilesToAdd.Count > 0)
            {
                var mobileData = _mobilesToAdd.Dequeue();
                SpawnMobileWithData(mobileData.Position, mobileData.Data);
            }
        }

        private void SpawnMobileWithData(Vector2 position, string data)
        {
            if (_appMobiles.ContainsKey(data))
                return;
            var mobile = Instantiate(_mobilePrefab, transform);
            mobile.Init(_manager, 1, position, ref _appMobiles ,data);
            if (data != null)
                _appMobiles.Add(data, mobile);
        }

        private void SpawnMobile(Vector2 position)
        {
            SpawnMobileWithData(position, null);
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
