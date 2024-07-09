using OSC.NET;
using TuioNet.Common;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Manager : ITuioManager
    {

        private readonly Tuio20Repository _repository;
        
        private OSCBundle _frameBundle;
        private uint _frameId = 0;
        public uint CurrentSessionId { get; private set; } = 0;

        public Tuio20Manager(string sourceName)
        {
            _repository = new Tuio20Repository(sourceName, Vector2.zero);
        }

        public OSCBundle FrameBundle
        {
            get
            {
                UpdateFrameBundle();
                return _frameBundle;
            }
        }

        public void AddEntity(ITuioEntity entity)
        {
            _repository.AddEntity(entity);
            CurrentSessionId++;
        }

        public void RemoveEntity(ITuioEntity entity)
        {
            _repository.RemoveEntity(entity);
        }

        public void Update()
        {
            _frameId += 1;
            _repository.Update(_frameId);
        }

        public void Quit()
        {
            _repository.Clear();
            Update();
        }

        private void UpdateFrameBundle()
        {
            _frameBundle = new OSCBundle();
            _repository.UpdateBundle(_frameBundle);
        }
    }
}