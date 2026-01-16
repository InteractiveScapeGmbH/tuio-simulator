using System;
using TuioSimulator.QR;
using UnityEngine;

namespace TuioSimulator.Sxm
{
    public class SxmUrlGenerator : MonoBehaviour
    {
        [SerializeField] private QrImage _qrImage;
        [SerializeField] private SxmConfig _sxmConfig;

        private void Start()
        {
            _qrImage.Url = _sxmConfig.Url;
        }
    }
}