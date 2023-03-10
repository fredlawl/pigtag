using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    public static readonly int DefaultMask = 1 << 0;
    public static readonly int PlayerMask = 1 << 3;
    public static readonly int WaterMask = 1 << 4;
    public static readonly int MapMask = 1 << 6;
    public static readonly int MapObstacleMask = 1 << 8;

    public static readonly int Collidables = DefaultMask | PlayerMask | WaterMask | MapObstacleMask;
}
