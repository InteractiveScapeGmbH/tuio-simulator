using System;
using System.Collections.Generic;
using TuioNet.Server;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TuioSimulator.Tuio.Tuio20
{
    [RequireComponent(typeof(MouseClicker))]
    public class MobileSpawner : SpawnerBase
    {
        [SerializeField] private Tuio20Mobile _mobilePrefab;

        private Tuio20Manager _manager;
        
        private readonly Queue<string> _mobilesToAdd = new();
        private readonly Queue<string> _mobilesToRemove = new();
        private Dictionary<string, Tuio20Mobile> _appMobiles = new();
        private MouseClicker _mouseClicker;

        private void Awake()
        {
            _mouseClicker = GetComponent<MouseClicker>();
        }

        private void OnEnable()
        {
            _mouseClicker.OnRightDoubleClick += SpawnMobile;
        }

        private void OnDisable()
        {
            _mouseClicker.OnRightDoubleClick -= SpawnMobile;
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

        public override void SetManager(ITuioManager manager)
        {
            _manager = manager as Tuio20Manager;
        }
    }
}
