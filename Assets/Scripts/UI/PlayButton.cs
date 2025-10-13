using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private string _onText;
        [SerializeField] private string _offText;

        public void UpdateText(bool isOn)
        {
            _text.text = isOn ? _onText : _offText; 
        }

        public void AddListener(UnityAction listener)
        {
            _button.onClick.AddListener(listener);
        }

        public void RemoveAllListeners()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}