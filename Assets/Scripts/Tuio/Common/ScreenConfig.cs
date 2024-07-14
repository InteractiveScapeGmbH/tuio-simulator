using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    [CreateAssetMenu(fileName = "ScreenConfig", menuName = "TuioSimulator/New ScreenConfig", order = 0)]
    public class ScreenConfig : ScriptableObject
    {
        [field: SerializeField] public Vector2 Resolution { get; set; }
        [field: SerializeField] public float Diagonal { get; set; }
        [field:SerializeField] public float PixelPerMM { get; set; }

        private const float InchToMM = 25.4f;

        private void OnValidate()
        {
            CalcPixelPerMM();
        }

        public float AspectRatio => Resolution.x / Resolution.y;
        
        /// <summary>
        /// Converts physical size in mm to screen size in pixels.
        /// </summary>
        /// <param name="physicalSize">Physical size in mm.</param>
        /// <returns>Screen size in pixels.</returns>
        public float PhysicalToScreenSize(float physicalSize)
        {
            return physicalSize * PixelPerMM;
        }

        private void CalcPixelPerMM()
        {
            var diagonalMM = Diagonal * InchToMM;
            var height = Mathf.Sqrt((diagonalMM * diagonalMM) / ((AspectRatio * AspectRatio) + 1));
            PixelPerMM = Resolution.y / height;
        }
    }
}