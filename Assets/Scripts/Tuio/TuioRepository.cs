using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSC.NET;
using TuioNet.Tuio11;

namespace TuioSimulator.Tuio
{
    public class TuioRepository<T> where T: ITuio11Entity
    {
        private IEnumerable<T> _entities;

        private readonly string _sourceName;

        private readonly string _tuioAddress;
        
        private int _frameId = 0;

        public TuioRepository(string sourceName, string tuioAddress)
        {
            _sourceName = sourceName;
            _tuioAddress = tuioAddress;
        }

        public void Update(int frameId, IEnumerable<T> activeEntities)
        {
            _frameId = frameId;
            _entities = activeEntities;
        }
        
        private OSCMessage SourceMessage
        {
            get
            {
                var message = new OSCMessage(_tuioAddress);
                message.Append("source");
                message.Append(_sourceName);
                return message;
            }
        }
        
        private OSCMessage AliveMessage
        {
            get
            {
                var message = new OSCMessage(_tuioAddress);
                message.Append("alive");
                foreach (var entity in _entities)
                {
                    message.Append(entity.SessionId);
                }

                return message;
            }
        }

        private OSCMessage FseqMessage
        {
            get
            {
                var message = new OSCMessage(_tuioAddress);
                message.Append("fseq");
                message.Append(_frameId);
                return message;
            }
        }

        public OSCBundle UpdateBundle(OSCBundle bundle)
        {
            if (_entities == null || !_entities.Any()) return bundle;
            bundle.Append(SourceMessage);
            bundle.Append(AliveMessage);
            foreach (var entity in _entities)
            {
                bundle.Append(entity.SetMessage);
            }
            bundle.Append(FseqMessage);
            var str = Encoding.UTF8.GetString(bundle.BinaryData);
            return bundle;
        }
    }
}