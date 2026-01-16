using System;
using Newtonsoft.Json;

namespace TuioSimulator.Sxm
{
    public class DeviceInfo
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
        [JsonProperty("device_movement")]
        public string DeviceMovement { get; set; }
        [JsonProperty("device_tilt")]
        public string DeviceTilt { get; set; }
    }
}