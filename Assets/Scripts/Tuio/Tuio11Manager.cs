using System.Collections.Generic;
using OSC.NET;
using TuioNet.Tuio11;
using TuioSimulator.Networking;

namespace TuioSimulator.Tuio
{
    public class Tuio11Manager : ITuioManager
    {
        private readonly TuioRepository<Tuio11Cursor> _cursorRepository;
        private readonly TuioRepository<Tuio11Object> _objectRepository;
        private readonly TuioRepository<Tuio11Blob> _blobRepository;
        
        private readonly IList<Tuio11Cursor> _cursors = new List<Tuio11Cursor>();
        private readonly IList<Tuio11Object> _objects = new List<Tuio11Object>();
        private IList<Tuio11Blob> _blobs = new List<Tuio11Blob>();
        
        private OSCBundle _frameBundle;
        private int _frameId = 0;

        public int CurrentSessionId { get; private set; } = 0;
        public OSCBundle FrameBundle
        {
            get
            {
                UpdateFrameBundle();
                return _frameBundle;
            }
        }

        public void AddCursor(Tuio11Cursor tuioCursor)
        {
            _cursors.Add(tuioCursor);
            CurrentSessionId++;
        }

        public void RemoveCursor(Tuio11Cursor tuio11Cursor)
        {
            _cursors.Remove(tuio11Cursor);
        }

        public void AddObject(Tuio11Object tuioObject)
        {
            _objects.Add(tuioObject);
            CurrentSessionId++;
        }

        public void RemoveObject(Tuio11Object tuioObject)
        {
            _objects.Remove(tuioObject);
        }
        
        public Tuio11Manager(string sourceName)
        {
            _cursorRepository = new TuioRepository<Tuio11Cursor>(sourceName, "/tuio/2Dcur");
            _objectRepository = new TuioRepository<Tuio11Object>(sourceName, "/tuio/2Dobj");
            _blobRepository = new TuioRepository<Tuio11Blob>(sourceName, "/tuio/2Dblb");
        }
        
        
        public void Update()
        {
            _frameId += 1;
            _cursorRepository.Update(_frameId, _cursors);
            _objectRepository.Update(_frameId, _objects);
            _blobRepository.Update(_frameId, _blobs);
        }
        
        private void UpdateFrameBundle()
        {
            _frameBundle = new OSCBundle();
            _cursorRepository.UpdateBundle(_frameBundle);
            _objectRepository.UpdateBundle(_frameBundle);
            _blobRepository.UpdateBundle(_frameBundle);
        }
    }
}