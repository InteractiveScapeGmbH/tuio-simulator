using UnityEngine;
using UnityEngine.UI;

namespace TuioSimulator.QR
{
    [RequireComponent(typeof(RawImage))]
    public class QrImage : MonoBehaviour
    {
        [SerializeField] private string _url;
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                UpdateTexture();
            }
        }
        private RawImage _image;
        private QrCodeGenerator _generator;
        private Material _material;

        private void Awake()
        {
            _image = GetComponent<RawImage>();
            _generator = new QrCodeGenerator();
            _material = new Material(_image.material);
            _image.material = _material;
            UpdateTexture();
        }

        private void UpdateTexture()
        {
            if (string.IsNullOrEmpty(_url)) return;
            var data = _generator.CreateQrCode(_url, QrCodeGenerator.ECCLevel.Q);
            var qrCode = new QrCode(data);
            _image.material.mainTexture = qrCode.GetGraphic(20);
            _image.enabled = false;
            _image.enabled = true;
        }
    }
}