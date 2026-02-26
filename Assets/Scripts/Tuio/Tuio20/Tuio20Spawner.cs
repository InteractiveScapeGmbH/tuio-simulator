using System;
using System.Collections.Generic;
using TuioNet.Server;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Spawner : MonoBehaviour
    {
        [SerializeField] private Tuio20PointerBehaviour _pointerPrefab;
        [SerializeField] private Tuio20TokenBehaviour _tokenPrefab;
        [SerializeField] private Tuio20Mobile _mobilePrefab;
      
        [SerializeField] private CurrentIdSO _currentId;
        // [SerializeField] private RadialMenu

        private Tuio20Manager _manager;
        private readonly Dictionary<int, Tuio20PointerBehaviour> _activePointers = new();
        
        private readonly Queue<string> _mobilesToAdd = new();
        private readonly Queue<string> _mobilesToRemove = new();

        private Dictionary<string, Tuio20Mobile> _appMobiles = new();

        public void MovePointer(Vector2 position, int pointerId)
        {
            _activePointers[pointerId].Position = position;
        }

        public void AddMobile(string data)
        {
            _mobilesToAdd.Enqueue(data);
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
                var randomPosition = SpawnInRadius(100);
                SpawnMobileWithData(randomPosition, _mobilesToAdd.Dequeue());
            }
        }

        private Vector2 SpawnInRadius(float radius)
        {
            return Random.insideUnitCircle * radius + 0.5f * new Vector2(Screen.width, Screen.height);
        }

        private void SpawnMobileWithData(Vector2 position, string data)
        {
            if (data != null && _appMobiles.ContainsKey(data))
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

        public void SpawnToken(Vector2 position)
        {
            var token = Instantiate(_tokenPrefab, transform);
            token.Init(_manager, _currentId.CurrentId, position);
        }

        public void SpawnPointer(Vector2 position, int pointerId)
        {
            var pointer = Instantiate(_pointerPrefab, transform);
            pointer.Init(_manager, position);
            _activePointers[pointerId] = pointer;
        }


        public void RemovePointer(int pointerId)
        {
            if (_activePointers.Remove(pointerId, out var pointerBehaviour))
            {
                Destroy(pointerBehaviour.gameObject);
            }
        }
    }
}
