using System;
using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.UI
{
    public class MenuEntry : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Init(int index, int count)
        {
            float sat = index / (float)count;
            var color = Color.HSVToRGB(1, sat, 1);
            _image.color = color;
        }
    }
}