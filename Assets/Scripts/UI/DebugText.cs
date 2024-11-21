using System;
using TMPro;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.UI
{
    public class DebugText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private DebugTuio _debugTuio;

        private void Update()
        {
            _text.text = _debugTuio.DebugText();
        }
    }
}