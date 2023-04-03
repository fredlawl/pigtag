using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pathing
{
    interface IPathfinder
    {
        delegate void ObsticleAdded();
        public Queue<Vector3> FindPath(Vector3 from, Vector3 to);
    }
}
