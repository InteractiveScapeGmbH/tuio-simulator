using OSC.NET;
using TuioNet.Common;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Manager : ITuioManager
    {

        private readonly Tuio20Repository _repository;
        
        // private readonly IList<Tuio20Pointer> _pointers = new List<Tuio20Pointer>();
        // private readonly IList<Tuio20Token> _tokens = new List<Tuio20Token>();
        // private readonly IList<Tuio20Bounds> _bounds = new List<Tuio20Bounds>();
        // private readonly IList<Tuio20Symbol> _symbols = new List<Tuio20Symbol>();


        private OSCBundle _frameBundle;
        private int _frameId = 0;
        public int CurrentSessionId { get; private set; } = 0;

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

        // public void AddPointer(Tuio20Pointer tuioPointer)
        // {
        //     _pointers.Add(tuioPointer);
        //     CurrentSessionId++;
        // }
        //
        // public void RemovePointer(Tuio20Pointer tuioPointer)
        // {
        //     _pointers.Remove(tuioPointer);
        // }
        //
        // public void AddToken(Tuio20Token tuioToken)
        // {
        //     _tokens.Add(tuioToken);
        //     CurrentSessionId++;
        // }
        //
        // public void RemoveToken(Tuio20Token tuioToken)
        // {
        //     _tokens.Remove(tuioToken);
        // }
        //
        // public void AddBounds(Tuio20Bounds tuioBounds)
        // {
        //     _bounds.Add(tuioBounds);
        //     CurrentSessionId++;
        // }
        //
        // public void RemoveBounds(Tuio20Bounds tuioBounds)
        // {
        //     _bounds.Remove(tuioBounds);
        // }
        //
        // public void AddSymbol(Tuio20Symbol tuioSymbol)
        // {
        //     _symbols.Add(tuioSymbol);
        //     CurrentSessionId++;
        // }
        //
        // public void RemoveSymbol(Tuio20Symbol tuioSymbol)
        // {
        //     _symbols.Remove(tuioSymbol);
        // }
        

        public void Update()
        {
            _frameId += 1;
            _repository.Update(_frameId);
        }
        
        private void UpdateFrameBundle()
        {
            _frameBundle = new OSCBundle();
            _repository.UpdateBundle(_frameBundle);
        }
    }
}