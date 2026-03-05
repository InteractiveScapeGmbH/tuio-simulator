using TuioNet.Server;
using UnityEngine;

namespace TuioSimulator.Tuio.Common
{
    public abstract class SpawnerBase : MonoBehaviour
    {
        public abstract void SetManager(ITuioManager manager);
    }
}