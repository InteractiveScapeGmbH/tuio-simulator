using System;
using TuioNet.Server;
using TuioSimulator.Input;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio20
{
    [RequireComponent(typeof(MouseClicker))]
    public class TokenSpawner : MonoBehaviour
    {
        [SerializeField] private Tuio20TokenBehaviour _tokenPrefab;
        [SerializeField] private CurrentIdSO _currentId;
        
        private MouseClicker _mouseClicker;
        private Tuio20Manager _manager;

        public void SetManager(ITuioManager manager)
        {
            _manager = manager as Tuio20Manager;
        }

        private void Awake()
        {
            _mouseClicker = GetComponent<MouseClicker>();
        }

        private void OnEnable()
        {
            _mouseClicker.OnLeftDoubleClick += AddToken;
        }

        private void OnDisable()
        {
            _mouseClicker.OnLeftDoubleClick -= AddToken;
        }

        private void AddToken(Vector2 position)
        {
            var token = Instantiate(_tokenPrefab, transform);
            token.Init(_manager, _currentId.CurrentId, position);
        }
    }
}