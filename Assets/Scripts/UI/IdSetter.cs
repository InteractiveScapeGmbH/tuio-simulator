using System;
using TMPro;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.UI
{
    public class IdSetter : MonoBehaviour
    {
        [SerializeField] private CurrentIdSO _currentId;
        [SerializeField] private TMP_InputField _inputField;

        private void Start()
        {
            _inputField.text = _currentId.CurrentId.ToString();
        }

        private void OnEnable()
        {
            _inputField.onValueChanged.AddListener(UpdateCurrentId);
        }

        private void OnDisable()
        {
            _inputField.onValueChanged.RemoveAllListeners();
        }

        private void UpdateCurrentId(string inputValue)
        {
            uint id = uint.TryParse(inputValue, out var newId) ? newId : 0;
            _currentId.CurrentId = id;
        }
    }
}