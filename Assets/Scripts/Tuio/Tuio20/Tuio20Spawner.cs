using TuioNet.Server;
using TuioSimulator.Tuio.Common;
using UnityEngine;

namespace TuioSimulator.Tuio.Tuio20
{
    public class Tuio20Spawner : MonoBehaviour
    {
        [SerializeField] private SpawnerBase[] _spawner;

        public void Init(ITuioManager manager)
        {
            foreach (var spawner in _spawner)  
            {
                spawner.SetManager(manager);
            }
        }
    }
}