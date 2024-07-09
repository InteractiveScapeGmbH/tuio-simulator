using System;
using System.Collections.Generic;
using System.Linq;
using OSC.NET;
using TuioNet.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Repository
    {
        private readonly IList<ITuioEntity> _entities;

        private readonly string _sourceName;

        private uint _screenResolution = 36;
        
        private uint _frameId = 1;
        private TuioTime _time;

        public Tuio20Repository(string sourceName, Vector2 screenResolution)
        {
            _sourceName = sourceName;
            _entities = new List<ITuioEntity>();
        }

        public void Update(uint frameId)
        {
            _frameId = frameId;
            _time = TuioTime.GetSystemTime();
        }
        
        private OSCMessage AliveMessage
        {
            get
            {
                var message = new OSCMessage("/tuio2/alv");
                foreach (var entity in _entities)
                {
                    message.Append(entity.SessionId);
                }
                return message;
            }
        }

        public void AddEntity(ITuioEntity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(ITuioEntity entity)
        {
            _entities.Remove(entity);
        }

        private OSCMessage FrameMessage
        {
            get
            {
                var message = new OSCMessage("/tuio2/frm");
                message.Append(_frameId);
                message.Append(new OscTimeTag(DateTime.Now));
                message.Append(_screenResolution);
                message.Append(_sourceName);
                return message;
            }
        }

        public OSCBundle UpdateBundle(OSCBundle bundle)
        {
            bundle.Append(FrameMessage);
            foreach (var entity in _entities)
            {
                bundle.Append(entity.OscMessage);
            }
            bundle.Append(AliveMessage);
            return bundle;
        }

        public void Clear()
        {
            _entities.Clear();
        }
    }
}