using System;
using System.Net.NetworkInformation;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.UI
{
    public class TuioPresetLibrary : MonoBehaviour
    {
        [SerializeField] private PresetLibrary _library;
        [SerializeField] private TokenPresetUI _tokenUIPrefab;
        [SerializeField] private RectTransform _tokenParent;
        [SerializeField] private PhonePresetUI _phoneUIPrefab;
        [SerializeField] private RectTransform _phoneParent;
        
        private void Start()
        {
            foreach (var tokenPreset in _library.TokenPresets)
            {
                var ui = Instantiate(_tokenUIPrefab, _tokenParent);
                ui.Init(tokenPreset.Name, tokenPreset.Icon);
            }

            foreach (var phonePreset in _library.PhonePresets)
            {
                var ui = Instantiate(_phoneUIPrefab, _phoneParent);
                ui.SetText(phonePreset.Name);
            }
        }
    }
}