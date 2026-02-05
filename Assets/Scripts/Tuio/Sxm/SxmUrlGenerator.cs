using TuioSimulator.QR;
using TuioSimulator.Tuio.Tuio20;
using UnityEngine;

namespace TuioSimulator.Tuio.Sxm
{
    public class SxmUrlGenerator : MonoBehaviour
    {
        [SerializeField] private Tuio20Mobile _mobile;
        [SerializeField] private QrImage _qrImage;
        [SerializeField] private SxmConfig _sxmConfig;

        private void Start()
        {
            _qrImage.Url = $"{_sxmConfig.WebAppUrl}?r={_sxmConfig.RoomId}/{_mobile.UUID}&u={_sxmConfig.BrokerUrl}:{_sxmConfig.BrokerPort}";
        }
    }
}