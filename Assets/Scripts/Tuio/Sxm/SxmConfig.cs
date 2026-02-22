using System;
using TuioNet.OSC;
using UnityEngine;

namespace TuioSimulator.Tuio.Sxm
{
    [CreateAssetMenu(fileName = "SxmConfig", menuName = "TuioSimulator/New Sxm Config", order = 0)]
    public class SxmConfig : ScriptableObject
    {
        [field:SerializeField] public string WebAppUrl { get; set; }
        [field:SerializeField] public string RoomId { get; set; }
        [field:SerializeField] public string BrokerUrl { get; set; }
        [field:SerializeField] public int BrokerPort { get; set; }


        public OSCMessage Message
        {
            get
            {
                var message = new OSCMessage("/scape_x_mobile/def");
                message.Append(new OscTimeTag(DateTime.Now));
                message.Append(RoomId);
                message.Append($"{BrokerUrl}:{BrokerPort}");
                return message;
            }
        }
    }
}