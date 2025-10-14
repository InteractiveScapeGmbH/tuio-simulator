using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    public class SliderManager : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _label, _minLabel, _maxLabel;
        [SerializeField] private float _min, _max;
        [SerializeField] private String _unit;

        void Awake()
        {
            _slider.maxValue = _max;
            _maxLabel.text = _max + _unit;

            _slider.minValue = _min;
            _minLabel.text = _min + _unit;

            _label.text = _slider.value + _unit;

            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            _label.text = (int) value + _unit;
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}