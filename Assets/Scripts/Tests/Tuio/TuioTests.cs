using System.Numerics;
using NUnit.Framework;
using TuioNet.Common;
using TuioNet.Tuio11;
using TuioSimulator.Tuio;

namespace TuioSimulator.Tests.Tuio
{
    public class TuioTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Test_Tuio11_Cursor_Creation()
        {
            var cursor = new Tuio11Cursor(TuioTime.GetSystemTime(), 3, 2, Vector2.One, Vector2.Zero, 0.0f);
            Assert.AreEqual(3, cursor.SessionId);
            Assert.AreEqual(2, cursor.CursorId);
            Assert.AreEqual(Vector2.One, cursor.Position);
            Assert.AreEqual(Vector2.Zero, cursor.Velocity);
            Assert.AreEqual(0.0, cursor.Acceleration);
        }

        [Test]
        public void Test_Tuio11_Cursor_Get_SetMessage_Int()
        {
            var cursor = new Tuio11Cursor(TuioTime.GetSystemTime(), 3, 2, Vector2.One, Vector2.Zero, 0.0f);
            var setMessage = cursor.SetMessage;
            Assert.AreEqual("/tuio/2Dcur set 3 1 1 0 0 0 ", setMessage.ToString());
        }
        
        [Test]
        public void Test_Tuio11_Cursor_Get_SetMessage_Float()
        {
            var cursor = new Tuio11Cursor(TuioTime.GetSystemTime(), 3, 2, new Vector2(1.2f, 3.2f), new Vector2(2.4f, 3.5f), 0.0f);
            var setMessage = cursor.SetMessage;
            Assert.AreEqual("/tuio/2Dcur set 3 1.2 3.2 2.4 3.5 0 ", setMessage.ToString());
        }

        [Test]
        public void Test_Tuio11_Object_Get_SetMessage()
        {
            var obj = new Tuio11Object(TuioTime.GetSystemTime(), 5, 2, new Vector2(1.2f, 2.2f), 47.2f,
                new Vector2(3.5f, 4.2f), 2.2f, 5.2f, 1.3f);
            var setMessage = obj.SetMessage;
            Assert.AreEqual("/tuio/2Dobj set 5 2 1.2 2.2 47.2 3.5 4.2 2.2 5.2 1.3 ", setMessage.ToString());
        }

        [Test]
        public void Test_Tuio11_Blob_Get_SetMessage()
        {
            var blob = new Tuio11Blob(TuioTime.GetSystemTime(), 7, 3, new Vector2(2.2f, 5.2f), 3.6f,
                new Vector2(3.4f, 2.4f), 3.5f, new Vector2(2.2f, 2.5f), 3.5f, 2.3f, 5.6f);
            var setMessage = blob.SetMessage;
            Assert.AreEqual("/tuio/2Dblb set 7 2.2 5.2 3.6 3.4 2.4 3.5 2.2 2.5 3.5 2.3 5.6 ", setMessage.ToString());
        }
    }
}
