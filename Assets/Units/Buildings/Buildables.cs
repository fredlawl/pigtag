using Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public class Buildables : MonoBehaviour
    {
        [SerializeField]
        private List<BuildingManager> buildables = new List<BuildingManager>();
    }
}

