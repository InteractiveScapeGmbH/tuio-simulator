using System.Collections.Generic;
using System.Linq;
using OSC.NET;
using TuioNet.Common;
using TuioNet.Tuio11;

namespace TuioSimulator.Tuio.Tuio11
{
    public class Tuio11Repository
    {
        private IList<ITuioEntity> _entities;

        private readonly string _sourceName;

        private readonly string _tuioAddress;
        
        private int _frameId = 0;

        public Tuio11Repository(string sourceName, string tuioAddress)
        {
            _sourceName = sourceName;
            _tuioAddress = tuioAddress;
            _entities = new List<ITuioEntity>();
        }

        public void Update(int frameId)
        {
            _frameId = frameId;
        }

        public void Add(ITuioEntity entity)
        {
            _entities.Add(entity);
        }

        public void Remove(ITuioEntity entity)
        {
            _entities.Remove(entity);
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

        public void Clear()
        {
            _entities.Clear();
        }

        public OSCBundle UpdateBundle(OSCBundle bundle)
        {
            if (_entities == null || !_entities.Any()) return bundle;
            bundle.Append(SourceMessage);
            bundle.Append(AliveMessage);
            foreach (var entity in _entities)
            {
                bundle.Append(entity.OscMessage);
            }
            bundle.Append(FseqMessage);
            return bundle;
        }
    }
}